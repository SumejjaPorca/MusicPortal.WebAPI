using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MusicPortal.WebAPI.Domain_Models
{
    public class Playlist
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public virtual Collection<PlaylistSong> Songs { get; set; }
    }
}