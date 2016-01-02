using MusicPortal.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Domain_Models
{
    public class HeartedSong
    {
        [Key]
        public long Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public long SongId { get; set; }
        [ForeignKey("SongId")]
        public virtual Song Song { get; set; }

        public bool IsHearted { get; set; }
    }
}