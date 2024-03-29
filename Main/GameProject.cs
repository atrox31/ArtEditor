﻿using ArtCore_Editor.Assets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using ArtCore_Editor.AdvancedAssets.Instance_Manager;
using ArtCore_Editor.etc;
using ArtCore_Editor.AdvancedAssets.SceneManager;
using ArtCore_Editor.AdvancedAssets.SpriteManager;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ArtCore_Editor.Main
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class GameProject : IDisposable
    {
        // static path to game project path
        public static string ProjectPath;

        // get object instance from static call
        public static GameProject GetInstance()
        {
            return MainWindow.GetInstance().GlobalProject;
        }
        public void Dispose()
        {
            MainWindow.GetInstance().GlobalProject = null;
            Sprites = null;
            Sounds = null;
            Music = null;
            Fonts = null;
            Textures = null;
            Instances = null;
            Scenes = null;
            GC.Collect();
        }
        
        [JsonProperty]
        public float Version = 0.0f;
        [JsonProperty]
        public string ProjectName = "New game";
        [JsonProperty]
        public string StartingScene = null;

        [JsonProperty]
        public OrderedDictionary UserProperties = null;

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

        private void CalculateHashForAllAssets<T>(T assetListDictionary) where T : Dictionary<string, Asset>
        {
            foreach (KeyValuePair<string, Asset> item in assetListDictionary)
            {
                item.Value.FileMd5 = Functions.Functions.CalculateHash(ProjectPath + "\\" + item.Value.ProjectPath + "\\" + item.Value.FileName);
            }
        }

        [JsonProperty] 
        public List<string> TargetPlatforms;

        public void SaveToFile()
        {
            Version = Program.Version;
            LoadScreen loadScreen = new LoadScreen(true);
            loadScreen.Show();
            // project
            using (FileStream createStream = File.Create(ProjectPath + "\\" + Program.ProjectFilename))
            {
                byte[] buffer = JsonConvert.SerializeObject(this, Formatting.Indented).Select(c => (byte)c).ToArray();
                createStream.Write(buffer);

            }

            MainWindow.GetInstance().MakeSaved();
            // md5 hash is use to skip asset in pack to asset.pak file
            CalculateHashForAllAssets(Music);
            CalculateHashForAllAssets(Fonts);
            CalculateHashForAllAssets(Sounds);
            CalculateHashForAllAssets(Textures);
            loadScreen.Close();
        }

        private static void TryToFindAndAddAsset<T>(GameProject sender, T assetListDictionary, string assetKind) where T : Dictionary<string, Asset>
        {
            if (!Directory.Exists(ProjectPath + "\\assets\\" + assetKind)) return;
            foreach (string file in Directory.GetFiles(ProjectPath + "\\assets\\" + assetKind))
            {
                assetListDictionary.Add(Path.GetFileName(file).Split('.')[0], new Asset()
                {
                    Name = Path.GetFileName(file).Split('.')[0],
                    FileMd5 = "",
                    ProjectPath = "\\assets\\" + assetKind,
                    FileName = Path.GetFileName(file),
                });
            }
        }

        public static GameProject LoadFromFile(string project)
        {
            string fileContents;
            using (StreamReader reader = new StreamReader(File.Open(project, FileMode.Open)))
            {
                fileContents = reader.ReadToEnd();
            }
            GameProject gameProject = JsonConvert.DeserializeObject<GameProject>(fileContents);

            if (gameProject == null)
            {
                //deserialization failed, try to recover
                MessageBox.Show("Load data error, try to recover");
                gameProject = new GameProject();
                gameProject.Prepare_new();
                gameProject.ProjectName = Path.GetFileName(project);

                string projectPath = Path.GetDirectoryName(project);
                if (Directory.Exists(projectPath + "\\assets"))
                {
                    TryToFindAndAddAsset(gameProject, gameProject.Textures, "texture");
                    TryToFindAndAddAsset(gameProject, gameProject.Music, "music");
                    TryToFindAndAddAsset(gameProject, gameProject.Sounds, "sound");
                    TryToFindAndAddAsset(gameProject, gameProject.Fonts, "font");
                    // special type to load
                    if (Directory.Exists(projectPath + "\\assets\\sprite"))
                    {
                        foreach (string file in Directory.GetDirectories(projectPath + "\\assets\\sprite", "*", SearchOption.TopDirectoryOnly))
                        {
                            string fileName = projectPath + "\\assets\\sprite\\" + file.Split("\\").Last() + "\\" +
                                              file.Split("\\").Last() + "" + Program.FileExtensions_Sprite;
                            if (File.Exists(fileName))
                            {
                                using (StreamReader reader = new StreamReader(File.Open(fileName, FileMode.Open)))
                                {
                                    fileContents = reader.ReadToEnd();
                                    Sprite sprite = JsonConvert.DeserializeObject<Sprite>(fileContents);
                                    if (sprite != null)
                                    {
                                        gameProject.Sprites.Add(Path.GetFileName(file.Split("\\").Last()).Split('.')[0], sprite);
                                    }
                                }
                            }
                        }
                    }
                }

                if (Directory.Exists(projectPath + "\\object"))
                {
                    foreach (string file in Directory.GetDirectories(projectPath + "\\object", "*", SearchOption.TopDirectoryOnly))
                    {
                        string fileName = file + "\\" + file.Split('\\').Last() + "" + Program.FileExtensions_InstanceObject;
                        using StreamReader reader = new StreamReader(File.Open(fileName, FileMode.Open));
                        fileContents = reader.ReadToEnd();
                        Instance instance = JsonConvert.DeserializeObject<Instance>(fileContents);
                        if (instance != null)
                        {
                            gameProject.Instances.Add(Path.GetFileName(file).Split('.')[0], instance);
                        }
                    }
                }
                if (Directory.Exists(projectPath + "\\scene"))
                {
                    foreach (string file in Directory.GetDirectories(projectPath + "\\scene", "*", SearchOption.TopDirectoryOnly))
                    {
                        string fileName = file.Split("\\").Last();
                        if (File.Exists(projectPath + "\\scene\\" + fileName + "\\" + fileName + Program.FileExtensions_SceneObject))
                        {
                            using StreamReader reader = new StreamReader(File.Open(projectPath + "\\scene\\" + fileName + "\\" + fileName + Program.FileExtensions_SceneObject, FileMode.Open));
                            fileContents = reader.ReadToEnd();
                            Scene scene = JsonConvert.DeserializeObject<Scene>(fileContents);
                            if (scene != null)
                            {
                                gameProject.Scenes.Add(fileName, scene);
                            }
                        }
                    }
                }
            }

            foreach (Scene cScene in gameProject.Scenes.Values.ToList())
            {
                foreach (string ins in cScene.SceneInstancesList)
                {
                    string[] data = ins.Split('|');
                    if (gameProject.Instances.ContainsKey(data[0]))
                    {
                        cScene.SceneInstances.Add(new SceneManager.SceneInstance(Convert.ToInt32(data[1]), Convert.ToInt32(data[2]), gameProject.Instances[data[0]]));
                    }
                    else
                    {
                        // TODO: maby show error
                    }
                }
            }

            if (gameProject.Version < Program.Version)
            {
                // update needed
                System.Windows.Forms.MessageBox.Show(
                    "Project have been created in older version of ArtCore Editor.\nTry to save it to update?", "Older version",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }
            
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
            TargetPlatforms = new List<string>();
            UserProperties = new OrderedDictionary();
            foreach (string directory in Program.ProjectDirectoryStructure)
            {
                Directory.CreateDirectory(ProjectPath + directory);
            }
        }
        
        // open new settings editor menu and allow to edit data
        // if user want to save changes
        public void UpdateUserSettings()
        {
            ArtCoreSettings settings = new ArtCoreSettings(UserProperties);
            if (settings.ShowDialog() == DialogResult.OK)
            {
                // get updated settings
                UserProperties = settings.UserProperties;
            };
        }
    }
}
