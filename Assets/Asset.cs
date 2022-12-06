using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor.Assets
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Asset : ICloneable
    {
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string FileMd5 { get; set; }
        [JsonProperty]
        public string ProjectPath { get; set; }
        [JsonProperty]
        public string FileName { get; set; }
        
        public object Clone()
        {
            return MemberwiseClone();
        }
        public Asset()
        {
            Name = "";
            FileMd5 = "";
            ProjectPath = "";
            FileName = "";
        }
        
    }
}
