using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ArtCore_Editor.Assets.Sprite
{
    public class Sprite : Asset
    {
        public Sprite()
        {
            SpriteAnimationSequence = new Dictionary<string, AnimationSequence>();
            this.CollisionMask = Sprite.CollisionMaskEnum.None;
            CollisionMaskValue = 0;
            this.SpriteCenter = Sprite.SpriteCenterEnum.LeftCorner;
            SpriteCenterX = 0;
            SpriteCenterY = 0;
            SpriteWidth = 0;
            SpriteHeight = 0;

            EditorFps = 60;

            EditorPreviewLoop = false;
            EditorPreviewPlay = false;
            EditorShowCenter = false;
            EditorShowMask = false;
            Textures = new List<Image>();
        }

        public List<Image> Textures;
        public enum CollisionMaskEnum
        {
            None, Circle, Rectangle, Perpixel
        }
        public CollisionMaskEnum CollisionMask;
        public int CollisionMaskValue;
        public enum SpriteCenterEnum
        {
            Center, LeftCorner, Custom
        }
        public SpriteCenterEnum SpriteCenter;
        public int SpriteCenterX;
        public int SpriteCenterY;

        // editor value
        public bool EditorShowMask;
        public bool EditorShowCenter;
        public bool EditorPreviewPlay;
        public bool EditorPreviewLoop;
        public int EditorFps;
        public int EditorFrame;
        public int EditorFrameMax;

        public class AnimationSequence
        {
            public string Name;
            public string Index;
            public int FrameFrom;
            public int FrameTo;
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
        public Dictionary<string, AnimationSequence> SpriteAnimationSequence;
        public int SpriteWidth;
        public int SpriteHeight;

        public int ImgId { get; private set; }

        //public string path;

        public void AddImage(string file)
        {
            byte[] content = File.ReadAllBytes(file);
            using (Image image = Image.FromStream(new MemoryStream(content)))
            {
                Textures.Add(new Bitmap(new MemoryStream(content)));
                EditorFrameMax = Textures.Count() - 1;
            }
        }

        public void ClearImages()
        {
            if (Textures != null)
            {
                if (Textures.Count > 0)
                {
                    for (int i = 0; i < Textures.Count; i++)
                    {
                        Textures[i].Dispose();
                    }
                    Textures.Clear();
                }
            }
        }

        public void Save()
        {
            string buffer = "";

            buffer += "[editor]\n";
            buffer += "editor_show_mask=" + (EditorShowMask ? "1" : "0") + "\n";
            buffer += "editor_show_center=" + (EditorShowCenter ? "1" : "0") + "\n";
            buffer += "editor_preview_play=" + (EditorPreviewPlay ? "1" : "0") + "\n";
            buffer += "editor_preview_loop=" + (EditorPreviewLoop ? "1" : "0") + "\n";

            buffer += "editor_fps=" + EditorFps.ToString() + "\n";
            buffer += "editor_frame=" + EditorFrame.ToString() + "\n";
            buffer += "editor_frame_max=" + EditorFrameMax.ToString() + "\n";
            //buffer += "path=" + path + "\n";
            buffer += "[/editor]\n";

            if (SpriteAnimationSequence.Any())
            {
                buffer += "[animation]\n";
                foreach (AnimationSequence entry in SpriteAnimationSequence.Values)
                {
                    buffer += entry.Get() + '\n';
                }
                buffer += "[/animation]\n";
            }
            buffer += "[sprite]\n";

            buffer += "name=" + Name + "\n";
            switch (CollisionMask)
            {
                case CollisionMaskEnum.None: buffer += "collision_mask=none\n"; break;
                case CollisionMaskEnum.Circle: buffer += "collision_mask=circle\n"; break;
                case CollisionMaskEnum.Rectangle: buffer += "collision_mask=rectangle\n"; break;
                case CollisionMaskEnum.Perpixel: buffer += "collision_mask=perpixel\n"; break;
                default: buffer += "collision_mask=undefined\n"; break;
            }

            buffer += "collision_mask_value=" + CollisionMaskValue.ToString() + "\n";

            switch (SpriteCenter)
            {
                case SpriteCenterEnum.Center: buffer += "sprite_center=center\n"; break;
                case SpriteCenterEnum.LeftCorner: buffer += "sprite_center=leftcorner\n"; break;
                case SpriteCenterEnum.Custom: buffer += "sprite_center=custom\n"; break;
                default: buffer += "sprite_center=undefined\n"; break;
            }
            buffer += "sprite_center_x=" + SpriteCenterX.ToString() + "\n";
            buffer += "sprite_center_y=" + SpriteCenterY.ToString() + "\n";
            buffer += "sprite_width=" + SpriteWidth.ToString() + "\n";
            buffer += "sprite_height=" + SpriteWidth.ToString() + "\n";

            buffer += "[/sprite]\n";
            buffer += "[image_list]\n";

            buffer += "count=" + (Textures == null ? "0" : Textures.Count.ToString()) + "\n";

            string filePath = GameProject.ProjectPath + "\\assets\\sprite\\" + Name;
            Directory.CreateDirectory(filePath + "\\img\\");

            if (Textures != null && Textures.Count > 0)
            {
                int i = 0;
                foreach (Image img in Textures)
                {
                    string pth = filePath + "\\img\\" + i.ToString() + ".png";
                    if (!File.Exists(pth))
                    {
                        img.Save(pth, System.Drawing.Imaging.ImageFormat.Png);

                    }
                    i++;
                }
            }
            buffer += "[/image_list]\n";
            File.WriteAllText(filePath + "\\" + Name + ".spr", buffer);

            FileName = Name + ".spr";
            ProjectPath = "\\assets\\sprite\\" + Name + "\\";
        }

        public bool Load(string file)
        {
            if (!file.Contains(".spr")) { file += ".spr"; }
            if (!File.Exists(file)) return false;


            int phase = 0;
            int nlin = -1;
            int imgc = -1;

            foreach (string line in File.ReadAllLines(file))
            {
                nlin++;
                switch (phase)
                {
                    case 0:
                        {
                            if (line == "[editor]") phase = 1;
                            if (line == "[sprite]") phase = 2;
                            if (line == "[image_list]") phase = 3;
                            if (line == "[animation]") phase = 4;
                        }
                        break;
                    case 1: // editor
                        {
                            if (line == "[/editor]") { phase = 0; break; }
                            string[] tmp = line.Split('=');
                            //if (tmp[0] == "path") { path = tmp[1]; break; }
                            if (tmp[0] == "editor_show_mask") { EditorShowMask = (tmp[1] == "1" ? true : false); break; }
                            if (tmp[0] == "editor_show_center") { EditorShowCenter = (tmp[1] == "1" ? true : false); break; }
                            if (tmp[0] == "editor_preview_play") { EditorPreviewPlay = (tmp[1] == "1" ? true : false); break; }
                            if (tmp[0] == "editor_preview_loop") { EditorPreviewLoop = (tmp[1] == "1" ? true : false); break; }

                            if (tmp[0] == "editor_fps") { EditorFps = Convert.ToInt32(tmp[1]); break; }
                            if (tmp[0] == "editor_frame") { EditorFrame = Convert.ToInt32(tmp[1]); break; }
                            if (tmp[0] == "editor_frame_max") { EditorFrameMax = Convert.ToInt32(tmp[1]); break; }

                        }
                        break;
                    case 2: // editor
                        {
                            if (line == "[/sprite]") { phase = 0; break; }
                            string[] tmp = line.Split('=');
                            if (tmp[0] == "name") { Name = tmp[1]; break; }
                            

                            if (tmp[0] == "collision_mask")
                            {
                                if (tmp[1] == "none") CollisionMask = CollisionMaskEnum.None;
                                if (tmp[1] == "circle") CollisionMask = CollisionMaskEnum.Circle;
                                if (tmp[1] == "rectangle") CollisionMask = CollisionMaskEnum.Rectangle;
                                if (tmp[1] == "perpixel") CollisionMask = CollisionMaskEnum.Perpixel;
                                if (tmp[1] == "undefined") CollisionMask = 0;
                                break;
                            }

                            if (tmp[0] == "collision_mask_value") { CollisionMaskValue = Convert.ToInt32(tmp[1]); break; }

                            if (tmp[0] == "sprite_center")
                            {
                                if (tmp[1] == "center") SpriteCenter = SpriteCenterEnum.Center;
                                if (tmp[1] == "leftcorner") SpriteCenter = SpriteCenterEnum.LeftCorner;
                                if (tmp[1] == "custom") SpriteCenter = SpriteCenterEnum.Custom;
                                if (tmp[1] == "undefined") SpriteCenter = 0;
                                break;
                            }
                            if (tmp[0] == "sprite_center_x") { SpriteCenterX = Convert.ToInt32(tmp[1]); break; }
                            if (tmp[0] == "sprite_center_y") { SpriteCenterY = Convert.ToInt32(tmp[1]); break; }
                            if (tmp[0] == "sprite_width") { SpriteWidth = Convert.ToInt32(tmp[1]); break; }
                            if (tmp[0] == "sprite_height") { SpriteHeight = Convert.ToInt32(tmp[1]); break; }

                        }
                        break;
                    case 3:
                        {
                            if (line == "[/image_list]") { phase = 0; break; }
                            string[] tmp = line.Split('=');
                            if (tmp[0] == "count")
                            {
                                imgc = Convert.ToInt32(tmp[1]);
                                Textures = new List<Image>(imgc);
                                for (int i = 0; i < imgc; i++)
                                {
                                    AddImage(GameProject.ProjectPath + "\\" + ProjectPath + "\\img\\" + i.ToString() + ".png");
                                }
                            }
                        }
                        break;
                    case 4:
                        //animation
                        if (line == "[/animation]") { phase = 0; break; }
                        SpriteAnimationSequence.Add(line.Split('|')[1], new AnimationSequence(line));
                        break;
                    default:
                        return false;
                }
            }

            FileName = Name + ".spr";
            ProjectPath = "\\assets\\sprite\\" + Name + "\\";
            
            return true;
        }
    }
}
