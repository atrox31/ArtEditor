using ArtCore_Editor.Assets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ArtCore_Editor
{
    public class Sprite : Asset
    {
        /*
        public static Sprite Default()
        {
            Sprite sprite = new Sprite();
            sprite.Width = 32;
            sprite.Height = 32;
            sprite.CenterX = 16;
            sprite.CenterY = 16;
            sprite.EditorImage = Resources.interrogation;
            return sprite;
        }*/
        public void SetImgId(bool force = false)
        {
            if (textures != null)
            {
                if (imgId != -1 || force)
                {
                    //imgId = Form1.addImageToList(textures[0].GetThumbnailImage(64, 64, null, IntPtr.Zero));
                }
            }
        }
        public Sprite()
        {
            sprite_animationSquence = new Dictionary<string, animationSequence>();
            this.name = "";
            this.type = Sprite.Type.asset;
            this.collision_mask = Sprite.CollisionMask.none;
            this.collision_mask_value = 0;
            this.sprite_center = Sprite.SpriteCenter.leftcorner;
            this.sprite_center_x = 0;
            this.sprite_center_y = 0;
            this.sprite_width = 0;
            this.sprite_height = 0;

            this.editor_fps = 60;

            this.editor_preview_loop = false;
            this.editor_preview_play = false;
            this.editor_show_center = false;
            this.editor_show_mask = false;
        }

        public string name;
        public List<Image> textures;
        int textures_count = 0;
        public enum Type
        {
            asset, core, particle
        }
        public Type type;
        public enum CollisionMask
        {
            none, circle, rectangle, perpixel
        }
        public CollisionMask collision_mask;
        public int collision_mask_value;
        public enum SpriteCenter
        {
            center, leftcorner, custom
        }
        public SpriteCenter sprite_center;
        public int sprite_center_x;
        public int sprite_center_y;

        // editor value
        public bool editor_show_mask;
        public bool editor_show_center;
        public bool editor_preview_play;
        public bool editor_preview_loop;
        public int editor_fps;
        public int editor_frame;
        public int editor_frame_max;

        public class animationSequence
        {
            public string name;
            public string index;
            public int frameFrom;
            public int frameTo;
            public animationSequence(string data)
            {
                string[] tmp = data.Split('|');
                this.name = tmp[0];
                this.index = tmp[1];
                this.frameFrom = Convert.ToInt32(tmp[2]);
                this.frameTo = Convert.ToInt32(tmp[3]);
            }
            public animationSequence(string name, string index, int frameFrom, int frameTo)
            {
                this.name = name;
                this.index = index;
                this.frameFrom = frameFrom;
                this.frameTo = frameTo;
            }
            public string get()
            {
                return name + '|' + index + '|' + frameFrom + '|' + frameTo;
            }

        }
        public Dictionary<string, animationSequence> sprite_animationSquence;
        public int sprite_width;
        public int sprite_height;

        public int imgId { get; private set; }

        //public string path;

        public void AddImage(string file)
        {
            byte[] content = File.ReadAllBytes(file);
            using (Image image = Image.FromStream(new MemoryStream(content)))
            {
                textures.Add(new Bitmap(new MemoryStream(content)) );
            }
        }

        public void ClearImages()
        {
            if (textures != null)
            {
                if (textures.Count> 0)
                {
                    for (int i = 0; i < textures.Count; i++)
                    {
                        textures[i].Dispose();
                    }
                }
            }
            textures_count = 0;
        }

        public void Save()
        {
            string buffer = "";

            buffer += "[editor]\n";
            buffer += "editor_show_mask=" + (editor_show_mask ? "1" : "0") + "\n";
            buffer += "editor_show_center=" + (editor_show_center ? "1" : "0") + "\n";
            buffer += "editor_preview_play=" + (editor_preview_play ? "1" : "0") + "\n";
            buffer += "editor_preview_loop=" + (editor_preview_loop ? "1" : "0") + "\n";

            buffer += "editor_fps=" + editor_fps.ToString() + "\n";
            buffer += "editor_frame=" + editor_frame.ToString() + "\n";
            buffer += "editor_frame_max=" + editor_frame_max.ToString() + "\n";
            //buffer += "path=" + path + "\n";
            buffer += "[/editor]\n";

            if (sprite_animationSquence.Count() > 0)
            {
                buffer += "[animation]\n";
                foreach (animationSequence entry in sprite_animationSquence.Values)
                {
                    buffer += entry.get() + '\n';
                }
                buffer += "[/animation]\n";
            }
            buffer += "[sprite]\n";

            buffer += "name=" + name + "\n";
            switch (type)
            {
                case Type.asset: buffer += "type=asset\n"; break;
                case Type.core: buffer += "type=core\n"; break;
                case Type.particle: buffer += "type=particle\n"; break;
                default: buffer += "type=undefined\n"; break;
            }
            switch (collision_mask)
            {
                case CollisionMask.none: buffer += "collision_mask=none\n"; break;
                case CollisionMask.circle: buffer += "collision_mask=circle\n"; break;
                case CollisionMask.rectangle: buffer += "collision_mask=rectangle\n"; break;
                case CollisionMask.perpixel: buffer += "collision_mask=perpixel\n"; break;
                default: buffer += "collision_mask=undefined\n"; break;
            }

            buffer += "collision_mask_value=" + collision_mask_value.ToString() + "\n";

            switch (sprite_center)
            {
                case SpriteCenter.center: buffer += "sprite_center=center\n"; break;
                case SpriteCenter.leftcorner: buffer += "sprite_center=leftcorner\n"; break;
                case SpriteCenter.custom: buffer += "sprite_center=custom\n"; break;
                default: buffer += "sprite_center=undefined\n"; break;
            }
            buffer += "sprite_center_x=" + sprite_center_x.ToString() + "\n";
            buffer += "sprite_center_y=" + sprite_center_y.ToString() + "\n";
            buffer += "sprite_width=" + sprite_width.ToString() + "\n";
            buffer += "sprite_height=" + sprite_width.ToString() + "\n";

            buffer += "[/sprite]\n";
            buffer += "[image_list]\n";

            buffer += "count=" + (textures == null ? "0" : textures.Count.ToString()) + "\n";

            string FilePath = GameProject.GetInstance().ProjectPath + "\\assets\\sprite\\" + name;
            Directory.CreateDirectory(FilePath + "\\img\\");

            if (textures != null && textures.Count > 0)
            {
                int i = 0;
                foreach (Image img in textures)
                {
                    string pth = FilePath + "\\img\\" + i.ToString() + ".png";
                    if (!File.Exists(pth))
                    {
                        img.Save(pth, System.Drawing.Imaging.ImageFormat.Png);

                    }
                    buffer += "\\assets\\sprite\\" + name + "\\img\\" + i.ToString() + ".png\n";
                    i++;
                }
            }
            buffer += "[/image_list]\n";
            File.WriteAllText(FilePath + "\\" + name + ".spr", buffer);

            FileName = "\\assets\\sprite\\" + name + "\\" + name + ".spr";
            Name = name;
            ProjectPath = "\\assets\\sprite\\" + name + "\\";
        }

        public bool Load(string file)
        {
            if (!file.Contains(".spr")) { file += ".spr"; }
            if (!File.Exists(file)) return false;


            FileName = "\\assets\\" + file.Split("assets")[1];
            ProjectPath = "\\assets\\sprite\\" + name;

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
                            if (tmp[0] == "editor_show_mask") { editor_show_mask = (tmp[1] == "1" ? true : false); break; }
                            if (tmp[0] == "editor_show_center") { editor_show_center = (tmp[1] == "1" ? true : false); break; }
                            if (tmp[0] == "editor_preview_play") { editor_preview_play = (tmp[1] == "1" ? true : false); break; }
                            if (tmp[0] == "editor_preview_loop") { editor_preview_loop = (tmp[1] == "1" ? true : false); break; }

                            if (tmp[0] == "editor_fps") { editor_fps = Convert.ToInt32(tmp[1]); break; }
                            if (tmp[0] == "editor_frame") { editor_frame = Convert.ToInt32(tmp[1]); break; }
                            if (tmp[0] == "editor_frame_max") { editor_frame_max = Convert.ToInt32(tmp[1]); break; }

                        }
                        break;
                    case 2: // editor
                        {
                            if (line == "[/sprite]") { phase = 0; break; }
                            string[] tmp = line.Split('=');
                            if (tmp[0] == "name") { name = tmp[1]; Name = name; break; }

                            if (tmp[0] == "type")
                            {
                                if (tmp[1] == "asset") type = Type.asset;
                                if (tmp[1] == "core") type = Type.core;
                                if (tmp[1] == "particle") type = Type.particle;
                                if (tmp[1] == "undefined") type = 0;
                                break;
                            }

                            if (tmp[0] == "collision_mask")
                            {
                                if (tmp[1] == "none") collision_mask = CollisionMask.none;
                                if (tmp[1] == "circle") collision_mask = CollisionMask.circle;
                                if (tmp[1] == "rectangle") collision_mask = CollisionMask.rectangle;
                                if (tmp[1] == "perpixel") collision_mask = CollisionMask.perpixel;
                                if (tmp[1] == "undefined") collision_mask = 0;
                                break;
                            }

                            if (tmp[0] == "collision_mask_value") { collision_mask_value = Convert.ToInt32(tmp[1]); break; }

                            if (tmp[0] == "sprite_center")
                            {
                                if (tmp[1] == "center") sprite_center = SpriteCenter.center;
                                if (tmp[1] == "leftcorner") sprite_center = SpriteCenter.leftcorner;
                                if (tmp[1] == "custom") sprite_center = SpriteCenter.custom;
                                if (tmp[1] == "undefined") sprite_center = 0;
                                break;
                            }
                            if (tmp[0] == "sprite_center_x") { sprite_center_x = Convert.ToInt32(tmp[1]); break; }
                            if (tmp[0] == "sprite_center_y") { sprite_center_y = Convert.ToInt32(tmp[1]); break; }
                            if (tmp[0] == "sprite_width") { sprite_width = Convert.ToInt32(tmp[1]); break; }
                            if (tmp[0] == "sprite_height") { sprite_height = Convert.ToInt32(tmp[1]); break; }

                        }
                        break;
                    case 3:
                        {
                            if (line == "[/image_list]") { phase = 0; break; }
                            if (imgc == -1)
                            {
                                string[] tmp = line.Split('=');
                                if (tmp[0] == "count")
                                {
                                    imgc = Convert.ToInt32(tmp[1]);
                                    textures = new List<Image>(imgc);
                                }

                            }
                            else
                            {
                                AddImage(GameProject.GetInstance().ProjectPath + line);
                            }

                        }
                        break;
                    case 4:
                        //animation
                        if (line == "[/animation]") { phase = 0; break; }
                        sprite_animationSquence.Add(line.Split('|')[1], new animationSequence(line));
                        break;
                    default:
                        return false;
                }
            }
            SetImgId(true);
            return true;
        }
    }
}
