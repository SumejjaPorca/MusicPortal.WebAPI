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

    }
}