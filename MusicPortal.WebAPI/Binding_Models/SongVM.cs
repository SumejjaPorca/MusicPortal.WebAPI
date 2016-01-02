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
        public string AuthorNames { get; set; } //TO DO: get authors name when returning this view model

        public SongVM() {

        }

        public SongVM(Song song) {
            this.Id = song.Id;
            this.Name = song.Name;
        }

    }
}