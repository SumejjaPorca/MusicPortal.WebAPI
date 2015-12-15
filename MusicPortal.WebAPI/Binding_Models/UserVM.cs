using MusicPortal.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Binding_Models {
    public class UserVM {

        public string Id { get; set; }

        public string UserName { get; set; }

        public UserVM(ApplicationUser user) {
            this.Id = user.Id;
            this.UserName = user.UserName;
        }

    }
}