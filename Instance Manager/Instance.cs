using ArtCore_Editor.Assets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArtCore_Editor
{
    public partial class GameProject
    {
        [JsonObject(MemberSerialization.OptIn)]
        public class Instance : Asset
        {
            [JsonProperty]
            public Sprite Sprite { get; set; }
            // TODO
            // dataset
            [JsonProperty]
            public Dictionary<Event.EventType, string> Events { get; set; }
            [JsonProperty]
            public List<Varible> Varible { get; set; }
            public Instance()
            {
                Varible = new List<Varible>();
                Events = new Dictionary<Event.EventType, string>();
                Name = "new_instance";
            }

        }

    }
}
