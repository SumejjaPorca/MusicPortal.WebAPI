using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Collections.ObjectModel;

namespace MusicPortal.WebAPI.Domain_Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public virtual Collection<Song> Songs { get; set; }
    }
}