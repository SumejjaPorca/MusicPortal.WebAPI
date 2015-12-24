using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.BL
{
    public class AIManager
    {
        
        private MusicPortalDbContext _db;

        public AIManager() {
            _db = new MusicPortalDbContext();
        }

        public List<SongVM> GetFlow(string userId) {
            //get preferred tags 
            //extract songs from those tags
            throw new NotImplementedException();
        }

    }
}