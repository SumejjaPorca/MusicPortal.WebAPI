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
        public long Id { get; set; }
        [DisplayName("TagName")]
        public String Name { get; set; }

        public virtual Collection<TagSong> Songs { get; set; }
        public virtual Collection<TagUser> Users { get; set; }
        public virtual Collection<Tag> ChildrenTags { get; set; }
        [Index("Parent", 1)]
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Tag ParentTag { get; set; }

        public int Popularity { get; set; }
        public override bool Equals(Object t)
        {
            if (t == null)
                return false;
            Tag tag = t as Tag;
            if ((System.Object) tag == null)
                return false;
            return tag.Id == Id;
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(Id);
        }
    }
}