using ArtCore_Editor.Assets;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArtCore_Editor
{
    public partial class GameProject
    {
        [JsonObject(MemberSerialization.OptIn)]
        public class Instance : Asset
        {
            public class Body
            {
                public enum type
                {
                    NONE,SPRITE,RECT,CIRCLE
                };
                public type Type;
                public int Value;
                public Body()
                {
                    Type = type.NONE;
                    Value = 0;
                }
            }
            [JsonProperty]
            public Body BodyType { get; set; }
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
                BodyType = new Body();
            }

        }

    }
}
