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
            // structs                                           as string
            VTypePoint, VTypeRectangle, VTypeString, VTypeColor, VTypeEnum
        }
        public VariableType Type { get; set; }
        public string Name { get; set; }
        public string Default { get; set; }
        public Variable(VariableType type, string name, string @default)
        {
            Type = type;
            Name = name;
            Default = @default;
        }
    }
}
