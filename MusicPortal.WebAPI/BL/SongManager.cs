using MusicPortal.WebAPI.Binding_Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace MusicPortal.WebAPI.BL
{
    public class SongManager
    {
        private Data.MusicPortalDbContext _db;

        public SongManager(Data.MusicPortalDbContext db)
        {
            this._db = db;
        }

        public Task<List<SongVM> > GetAllAsync(){
            return Task.Run(() =>
            {
                return _db.Songs.Select(s => new SongVM
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
            });

        }

		public List<SongVM> GetFuzzy(string songName){
			// third param is the fuzzyness so change it in order to adjust
			// 0 means crisp, 1 totally fuzzy
            return this.FuzzySearch(songName, db.Songs.ToList(), 0.5);
        }

		private static List<string> FuzzySearch(
			string word,
			List<SongVM> songList,
			double fuzzyness)
		{
			List<string> foundSongs = new List<SongVM>();

			foreach (SongVM song in songList)
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
    }
}
