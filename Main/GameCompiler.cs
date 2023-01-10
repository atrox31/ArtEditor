using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ArtCore_Editor.AdvancedAssets.Instance_Manager;
using ArtCore_Editor.AdvancedAssets.SceneManager;
using ArtCore_Editor.AdvancedAssets.SpriteManager;
using ArtCore_Editor.Assets;
using ArtCore_Editor.Enums;
using ArtCore_Editor.etc;
using ArtCore_Editor.Functions;
using Newtonsoft.Json;

#pragma warning disable IDE0090

namespace ArtCore_Editor.Main
{
    public partial class GameCompiler : Form
    {
        private static GameCompiler _instance;


        private readonly bool _runGame;
        private readonly bool _closeAfterDone;
        private const string AssetPackFileName = "assets" + Program.FileExtensions_AssetPack;
        private const string GameDataFileName = "game" + Program.FileExtensions_GameDataPack; 
        private readonly List<string> _fileList = new List<string>();

        public GameCompiler(bool debugMode, bool runGame = false, bool closeAfterDone = false)
        {
            InitializeComponent();
            Program.ApplyTheme(this);
            _instance = this;
            _runGame = runGame;
            _closeAfterDone = closeAfterDone;
            button2.Visible = debugMode;
        }

        // object that is pass by background worker to show messages in log
        private class Message
        {
            public string Text { get; set; }
            public int ProgressBarValue { get; set; }
            public bool ReplaceLastLine { get; set; }


            public Message(string text, int progressBarValue, bool replaceLastLine)
            {
                Text = text;
                ReplaceLastLine = replaceLastLine;
                ProgressBarValue = progressBarValue;
            }
        }

        // write to compiler console
        private static void OutputWrite(string message, bool replaceLastLine = false)
        {
            if (replaceLastLine)
            {
                _instance.OutputLog.Items[^1] = message;
            }
            else
            {
                _instance.OutputLog.Items.Add(message);
            }
            _instance.OutputLog.SelectedIndex = _instance.OutputLog.Items.Count - 1;
        }
        private static bool CancelRequest(BackgroundWorker obj, DoWorkEventArgs e)
        {
            if (obj.CancellationPending != true) return false;
            e.Cancel = true;
            return true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bgw.ReportProgress(1, new Message("ArtCore Editor version " + Program.Version.ToString(), 1, false));

            {   // saving project
                Bgw.ReportProgress(1, new Message("Saving project", 2, false));
                GameProject.GetInstance().SaveToFile();
                Bgw.ReportProgress(1, new Message("Saving project ..done", -1, true));
                if (CancelRequest(Bgw, e)) return;

                // write platform files to Platform.dat
                Bgw.ReportProgress(1, new Message("Platform settings", 3, false));
                PreparePlatformSettings();
                Bgw.ReportProgress(1, new Message("Platform settings ..done", -1, true));
                if (CancelRequest(Bgw, e)) return;

                // game core files like default font, shaders etc.
                Bgw.ReportProgress(1, new Message("Prepare game file", 4, false));
                PrepareGameCoreFiles(Bgw, e);
                Bgw.ReportProgress(1, new Message("Prepare game file ..done", -1, true));
                if (CancelRequest(Bgw, e)) return;
            }
            {   // assets
                if (!PrepareAssets(Bgw, e, GameProject.GetInstance().Textures, "Textures", 10, 20)) return;
                if (CancelRequest(Bgw, e)) return;

                if (!PrepareAssets(Bgw, e, GameProject.GetInstance().Music, "Music", 20, 30)) return;
                if (CancelRequest(Bgw, e)) return;

                if (!PrepareAssets(Bgw, e, GameProject.GetInstance().Sounds, "Sounds", 30, 40)) return;
                if (CancelRequest(Bgw, e)) return;

                if (!PrepareAssets(Bgw, e, GameProject.GetInstance().Fonts, "Fonts", 40, 50)) return;
                if (CancelRequest(Bgw, e)) return;

                if (!CreateSpriteDefinitions(Bgw, e, 50, 65)) return;
                if (CancelRequest(Bgw, e)) return;

                // write all assets to list in assets.pak
                if (!ZipIO.WriteListToArchive(GameProject.ProjectPath + "\\" + AssetPackFileName, "filelist.txt",
                        _fileList,
                        true)) return;
                Bgw.ReportProgress(1, new Message("Asset prepare done", -1, false));
                if (CancelRequest(Bgw, e)) return;
            }
            {
                // object definitions
                Bgw.ReportProgress(1, new Message("Objects", 65, false));
                if (!CreateObjectDefinitions(Bgw, e)) return;
                Bgw.ReportProgress(1, new Message("Objects ..done", 75, false));
                if (CancelRequest(Bgw, e)) return;
            }
            {
                // scene definitions
                Bgw.ReportProgress(1, new Message("Scenes ", 75, false));
                if (!CreateSceneDefinitions(Bgw, e, 75, 99)) return;
                if (CancelRequest(Bgw, e)) return;
                Bgw.ReportProgress(1, new Message("Scenes ..done", -1, true));
            }

            Bgw.ReportProgress(1, new Message("Game ready", 100, false));
        }


