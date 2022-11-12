using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtCore_Editor
{
    [Serializable]
    public class Varible
    {
        public enum type
        {
            INT,FLOAT,
            BOOL,
            INSTANCE,OBJECT,
            SPRITE,TEXTURE,SOUND,MUSIC,FONT,
            POINT,RECTANGLE,STRING
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
