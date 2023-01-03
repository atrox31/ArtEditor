using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArtCore_Editor.Enums
{
    [JsonObject(MemberSerialization.OptIn)]
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
        [JsonProperty] public VariableType Type { get; set; }
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public string Default { get; set; }
        [JsonProperty]   public List<string> EnumValues { get; set; }
        public Variable(VariableType type, string name, string @default, List<string> enumValues = null)
        {
            Type = type;
            Name = name;
            Default = @default;
            EnumValues = enumValues;
        }
    }
}