        private bool PrepareAssets(BackgroundWorker sender, DoWorkEventArgs e, Dictionary<string, Asset> asset,
            string assetName, int progressMin, int progressMax, bool alwaysReplace = false)
        {
            string output = GameProject.ProjectPath + "\\" + AssetPackFileName;

            int maxCount = asset.Count();
            int currentItem = 0;
            int skipped = 0;
            sender.ReportProgress(1,
                new Message(assetName + " (" + currentItem.ToString() + "/" + maxCount.ToString() + ")", progressMin,
                    false));
            foreach (KeyValuePair<string, Asset> item in asset)
            {
                int currentProgress = Functions.Functions.Scale(currentItem, 0, maxCount, progressMin, progressMax);
                if (CancelRequest(sender, e)) return false;
               
                sender.ReportProgress(1,
                    new Message(assetName + " (" + (currentItem).ToString() + "/" + maxCount.ToString() + ") " + item.Value.Name,
                        currentProgress, true));

                if (!File.Exists(item.Value.GetFilePath()))
                {
                    sender.ReportProgress(1,
                        new Message("Asset type: '" + item.Value.Name + "' file not exists", currentProgress, false));
                    return false;
                }

                // check MD5 checksum
                string hashFile = $"DEBUG\\{item.Value.FileName}.MD5";
                string hash = ZipIO.ReadFromZip(output, hashFile, false)?.RemoveWhitespace();
                if (hash == null) return false; // zip file error
                if (hash == String.Empty || hash != item.Value.FileMd5 || alwaysReplace)
                {
                    // write asset content
                    if (!ZipIO.WriteFileToZipFile(
                        output,
                        assetName + "\\" + item.Value.FileName,
                        item.Value.GetFilePath(),
                        true
                    )) return false;
                    // write hash sum
                    if (!ZipIO.WriteLineToArchive(
                        output,
                        hashFile,
                        Functions.Functions.CalculateHash(item.Value.GetFilePath()),
                        true)) return false;
                }
                else
                {
                    skipped++;
                }
                // add to file list so engine can access by normal name
                _fileList.Add(assetName + "\\" + item.Value.FileName + ";" + item.Value.Name);
                if (CancelRequest(sender, e)) return false;
                
                sender.ReportProgress(1,
                    new Message(assetName + " (" + (++currentItem).ToString() + "/" + maxCount.ToString() + ") " + item.Value.Name,
                        currentProgress, true));
            }

            sender.ReportProgress(1,
                new Message(assetName + " (" + (currentItem).ToString() + "/" + maxCount.ToString() + ") ", progressMax,
                    true));
            sender.ReportProgress(1, new Message(skipped.ToString() + " skipped", progressMax, false));
            return true;
        }

