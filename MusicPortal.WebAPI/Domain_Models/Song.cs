using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace MusicPortal.WebAPI.Domain_Models
{
    public class Song
    {
        [Key]
        public long Id { get; set; }
        [DisplayName("Name of the song")]
        public string Name { get; set; }
        public string Link { get; set; }

        public virtual Collection<Tag> Tags { get; set; }
    }
}