using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Domain_Models
{
    public class AuthorSong
    {
        [Key]
        public int Id { get; set; }
        public int SongId { get; set; }

        [ForeignKey("SongId")]
        public Song Song { get; set; }
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
    }
}