﻿using System;

namespace ArtCore_Editor
{
    [Serializable]
    public class Varible
    {
        public enum type
        {
            NULL, INT, FLOAT,
            BOOL,
            INSTANCE, OBJECT, COLOR, SCENE,
            SPRITE, TEXTURE, SOUND, MUSIC, FONT,
            POINT, RECTANGLE, STRING
        }
        public type Type { get; set; }
        public string Name { get; set; }
        public string Default { get; set; }
        public bool ReadOnly { get; set; }
        public Varible(type Type, string Name, string Default, bool ReadOnly = false)
        {
            this.Type = Type;
            this.Name = Name;
            this.Default = Default;
            this.ReadOnly = ReadOnly;
        }
    }
}
