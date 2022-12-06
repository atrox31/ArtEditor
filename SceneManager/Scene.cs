using ArtCore_Editor.Assets;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ArtCore_Editor
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Scene : Asset
    {
        [JsonIgnore] public List<SceneManager.SceneInstance> SceneInstances;
        [JsonProperty] public List<string> SceneInstancesList = new List<string>();

        [JsonProperty] public string GuiFile;

        public class Region
        {
            public int X1 { get; set; }
            public int Y1 { get; set; }
            public int X2 { get; set; }
            public int Y2 { get; set; }

            public Region(int x1, int y1, int x2, int y2)
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }

            public override string ToString()
            {
                return X1.ToString() + "|" + Y1.ToString() + "|" + X2.ToString() + "|" + Y2.ToString();
            }
        }

        [JsonProperty] public int Width;
        [JsonProperty] public int Height;
        [JsonProperty] public Color BackGroundColor;
        [JsonProperty] public string BackGroundTexture;
        [JsonProperty] public string BackGroundTextureName;

        public enum BackGroundTypeEnum
        {
            DrawColor,
            DrawTexture
        };

        [JsonProperty] public BackGroundTypeEnum BackGroundType;
        [JsonProperty] public WrapMode BackGroundWrapMode;
        [JsonProperty] public List<Region> Regions { get; set; } = new List<Region>();

        public Scene()
        {
            GuiFile = "";
            SceneInstances = new List<SceneManager.SceneInstance>();
            SceneTriggers = new List<string>();
            Name = "new_scene";
            BackGroundColor = Functions.HexToColor("#E7F6F2");
            BackGroundTexture = null;
            BackGroundTextureName = null;
            BackGroundType = BackGroundTypeEnum.DrawColor;
            BackGroundWrapMode = WrapMode.Tile;
        }

        [JsonProperty] public List<string> SceneTriggers { get; set; }
    }


}