        private bool CreateSpriteDefinitions(BackgroundWorker sender, DoWorkEventArgs e, int progress, int progressMax)
        {
            float progressStep = (float)(progressMax - progress) / (float)GameProject.GetInstance().Sprites.Count;
            float cProgress = (float)(progress);

            int currentItem = 0;
            foreach (KeyValuePair<string, Sprite> sprite in GameProject.GetInstance().Sprites)
            {
                sender.ReportProgress(1,
                    new Message(sprite.Key + " (" + (++currentItem).ToString() + "/" + GameProject.GetInstance().Sprites.Count.ToString() + ") ",
                        (int)cProgress, false));

                // save all sprite data to files
                string buffer = JsonConvert.SerializeObject(sprite.Value);
                if (!ZipIO.WriteLineToArchive(
                        GameProject.ProjectPath + "\\" + AssetPackFileName,
                        "Sprites\\" + sprite.Value.FileName,
                        buffer, true)) return false;

                foreach (string enumerateFile in Directory.EnumerateFiles( GameProject.ProjectPath + "\\" + sprite.Value.ProjectPath + "\\img\\" ))
                {
                    if (!ZipIO.WriteFileToZipFile(
                            GameProject.ProjectPath + "\\" + AssetPackFileName,
                            "Sprites\\" + sprite.Value.Name + "\\" + Path.GetFileName(enumerateFile),
                            enumerateFile,
                            true)) return false;
                }

                // add to file list so engine can access by normal name
                _fileList.Add("Sprites\\" + sprite.Value.FileName + ";" + sprite.Value.Name);
                if (CancelRequest(sender, e)) return false;
                cProgress += progressStep;
                sender.ReportProgress(1,
                    new Message(sprite.Key + " (" + (currentItem).ToString() + "/" + GameProject.GetInstance().Sprites.Count.ToString() + ") ...done",
                        (int)cProgress, true));
            }
            return true;
        }

        private static bool RunArtCompiler(BackgroundWorker sender, DoWorkEventArgs e, string outputFile, string inputs, bool quiet = false)
        {
            // check if compiler exists
            if (!File.Exists(Program.ProgramDirectory + "\\" + "Core\\ACompiler.exe"))
            {
                sender.ReportProgress(1, new Message("\"Core\\ACompiler.exe\" - file not found", -1, false));
                return false;
            }

            // check if library exists
            if (!File.Exists(Program.ProgramDirectory + "\\" + "Core\\AScript.lib"))
            {
                sender.ReportProgress(1, new Message("\"Core\\AScript.lib\" - file not found", -1, false));
                return false;
            }
            // write arguments
            string args = "-lib \"" + Program.ProgramDirectory + "\\" + "Core\\AScript.lib\" -output \"" + outputFile + "\" " + inputs;

            Process compiler = new Process();
            compiler.StartInfo.FileName = Program.ProgramDirectory + "\\" + "Core\\ACompiler.exe";
            compiler.StartInfo.Arguments = (quiet ? "-q " : "") + args;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.CreateNoWindow = true;
            //compiler.StartInfo.WorkingDirectory = GameProject.ProjectPath;
            compiler.Start();
            if (CancelRequest(sender, e)) return false;

            // ReSharper disable once MoveVariableDeclarationInsideLoopCondition
            string standardOutput;
            while ((standardOutput = compiler.StandardOutput.ReadLine()) != null)
            {
                sender.ReportProgress(1, new Message(standardOutput, -1, false));
                if (CancelRequest(sender, e))
                {
                    compiler.Kill();
                    return false;
                }
            }
            compiler.WaitForExit();

            if (compiler.ExitCode != 0) return false;
            return true;
        }

