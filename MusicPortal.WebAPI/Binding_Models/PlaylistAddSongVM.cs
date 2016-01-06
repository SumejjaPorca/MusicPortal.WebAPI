using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPortal.WebAPI.Binding_Models
{
    public class PlaylistAddSongVM
    {
        [JsonProperty(PropertyName = "playlist_id")]
        public long PlaylistId { get; set; }

        [JsonProperty(PropertyName = "song_id")]
        public long SongId { get; set; }

    }
}
