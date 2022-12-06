using System;

namespace ArtCore_Editor.Enums
{
    [Serializable]
    public class Variable
    {
        public enum VariableType
        {
            // primitive type
            VTypeNull, VTypeInt, VTypeFloat, VTypeBool, 
            // reference
            VTypeInstance, VTypeObject, VTypeScene,
            // assets
            VTypeSprite, VTypeTexture, VTypeSound, VTypeMusic, VTypeFont,
            // structs
            VTypePoint, VTypeRect, VTypeString, VTypeColor
        }
        public VariableType Type { get; set; }
        public string Name { get; set; }
        public string Default { get; set; }
        public bool ReadOnly { get; set; }
        public Variable(VariableType type, string name, string @default, bool readOnly = false)
        {
            Type = type;
            Name = name;
            Default = @default;
            ReadOnly = readOnly;
        }
    }
}
