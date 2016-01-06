using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using MusicPortal.WebAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPortal.WebAPI.Domain_Models
{
    public class Playlist
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }

        public string OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public ApplicationUser Owner { get; set; }

        public virtual Collection<PlaylistSong> Songs { get; set; }
    }
}