using MusicPortal.WebAPI.Binding_Models;
using MusicPortal.WebAPI.Data;
using MusicPortal.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.BL {
    public class UserManager {

        private MusicPortalDbContext _db;

        public UserManager() {
            _db = new MusicPortalDbContext();
        }

        public List<UserVM> GetAll() {
            List<ApplicationUser> allUsers = _db.Users.ToList();

            List<UserVM> allUserModels = new List<UserVM>();
            foreach (var user in allUsers) {
                allUserModels.Add(new UserVM(user));
            }

            return allUserModels;
        }

        public UserVM GetById(string id) {
            return new UserVM(_db.Users.Where(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase)).SingleOrDefault());
        }

        public List<SongVM> GetUserHeartedSongs(string id) {

            return null;
        }

    }
}