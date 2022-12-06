using ArtCore_Editor.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ArtCore_Editor.Assets.Sprite;
using ArtCore_Editor.Instance_Manager;

namespace ArtCore_Editor
{
    public partial class GameCompiler : Form
    {
        static GameCompiler _instance;
        class Message
        {
            public string Text { get; set; }
            public int ProgressBarValue { get; set; }
            public bool ReplaceLastLine { get; set; }
            public Message() { }
            public Message(string text, int progressBarValue, bool replaceLastLine)
            {
                Text = text;
                ReplaceLastLine = replaceLastLine;
                ProgressBarValue = progressBarValue;
            }
        }

        public static void OutputWrite(string message, bool replaceLastLine = false)
        {
            if (replaceLastLine)
            {
                _instance.OutputLog.Items[_instance.OutputLog.Items.Count - 1] = message;
            }
            else
            {
                _instance.OutputLog.Items.Add(message);
            }
        }
        bool _isDebug;
        bool _runGame;
        bool _closeAfterDone;
        public GameCompiler(bool debugMode, bool runGame = false, bool closeAfterDone = false)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            _instance = this;
            _isDebug = debugMode;
            _runGame = runGame;
            _closeAfterDone = closeAfterDone;

                if (File.Exists(GameProject.ProjectPath + "\\" + "assets.pak"))
                {
                    File.Delete(GameProject.ProjectPath + "\\" + "assets.pak");
                }
            
