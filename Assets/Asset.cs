using Newtonsoft.Json;
using System;
using ArtCore_Editor.Functions;

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

        public Asset()
        {
            Name = "";
            FileMd5 = "";
            ProjectPath = "";
            FileName = "";
        }
        
        // return Path.Combine(GameProject.ProjectPath, ProjectPath, FileName);
        public string GetFilePath()
        {
            return StringExtensions.Combine(GameProject.ProjectPath, ProjectPath, FileName);
        }

        public string GetTypeName()
        {
            string[] ppath = ProjectPath.Split('\\');
            if (ppath.Length > 1)
            {
                return ppath[1];
            }
            return "unknown";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
