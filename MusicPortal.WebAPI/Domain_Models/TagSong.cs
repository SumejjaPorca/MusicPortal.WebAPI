using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Domain_Models
{
    public class TagSong
    {
        [Key]
        public string Id { get; set; }
        public int SongId { get; set; }

        [ForeignKey("SongId")]
        public Song Song { get; set; }
        public int TagId { get; set; }

        [ForeignKey("TagId")]
        public Tag Tag { get; set; }
    }
}