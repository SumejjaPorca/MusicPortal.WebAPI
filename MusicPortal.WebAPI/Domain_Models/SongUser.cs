using MusicPortal.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Domain_Models
{
    public class SongUser
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [ForeignKey("SongId")]
        public Song Song { get; set; }
        public string SongId { get; set; }

        public DateTime Date { get; set; }

        public bool IsHearted { get; set; }
    }
}