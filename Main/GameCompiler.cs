using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using ArtCore_Editor.AdvancedAssets.Instance_Manager;
using ArtCore_Editor.AdvancedAssets.Instance_Manager.Behavior;
using ArtCore_Editor.AdvancedAssets.SceneManager;
using ArtCore_Editor.AdvancedAssets.SpriteManager;
using ArtCore_Editor.Assets;
using ArtCore_Editor.Enums;
using ArtCore_Editor.etc;
using ArtCore_Editor.Functions;
using Newtonsoft.Json;

using static System.Formats.Asn1.AsnWriter;

#pragma warning disable IDE0090

namespace ArtCore_Editor.Main
{
    public partial class GameCompiler : Form
    {
        private static GameCompiler _instance;


        private readonly bool _runGame;
        private readonly bool _closeAfterDone;
        private const string AssetPackFileName = "output\\assets" + Program.FileExtensions_AssetPack;
        private const string GameDataFileName = "output\\game" + Program.FileExtensions_GameDataPack; 
        private const string PlatformFileName = "output\\Platform" + Program.FileExtensions_GameDataPack; 
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
            if (!obj.CancellationPending) return false;
            e.Cancel = true;
            return true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bgw.ReportProgress(1, new Message("ArtCore Editor version " + Program.Version.ToString(), 1, false));

            {   // saving project
                Bgw.ReportProgress(1, new Message("Saving project", 2, false));
                    Invoke(new Action(GameProject.GetInstance().SaveToFile));
                Bgw.ReportProgress(1, new Message("Saving project ..done", -1, true));
                if (CancelRequest(Bgw, e)) return;

                // write platform files to Platform.dat
                Bgw.ReportProgress(1, new Message("Platform settings", 3, false));
                    if (!PreparePlatformSettings()) return;
                Bgw.ReportProgress(1, new Message("Platform settings ..done", -1, true));
                if (CancelRequest(Bgw, e)) return;

                // game core files like default font, shaders etc.
                Bgw.ReportProgress(1, new Message("Prepare game file", 4, false));
                    if(!PrepareGameCoreFiles(Bgw, e)) return;
                Bgw.ReportProgress(1, new Message("Prepare game file ..done", -1, true));
                if (CancelRequest(Bgw, e)) return;
            }
            {   // assets
                if (!PrepareAssets(Bgw, e, GameProject.GetInstance().Textures, "Texture", 10, 20)) return;
                if (CancelRequest(Bgw, e)) return;

                if (!PrepareAssets(Bgw, e, GameProject.GetInstance().Music, "Music", 20, 30)) return;
                if (CancelRequest(Bgw, e)) return;

                if (!PrepareAssets(Bgw, e, GameProject.GetInstance().Sounds, "Sound", 30, 40)) return;
                if (CancelRequest(Bgw, e)) return;

                if (!PrepareAssets(Bgw, e, GameProject.GetInstance().Fonts, "Font", 40, 50)) return;
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

            int maxCount = asset.Count;
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
                        "Sprite\\" + sprite.Value.FileName,
                        buffer, true)) return false;
                
                for(int i=0; i<sprite.Value.SpriteFrames; i++)
                {
                    string frameName = i.ToString() + ".png";

                    if (!ZipIO.CopyImageToArchive(
                 /*ZipArchiveInput*/  StringExtensions.Combine(GameProject.ProjectPath, sprite.Value.DataPath),
                 /*EntryNameInput*/   frameName,
                 /*ZipArchiveOutput*/ StringExtensions.Combine(GameProject.ProjectPath, AssetPackFileName),
                 /*EntryNameOutput*/  StringExtensions.Combine("Sprite",sprite.Value.Name, frameName)
                        ))
                        return false;
                }

                // add to file list so engine can access by normal name
                _fileList.Add("Sprite\\" + sprite.Value.FileName + ";" + sprite.Value.Name);
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

