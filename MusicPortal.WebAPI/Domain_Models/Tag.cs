using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace MusicPortal.WebAPI.Domain_Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("TagName")]
        public String Name { get; set; }
        public virtual Collection<Song> Songs { get; set; }
    }
}