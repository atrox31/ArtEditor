using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ArtCore_Editor.Assets;
using ArtCore_Editor.etc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArtCore_Editor.AdvancedAssets.SpriteManager
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Sprite : Asset
    {
        // enums
        public enum CollisionMaskEnum
        {
            None, Circle, Rectangle//, PerPixel
        }
        public enum SpriteCenterEnum
        {
            Center, LeftCorner, Custom
        }

        // sprite properties
        [JsonProperty][JsonConverter(typeof(StringEnumConverter))]
        public CollisionMaskEnum CollisionMask;

        [JsonProperty] public int CollisionMaskValue;

        [JsonProperty] public SpriteCenterEnum SpriteCenter;

        [JsonProperty] public int SpriteCenterX;
        [JsonProperty] public int SpriteCenterY;

        [JsonProperty] public int SpriteWidth;
        [JsonProperty] public int SpriteHeight;
        [JsonProperty] public Dictionary<string, AnimationSequence> SpriteAnimationSequence;

        // editor value
        [JsonProperty] public bool EditorShowMask;
        [JsonProperty] public bool EditorShowCenter;
        [JsonProperty] public bool EditorPreviewPlay;
        [JsonProperty] public bool EditorPreviewLoop;
        [JsonProperty] public int EditorFps;
        [JsonProperty] public int EditorFrame;
        [JsonProperty] public int EditorFrameMax;
        [JsonIgnore]   public List<Image> Textures;
        [JsonProperty] public int TexturesCount;

        [JsonObject(MemberSerialization.OptIn)]
        public class AnimationSequence
        {
            [JsonProperty] public string Name;
            [JsonProperty] public readonly string Index;
            [JsonProperty] public int FrameFrom;
            [JsonProperty] public int FrameTo;
            public AnimationSequence(string data)
            {
                string[] tmp = data.Split('|');
                Name = tmp[0];
                Index = tmp[1];
                FrameFrom = Convert.ToInt32(tmp[2]);
                FrameTo = Convert.ToInt32(tmp[3]);
            }
            public AnimationSequence(string name, string index, int frameFrom, int frameTo)
            {
                Name = name;
                Index = index;
                FrameFrom = frameFrom;
                FrameTo = frameTo;
            }
            public string Get()
            {
                return Name + '|' + Index + '|' + FrameFrom + '|' + FrameTo;
            }

        }

        public Sprite()
        {
            SpriteAnimationSequence = new Dictionary<string, AnimationSequence>();
            CollisionMask = CollisionMaskEnum.None;
            CollisionMaskValue = 0;
            SpriteCenter = SpriteCenterEnum.LeftCorner;
            SpriteCenterX = 0;
            SpriteCenterY = 0;
            SpriteWidth = 0;
            SpriteHeight = 0;
            TexturesCount = 0;

            EditorFps = 60;

            EditorPreviewLoop = false;
            EditorPreviewPlay = false;
            EditorShowCenter = false;
            EditorShowMask = false;
            Textures = new List<Image>();
        }

        public void AddImage(string file)
        {
            byte[] content = File.ReadAllBytes(file);
            Textures.Add(Image.FromStream(new MemoryStream(content)));

            // set size of sprite to match rest of frames
            if (Textures[^1].Width > SpriteWidth ||
                Textures[^1].Height > SpriteHeight)
            {
                SpriteWidth = Textures[^1].Width;
                SpriteHeight = Textures[^1].Height;
            }

            EditorFrameMax = Textures.Count - 1;
            TexturesCount = Textures.Count;
        }

        public void ClearImages()
        {
            if (Textures == null) return;
            foreach (Image t in Textures)
            {
                t.Dispose();
            }
            Textures.Clear();
        }

        public void Save()
        {
            LoadScreen load = new LoadScreen(true);
            load.Show();
            {
                if (Directory.Exists(GameProject.ProjectPath + ProjectPath) && ProjectPath.Length > 0)
                {
                    Directory.Delete(GameProject.ProjectPath + ProjectPath, true);
                }

                Directory.CreateDirectory(GameProject.ProjectPath + ProjectPath + "\\img");

                using FileStream createStream =
                    File.Create(GameProject.ProjectPath + ProjectPath + "\\" + FileName);

                byte[] buffer = JsonConvert.SerializeObject(this).Select(c => (byte)c).ToArray();
                createStream.Write(buffer);

                if (Textures.Count <= 0) return;
                int i = 0;
                foreach (Image texture in Textures)
                {
                    texture.Save(GameProject.ProjectPath + ProjectPath + "\\img\\" + (i++) + ".png");
                }
            }
            load.Close();
            load.Dispose();
        }

        public bool Load()
        {
            LoadScreen load = new LoadScreen(true);
            load.Show();
            {
                foreach (string enumerateFile in 
                         Directory.EnumerateFiles(GameProject.ProjectPath + "\\" + ProjectPath + "\\img\\"))
                {
                    AddImage(enumerateFile);
                }
            }
            load.Close();
            load.Dispose();
            return true;
        }
            
    }
}
