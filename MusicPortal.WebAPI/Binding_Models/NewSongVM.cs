using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Binding_Models
{
    public class NewSongVM
    {
        public string Name { get; set; }

        public string Link { get; set; }
        public long GenreTagId { get; set; }
        public string AuthorName { get; set; }
    }
}