﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Domain_Models
{
    public class Author
    {
        [Key]
        public string Id { get; set; }
        [DisplayName("Name of the author")]
        public string Name { get; set; }

        public virtual Collection<Song> Songs { get; set; }

    }
}