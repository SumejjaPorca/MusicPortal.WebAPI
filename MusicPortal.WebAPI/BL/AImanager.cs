using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.Data;
using MusicPortal.WebAPI.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.BL
{
    public class AIManager
    {
        // PROBABILITIES FOR EACH TAG:
        // *local(user) probability = probability that the user will like the tag - taken from the userTag tree
        // *global probability = probability that ANY user will like the tag - take from the Tag tree
        // *neighbor probability = probability among the tags of the same parent
        // *real user probability = user probability * global probability
        
        private MusicPortalDbContext _db;
        private static int _tagLimit = 50;
        private static double _delta = 0.00001;
        private static int _songLimit = 15;

        public AIManager(MusicPortalDbContext _db)
        {
             this._db = _db;
        }

        public List<HeartedSongVM> GetFlow(string userId) {
            //get userTags with their neighbor* user* probability
            Dictionary<Tag, double> userTags = (from tu in _db.TagUsers
                         where tu.UserId == userId
                         group tu by tu.ParentTagId into tags
                         from t in _db.Tags
                         join parent in _db.Tags on t.ParentId.Value equals parent.Id
                         where t.ParentId == tags.Key
                         select new {tag = t, popularity = (double)t.Popularity / (1 + tags.Sum(tag => tag.Popularity))}).ToDictionary(kv => kv.tag, kv => kv.popularity, new TagComparer());

            //get songs
            List<long?> parentIds = userTags.Keys.Select(t => t.ParentId).Distinct().ToList();
            List<Tag> songs = userTags.Keys.Where(t => !parentIds.Contains(t.Id)).ToList();
            List<long> songIds = songs.Select(s => s.Id).ToList();

            //get all author tag ids
            parentIds = songs.Select(t => t.ParentId).Distinct().ToList();
            
            //get all subgenre tag ids
            List<long> genreIds = userTags.Keys.Where(t => parentIds.Contains(t.Id)).Select(t => t.ParentId.Value).ToList();

            //get real user* neighbor probabilites for all songs
            Dictionary<Tag, double> realNeighborProbSongs = (from t in _db.Tags
                         where songIds.Contains(t.Id)
                         group t by t.ParentId into tags
                         from t in _db.Tags
                         join parent in _db.Tags on t.ParentId equals parent.Id
                         where t.ParentId == tags.Key
                         select new {tag = t, popularity = (double) t.Popularity / (1 + tags.Sum(tag => tag.Popularity))}).ToDictionary(kv => kv.tag, kv => kv.popularity, new TagComparer());
            foreach (Tag song in realNeighborProbSongs.Keys) {
                realNeighborProbSongs[song] *= userTags[song];
            }

            //get new songs with higher global neighbor probabilities
            Dictionary<Tag, double> newSongs = (
                         from t in _db.Tags
                         where parentIds.Contains(t.ParentId)
                         group t by t.ParentId
                         into tags
                         from t in _db.Tags
                         where t.ParentId == tags.Key && !songIds.Contains(t.Id)
                         select new {tag = t, popularity = (double) t.Popularity / (1 + tags.Sum(tag => tag.Popularity))}).ToDictionary(kv => kv.tag, kv => kv.popularity, new TagComparer());

            Dictionary<Tag, Dictionary<Tag, double>> authorSongs = new Dictionary<Tag, Dictionary<Tag, double>>();
            foreach (KeyValuePair<Tag, double> song in realNeighborProbSongs) {
                authorSongs[song.Key.ParentTag].Add(song.Key, song.Value);
            }

            foreach (Tag newSong in newSongs.Keys)
            {
                newSongs[newSong] *= 1.0 / (1.0 + authorSongs[newSong.ParentTag].Sum(kv => kv.Key.Popularity));
                if (newSongs[newSong] >= authorSongs[newSong.ParentTag].Values.Max())
                {
                    authorSongs[newSong.ParentTag].Add(newSong, newSongs[newSong]);
                    realNeighborProbSongs.Add(newSong, newSongs[newSong]);
                }
            }

            //get real user* neighbor probabilites for all author tags
            Dictionary<Tag, double> realNeighborProbAuthors = (from t in _db.Tags
                         where parentIds.Contains(t.Id)
                         group t by t.ParentId into tags
                         from t in _db.Tags
                         join parent in _db.Tags on t.ParentId equals parent.Id
                         where t.ParentId == tags.Key
                         select new {tag = t, popularity = (double) t.Popularity / (1 + tags.Sum(tag => tag.Popularity))}).ToDictionary(kv => kv.tag, kv => kv.popularity, new TagComparer());
            
            foreach (Tag author in realNeighborProbAuthors.Keys)
            {
                realNeighborProbAuthors[author] *= userTags[author];
            }


            //get new authors with higher global neighbor probabilities
            Dictionary<Tag, double> newAuthors = (
                         from t in _db.Tags
                         where genreIds.Contains(t.ParentId.Value)
                         group t by t.ParentId
                             into tags
                             from t in _db.Tags
                             where t.ParentId == tags.Key && !songIds.Contains(t.Id)
                             select new { tag = t, popularity = (double)t.Popularity / (1 + tags.Sum(tag => tag.Popularity))}).ToDictionary(kv => kv.tag, kv => kv.popularity, new TagComparer());

            Dictionary<Tag, Dictionary<Tag, double>> authorTags = new Dictionary<Tag, Dictionary<Tag, double>>();
            foreach (KeyValuePair<Tag, double> author in realNeighborProbAuthors)
            {
                authorTags[author.Key.ParentTag].Add(author.Key, author.Value);
            }

            foreach (Tag newAuthor in newAuthors.Keys)
            {
                newAuthors[newAuthor] *= 1.0 / (1.0 + authorTags[newAuthor.ParentTag].Sum(kv => kv.Key.Popularity));
                if (newAuthors[newAuthor] >= authorTags[newAuthor.ParentTag].Values.Max())
                {
                    authorTags[newAuthor.ParentTag].Add(newAuthor, newAuthors[newAuthor]);
                    realNeighborProbAuthors.Add(newAuthor, newAuthors[newAuthor]);
                }
            }
            
            //getting real user probabilities for all tags
            List<Tag> genres = userTags.Keys.Where(t => genreIds.Contains(t.Id)).ToList();

            foreach(Tag t in genres){
                userTags[t] *= userTags[t.ParentTag];
            }

            foreach (Tag t in realNeighborProbAuthors.Keys)
            {
                userTags[t] = realNeighborProbAuthors[t] * userTags[t.ParentTag];
            }

            foreach (Tag t in realNeighborProbSongs.Keys)
            {
                userTags[t] = realNeighborProbSongs[t] * userTags[t.ParentTag];
            }

            var final_user_tags = userTags.Take(_tagLimit).ToDictionary(kv => kv.Key, kv => kv.Value, new TagComparer());
            var final_keys = final_user_tags.Keys;
            //TODO: extract songs from those tags

            //In DB
            var _songs = (from st in _db.TagSongs
                          join t in _db.Tags on st.TagId equals t.Id
                          where final_keys.Contains(t)
                          group t by st.SongId into ts
                          from s in _db.Songs
                          where s.Id == ts.Key
                          select new { Song = s, TSs = ts }).AsEnumerable();
            //In memory
            var _songs_ids = (from s in _songs orderby s.TSs.Sum(t => userTags[t]) descending select s.Song.Id).Take(_songLimit);

            SongManager sm = new SongManager(_db);

            var _s3 = (from s in _db.Songs where _songs_ids.Contains(s.Id) select s);

            var hearted = sm.MakeHeartedSong(_s3, userId).ToList();

            Random r = new Random();
            //Shuffle
            return hearted.OrderBy(h => r.Next()).ToList();
        }

        public void ReduceSubTree(string userId, long tagId) {

            long id = (from t in _db.TagUsers where t.Id == tagId && t.UserId == userId select t.ParentTagId).FirstOrDefault();
            List<TagUser> tags = (from tu in _db.TagUsers
                        where tu.UserId == userId && tu.ParentTagId == id
                        select new TagUser{
                            Id = tu.Id,
                            Popularity = tu.Popularity,
                            TagId = tu.TagId,
                            UserId = tu.UserId,
                            ParentTagId = tu.ParentTagId
                        }).ToList();
            //we add 1 to sum so that we consider the probability of listening new song
            double sum = 1.0 + Convert.ToDouble(tags.Sum(t => t.Popularity));
            foreach(TagUser t in tags){
                if(Convert.ToDouble(t.Popularity)/sum < _delta)
                    _db.TagUsers.Remove(t); //TODO: resolve cascade relationships between tag and it's parent
            }
            
        }

        private double getFullProbability(Tag tag, Dictionary<Tag, double> tags) {
            if (tag == null)
                return 1;
            return tags[tag] * getFullProbability(tag.ParentTag, tags);
        }

        private class TagComparer : IEqualityComparer<Tag>
        {
            public bool Equals(Tag x, Tag y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(Tag obj)
            {
                throw new NotImplementedException();
            }
        }


    }
}