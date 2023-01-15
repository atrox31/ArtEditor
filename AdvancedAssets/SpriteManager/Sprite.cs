using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ArtCore_Editor.Assets;
using ArtCore_Editor.etc;
using ArtCore_Editor.Main;
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

        [JsonProperty] public int CollisionMaskValue1;
        [JsonProperty] public int CollisionMaskValue2;

        [JsonProperty] public SpriteCenterEnum SpriteCenter; // for editor

        [JsonProperty] public int SpriteCenterX;
        [JsonProperty] public int SpriteCenterY;

        [JsonProperty] public int SpriteWidth;
        [JsonProperty] public int SpriteHeight;

        [JsonProperty] public int SpriteFrames;
        [JsonProperty] public Dictionary<string, AnimationSequence> SpriteAnimationSequence; 

        // path to frames data file
        [JsonProperty] public string DataPath;

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
                if (tmp.Count() != 4) return;
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
            // default contructor for deserialization
            public AnimationSequence()
            {
                Name = "";
                Index = "";
                FrameFrom = 0;
                FrameTo = 0;
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
            CollisionMaskValue1 = 1;
            CollisionMaskValue2 = 1;
            SpriteCenter = SpriteCenterEnum.LeftCorner;
            SpriteCenterX = 0;
            SpriteCenterY = 0;
            SpriteWidth = 0;
            SpriteHeight = 0;
            SpriteFrames = 0;
        }
    }
}
