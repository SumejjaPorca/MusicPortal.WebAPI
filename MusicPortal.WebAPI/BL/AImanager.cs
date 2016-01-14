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

            var userTags = (from tu in _db.TagUsers.Where(tu => tu.UserId == userId)
                         group tu by tu.ParentTagId into tags
                         from t in _db.Tags
                         join parent in _db.Tags on t.ParentId.Value equals parent.Id
                         where t.ParentId == tags.Key && tags.Select(prop => prop.TagId).Contains(t.Id)
                         select new {tag = t, popularity = (double)t.Popularity / (1.0 + tags.Sum(tag => tag.Popularity))}).ToDictionary(kv => kv.tag, kv => kv.popularity, new TagComparer());

            var taggilies = (from tu in _db.TagUsers
                                 join t in _db.Tags on tu.TagId equals t.Id
                                 where tu.UserId == userId && tu.ParentTagId == null
                                 select t).ToList();
            
            foreach(Tag t in taggilies)
            {
                userTags[t] =(double) t.Popularity / (1.0 + taggilies.Sum(tag => tag.Popularity));
            }
            //get songs
            List<long?> parentIds = userTags.Keys.Select(t => t.ParentId).Distinct().ToList();
            List<Tag> songs = userTags.Keys.Where(t => !parentIds.Contains(t.Id)).ToList();
            List<long> songIds = songs.Select(s => s.Id).ToList();

            //get all author tag ids
            parentIds = songs.Select(t => t.ParentId).Distinct().ToList();
            
            //get all subgenre tag ids
            List<long> genreIds = userTags.Keys.Where(t => parentIds.Contains(t.Id)).Select(t => t.ParentId.Value).ToList();

            //get real user* neighbor probabilites for all songs
            var GlobalProbSongs = (from t in _db.Tags
                         where songIds.Contains(t.Id)
                         group t by t.ParentId into tags
                         from t in _db.Tags
                         where t.ParentId == tags.Key
                         select new {tag = t.ParentId, popularity = (double) 1 + tags.Sum(tag => tag.Popularity)}).Distinct().ToDictionary(kv => kv.tag, kv => kv.popularity);

            foreach (Tag t in userTags.Keys.ToList()) {
                if ( t.ParentId != null  && GlobalProbSongs.Keys.Contains(t.ParentId))
                    userTags[t] *= (double)t.Popularity / GlobalProbSongs[t.ParentId];
            }

            //get new songs with higher global neighbor probabilities
            var newSongs = (
                         from t in _db.Tags
                         where parentIds.Contains(t.ParentId)
                         group t by t.ParentId
                         into tags
                         from t in _db.Tags
                         where t.ParentId == tags.Key && !songIds.Contains(t.Id)
                         select new {tag = t, popularity = (double) t.Popularity / (1 + tags.Sum(tag => tag.Popularity))}).ToDictionary(kv => kv.tag, kv => kv.popularity, new TagComparer());

            Dictionary<Tag, Dictionary<Tag, double>> authorSongs = new Dictionary<Tag, Dictionary<Tag, double>>(new TagComparer());
          
            foreach (KeyValuePair<Tag, double> song in userTags) {
                if (songIds.Contains(song.Key.Id))
                {
                    if (!authorSongs.Keys.Contains(song.Key.ParentTag))
                        authorSongs[song.Key.ParentTag] = new Dictionary<Tag, double>();
                    authorSongs[song.Key.ParentTag].Add(song.Key, song.Value);
                }                
            }

            foreach (Tag newSong in newSongs.Keys.ToList())
            {
                newSongs[newSong] *= 1.0 / (1.0 + authorSongs[newSong.ParentTag].Sum(kv => kv.Key.Popularity));
                if (newSongs[newSong] >= authorSongs[newSong.ParentTag].Values.Max())
                {
                    authorSongs[newSong.ParentTag].Add(newSong, newSongs[newSong]);
                    userTags.Add(newSong, newSongs[newSong]);
                    songIds.Add(newSong.Id);
                }
            }

            //get real user* neighbor probabilites for all author tags
            var realNeighborProbAuthors = (from t in _db.Tags
                         where parentIds.Contains(t.Id)
                         group t by t.ParentId into tags
                         from t in _db.Tags
                         where t.ParentId == tags.Key
                         select new {tag = t.ParentId, popularity = (double) 1 + tags.Sum(tag => tag.Popularity)}).Distinct().ToDictionary(kv => kv.tag, kv => kv.popularity);
            
            foreach (Tag author in userTags.Keys.ToList())
            {
                if(parentIds.Contains(author.Id))
                    userTags[author] *= (double)author.Popularity/realNeighborProbAuthors[author.ParentId];
            }


            //get new authors with higher global neighbor probabilities
            var newAuthors = (
                         from t in _db.Tags
                         where genreIds.Contains(t.ParentId.Value)
                         group t by t.ParentId
                             into tags
                             from t in _db.Tags
                             where t.ParentId == tags.Key && !parentIds.Contains(t.Id)
                             select new { tag = t, popularity = (double)t.Popularity / (1 + tags.Sum(tag => tag.Popularity))}).ToDictionary(kv => kv.tag, kv => kv.popularity, new TagComparer());

            Dictionary<Tag, Dictionary<Tag, double>> authorTags = new Dictionary<Tag, Dictionary<Tag, double>>(new TagComparer());
            foreach (KeyValuePair<Tag, double> author in userTags)
            {
                if (parentIds.Contains(author.Key.Id))
                {
                    if (!authorTags.Keys.Contains(author.Key.ParentTag) && author.Key.ParentId != null)
                        authorTags[author.Key.ParentTag] = new Dictionary<Tag, double>();
                    authorTags[author.Key.ParentTag].Add(author.Key, author.Value);
                }
            }

            foreach (Tag newAuthor in newAuthors.Keys.ToList())
            {
                newAuthors[newAuthor] *= 1.0 / (1.0 + authorTags[newAuthor.ParentTag].Sum(kv => kv.Key.Popularity));
                if (newAuthors[newAuthor] >= authorTags[newAuthor.ParentTag].Values.Max())
                {
                    authorTags[newAuthor.ParentTag].Add(newAuthor, newAuthors[newAuthor]);
                    userTags.Add(newAuthor, newAuthors[newAuthor]);
                    parentIds.Add(newAuthor.Id);
                }
            }
            
            //getting real user probabilities for all tags
            List<Tag> genres = userTags.Keys.Where(t => genreIds.Contains(t.Id)).ToList();

            foreach(Tag t in genres){
                if(t.ParentId != null && userTags.Keys.Contains(t.ParentTag))
                userTags[t] *= userTags[t.ParentTag];
            }

            foreach (Tag t in userTags.Keys.ToList())
            {
                if(parentIds.Contains(t.Id) && t.ParentId != null)
                userTags[t] *= userTags[t.ParentTag];
            }

            foreach (Tag t in userTags.Keys.ToList())
            {
                if (songIds.Contains(t.Id) && t.ParentId != null)
                userTags[t] *= userTags[t.ParentTag];
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
            List<HeartedSongVM> sorted = new List<HeartedSongVM>();
            foreach (var id in _songs_ids)
            {
                var song_with_this_id = hearted.Where(h => h.SongId == id).FirstOrDefault();

                if (song_with_this_id == null)
                    throw new Exception("Ne bi trebalo");

                sorted.Add(song_with_this_id);
            }
            return hearted;
        }

        public void ReduceSubTree(string userId, long tagId) {

            long? id = (from t in _db.TagUsers where t.TagId == tagId && t.UserId == userId select t.ParentTagId).FirstOrDefault();
            List<TagUser> tags = (from tu in _db.TagUsers
                        where tu.UserId == userId && tu.ParentTagId == id
                        select tu).ToList();
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
                return Convert.ToInt32(obj.Id);
            }
        }


    }
}