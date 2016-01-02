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
        public long Id { get; set; }

        public long SongId { get; set; }
        [ForeignKey("SongId")]
        public virtual Song Song { get; set; }

        public long TagId { get; set; }
        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }
    }
}