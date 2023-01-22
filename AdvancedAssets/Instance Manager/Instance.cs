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
        public int Value1;
        public int Value2;

        public BodyData()
        {
            Type = BType.None;
            Value1 = 0;
            Value2 = 0;
        }
    }

    [JsonProperty] public BodyData BodyDataType { get; set; }

    //TODO change Sprite type to string type, for better saving system
    [JsonProperty] public Sprite Sprite { get; set; }
    
    [JsonProperty] public Dictionary<Event.EventType, string> Events { get; set; }
    [JsonProperty] public List<Variable> Variables { get; set; }

    [JsonProperty] public bool EditorShowInScene { get; set; }
    [JsonProperty] public bool EditorShowInLevel { get; set; }

    public Instance()
    {
        Variables = new List<Variable>();
        Events = new Dictionary<Event.EventType, string>();
        Name = "new_instance";
        BodyDataType = new BodyData();
        EditorShowInScene = true;
        EditorShowInLevel = true;
    }

}
