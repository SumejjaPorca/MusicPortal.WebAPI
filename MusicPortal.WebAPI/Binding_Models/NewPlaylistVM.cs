using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPortal.WebAPI.Binding_Models
{
    public class NewPlaylistVM
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}
