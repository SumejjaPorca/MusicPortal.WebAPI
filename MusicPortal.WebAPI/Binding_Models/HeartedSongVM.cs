using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Binding_Models
{
    public class HeartedSongVM
    {
        //public string UserId { get; set; }
        public long SongId { get; set; }
        public string Name { get; set; }
        public bool IsHearted { get; set; }      
        public string Link { get; set; }
        public IEnumerable<string> Authors { get; set; } //TO DO: get authors name when returning this view model

    }
}