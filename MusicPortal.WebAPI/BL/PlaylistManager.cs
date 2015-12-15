using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.Data;
using MusicPortal.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.BL
{
    public class PlaylistManager
    {

        private MusicPortalDbContext _db;

        public PlaylistManager() {
            _db = new MusicPortalDbContext();
        }

    }
}