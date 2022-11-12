using Newtonsoft.Json;
using System;
using System.Drawing;

namespace ArtCore_Editor.Assets
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Asset : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public Asset()
        {
            Name = "";
            File_MD5 = "";
            ProjectPath = "";
            FileName = "";
            EditorImage = null;
        }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string File_MD5 { get; set; }
        [JsonProperty]
        public string ProjectPath { get; set; }
        [JsonProperty]
        public string FileName { get; set; }

        public Image EditorImage { get; set; }
    }
}