        private static bool CreateObjectDefinitions(BackgroundWorker sender, DoWorkEventArgs e)
        {
            // make sure if every object have main.asc by save all
            foreach (KeyValuePair<string, Instance> ins in GameProject.GetInstance().Instances)
            {
                ObjectManager obm = new ObjectManager(ins.Key);
                obm.WriteObjectCode();
                obm.Dispose();
            }
            if (CancelRequest(sender, e)) return false;

            // get all objects input
            string inputs = "";
            foreach (KeyValuePair<string, Instance> obj in GameProject.GetInstance().Instances)
            {
                inputs += "-obj \"" + GameProject.ProjectPath + "\\object\\" + obj.Key.ToString() + "\\main" + Program.FileExtensions_ArtCode + "\" ";
            }

            if (!RunArtCompiler(sender, e, GameProject.ProjectPath + "\\" + "object_compile" + Program.FileExtensions_CompiledArtCode, inputs)) return false;

            if (CancelRequest(sender, e)) return false;

            if (!ZipIO.WriteFileToZipFile(
                GameProject.ProjectPath + "\\" + GameDataFileName, 
                "object_compile" + Program.FileExtensions_CompiledArtCode, 
                GameProject.ProjectPath + "\\object_compile" + Program.FileExtensions_CompiledArtCode, 
                true)) return false;
            // cleanup
            Functions.Functions.FileDelete(GameProject.ProjectPath + "\\" + "object_compile" +
                                           Program.FileExtensions_CompiledArtCode);
            return true;
        }

        void UpdateCoreFiles(string folder, string file, int c, int max)
        {

            if (!ZipIO.WriteFileToZipFile(
                GameProject.ProjectPath + "\\" + GameDataFileName, 
                "files\\" + file,
                Program.ProgramDirectory + "\\" + folder + "\\" + file,
                false)) return;
            Bgw.ReportProgress(1, new Message("Prepare game file (" + c.ToString() + "/" + max.ToString() + ")", -1, true));
        }

        private void PrepareGameCoreFiles(BackgroundWorker obj, DoWorkEventArgs e)
        {
            {
                for (int i = 0; i < Program.coreFiles.Count; i++)
                {
                    UpdateCoreFiles("Core\\" + Program.coreFiles[i][0], Program.coreFiles[i][1], i, Program.coreFiles.Count);
                    if (CancelRequest(obj, e)) return;
                }
                if (File.Exists(GameProject.ProjectPath + "\\bg_img.png"))
                {
                    if (!ZipIO.WriteFileToZipFile(
                            GameProject.ProjectPath + "\\" + GameDataFileName,
                            "bg_img.png",
                            GameProject.ProjectPath + "\\bg_img.png",
                            true)) return;
                    if (CancelRequest(obj, e)) return;
                }
            }
        }

        private void PreparePlatformSettings()
        {
            List<string> content = new List<string>();
            foreach (PropertyInfo property in typeof(GameProject.ArtCorePreset).GetProperties())
            {
                int value = (int)property.GetValue(GameProject.GetInstance().ArtCoreDefaultSettings, null)!;
                content.Add(property.Name + "=" + value);
            }

            if (!ZipIO.WriteListToArchive(GameProject.ProjectPath + "\\" + "Platform.dat", "setup.ini", content,
                    true)) return;
        }

