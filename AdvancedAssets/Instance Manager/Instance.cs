using System.Collections.Generic;
using ArtCore_Editor.AdvancedAssets.SpriteManager;
using ArtCore_Editor.Assets;
using ArtCore_Editor.Enums;
using Newtonsoft.Json;

namespace ArtCore_Editor.AdvancedAssets.Instance_Manager;

[JsonObject(MemberSerialization.OptIn)]
public class Instance : Asset
{
    public class BodyData
    {
        public enum BType
        {
            None,
            Sprite,
            Rect,
            Circle
        };

        public BType Type;
        public int Value;

        public BodyData()
        {
            Type = BType.None;
            Value = 0;
        }
    }

    [JsonProperty] public BodyData BodyDataType { get; set; }

    [JsonProperty] public Sprite Sprite { get; set; }

    // TODO
    // dataset
    [JsonProperty] public Dictionary<Event.EventType, string> Events { get; set; }
    [JsonProperty] public List<Variable> Variables { get; set; }

    public Instance()
    {
        Variables = new List<Variable>();
        Events = new Dictionary<Event.EventType, string>();
        Name = "new_instance";
        BodyDataType = new BodyData();
    }

}
