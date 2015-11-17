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
    }
}