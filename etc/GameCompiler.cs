﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

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
            public Message(string Text, int ProgressBarValue, bool ReplaceLastLine)
            {
                this.Text = Text;
                this.ReplaceLastLine = ReplaceLastLine;
                this.ProgressBarValue = ProgressBarValue;
            }
        }

        public static void OutputWrite(string message, bool replace_last_line = false)
        {
            if (replace_last_line)
            {
                _instance.OutputLog.Items[_instance.OutputLog.Items.Count - 1] = message;
            }
            else
            {
                _instance.OutputLog.Items.Add(message);
            }
        }
        public static bool isdebug;
        public GameCompiler(bool debug)
        {
            InitializeComponent();Program.ApplyTheme(this);
            _instance = this;
            isdebug = debug;
        }
        public static void PrepareAssets<T>(BackgroundWorker sender, DoWorkEventArgs e, Dictionary<string, T> asset, int progress_min, int progress_max)
        {
            string output = GameProject.ProjectPath + "\\" + "assets.pak";

            int max_count = asset.Count();
            int current_item = 0;
            sender.ReportProgress(1, new Message((typeof(T)).Name + " (" + current_item.ToString() + "/" + max_count.ToString() + ")", progress_min, false));
            foreach (var item in asset)
            {
                int current_progress = Functions.Scale(current_item, 0, max_count, progress_min, progress_max);
                if (CancelRequest(sender, e)) return;

                string Name = (string)(typeof(T).GetProperty("Name").GetValue(item.Value, null));
                string File_MD5 = (string)(typeof(T).GetProperty("File_MD5").GetValue(item.Value, null));
                string FileName = (string)(typeof(T).GetProperty("FileName").GetValue(item.Value, null));

                sender.ReportProgress(1, new Message((typeof(T)).Name + " (" + (current_item).ToString() + "/" + max_count.ToString() + ") " + Name, current_progress, true));

                if (!File.Exists(GameProject.ProjectPath + "\\" + FileName))
                {
                    sender.ReportProgress(1, new Message("Asset type: '" + asset + "' file not exists", current_progress, false));
                    return;
                }
                if (File_MD5 == null)
                {
                    File_MD5 = Functions.CalculateMD5(GameProject.ProjectPath + "\\" + FileName);
                    typeof(T).GetProperty("File_MD5").SetValue(item.Value, File_MD5);
                }

                // check MD5 cheksum
                using (FileStream zipToOpen = new FileStream(output, FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        string MD5 = null;
                        if (archive.GetEntry(FileName + ".MD5") != null)
                        {
                            MD5 = new StreamReader(archive.GetEntry(FileName + ".MD5").Open()).ReadToEnd();
                        }
                        else
                        {
                            ZipArchiveEntry readmeEntry = archive.CreateEntry(FileName + ".MD5");
                            using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                            {
                                writer.Write(File_MD5);
                            }

                        }
                        if (MD5 == null || MD5 != File_MD5)
                        {
                            ZipArchiveEntry FileEntry = archive.CreateEntry(FileName);
                            using (StreamWriter writer = new StreamWriter(FileEntry.Open()))
                            {
                                using (StreamReader file = new StreamReader(GameProject.ProjectPath + "\\" + FileName))
                                {
                                    file.BaseStream.CopyTo(writer.BaseStream);
                                }

                            }
                        }
                        // else file ok
                    }
                }
                sender.ReportProgress(1, new Message((typeof(T)).Name + " (" + (++current_item).ToString() + "/" + max_count.ToString() + ") " + Name, current_progress, true));
            }
            sender.ReportProgress(1, new Message((typeof(T)).Name + " (" + (current_item).ToString() + "/" + max_count.ToString() + ") ", progress_max, true));
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

        public static void CreateObjectDefinitions(BackgroundWorker sender, DoWorkEventArgs e)
        {
            string inputs = "";
            foreach (var obj in GameProject.GetInstance().Instances)
            {
                inputs += "-obj \"" + GameProject.ProjectPath + "\\object\\" + obj.Key.ToString() + "\\main.asc\" ";
            }
            string args = "-lib \"" + GameProject.ProjectPath + "\\AScript.lib\" -output \"" + GameProject.ProjectPath + "\\object_compile.acp\" " + inputs;

            Process compiler = new Process();
            compiler.StartInfo.FileName = "D:\\projekt\\ACompiler\\x64\\Debug\\ACompiler.exe";
            compiler.StartInfo.Arguments = args;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;
            compiler.Start();

            string standard_output;
            while ((standard_output = compiler.StandardOutput.ReadLine()) != null)
            {
                sender.ReportProgress(1, new Message(standard_output, -1, false));
            }

            compiler.WaitForExit();
            if (File.Exists(GameProject.ProjectPath + "\\object_compile.acp"))
            {

                using (FileStream zipToOpen = new FileStream(GameProject.ProjectPath + "\\" + "game.dat", FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {

                        if (CancelRequest(sender, e)) return;
                        ZipArchiveEntry readmeEntry = archive.GetEntry("object_compile.acp");
                        if (readmeEntry == null)
                        {
                            readmeEntry = archive.CreateEntry("object_compile.acp");
                        }
                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            using (StreamReader file = new StreamReader(GameProject.ProjectPath + "\\object_compile.acp"))
                            {
                                file.BaseStream.CopyTo(writer.BaseStream);
                            }


                        }
                    }
                }
            }
            else
            {
                //TODO: error
            }

        }

        void UpdateCoreFiles(string folder, string File, int c, int max)
        {
            using (FileStream zipToOpen = new FileStream(GameProject.ProjectPath + "\\" + "game.dat", FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry readmeEntry = archive.GetEntry("files\\" + File);
                    if (readmeEntry == null)
                    {
                        readmeEntry = archive.CreateEntry("files\\" + File);
                    }
                    using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                    {
                        using (StreamReader file = new StreamReader(GameProject.ProjectPath + "\\" + folder + "\\" + File))
                        {
                            file.BaseStream.CopyTo(writer.BaseStream);
                        }
                    }



                }
            }

            Bgw.ReportProgress(1, new Message("Prepare game file (" + c.ToString() + "/" + max.ToString() + ")", -1, true));
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bgw.ReportProgress(1, new Message("ArtCore Editor version " + Program.VERSION.ToString(), 1, false));

            // saving project
            Bgw.ReportProgress(1, new Message("Saving project", 2, false));
            GameProject.GetInstance().SaveToFile();
            Bgw.ReportProgress(1, new Message("Saving project ..done", 3, true));
            if (CancelRequest(Bgw, e)) return;

            // preparing
            PrepareAssets(Bgw, e, GameProject.GetInstance().Textures, 10, 20);
            if (CancelRequest(Bgw, e)) return;

            PrepareAssets(Bgw, e, GameProject.GetInstance().Sprites, 20, 30);
            if (CancelRequest(Bgw, e)) return;

            PrepareAssets(Bgw, e, GameProject.GetInstance().Music, 30, 40);
            if (CancelRequest(Bgw, e)) return;

            PrepareAssets(Bgw, e, GameProject.GetInstance().Sounds, 40, 50);
            if (CancelRequest(Bgw, e)) return;

            PrepareAssets(Bgw, e, GameProject.GetInstance().Fonts, 50, 55);
            if (CancelRequest(Bgw, e)) return;

            Bgw.ReportProgress(1, new Message("Asset prepare complite", -1, false));

            Bgw.ReportProgress(1, new Message("Prepare game file", -1, false));
            /* 
	            []files
		            gamecontrollerdb.txt
		            TitilliumWeb-Light.ttf
		            bloom.frag
		            color.frag
		            common.vert
*	            []object_definitions.sdsd
*	            []scene_definitions
		            scene1.obj
		            ..
		            sceneX.obj
*	            syntax.txt
*	            setup.ini
            */

            {
                List<string[]> coreFiles = new List<string[]>()
                {
                    new string[]{"", "AScript.lib" },
                    new string[]{"pack", "gamecontrollerdb.txt" },
                    new string[]{"pack", "TitilliumWeb-Light.ttf" },
                    new string[]{"pack", "bloom.frag" },
                    new string[]{"pack", "color.frag" },
                    new string[]{"pack", "common.vert" },
                };
                int c = 1;
                foreach (var item in coreFiles)
                {
                    UpdateCoreFiles(item[0], item[1], c++, coreFiles.Count());
                    if (CancelRequest(Bgw, e)) return;
                }
            }
            Bgw.ReportProgress(1, new Message("Prepare game file ..done", 60, true));

            // setup.ini
            Bgw.ReportProgress(1, new Message("Game settings", 60, false));
            using (FileStream zipToOpen = new FileStream(GameProject.ProjectPath + "\\" + "game.dat", FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry readmeEntry = archive.GetEntry("setup.ini");
                    if (readmeEntry == null)
                    {
                        readmeEntry = archive.CreateEntry("setup.ini");
                    }
                    using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                    {
                        foreach (PropertyInfo property in typeof(GameProject.ArtCorePreset).GetProperties())
                        {
                            var f_name = property.Name;
                            int value = (int)property.GetValue(GameProject.GetInstance().ArtCoreDefaultSettings, null);
                            writer.WriteLine(f_name + "=" + value);
                        }
                    }


                }
            }
            if (CancelRequest(Bgw, e)) return;
            Bgw.ReportProgress(1, new Message("Game settings ..done", 65, true));

            // object definitions
            if (CancelRequest(Bgw, e)) return;
            Bgw.ReportProgress(1, new Message("Objects", 70, false));
            CreateObjectDefinitions(Bgw, e);
            Bgw.ReportProgress(1, new Message("Objects ..done", 90, false));


            // scene definitions
            if (CancelRequest(Bgw, e)) return;
            Bgw.ReportProgress(1, new Message("Scenes ", 91, false));
            CreateSceneDefinitions(Bgw, e);
            Bgw.ReportProgress(1, new Message("Scenes ..done", 99, true));

            if (CancelRequest(Bgw, e)) return;
            Bgw.ReportProgress(1, new Message("Starting game", 100, false));
        }

        private void CreateSceneDefinitions(BackgroundWorker bgw, DoWorkEventArgs e)
        {
            foreach (var scene in GameProject.GetInstance().Scenes)
                using (FileStream zipToOpen = new FileStream(GameProject.ProjectPath + "\\" + "game.dat", FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        if (archive.GetEntry("scene_definitions\\" + scene.Key + ".asd") == null)
                        {
                            ZipArchiveEntry readmeEntry = archive.CreateEntry("scene_definitions\\" + scene.Key + ".asd");
                            using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                            {
                                writer.WriteLine("[instance]");
                                foreach (var sIns in scene.Value.SceneInstances)
                                {
                                    writer.WriteLine(sIns.instance.Name + "|" + sIns.x + "|" + sIns.y);
                                }
                                writer.WriteLine("[end]");
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
    }
}
