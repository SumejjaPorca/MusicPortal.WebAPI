using MusicPortal.WebAPI.Binding_Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MusicPortal.WebAPI.Domain_Models;

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

        public SongVM CreateSong(SongVM song) {
            Song createdSong = new Song() { Name = song.Name };
            createdSong = _db.Songs.Add(createdSong);
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
            List<Song> songsByTagName = 
                _db.Songs.Include("Tags")
                .Where(x => x.Tags.Where(y => y.Name == tagName).Count() >= 1).ToList();
            
            List<SongVM> songModelsByTagName = new List<SongVM>();
            foreach (var song in songsByTagName) {
                songModelsByTagName.Add(new SongVM(song));
            }

            return songModelsByTagName;
        }

        public SongVM UpdateSong(SongVM song) {
            Song updatedSong = _db.Songs.Where(x => x.Id == song.Id).FirstOrDefault();

            updatedSong.Name = song.Name;
            updatedSong.Link = song.Link;

            _db.SaveChanges();

            return new SongVM(updatedSong);
        }

        public SongVM DeleteSong(long songId) {
            Song removedSong = _db.Songs.Remove(_db.Songs.Where(x => x.Id == songId).FirstOrDefault());

            _db.SaveChanges();

            return new SongVM(removedSong);
        } 

    }
}
