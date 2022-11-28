using ArtCore_Editor.Assets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace ArtCore_Editor
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class GameProject : IDisposable
    {
        public static string ProjectPath;
        public static GameProject GetInstance()
        {
            return MainWindow.GetInstance().Game_Project;
        }
        public void Dispose()
        {
            MainWindow.GetInstance().Game_Project = null;
            Sprites = null;
            Sounds = null;
            Music = null;
            Fonts = null;
            Textures = null;
            Instances = null;
            Scenes = null;
            ArtCoreDefaultSettings = null;
            GC.Collect();
        }

        // Game settings

        public class ArtCorePreset
        {
            public int DefaultResolution_x { get; set; } = 1920;
            public int DefaultResolution_y { get; set; } = 1080;
            public int DefaultFramerate { get; set; } = 60;
            public int GameUsingController { get; set; } = 0;
            public int FullScreen { get; set; } = 0;
            public int SDL_HINT_RENDER_SCALE_QUALITY { get; set; } = 1;
            public int SDL_GL_MULTISAMPLEBUFFERS { get; set; } = 1;
            public int SDL_GL_MULTISAMPLESAMPLES { get; set; } = 4;
            public int SDL_GL_DEPTH_SIZE { get; set; } = 16;
            public int SDL_GL_RED_SIZE { get; set; } = 4;
            public int SDL_GL_GREEN_SIZE { get; set; } = 4;
            public int SDL_GL_BLUE_SIZE { get; set; } = 4;
            public int SDL_GL_ALPHA_SIZE { get; set; } = 4;
            public int SDL_HINT_RENDER_VSYNC { get; set; } = 0;
            public int AUDIO_CHUNKSIZE { get; set; } = 4096;
            public int AUDIO_FREQ { get; set; } = 44100;
        }

        [JsonProperty]
        public ArtCorePreset ArtCoreDefaultSettings;
        // project properties
        [JsonProperty]
        public float Version = 0.0f;
        [JsonProperty]
        public string ProjectName = "New game";
        [JsonProperty]
        public Scene StartingScene = null;

        // game assets
        [JsonProperty]
        public Dictionary<string, Sprite> Sprites { get; set; }

        [JsonProperty]
        public Dictionary<string, Asset> Sounds { get; set; }

        [JsonProperty]
        public Dictionary<string, Asset> Fonts { get; set; }

        [JsonProperty]
        public Dictionary<string, Asset> Textures { get; set; }

        [JsonProperty]
        public Dictionary<string, Asset> Music { get; set; }

        [JsonProperty]
        public Dictionary<string, Instance> Instances { get; set; }

        [JsonProperty]
        public Dictionary<string, Scene> Scenes { get; set; }

        public GameProject()
        {
            ArtCoreDefaultSettings = new ArtCorePreset();
        }
        public void SaveToFile()
        {
            Version = Program.VERSION;
            LoadScreen loadScreen = new LoadScreen(true);
            loadScreen.Show();
            // project
            using (FileStream createStream = File.Create(ProjectPath + "\\" + Program.PROJECT_FILENAME))
            {
                byte[] buffer = JsonConvert.SerializeObject(this, Formatting.Indented).Select(c => (byte)c).ToArray();
                createStream.Write(buffer);
            }
            // hashcode md5 for assets to skip in compilation
            foreach (var item in this.Music)
            {
                item.Value.File_MD5 = Functions.CalculateMD5(ProjectPath + "\\" + item.Value.ProjectPath + "\\" + item.Value.FileName);
            }
            foreach (var item in this.Fonts)
            {
                item.Value.File_MD5 = Functions.CalculateMD5(ProjectPath + "\\" + item.Value.ProjectPath + "\\" + item.Value.FileName);
            }
            foreach (var item in this.Sounds)
            {
                item.Value.File_MD5 = Functions.CalculateMD5(ProjectPath + "\\" + item.Value.ProjectPath + "\\" + item.Value.FileName);
            }
            foreach (var item in this.Textures)
            {
                item.Value.File_MD5 = Functions.CalculateMD5(ProjectPath + "\\" + item.Value.ProjectPath + "\\" + item.Value.FileName);
            }
            loadScreen.Close();
        }

        public static GameProject LoadFromFile(string Project)
        {
            string fileContents;
            using (StreamReader reader = new StreamReader(File.Open(Project, FileMode.Open)))
            {
                fileContents = reader.ReadToEnd();
            }
            //GameProject? gameProject = JsonSerializer.Deserialize<GameProject>(s);
            GameProject gameProject = JsonConvert.DeserializeObject<GameProject>(fileContents);
            foreach (var cScene in gameProject.Scenes.Values.ToList())
            {
                foreach (var ins in cScene.SceneInstancesList)
                {
                    var data = ins.Split('|');
                    if (gameProject.Instances.ContainsKey(data[0]))
                    {
                        cScene.SceneInstances.Add(new Scene.SceneInstance(Convert.ToInt32(data[1]), Convert.ToInt32(data[2]), gameProject.Instances[data[0]]));
                    }
                    else
                    {
                        // TODO: maby show error
                    }
                }
            }

            if (gameProject == null)
            {
                MessageBox.Show("Target project file is propably corrupted or has beed created in older verion of editor.", "Cannot open", MessageBoxButton.OK, MessageBoxImage.Stop);
                MainWindow.GetInstance().Game_Project = null;
                return null;
            }

            if (gameProject.Version < Program.VERSION)
            {
                // update needed
                System.Windows.Forms.MessageBox.Show(
                    "Project have been created in older verion of ArtCore Editor.\nTry to save it to update?", "Older verion",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }

            if (gameProject.ArtCoreDefaultSettings == null) gameProject.ArtCoreDefaultSettings = new ArtCorePreset();
            return gameProject;
        }

        internal void Prepare_new()
        {
            Sprites = new Dictionary<string, Sprite>();
            Sounds = new Dictionary<string, Asset>();
            Music = new Dictionary<string, Asset>();
            Fonts = new Dictionary<string, Asset>();
            Textures = new Dictionary<string, Asset>();
            Instances = new Dictionary<string, Instance>();
            Scenes = new Dictionary<string, Scene>();
        }

    }
}
