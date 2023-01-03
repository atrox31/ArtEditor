using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.Functions;

namespace ArtCore_Editor.Assets
{
    // template is strict, no worry for nulls
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]

    public abstract class AssetManagerTemplate : Form
    {
        // common asset manager members
        // reference to dictionary of assets
        protected Dictionary<string, Asset> RefToGameProjectDictionary;

        // new or old asset, this manager take care of it
        protected Asset CurrentAsset;

        // file filter extensions, must set on init
        protected string AssetFileExtensionsFilter;
        // file type for project path
        private string _assetFileType;

        protected void PrepareManager(string assetId, Type senderType)
        {
            // get control of controls
            TextBox nameInputBox = this.Controls.Find("nameInputBox", true).FirstOrDefault() as TextBox;
            TextBox fileBox = this.Controls.Find("fileBox", true).FirstOrDefault() as TextBox;

            // get type of asset from caller name
            _assetFileType = senderType.Name.Replace("Manager", "").ToLower();

            OnLoad();
            // set current edited asset from given name
            if (assetId != null)
            {
                // get reference to temp asset, if exists clone else prepare new
                Asset tempAsset = RefToGameProjectDictionary.GetValueOrDefault(assetId, null);
                CurrentAsset = (tempAsset != null ? (Asset)tempAsset.Clone() : new Asset());
                
                nameInputBox.Text = CurrentAsset.Name;

                if (!File.Exists(CurrentAsset.GetFilePath()))
                {
                    fileBox.Text = "FILE NOT FOUND";
                }
                else
                {
                    fileBox.Text = CurrentAsset.ProjectPath + CurrentAsset.FileName;
                    SetAssetPreview();
                }
            }
            else
            {
                // create new asset
                CurrentAsset = new Asset();
            }
            SetInfoBox();

        }
        protected void ButtonCancel_click(object sender, EventArgs e)
        {
            OnExit();
            // cancel
            DialogResult = DialogResult.Cancel;
            Close();
        }
        protected void ButtonAddFile_click(object sender, EventArgs e)
        {
            TextBox nameInputBox = this.Controls.Find("nameInputBox", true).FirstOrDefault() as TextBox;
            TextBox fileBox = this.Controls.Find("fileBox", true).FirstOrDefault() as TextBox;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = AssetFileExtensionsFilter;
            openFileDialog.Title = "Select file";

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            string sourceFileName = openFileDialog.FileName;
            string fileExtension = sourceFileName.Split('\\').Last().Split('.').Last();

            // if user not type name in name box, get new name from filename
            if (nameInputBox.Text.Length == 0)
            {
                nameInputBox.Text = sourceFileName.Split('\\').Last().Split('.').First();
            }

            // set new file name with hash algorithm
            string newFileName = Functions.Functions.CalculateHash(sourceFileName) + '.' + fileExtension;
            string newFilePath = StringExtensions.Combine(GameProject.ProjectPath, "assets", _assetFileType, newFileName);

            if (File.Exists(newFilePath))
            {
                if (MessageBox.Show("File is not unique, do You want to replace it?", "File exists!",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            File.Copy(sourceFileName, newFilePath, true);

            CurrentAsset = new Asset()
            {
                Name = nameInputBox.Text,
                ProjectPath = $"\\assets\\{_assetFileType}\\",
                FileName = $"{newFileName}"
            };
            fileBox.Text = CurrentAsset.ProjectPath + CurrentAsset.FileName;
            SetAssetPreview();
            SetInfoBox();
        }
        protected void ButtonAccept_click(object sender, EventArgs e)
        {
            TextBox nameInputBox = this.Controls.Find("nameInputBox", true).FirstOrDefault() as TextBox;

            // check if asset have name
            if (Functions.Functions.ErrorCheck(nameInputBox.Text.Length > 0,
                    $"Asset must have name!")) return;

            // check if asset name is longer than 3 chars
            if (Functions.Functions.ErrorCheck(nameInputBox.Text.Length > 3,
                    $"Asset name must have more that 3 chars ({nameInputBox.Text.Length} current)")) return;

            // check if file exists
            if (Functions.Functions.ErrorCheck(File.Exists(CurrentAsset.GetFilePath()),
                    $"Asset file not exists ({CurrentAsset.GetFilePath()})")) return;

            // check if asset name is changed
            if (CurrentAsset.Name != nameInputBox.Text)
            {
                if (RefToGameProjectDictionary.ContainsKey(CurrentAsset.Name))
                {
                    if (Functions.Functions.ErrorCheck(!RefToGameProjectDictionary.ContainsKey(nameInputBox.Text),
                            $"Asset with that name exists! First delete old asset.")) return;
                    RefToGameProjectDictionary.RenameKey(CurrentAsset.Name, nameInputBox.Text);
                }

                CurrentAsset.Name = nameInputBox.Text;
            }

            RefToGameProjectDictionary[CurrentAsset.Name] = (Asset)CurrentAsset.Clone();

            OnExit();
            DialogResult = DialogResult.OK;
            Close();

        }

        protected virtual void OnExit()
        {
            // override this if necessary
        }
        protected virtual void OnLoad()
        {
            // override this if necessary
        }

        protected virtual void SetInfoBox()
        {
            // override this if necessary
        }

        protected virtual void SetAssetPreview()
        {
            // override this if necessary
        }

    }
}