            return (compiler.ExitCode == 0);
        }

        private static string WriteObjectCode(
            string objectName,
            List<Variable> variables,
            List<string> functions,
            string defaultValues,
            StringBuilder eventData
            )
        {
            StringBuilder content = new StringBuilder();

            content.Append("object " + objectName + "\n");
            foreach (string item in functions)
            {
                content.Append("define " + item + "\n");
            }
            foreach (Variable item in variables)
            {
                content.Append(
                    "local " + item.Type.ToString().ToLower()["vtype".Length..] + " " + item.Name + "\n");
            }
            content.Append("@end\n");

            List<string> defaultValuesList = new List<string>();
            if(defaultValues != null) 
                defaultValuesList.Add(defaultValues);
            foreach (Variable item in variables.Where(item => !string.IsNullOrEmpty(item.Default)))
            {
                switch (item.Type)
                {
                    case Variable.VariableType.VTypeObject:
                        // can not
                        break;
                    case Variable.VariableType.VTypeScene:
                        // can not
                        break;
                    case Variable.VariableType.VTypeSprite:
                        defaultValuesList.Add($"{item.Name} := get_sprite(\"{item.Default}\")\n");
                        break;
                    case Variable.VariableType.VTypeTexture:
                        defaultValuesList.Add($"{item.Name} := get_texture(\"{item.Default}\")\n");
                        break;
                    case Variable.VariableType.VTypeSound:
                        defaultValuesList.Add($"{item.Name} := get_sound(\"{item.Default}\")\n");
                        break;
                    case Variable.VariableType.VTypeMusic:
                        defaultValuesList.Add($"{item.Name} := get_music(\"{item.Default}\")\n");
                        break;
                    case Variable.VariableType.VTypeFont:
                        defaultValuesList.Add($"{item.Name} := get_font(\"{item.Default}\")\n");
                        break;
                    case Variable.VariableType.VTypePoint:
                        {
                            string[] pt = item.Default.Split(':');
                            if (pt.Length == 2)
                                defaultValuesList.Add($"{item.Name} := new_point( {pt[0]}, {pt[1]})\n");
                        }
                        break;
                    case Variable.VariableType.VTypeRectangle:
                        {
                            string[] pt = item.Default.Split(':');
                            if (pt.Length == 4)
                                defaultValuesList.Add(
                                    $"{item.Name} := new_rectangle( {pt[0]}, {pt[1]}, {pt[2]}, {pt[3]})\n");
                        }
                        break;
                    case Variable.VariableType.VTypeBool:
                        defaultValuesList.Add(item.Name + " := " + item.Default.ToLower() + "\n");
                        break;
                    case Variable.VariableType.VTypeNull:
                    case Variable.VariableType.VTypeInt:
                    case Variable.VariableType.VTypeFloat:
                    case Variable.VariableType.VTypeInstance:
                    case Variable.VariableType.VTypeString:
                    case Variable.VariableType.VTypeColor:
                    case Variable.VariableType.VTypeEnum:
                    default:
                        defaultValuesList.Add(item.Name + " := " + item.Default + "\n");
                        break;
                }
            }

            if (defaultValuesList.Count > 0)
            {
                content.Append($"function {objectName}:DEF_VALUES\n");
                foreach (string defaultValue in defaultValuesList)
                {
                    content.Append(defaultValue);
                }
                content.Append("\n@end\n");
            }

            content.Append(eventData);

            //File.WriteAllText(GameProject.ProjectPath + "\\" + objectName + ".txt", content.ToString());
            
            return content.ToString();
        }
        private static string WriteSceneCode(Scene scene)
        {
            // get events
            StringBuilder eventData = new StringBuilder();

            // scene triggers
            foreach (string enumerateFile in Directory.EnumerateFiles(
                         GameProject.ProjectPath + "\\" + "scene" + "\\" + scene.Name + "\\",
                         "*" + Program.FileExtensions_ArtCode))
            {
                eventData.Append("function scene_" + scene.Name + ":" + Path.GetFileNameWithoutExtension(enumerateFile) + "\n");
                eventData.Append(File.ReadAllText(enumerateFile));
                eventData.Append('\n');
                eventData.Append("@end\n");
            }

            // level triggers
            foreach (string levelPath in Directory.EnumerateFiles(StringExtensions.Combine(
                         GameProject.ProjectPath, scene.ProjectPath,
                         "levels"), "*" + Program.FileExtensions_SceneLevel))
            {
                List<string> triggerList = ZipIO.ReadFromZip(
                        levelPath, "triggers.txt")
                    /* get list of triggers */  .Split('\n').ToList();

                foreach (string trigger in triggerList)
                {
                    string triggerContent = ZipIO.ReadFromZip(levelPath, trigger, true);
                    if (string.IsNullOrEmpty(triggerContent)) continue;
                    eventData.Append($"function {levelPath.WithoutExtension()}_{scene.Name}:{trigger.WithoutExtension()}\n");
                    eventData.Append(triggerContent);
                    eventData.Append('\n');
                    eventData.Append("@end\n");
                }
            }

            return WriteObjectCode(
                "scene_" + scene.Name,
                scene.SceneVariables,
                Directory.EnumerateFiles(
                    GameProject.ProjectPath + "\\" + "scene" + "\\" + scene.Name + "\\",
                    "*" + Program.FileExtensions_ArtCode)
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToList(),
                null,
                eventData
            );
        }


        private static string WriteObjectCode(Instance currentObject)
        {
            // get body type
            string instanceBody = (currentObject.Sprite == null
                ? ""
                : $"set_self_sprite(get_sprite(\"{currentObject.Sprite.Name}\"))\n");

            instanceBody += currentObject.BodyDataType.Type switch
            {
                Instance.BodyData.BType.None => $"instance_set_body_none()",
                Instance.BodyData.BType.Circle => $"instance_set_body_as_circle({currentObject.BodyDataType.Value1})",
                Instance.BodyData.BType.Rect => $"instance_set_body_as_rect({currentObject.BodyDataType.Value1}, {currentObject.BodyDataType.Value2})",
                _ => "instance_set_body_from_sprite()",
            };

            // get events
            StringBuilder eventData = new StringBuilder();
            // value is only string name of target event
            foreach (KeyValuePair<Event.EventType, string> item in currentObject.Events)
            {
                string pathToObjectData = StringExtensions.Combine(
                    GameProject.ProjectPath, currentObject.ProjectPath,
                    item.Value + Program.FileExtensions_ArtCode);

                if (!File.Exists(pathToObjectData)) continue;

                eventData.Append("function " + currentObject.Name + ":" + item.Value + "\n");
                eventData.Append(File.ReadAllText(pathToObjectData));
                eventData.Append("\n@end\n");
            }

            return WriteObjectCode(
                currentObject.Name,
                currentObject.Variables,
                currentObject.Events.Values.ToList(),
                instanceBody,
                eventData
            );
        }
        private static bool CreateObjectDefinitions(BackgroundWorker sender, DoWorkEventArgs e)
        {
            // get all objects input
            string inputs = "";
            foreach (KeyValuePair<string, Instance> obj in GameProject.GetInstance().Instances)
            {
                string pathToObjectData = GameProject.ProjectPath + "\\object\\" + obj.Key.ToString() + "\\main" +
                                          Program.FileExtensions_ArtCode;

                File.WriteAllText(pathToObjectData, WriteObjectCode(obj.Value));

                inputs += "-obj \"" + pathToObjectData + "\" ";
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

        private void CopyCoreFiles(string folder, string file, int c, int max)
        {

            if (!ZipIO.WriteFileToZipFile(
                GameProject.ProjectPath + "\\" + GameDataFileName, 
                "files\\" + file,
                Program.ProgramDirectory + "\\" + folder + "\\" + file,
                false)) return;
            Bgw.ReportProgress(1, new Message("Prepare game file (" + c.ToString() + "/" + max.ToString() + ")", -1, true));
        }

        private bool PrepareGameCoreFiles(BackgroundWorker obj, DoWorkEventArgs e)
        {
            {
                for (int i = 0; i < Program.coreFiles.Count; i++)
                {
                    CopyCoreFiles("Core\\" + Program.coreFiles[i][0], Program.coreFiles[i][1], i, Program.coreFiles.Count);
                }

                if (CancelRequest(obj, e)) return false;

                if (File.Exists(GameProject.ProjectPath + "\\bg_img.png"))
                {
                    if (!ZipIO.WriteFileToZipFile(
                            GameProject.ProjectPath + "\\" + GameDataFileName,
                            "bg_img.png",
                            GameProject.ProjectPath + "\\bg_img.png",
                            true)) return false;
                }
            }
            return true;
        }

        private bool PreparePlatformSettings()
        {
            // prepare nice format ini file
            List<string> content = (
                from DictionaryEntry dictionaryEntry 
                in GameProject.GetInstance().UserProperties 
                select dictionaryEntry.Key + "=" + dictionaryEntry.Value
                ).ToList();

            // save to archive
            return ZipIO.WriteListToArchive(GameProject.ProjectPath + "\\" + PlatformFileName, "setup.ini", content,
                true);
        }

        private bool CreateSceneDefinitions(BackgroundWorker bgw, DoWorkEventArgs e, int currentProgress, int progressMax)
        {
            if (GameProject.GetInstance().Scenes.Count == 0) return false;
            string zipArchiveName = StringExtensions.Combine(GameProject.ProjectPath, GameDataFileName);
            float stepProgress = (float)(progressMax - currentProgress) / GameProject.GetInstance().Scenes.Count;
            float progress = (float)(currentProgress);

            foreach (KeyValuePair<string, Scene> scene in GameProject.GetInstance().Scenes)
            {
                bgw.ReportProgress(1, new Message("Scene: " + scene.Key, (int)progress, false));
                List<string> content = new List<string>
                {
                    "[setup]",
                    "ViewWidth=" + scene.Value.ViewWidth,
                    "ViewHeight=" + scene.Value.ViewHeight,
                    "EnableCamera=" + (scene.Value.EnableCamera ? "1" : "0"),
                    "BackGroundTexture=" + scene.Value.BackGroundTexture?.Name,
                    "BackGroundType=" + scene.Value.BackGroundType.ToString(),
                    "BackGroundWrapMode=" + scene.Value.BackGroundWrapMode,
                    "BackGroundColor=" + scene.Value.BackGroundColor.ColorToHex(),
                    "SceneStartingTrigger=" + scene.Value.SceneStartingTrigger
                };

                content.Add("[regions]");
                content.AddRange(scene.Value.Regions.Select(regions => regions.ToString()));

                content.Add("[triggers]");
                content.AddRange(scene.Value.SceneTriggers.Select(triggers => triggers.ToString()));

                content.Add("[instance]");
                content.AddRange(scene.Value.SceneInstances.Select(instance => instance.ToString()));

                // gui events
                
                {
                    string tmpFilePath = GameProject.ProjectPath + "\\" + "tmp_scene_triggers" + Program.FileExtensions_ArtCode;
                    File.WriteAllText(tmpFilePath, WriteSceneCode( scene.Value ));
                    if (!RunArtCompiler(bgw, e, GameProject.ProjectPath + "\\" + "scene_triggers" + Program.FileExtensions_CompiledArtCode,
                            "-obj " + tmpFilePath, true)) return false;

                    Functions.Functions.FileDelete(tmpFilePath);

                    if (CancelRequest(bgw, e)) return false;

                    if (!ZipIO.WriteFileToZipFile(
                            zipArchiveName,
                        "scene\\" + scene.Key + "\\" + "scene_triggers" + Program.FileExtensions_CompiledArtCode,
                        GameProject.ProjectPath + "\\" + "scene_triggers" + Program.FileExtensions_CompiledArtCode,
                        true
                    )) return false;
                    Functions.Functions.FileDelete(GameProject.ProjectPath + "\\" + "scene_triggers" + Program.FileExtensions_CompiledArtCode);
                }

                // pack scene file
                if (CancelRequest(bgw, e)) return false;
                if (!ZipIO.WriteListToArchive(
                        zipArchiveName,
                        "scene\\" + scene.Key + "\\" + scene.Key + "" + Program.FileExtensions_SceneObject,
                        content,
                        true
                    )) return false;

                // pack gui schema
                if (CancelRequest(bgw, e)) return false;
                if (File.Exists(StringExtensions.Combine(GameProject.ProjectPath, "scene", scene.Key, "GuiSchema.json")))
                {
                    if (!ZipIO.WriteFileToZipFile(
                            zipArchiveName,
                            $"scene\\{scene.Key}\\GuiSchema.json",
                            StringExtensions.Combine(GameProject.ProjectPath, "scene", scene.Key, "GuiSchema.json"),
                            true
                        )) return false;
                }

                // pack all level data
                if (CancelRequest(bgw, e)) return false;
                foreach (string levelPath in Directory.EnumerateFiles( StringExtensions.Combine(
                    GameProject.ProjectPath, scene.Value.ProjectPath,
                    "levels"), "*" + Program.FileExtensions_SceneLevel))
                {
                    ZipIO.WriteToZipArchiveFromStream(
                        zipArchiveName,
                        $"scene\\{scene.Key}\\instances_{levelPath.WithoutExtension()}.txt",
                        ZipIO.ReadStreamFromArchive(
                            levelPath, "instances.txt"
                            ),
                        true
                    );
                }
                
                progress += stepProgress;
                bgw.ReportProgress(1, new Message("Scene: " + scene.Key + "... done", (int)progress, true));
            }
            // all scenes prepared, now make list and write first scene
            if (!ZipIO.WriteListToArchive(GameProject.ProjectPath + "\\" + GameDataFileName, "scene\\list.txt",
                    GameProject.GetInstance().Scenes.Keys.ToList(), true)) return false;

            // write first scene, if not set get first
            if (!ZipIO.WriteLineToArchive(GameProject.ProjectPath + "\\" + GameDataFileName, "scene\\StartingScene.txt",
                     (GameProject.GetInstance().StartingScene?.Length > 0 ?
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
