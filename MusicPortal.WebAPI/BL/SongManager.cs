using MusicPortal.WebAPI.Binding_Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MusicPortal.WebAPI.Domain_Models;
using System.Data.Entity;
using System.Data.SqlClient;

namespace MusicPortal.WebAPI.BL
{
    public class SongManager
    {
        private Data.MusicPortalDbContext _db;

        public SongManager(Data.MusicPortalDbContext db)
        {
            this._db = db;
        }

        public IQueryable<HeartedSongVM> MakeHeartedSong(IQueryable<Song> songs)
        {

            var songs_author = (from song in songs
                                join sa in _db.AuthorSongs on song.Id equals sa.SongId
                                join authors in _db.Authors on sa.AuthorId equals authors.Id
                                group authors by song into row
                                select new { Song = row.Key, Authors = row.Select(a => a.Name).ToList() });
            var luka = songs_author.ToList();
            //TODO
            return songs_author.Select(sa => new HeartedSongVM
            {
                SongId = sa.Song.Id,
                Name = sa.Song.Name,
                Link = sa.Song.Link,
                IsHearted = false, //TODO
                Authors = sa.Authors

            });
                     
            

        }

        public Task<List<SongVM> > GetAllAsync() {
            return Task.Run(() =>
            {
                return _db.Songs.Select(s => new SongVM
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
            });

        }

        public SongVM GetById(long id) {
            Song songById = _db.Songs.Where(x => x.Id == id).FirstOrDefault();

            return new SongVM(songById);
        }

        public SongVM CreateSong(NewSongVM song) {
            //TODO: handle many authors separeted with coma case
            //List<int> authorIds = _db.AuthorSongs.Where(authorsong => authorsong.SongId == songId).Select(author => author.AuthorId).ToList();
            //List<String> authors = _db.Authors.Where(a => authorIds.Contains(a.Id)).Select(a => a.Name).ToList();
            //Tag genre = tags.Where(t => t.Name != song.Name && !authors.Contains(t.Name)).FirstOrDefault();

            Author author = _db.Authors.FirstOrDefault(a => a.Name == song.AuthorName);
            if (author == null)
                _db.Authors.Add(new Author
                {
                    Name = song.AuthorName
                });
            Song createdSong = new Song() { 
                Name = song.Name,
                Link = song.Link
            };
            _db.Songs.Add(createdSong);
            AuthorSong authorSong = new AuthorSong { 
                AuthorId = author.Id,
                SongId = createdSong.Id
            };
            _db.AuthorSongs.Add(authorSong);

            //New tag managing
            Tag genreTag = _db.Tags.FirstOrDefault(t => t.Id == song.GenreTagId);
            if (genreTag == null)
                //okay, this should never EVER happen
                throw new Exception("That genre doesn't exist in our tag database anymore.");
            Tag authorTag = _db.Tags.FirstOrDefault(t => t.Name == song.AuthorName && t.ParentId == genreTag.Id);
            if (authorTag == null)
                //we need to add another tag for this author for this genre for better search results
                _db.Tags.Add(new Tag {
                    Name = song.AuthorName,
                    ParentId = genreTag.Id,
                    Popularity = 0
                });
            Tag songTag = _db.Tags.FirstOrDefault(t => t.Name == song.Name && t.ParentId == authorTag.Id);
            if (songTag == null)
                //we need to add another tag for this song name that is from author called AuthorName in specific genre of music
                _db.Tags.Add(new Tag {
                    Name = song.Name,
                    ParentId = authorTag.Id,
                    Popularity = 0
                });
            _db.SaveChanges();
                
            return new SongVM(createdSong);
        }

		public List<HeartedSongVM> GetFuzzy(string songName){
            
            //Levenshteina smo prebacili u bazu. Posto ne znam nacin da pozovem iz EF LINQ-a funkciju s baze ovo je najbolje rjesenje
            var query = "SELECT[Id] FROM[dbo].[Songs] ORDER BY[dbo].[Levenshtein]([Name],  @q, 30) / CAST(LEN([Name]) AS decimal) OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";
            var best_ids = _db.Database.SqlQuery<long>(query, new SqlParameter("@q", songName)).ToList();
            var best_songs = _db.Songs.Where(so => best_ids.Contains(so.Id));

            return MakeHeartedSong(best_songs).ToList().OrderBy(so => best_ids.FindIndex(bo => bo == so.SongId)).ToList();
        }

		


        public List<SongVM> GetByTagName(string tagName) {
            List<SongVM> songModelsByTagName =
                _db.Songs.Include(song => song.Tags)
                //TODO prepravljao ovo... moguce da ce pasti jer nije includan Tags.Song
                .Where(x => x.Tags.Where(y => y.Song.Name == tagName).Count() >= 1).Select<Song, SongVM>(x => new SongVM(x)).ToList();
            
            return songModelsByTagName;
        }

        public void HeartSong(long songId, string userId) {
            Song song = _db.Songs.Where(x => x.Id == songId).FirstOrDefault();
            if (song == null)
                throw new Exception("Song with provided ID does not exist in the database.");
            HeartedSong heartedSong = _db.HeartedSongs.Where(x => x.Id == songId && x.UserId == userId).FirstOrDefault();
            if (heartedSong == null)
                _db.HeartedSongs.Add(new HeartedSong {
                    IsHearted = true,
                    SongId = songId,
                    UserId = userId
                });
            else
                heartedSong.IsHearted = !heartedSong.IsHearted;
            //TODO: change tag popularities according to this! 
            
            _db.SaveChanges();

        }

        public SongVM DeleteSong(long songId) {
            Song removedSong = _db.Songs.Remove(_db.Songs.Where(x => x.Id == songId).FirstOrDefault());

            _db.SaveChanges();

            return new SongVM(removedSong);
        }

        public void PlaySong(int songId, string userId) {
            if (userId != null)
            {
                //TODO: scale popularities and/or use Gaussian 
                Song song = _db.Songs.Where(s => s.Id == songId).FirstOrDefault();
                if (song == null)
                    throw new Exception("Song with provided id does not exist in our database!");

                List<long> tagIds = _db.TagSongs.Where(t => t.SongId == songId).Select(t => t.Id).ToList();
                List<Tag> tags = _db.Tags.Where(t => tagIds.Contains(t.Id)).ToList();
                              
                foreach (Tag t in tags)
                {
                    TagUser tagUser = _db.TagUsers.FirstOrDefault(tu => tu.TagId == t.Id && tu.UserId == userId);
                    if (tagUser != null)
                        tagUser.Popularity = tagUser.Popularity + 1;
                    else
                        _db.TagUsers.Add(new TagUser
                            {
                                Popularity = 1,
                                TagId = t.Id,
                                UserId = userId,
                                ParentTagId = t.ParentId
                            });
                        
                    t.Popularity = t.Popularity + 1;
                }
                _db.SaveChanges();
            }
            }

    }
}
