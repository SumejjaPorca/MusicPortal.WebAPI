using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using MusicPortal.WebAPI.Models;

namespace MusicPortal.WebAPI.Domain_Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("TagName")]
        public String Name { get; set; }
        public virtual Collection<Song> Songs { get; set; }
        public virtual Collection<ApplicationUser> Users { get; set; }
        public int ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Tag ParentTag { get; set; }
        public int Popularity { get; set; }
    }
}