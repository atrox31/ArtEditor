using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using ArtCore_Editor.Assets;
using ArtCore_Editor.Enums;
using ArtCore_Editor.Functions;
using Newtonsoft.Json;

namespace ArtCore_Editor.AdvancedAssets.SceneManager
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Scene : Asset
    {
        [JsonIgnore] public List<SceneManager.SceneInstance> SceneInstances;
        [JsonProperty] public List<string> SceneInstancesList = new List<string>();
        public class Region
        {
            private int X1 { get; set; }
            private int Y1 { get; set; }
            private int X2 { get; set; }
            private int Y2 { get; set; }

            public Region(int x1, int y1, int x2, int y2)
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }

            public override string ToString()
            {
                return X1 + "|" + Y1 + "|" + X2 + "|" + Y2;
            }
        }

        // base view dimensions
        // from this value view will be scaled to 
        // user selected resolution
        // this make inpact for instances coords
        [JsonProperty] public int ViewWidth;
        [JsonProperty] public int ViewHeight;
        [JsonProperty] public bool EnableCamera;

        [JsonProperty] public Color BackGroundColor;
        [JsonProperty] public Asset BackGroundTexture;
        [JsonProperty] public string BackGroundTextureName;

        [JsonProperty] public string SceneStartingTrigger;
        [JsonProperty] public List<Variable> SceneVariables = new List<Variable>();

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
            SceneInstances = new List<SceneManager.SceneInstance>();
            SceneTriggers = new List<string>();
            Name = "new_scene";
            BackGroundColor = "#E7F6F2".HexToColor();
            BackGroundTexture = null;
            BackGroundTextureName = null;
            EnableCamera = false;
            ViewWidth = 1;
            ViewHeight = 1;
            BackGroundType = BackGroundTypeEnum.DrawColor;
            BackGroundWrapMode = WrapMode.Tile;
        }

        [JsonProperty] public List<string> SceneTriggers { get; set; }
    }


}
