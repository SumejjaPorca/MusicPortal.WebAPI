﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPortal.WebAPI.Binding_Models
{
    public class PlaylistVM
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }


        [JsonProperty(PropertyName = "songs")]
        public IEnumerable<HeartedSongVM> Songs { get; set; }
    }
}
