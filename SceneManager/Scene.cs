using ArtCore_Editor.Assets;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ArtCore_Editor
{
    public partial class GameProject
    {
        [JsonObject(MemberSerialization.OptIn)]
        public class Scene : Asset
        {
            static Dictionary<string, Image> InstanceSprites = new Dictionary<string, Image>();
            public class SceneInstance
            {
                public int x { get; set; }
                public int y { get; set; }
                public float editorMask { get; set; }
                public Instance instance { get; set; }
                [JsonIgnore]
                public Image img;
                public SceneInstance(int x, int y, GameProject.Instance instance)
                {
                    this.x = x;
                    this.y = y;
                    this.instance = instance;

                    if (instance == null)
                    {
                        img = Properties.Resources.interrogation1;
                    }
                    else
                    {
                        if (instance.Sprite == null)
                        {
                            img = Properties.Resources.interrogation;
                        }
                        else
                        {
                            if (!InstanceSprites.ContainsKey(instance.Sprite.FileName))
                            {
                                Image tmp = Image.FromFile(GameProject.ProjectPath + "\\" + instance.Sprite.ProjectPath + "\\img\\0.png");
                               InstanceSprites.Add(instance.Sprite.FileName, tmp);
                            }
                            img = InstanceSprites[instance.Sprite.FileName];
                        }
                    }

                    editorMask = (img.Width + img.Height) / 4;
                }
            }
            [JsonProperty]
            public List<SceneInstance> SceneInstances = new List<SceneInstance>();

            [JsonProperty]
            public string GuiFile;

            public class Region
            {
                public int x1 { get; set; }
                public int y1 { get; set; }
                public int x2 { get; set; }
                public int y2 { get; set; }
                public Region(int x1, int y1, int x2, int y2)
                {
                    this.x1 = x1;
                    this.y1 = y1;
                    this.x2 = x2;
                    this.y2 = y2;
                }
            }
            [JsonProperty]
            public int Width;
            [JsonProperty]
            public int Height;
            [JsonProperty]
            public Color BackGroundColor;
            [JsonProperty]
            public string BackGroundTexture;
            [JsonProperty]
            public string BackGroundTexture_name;
            public enum BackGroundTypeEnum
            {
                DrawColor,DrawTexture
            };
            [JsonProperty]
            public BackGroundTypeEnum BackGroundType;
            [JsonProperty]
            public WrapMode BackGroundWrapMode;
            [JsonProperty]
            public List<Region> regions { get; set; } = new List<Region>();
            public Scene()
            {
                GuiFile = "";
                SceneInstances = new List<SceneInstance>();
                SceneTriggers = new List<string>();
                Name = "new_scene";
                BackGroundColor = Functions.HexToColor("#E7F6F2");
                BackGroundTexture = null;
                BackGroundTexture_name = null;
                BackGroundType = BackGroundTypeEnum.DrawColor;
                BackGroundWrapMode = WrapMode.Tile;
            }
            [JsonProperty]
            public List<string> SceneTriggers { get; set; }
        }

    }
}
