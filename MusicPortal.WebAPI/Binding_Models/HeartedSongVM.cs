using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MusicPortal.WebAPI.Binding_Models
{
    public class HeartedSongVM
    {
        //public string UserId { get; set; }
        [JsonProperty(PropertyName = "id")]
        public long SongId { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "hearted")]
        public bool IsHearted { get; set; }   
        [JsonProperty(PropertyName = "link")]   
        public string Link { get; set; }
        [JsonProperty(PropertyName = "authors")]
        public IEnumerable<AuthorVM> Authors { get; set; } //TO DO: get authors name when returning this view model

    }

}