            if (!_isDebug)
            {
                button2.Visible = false;
            }
        }
        public void PrepareAssets<T>(BackgroundWorker sender, DoWorkEventArgs e, Dictionary<string, T> asset, string assetName, int progressMin, int progressMax)
        {
            string output = GameProject.ProjectPath + "\\" + "assets.pak";

            int maxCount = asset.Count();
            int currentItem = 0;
            int skipped = 0;
            sender.ReportProgress(1, new Message(assetName + " (" + currentItem.ToString() + "/" + maxCount.ToString() + ")", progressMin, false));
            foreach (var item in asset)
            {
                int currentProgress = Functions.Scale(currentItem, 0, maxCount, progressMin, progressMax);
                if (CancelRequest(sender, e)) return;

                string name = (string)(typeof(T).GetProperty("Name")?.GetValue(item.Value, null));
                string fileMd5 = (string)(typeof(T).GetProperty("File_MD5")?.GetValue(item.Value, null));
                string fileName = ((string)(typeof(T).GetProperty("FileName")?.GetValue(item.Value, null)))?.Split('\\').Last();
                string projectPath = (string)(typeof(T).GetProperty("ProjectPath")?.GetValue(item.Value, null));

                //Console.WriteLine($"Name: {Name}; File_MD5: {File_MD5}; FileName: {FileName}; ProjectPath: {ProjectPath} ");

                sender.ReportProgress(1, new Message(assetName + " (" + (currentItem).ToString() + "/" + maxCount.ToString() + ") " + name, currentProgress, true));

                if (!File.Exists(GameProject.ProjectPath + "\\" + projectPath + "\\" + fileName))
                {
                    sender.ReportProgress(1, new Message("Asset type: '" + name + "' file not exists", currentProgress, false));
                    return;
                }

                // check MD5 cheksum
                using (FileStream zipToOpen = new FileStream(output, FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        string md5 = null;
                        if (_isDebug)
                        {
                            string md5File = $"DEBUG\\{(typeof(T)).FullName}.{fileName}.MD5";
                            if (archive.GetEntry(md5File) != null)
                            {
                                md5 = new StreamReader(archive.GetEntry(md5File).Open()).ReadToEnd();
                            }
                            else
                            {
                                ZipArchiveEntry readmeEntry = archive.CreateEntry(md5File);
                                using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                                {
                                    writer.Write(fileMd5);
                                }

                            }
                        }
                        if (md5 == null || md5 != fileMd5)
                        {
                            string entryName = assetName + "\\" + fileName;
                            ZipArchiveEntry fileEntry = archive.GetEntry(entryName);
                            if (fileEntry != null)
                            {
                                fileEntry.Delete();
                            }
                            fileEntry = archive.CreateEntry(entryName);

                            using (StreamWriter writer = new StreamWriter(fileEntry.Open()))
                            {
                                using (StreamReader file = new StreamReader(GameProject.ProjectPath + "\\" + projectPath + "\\" + fileName))
                                {
                                    file.BaseStream.CopyTo(writer.BaseStream);
                                }
                                //var t = typeof(T);
                                if (typeof(T) == typeof(Sprite))
                                {
                                    int i = 0;
                                    foreach (var f in Directory.GetFiles(GameProject.ProjectPath + "\\" + projectPath + "\\img\\"))
                                    {
                                        //foreach (var _f in Directory.GetFiles(GameProject.ProjectPath + "\\" + ProjectPath + "\\img\\"))
                                        ZipArchiveEntry fileEntryImg = archive.CreateEntry(assetName + "\\" + name + "\\" + i.ToString() + ".png");
                                        i++;
                                        using (StreamWriter writerImg = new StreamWriter(fileEntryImg.Open()))
                                        {
                                            using (StreamReader file = new StreamReader(f))
                                            {
                                                file.BaseStream.CopyTo(writerImg.BaseStream);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            skipped++;
                        }
                        _fileList.Add(assetName + "\\" + fileName);

                    }
                }
                sender.ReportProgress(1, new Message(assetName + " (" + (++currentItem).ToString() + "/" + maxCount.ToString() + ") " + name, currentProgress, true));
            }
            sender.ReportProgress(1, new Message(assetName + " (" + (currentItem).ToString() + "/" + maxCount.ToString() + ") ", progressMax, true));
            sender.ReportProgress(1, new Message(skipped.ToString() + " skipped", progressMax, false));
        }

        public static bool CancelRequest(BackgroundWorker obj, DoWorkEventArgs e)
        {
            if (obj.CancellationPending == true)
            {
                e.Cancel = true;
                return true;
            }
            return false;
        }

        public static bool CreateObjectDefinitions(BackgroundWorker sender, DoWorkEventArgs e)
        {
            foreach (var ins in GameProject.GetInstance().Instances)
            {
                ObjectManager obm = new ObjectManager(ins.Key);
                obm.Save();
            }
            string inputs = "";
            foreach (var obj in GameProject.GetInstance().Instances)
            {
                inputs += "-obj \"" + GameProject.ProjectPath + "\\object\\" + obj.Key.ToString() + "\\main.asc\" ";
            }
            string args = "-lib \"" + AppDomain.CurrentDomain.BaseDirectory + "\\" + "Core\\AScript.lib\" -output \"" + GameProject.ProjectPath + "\\object_compile.acp\" " + inputs;

            Process compiler = new Process();
            compiler.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + "Core\\ACompiler.exe";
            compiler.StartInfo.Arguments = args;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.CreateNoWindow = true;
            //compiler.StartInfo.WorkingDirectory = GameProject.ProjectPath;
            compiler.Start();

            string standardOutput;
            while ((standardOutput = compiler.StandardOutput.ReadLine()) != null)
            {
                sender.ReportProgress(1, new Message(standardOutput, -1, false));
            }

            compiler.WaitForExit();
            if (compiler.ExitCode == 0)
            {
                if (File.Exists(GameProject.ProjectPath + "\\object_compile.acp"))
                {

                    using (FileStream zipToOpen = new FileStream(GameProject.ProjectPath + "\\" + "game.dat", FileMode.OpenOrCreate))
                    {
                        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                        {
                            if (CancelRequest(sender, e)) return false;

                            string entryName = "object_compile.acp";
                            ZipArchiveEntry readmeEntry = archive.GetEntry(entryName);
                            if (readmeEntry != null)
                            {
                                readmeEntry.Delete();
                            }
                            readmeEntry = archive.CreateEntry(entryName);

                            using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                            {
                                using (StreamReader file = new StreamReader(GameProject.ProjectPath + "\\object_compile.acp"))
                                {
                                    file.BaseStream.CopyTo(writer.BaseStream);
                                }


                            }
                        }
                    }
                    return true;
                }
                else
                {
                    //TODO: error file not found
                    return false;
                }
            }
            return false;
        }

        void UpdateCoreFiles(string folder, string file, int c, int max)
        {
            using (FileStream zipToOpen = new FileStream(GameProject.ProjectPath + "\\" + "game.dat", FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry readmeEntry = archive.GetEntry("files\\" + file);
                    if (readmeEntry == null)
                    {
                        readmeEntry = archive.CreateEntry("files\\" + file);
                    }
                    using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                    {
                        using (StreamReader fileWriter = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\" + "Core\\" + folder + "\\" + file))
                        {
                            fileWriter.BaseStream.CopyTo(writer.BaseStream);
                        }
                    }



                }
            }

            Bgw.ReportProgress(1, new Message("Prepare game file (" + c.ToString() + "/" + max.ToString() + ")", -1, true));
        }

        void WriteFileToGameDat(string fileDest, string fileSource)
        {
            using (FileStream zipToOpen = new FileStream(GameProject.ProjectPath + "\\" + "game.dat", FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry readmeEntry = archive.GetEntry(fileSource);
                    if (readmeEntry == null)
                    {
                        readmeEntry = archive.CreateEntry(fileSource);
                    }
                    using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                    {
                        using (StreamReader file = new StreamReader(fileDest))
                        {
                            file.BaseStream.CopyTo(writer.BaseStream);
                        }
                    }



                }
            }
        }

        void WriteListToArchive(string archive, string entry, List<string> content)
        {
            using (FileStream zipToOpen = new FileStream(GameProject.ProjectPath + "\\" + archive, FileMode.OpenOrCreate))
            {
                using (ZipArchive arch = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry readmeEntry = arch.GetEntry(entry);
                    if (readmeEntry == null)
                    {
                        readmeEntry = arch.CreateEntry(entry);
                    }
                    using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                    {
                        foreach (string property in content)
                        {
                            writer.WriteLine(property);
                        }
                    }


                }
            }
        }
        static List<string> _fileList = new List<string>();
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bgw.ReportProgress(1, new Message("ArtCore Editor version " + Program.Version.ToString(), 1, false));

            // saving project
            Bgw.ReportProgress(1, new Message("Saving project", 2, false));
            GameProject.GetInstance().SaveToFile();
            Bgw.ReportProgress(1, new Message("Saving project ..done", 3, true));
            if (CancelRequest(Bgw, e)) return;


            _fileList.Clear();
            PrepareAssets(Bgw, e, GameProject.GetInstance().Textures, "Textures", 10, 20);
            if (CancelRequest(Bgw, e)) return;

            PrepareAssets(Bgw, e, GameProject.GetInstance().Sprites, "Sprites", 20, 30);
            if (CancelRequest(Bgw, e)) return;

            PrepareAssets(Bgw, e, GameProject.GetInstance().Music, "Music", 30, 40);
            if (CancelRequest(Bgw, e)) return;

            PrepareAssets(Bgw, e, GameProject.GetInstance().Sounds, "Sounds", 40, 50);
            if (CancelRequest(Bgw, e)) return;

            PrepareAssets(Bgw, e, GameProject.GetInstance().Fonts, "Fonts", 50, 55);
            if (CancelRequest(Bgw, e)) return;

            WriteListToArchive("assets.pak", "filelist.txt", _fileList);
            Bgw.ReportProgress(1, new Message("Asset prepare complite", -1, false));
            

            Bgw.ReportProgress(1, new Message("Prepare game file", -1, false));

            {
                List<string[]> coreFiles = new List<string[]>()
                {
                    new string[]{ "pack", "gamecontrollerdb.txt" },
                    new string[]{ "pack", "TitilliumWeb-Light.ttf" },
                    new string[]{ "shaders", "bloom.frag" },
                    new string[]{ "shaders", "color.frag" },
                    new string[]{ "shaders", "common.vert" },
                    new string[]{ "", "AScript.lib" },
                };
                int c = 1;
                foreach (var item in coreFiles)
                {
                    UpdateCoreFiles(item[0], item[1], c++, coreFiles.Count());
                    if (CancelRequest(Bgw, e)) return;
                }
                if (File.Exists(GameProject.ProjectPath + "\\bg_img.png"))
                {
                    WriteFileToGameDat(GameProject.ProjectPath + "\\bg_img.png", "bg_img.png");
                }
            }
            Bgw.ReportProgress(1, new Message("Prepare game file ..done", 60, true));

            // setup.ini

            Bgw.ReportProgress(1, new Message("Game settings", 60, false));
            {
                List<string> content = new List<string>();
                foreach (PropertyInfo property in typeof(GameProject.ArtCorePreset).GetProperties())
                {
                    var fName = property.Name;
                    int value = (int)property.GetValue(GameProject.GetInstance().ArtCoreDefaultSettings, null);
                    content.Add(fName + "=" + value);
                }
                WriteListToArchive("game.dat", "setup.ini", content);
            }

            if (CancelRequest(Bgw, e)) return;
            Bgw.ReportProgress(1, new Message("Game settings ..done", 65, true));


            // object definitions
            if (CancelRequest(Bgw, e)) return;
            Bgw.ReportProgress(1, new Message("Objects", 70, false));

            if (!CreateObjectDefinitions(Bgw, e))
            {
                return;
            }
            Bgw.ReportProgress(1, new Message("Objects ..done", 90, false));



            // scene definitions
            if (CancelRequest(Bgw, e)) return;
            Bgw.ReportProgress(1, new Message("Scenes ", 91, false));

            CreateSceneDefinitions(Bgw, e);
            WriteListToArchive("game.dat", "scene\\list.txt", GameProject.GetInstance().Scenes.Keys.ToList());
            WriteListToArchive("game.dat", "scene\\StartingScene.txt", new List<string>() { GameProject.GetInstance().StartingScene?.Name });


            Bgw.ReportProgress(1, new Message("Scenes ..done", 99, true));
 
            if (CancelRequest(Bgw, e)) return;
            Bgw.ReportProgress(1, new Message("Game ready", 100, false));
            
        }

        private void CreateSceneDefinitions(BackgroundWorker bgw, DoWorkEventArgs e)
        {
            using (FileStream zipToOpen = new FileStream(GameProject.ProjectPath + "\\" + "game.dat", FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    var garbage = archive.Entries.Where(item => item.FullName.StartsWith("scene")).ToArray();
                    foreach (var garbageItem in garbage)
                    {
                        garbageItem.Delete();
                    }
                    foreach (var scene in GameProject.GetInstance().Scenes)
                    {
                        string entryName = "scene\\" + scene.Key + "\\" + scene.Key + ".asd";
                        ZipArchiveEntry readmeEntry = archive.GetEntry(entryName);
                        if (readmeEntry != null)
                        {
                            readmeEntry.Delete();
                        }
                        readmeEntry = archive.CreateEntry(entryName);
                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {

                            writer.WriteLine("[setup]");
                            writer.WriteLine("GuiFile=" + scene.Value.GuiFile);
                            writer.WriteLine("Width=" + scene.Value.Width);
                            writer.WriteLine("Height=" + scene.Value.Height);
                            writer.WriteLine("BackGroundTexture=" + scene.Value.BackGroundTexture.Name);
                            writer.WriteLine("BackGroundType=" + scene.Value.BackGroundType.ToString());
                            writer.WriteLine("BackGroundWrapMode=" + scene.Value.BackGroundWrapMode);
                            writer.WriteLine("BackGroundColor=" + Functions.ColorToHex(scene.Value.BackGroundColor));
                            
                            writer.WriteLine("[regions]");
                            foreach (var regions in scene.Value.Regions)
                            {
                                writer.WriteLine(regions.ToString());
                            }

                            writer.WriteLine("[triggers]");
                            foreach (var triggers in scene.Value.SceneTriggers)
                            {
                                writer.WriteLine(triggers);
                            }

                            writer.WriteLine("[instance]");
                            foreach (var sIns in scene.Value.SceneInstances)
                            {
                                writer.WriteLine(sIns.Instance.Name + "|" + sIns.X + "|" + sIns.Y);
                            }
                        }


                    }
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (((Message)e.UserState).ProgressBarValue > 1)
            {
                progressBar1.Value = ((Message)e.UserState).ProgressBarValue;
            }
            OutputWrite(((Message)e.UserState).Text, ((Message)e.UserState).ReplaceLastLine);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (progressBar1.Value == 100 && e.Cancelled == false)
            {

                button2.Enabled = true;
                if (_runGame)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                if (_closeAfterDone)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // cancel button
            Bgw.CancelAsync();
            if (!Bgw.IsBusy)
            {
                Close();
            }
        }


        private void GameCompiler_Shown(object sender, EventArgs e)
        {
            Bgw.RunWorkerAsync(new Message());
        }

        private void OutputLog_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (OutputLog.SelectedIndex != -1)
            {
                LineViewer lv = new LineViewer(OutputLog.SelectedItem.ToString());
                lv.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
