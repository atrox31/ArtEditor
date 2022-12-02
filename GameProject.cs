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
        public string AssetsMD5 = "";
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

            if (gameProject == null)
            {
                //deserialization failed, try to recover
                MessageBox.Show("Load data error, try to recover");
                gameProject = new GameProject();
                gameProject.Prepare_new();
                gameProject.ProjectName = Path.GetFileName(Project);

                string ProjectPath = Path.GetDirectoryName(Project);
                if (Directory.Exists(ProjectPath + "\\assets"))
                {
                    if (Directory.Exists(ProjectPath + "\\assets\\texture"))
                    {
                        foreach(var file in Directory.GetFiles(ProjectPath + "\\assets\\texture"))
                        {
                            gameProject.Textures.Add(Path.GetFileName(file).Split('.')[0], new Asset()
                            {
                                Name = Path.GetFileName(file).Split('.')[0],
                                File_MD5 = "",
                                ProjectPath = "\\assets\\texture",
                                FileName = Path.GetFileName(file),
                                EditorImage = null
                        });
                        }
                    }
                    if (Directory.Exists(ProjectPath + "\\assets\\sprite"))
                    {
                        foreach (var file in Directory.GetDirectories(ProjectPath + "\\assets\\sprite", "*", SearchOption.TopDirectoryOnly))
                        {
                            string FileName = file.Split("\\").Last();
                            if (File.Exists(ProjectPath + "\\assets\\sprite\\" + FileName + "\\" + FileName + ".spr"))
                            {
                                Sprite tmp = new Sprite()
                                {
                                    Name = FileName,
                                    File_MD5 = "",
                                    ProjectPath = "\\assets\\sprite\\" + FileName,
                                    FileName = FileName + ".spr",
                                    EditorImage = null
                                };
                                if (tmp.Load(ProjectPath + "\\assets\\sprite\\" + FileName + "\\" + tmp.FileName))
                                {
                                    gameProject.Sprites.Add(tmp.Name, tmp);
                                }
                            }
                        }
                    }
                    if (Directory.Exists(ProjectPath + "\\assets\\music"))
                    {

                        foreach (var file in Directory.GetFiles(ProjectPath + "\\assets\\music"))
                        {
                            gameProject.Music.Add(Path.GetFileName(file).Split('.')[0], new Asset()
                            {
                                Name = Path.GetFileName(file).Split('.')[0],
                                File_MD5 = "",
                                ProjectPath = "\\assets\\music",
                                FileName = Path.GetFileName(file),
                                EditorImage = null
                            });
                        }
                    }
                    if (Directory.Exists(ProjectPath + "\\assets\\sound"))
                    {

                        foreach (var file in Directory.GetFiles(ProjectPath + "\\assets\\sound"))
                        {
                            gameProject.Sounds.Add(Path.GetFileName(file).Split('.')[0], new Asset()
                            {
                                Name = Path.GetFileName(file).Split('.')[0],
                                File_MD5 = "",
                                ProjectPath = "\\assets\\sound",
                                FileName = Path.GetFileName(file),
                                EditorImage = null
                            });
                        }
                    }
                    if (Directory.Exists(ProjectPath + "\\assets\\font"))
                    {

                        foreach (var file in Directory.GetFiles(ProjectPath + "\\assets\\font"))
                        {
                            gameProject.Fonts.Add(Path.GetFileName(file).Split('.')[0], new Asset()
                            {
                                Name = Path.GetFileName(file).Split('.')[0],
                                File_MD5 = "",
                                ProjectPath = "\\assets\\font",
                                FileName = Path.GetFileName(file),
                                EditorImage = null
                            });
                        }
                    }
                }
                if (Directory.Exists(ProjectPath + "\\database"))
                {

                }
                if (Directory.Exists(ProjectPath + "\\object"))
                {
                    foreach (var file in Directory.GetDirectories(ProjectPath + "\\object", "*", SearchOption.TopDirectoryOnly))
                    {
                        string FileName = file + "\\" + file.Split('\\').Last() + ".obj";
                        using (StreamReader reader = new StreamReader(File.Open(FileName, FileMode.Open)))
                        {
                            fileContents = reader.ReadToEnd();
                            Instance instance = JsonConvert.DeserializeObject<Instance>(fileContents);
                            if(instance != null)
                            {
                                gameProject.Instances.Add(Path.GetFileName(file).Split('.')[0], instance);
                            }
                        }
                    }
                }
                if (Directory.Exists(ProjectPath + "\\scene"))
                {
                    foreach (var file in Directory.GetDirectories(ProjectPath + "\\scene", "*", SearchOption.TopDirectoryOnly))
                    {
                        string FileName = file.Split("\\").Last();
                        if (File.Exists(ProjectPath + "\\scene\\" + FileName + "\\" + FileName + ".scd"))
                        {
                            using (StreamReader reader = new StreamReader(File.Open(ProjectPath + "\\scene\\" + FileName + "\\" + FileName + ".scd", FileMode.Open)))
                            {
                                fileContents = reader.ReadToEnd();
                                Scene instance = JsonConvert.DeserializeObject<Scene>(fileContents);
                                if (instance != null)
                                {
                                    gameProject.Scenes.Add(FileName, instance);
                                }
                            }
                        }
                    }
                }
                if (Directory.Exists(ProjectPath + "\\levels"))
                {

                }
                if (Directory.Exists(ProjectPath + "\\gui"))
                {

                }



            }

            if (gameProject == null)
            {
                MessageBox.Show("Target project file is propably corrupted or has beed created in older verion of editor.", "Cannot open", MessageBoxButton.OK, MessageBoxImage.Stop);
                MainWindow.GetInstance().Game_Project = null;
                return null;
            }

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
