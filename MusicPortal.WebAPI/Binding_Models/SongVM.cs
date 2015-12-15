using MusicPortal.WebAPI.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Binding_Models
{
    public class SongVM {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Link { get; set; }

        public SongVM() {

        }

        public SongVM(Song song) {
            this.Id = song.Id;
            this.Name = song.Name;
        }

    }
}