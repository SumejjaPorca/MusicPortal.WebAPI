using MusicPortal.WebAPI.Binding_Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MusicPortal.WebAPI.Domain_Models;
using System.Data.Entity;

namespace MusicPortal.WebAPI.BL
{
    public class SongManager
    {
        private Data.MusicPortalDbContext _db;

        public SongManager(Data.MusicPortalDbContext db)
        {
            this._db = db;
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

		public List<SongVM> GetFuzzy(string songName){
			// third param is the fuzzyness so change it in order to adjust
			// 0 means crisp, 1 totally fuzzy
            List<Song> songs = FuzzySearch(songName, _db.Songs.ToList(), 0.5);

            List<SongVM> songModels = new List<SongVM>();
            foreach (var song in songs) {
                songModels.Add(new SongVM(song));
            }

            return songModels;
        }

		private List<Domain_Models.Song> FuzzySearch(
			string word,
			List<Song> songList,
			double fuzzyness)
		{
			List<Domain_Models.Song> foundSongs = new List<Domain_Models.Song>();

			foreach (Domain_Models.Song song in songList)
			{
				string s = song.Name;
				// Calculate the Levenshtein-distance:
				int levenshteinDistance =
				    LevenshteinDistance(word, s);

				// Length of the longer string:
				int length = Math.Max(word.Length, s.Length);

				// Calculate the score:
				double score = 1.0 - (double)levenshteinDistance / length;

				// Match?
				if (score > fuzzyness)
				    foundSongs.Add(song);
			}
			return foundSongs;
		}

		private  static int LevenshteinDistance(string src, string dest) {
			int[,] d = new int[src.Length + 1, dest.Length + 1];
			int i, j, cost;
			char[] str1 = src.ToCharArray();
			char[] str2 = dest.ToCharArray();

			for (i = 0; i <= str1.Length; i++)
			{
				d[i, 0] = i;
			}
			for (j = 0; j <= str2.Length; j++)
			{
				d[0, j] = j;
			}
			for (i = 1; i <= str1.Length; i++)
			{
				for (j = 1; j <= str2.Length; j++)
				{

				    if (str1[i - 1] == str2[j - 1])
				        cost = 0;
				    else
				        cost = 1;

				    d[i, j] =
				        Math.Min(
				            d[i - 1, j] + 1,              // Deletion
				            Math.Min(
				                d[i, j - 1] + 1,          // Insertion
				                d[i - 1, j - 1] + cost)); // Substitution

				    if ((i > 1) && (j > 1) && (str1[i - 1] == 
				        str2[j - 2]) && (str1[i - 2] == str2[j - 1]))
				    {
				        d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
				    }
				}
			}

			return d[str1.Length, str2.Length];
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