        private bool CreateSceneDefinitions(BackgroundWorker bgw, DoWorkEventArgs e, int currentProgress, int progressMax)
        {
            if (GameProject.GetInstance().Scenes.Count == 0) return false;
            float stepProgress = (float)(progressMax - currentProgress) / GameProject.GetInstance().Scenes.Count;
            float progress = (float)(currentProgress);

            foreach (KeyValuePair<string, Scene> scene in GameProject.GetInstance().Scenes)
            {
                bgw.ReportProgress(1, new Message("Scene: " + scene.Key, (int)progress, false));
                List<string> content = new List<string>
                {
                    "[setup]",
                    "Width=" + scene.Value.Width,
                    "Height=" + scene.Value.Height,
                    "BackGroundTexture=" + scene.Value.BackGroundTexture?.Name,
                    "BackGroundType=" + scene.Value.BackGroundType.ToString(),
                    "BackGroundWrapMode=" + scene.Value.BackGroundWrapMode,
                    "BackGroundColor=" + scene.Value.BackGroundColor.ColorToHex(),
                    "SceneStartingTrigger=" + scene.Value.SceneStartingTrigger,
                    "[regions]"
                };

                foreach (Scene.Region regions in scene.Value.Regions)
                {
                    content.Add(regions.ToString());
                }

                content.Add("[triggers]");
                foreach (string triggers in scene.Value.SceneTriggers)
                {
                    content.Add(triggers);
                }

                content.Add("[instance]");
                foreach (SceneManager.SceneInstance sIns in scene.Value.SceneInstances)
                {
                    content.Add(sIns.Instance.Name + "|" + sIns.X + "|" + sIns.Y);
                }
                
                // gui events
                bool haveGuiTriggers = false;
                string guiTriggersContent = "object scene_"+scene.Value.Name + "\n";
                foreach (string enumerateFile in Directory.EnumerateFiles(
                             GameProject.ProjectPath + "\\" + "scene" + "\\" + scene.Value.Name + "\\", "*" + Program.FileExtensions_ArtCode))
                {
                    haveGuiTriggers = true;
                    guiTriggersContent += "define " + Path.GetFileNameWithoutExtension(enumerateFile) + "\n";
                }
                foreach (Variable item in scene.Value.SceneVariables)
                {
                    guiTriggersContent += "local " + item.Type.ToString().ToLower()["vtype".Length..] + " " + item.Name + "\n";
                }
                guiTriggersContent += "@end\n";

                if (haveGuiTriggers || scene.Value.SceneVariables.Count > 0)
                {
                    if (scene.Value.SceneVariables.Count > 0)
                    {
                        guiTriggersContent += "function scene_" + scene.Value.Name + ":" + "DEF_VALUES" + "\n";
                        foreach (Variable item in scene.Value.SceneVariables.Where(item => item.Default is
                                 {
                                     Length: > 0
                                 }))
                        {
                            switch (item.Type)
                            {
                                case Variable.VariableType.VTypeObject:
                                case Variable.VariableType.VTypeScene:
                                    // can not
                                    break;
                                case Variable.VariableType.VTypeSprite:
                                    guiTriggersContent += $"{item.Name} := get_sprite(\"{item.Default}\")\n";
                                    break;
                                case Variable.VariableType.VTypeTexture:
                                    guiTriggersContent += $"{item.Name} := get_texture(\"{item.Default}\")\n";
                                    break;
                                case Variable.VariableType.VTypeSound:
                                    guiTriggersContent += $"{item.Name} := get_sound(\"{item.Default}\")\n";
                                    break;
                                case Variable.VariableType.VTypeMusic:
                                    guiTriggersContent += $"{item.Name} := get_music(\"{item.Default}\")\n";
                                    break;
                                case Variable.VariableType.VTypeFont:
                                    guiTriggersContent += $"{item.Name} := get_font(\"{item.Default}\")\n";
                                    break;
                                case Variable.VariableType.VTypePoint:
                                    string[] pt = item.Default.Split(':');
                                    guiTriggersContent += $"{item.Name} := new_point( {pt[0]}, {pt[1]})\n";
                                    break;
                                case Variable.VariableType.VTypeRectangle:
                                    break;
                                default:
                                    guiTriggersContent += item.Name + " := " + item.Default + "\n";
                                    break;
                            }
                        }
                        guiTriggersContent += "@end\n";
                    }
                    foreach (string enumerateFile in Directory.EnumerateFiles(
                                 GameProject.ProjectPath + "\\" + "scene" + "\\" + scene.Value.Name + "\\",
                                 "*" + Program.FileExtensions_ArtCode))
                    {
                        guiTriggersContent += "function scene_" + scene.Value.Name + ":" + Path.GetFileNameWithoutExtension(enumerateFile) + "\n"; ;
                        guiTriggersContent += File.ReadAllText(enumerateFile);
                        guiTriggersContent += "\n";
                        guiTriggersContent += "@end\n";
                    }

                    string tmpFilePath = GameProject.ProjectPath + "\\" + "tmp_scene_triggers" + Program.FileExtensions_ArtCode;
                    File.WriteAllText(tmpFilePath, guiTriggersContent);
                    if (!RunArtCompiler(bgw, e, GameProject.ProjectPath + "\\" + "scene_triggers" + Program.FileExtensions_CompiledArtCode,
                            "-obj " + tmpFilePath, true)) return false;

                    Functions.Functions.FileDelete(tmpFilePath);

                    if (CancelRequest(bgw, e)) return false;

                    if (!ZipIO.WriteFileToZipFile(
                        GameProject.ProjectPath + "\\" + GameDataFileName,
                        "scene\\" + scene.Key + "\\" + "scene_triggers" + Program.FileExtensions_CompiledArtCode,
                        GameProject.ProjectPath + "\\" + "scene_triggers" + Program.FileExtensions_CompiledArtCode,
                        true
                    )) return false;
                    Functions.Functions.FileDelete(GameProject.ProjectPath + "\\" + "scene_triggers" + Program.FileExtensions_CompiledArtCode);
                }

                if (CancelRequest(bgw, e)) return false;
                if (!ZipIO.WriteListToArchive(
                        GameProject.ProjectPath + "\\" + GameDataFileName,
                        "scene\\" + scene.Key + "\\" + scene.Key + "" + Program.FileExtensions_SceneObject,
                        content,
                        true
                    )) return false;

                if (CancelRequest(bgw, e)) return false;
                if (File.Exists(GameProject.ProjectPath + "\\" + "scene\\" + scene.Key + "\\" + "GuiSchema.json"))
                {
                    if (!ZipIO.WriteFileToZipFile(
                            GameProject.ProjectPath + "\\" + GameDataFileName,
                            "scene\\" + scene.Key + "\\GuiSchema.json",
                            GameProject.ProjectPath + "\\" + "scene\\" + scene.Key + "\\" + "GuiSchema.json",
                            true
                        )) return false;
                }

                if (CancelRequest(bgw, e)) return false;
                
                progress += stepProgress;
                bgw.ReportProgress(1, new Message("Scene: " + scene.Key + "... done", (int)progress, true));
            }
            // all scenes prepared, now make list and write first scene
            if (!ZipIO.WriteListToArchive(GameProject.ProjectPath + "\\" + GameDataFileName, "scene\\list.txt",
                    GameProject.GetInstance().Scenes.Keys.ToList(), true)) return false;

            // write first scene, if not set get first
            if (!ZipIO.WriteLineToArchive(GameProject.ProjectPath + "\\" + GameDataFileName, "scene\\StartingScene.txt",
                     (GameProject.GetInstance().StartingScene.Length > 0 ?
                         GameProject.GetInstance().StartingScene :
                         GameProject.GetInstance().Scenes.Values.First().Name)
                     , true)) return false;

            return true;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ((((Message)e.UserState)!).ProgressBarValue > 1)
            {
                progressBar1.Value = ((Message)e.UserState).ProgressBarValue;
            }
            OutputWrite(((Message)e.UserState).Text, ((Message)e.UserState).ReplaceLastLine);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (progressBar1.Value != 100 || e.Cancelled)
            {
                OutputWrite("Error while preparing game files.", false);
                return;
            }
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
            Bgw.RunWorkerAsync();
        }

        private void OutputLog_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (OutputLog.SelectedIndex != -1)
            {
                string message = OutputLog.SelectedItem.ToString();
                if (message.StartsWith("Error at "))
                {
                    // art compiler error
                    string[] parms = message.Split('\'');
                    //Error at line: '1' in Object: 'obj_wall' Function: 'EvOnCreate' - Message: Unexpected token vertival[745]
                    // 0              1 2            3        4           5          6    
                    //Error at line: |1| in Object: |obj_wall| Function: |EvOnCreate| - Message: Unexpected token vertival[745]
                    if (parms.Length >= 7)
                    {
                        int Line = Convert.ToInt32(parms[1]);
                        string Object = parms[3];
                        string Function = parms[5];
                        if (Object.StartsWith("scene_"))
                        {
                            // scene error
                            return;
                        }
                        else 
                        { // instance object
                            ObjectManager objmng = new ObjectManager(Object, Line, Function);
                            if (objmng.ShowDialog() == DialogResult.OK)
                            {
                                // restart compiler
                                OutputLog.Items.Clear();
                                Bgw.RunWorkerAsync();
                                return;
                            }
                        }

                    }
                }

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
