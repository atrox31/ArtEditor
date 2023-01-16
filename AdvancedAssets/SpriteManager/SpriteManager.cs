using ArtCore_Editor.Functions;
using ArtCore_Editor.Main;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Image = System.Drawing.Image;

namespace ArtCore_Editor.AdvancedAssets.SpriteManager
{
    public partial class SpriteManager : Form
    {
        // control box
        private int _controlBoxFrame = 0;
        private int _controlBoxFps = 0;
        private int _controlBoxAnimationFrom = 0;
        private int _controlBoxAnimationTo = 0;
        private bool _controlBoxLoop = false;

        private List<Image> _imageList;

        // current edited sprite
        private Sprite _currentSprite;
        private string _aid;

        private bool _saved;

        private void MakeChanges()
        {
            _saved = false;
        }

        public SpriteManager(string assetId = null)
        {
            InitializeComponent();Program.ApplyTheme(this);
            _imageList = new List<Image>();

            _aid = assetId;
            if (assetId == null || (!GameProject.GetInstance().Sprites.ContainsKey(assetId)))
            {
                _currentSprite = new Sprite();
                _aid = null;
            }
            else
            {
                _currentSprite = (Sprite)GameProject.GetInstance().Sprites[assetId].Clone();
                LoadFramesFromSpriteArchive();
                if(_imageList.Count > 0)
                {
                    control_box_region.Enabled = true;
                }

            }

            switch (
                _currentSprite.CollisionMask)
            {
                default:
                    rb_mask_none.Checked = true;
                    break;
                case Sprite.CollisionMaskEnum.None:
                    rb_mask_none.Checked = true;
                    break;
                case Sprite.CollisionMaskEnum.Circle:
                    rb_mask_circle.Checked = true;
                    break;
                case Sprite.CollisionMaskEnum.Rectangle:
                    rb_mask_rect.Checked = true;
                    break;
            }

            nm_center_w.Value = _currentSprite.SpriteCenterX;
            nm_center_h.Value = _currentSprite.SpriteCenterY;

            tb_sprite_name.Text = _currentSprite.Name;

            UpdateAnimationList();
            UpdateSpriteInfo();
            ls_animation_sequence.SelectedIndex = 0;
            RefreshCurrentFrame();
            UpdateSpriteCenterBox();
            // prevent from fake changes in loading
            _saved = true;
        }

        private string GetFramesDataFileName()
        {
            return StringExtensions.Combine(GameProject.ProjectPath, _currentSprite.DataPath);
        }

        private void SaveFramesToSpriteArchive()
        {
            if (_imageList.Count == 0) return;
            int i = 0;
            foreach (var item in _imageList)
            {
                using MemoryStream stream = new MemoryStream();
                item.Save(stream, ImageFormat.Png);
                ZipIO.WriteToZipArchiveFromStream(GetFramesDataFileName(), $"{i++}.png", stream, true);
            }
        }

        private void LoadFramesFromSpriteArchive()
        {
            for (int i = 0; i <= _currentSprite.SpriteFrames; i++)
            {
                Bitmap image = ZipIO.ReadImageFromArchive(GetFramesDataFileName(), $"{i}.png");
                if (image != null)
                {
                    _imageList.Add(new Bitmap(image));
                }
            }
        }

        private bool SaveSprite()
        {
            // check if asset have name
            if (Functions.Functions.ErrorCheck(tb_sprite_name.Text.Length > 0,
                    $"Asset must have name!")) return false;

            // check if asset name is longer than 3 chars
            if (Functions.Functions.ErrorCheck(tb_sprite_name.Text.Length > 3,
                    $"Asset name must have more that 3 chars ({tb_sprite_name.Text.Length} current)")) return false;

            // check if new name file exists, if yes ask to overrride
            if (_aid != tb_sprite_name.Text 
                && File.Exists(StringExtensions.Combine(GameProject.ProjectPath, "\\assets\\sprite\\", tb_sprite_name.Text, tb_sprite_name.Text + Program.FileExtensions_Sprite)) 
                && MessageBox.Show("File '" + tb_sprite_name.Text + "' exists! Overrride?", "File exists.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return false;
            }

            _currentSprite.Name = tb_sprite_name.Text;
            _currentSprite.ProjectPath = $"\\assets\\sprite\\{_currentSprite.Name}"; 
            _currentSprite.FileName = $"{_currentSprite.Name}" + Program.FileExtensions_Sprite;

            _currentSprite.DataPath = $"\\{_currentSprite.ProjectPath}\\{_currentSprite.Name}_frames.data";

            // first save new file, later check or delete old
            Directory.CreateDirectory(StringExtensions.Combine(GameProject.ProjectPath, _currentSprite.ProjectPath));
            
            using FileStream createStream = File.Create(StringExtensions.Combine(GameProject.ProjectPath, _currentSprite.ProjectPath, _currentSprite.FileName));
            createStream.Write(JsonConvert.SerializeObject(_currentSprite).Select(c => (byte)c).ToArray());

            SaveFramesToSpriteArchive();

            // fresh sprite
            if (_aid == null)
            {
                _aid = _currentSprite.Name;
                GameProject.GetInstance().Sprites.Add(_aid, (Sprite)_currentSprite.Clone());
            }
            else
            {
                // change sprite name
                if(_aid != _currentSprite.Name)
                {
                    GameProject.GetInstance().Sprites.RenameKey(_aid, _currentSprite.Name);
                    _aid = _currentSprite.Name;
                }
                GameProject.GetInstance().Sprites[_aid]=  (Sprite)_currentSprite.Clone();
            }

            _saved = true;
            return true;
        }

        private void SpriteManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_saved)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            switch (MessageBox.Show("Do You want to save changes?", "Unsaved changes", MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    if (SaveSprite())
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                    return;
                case DialogResult.No:
                    DialogResult = DialogResult.Cancel;
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
            }
            // if exit clear images
            ClearImageList();
        }

        #region control_box

        // animation
        private void update_preview_Tick(object sender, EventArgs e)
        {
            if (++_controlBoxFrame > _controlBoxAnimationTo)
            {
                if (_controlBoxLoop)
                {
                    _controlBoxFrame = _controlBoxAnimationFrom;
                }
                else
                {
                    _controlBoxFrame = _controlBoxAnimationTo;
                    update_preview_timer.Stop();
                }
            }
            RefreshCurrentFrame();
        }

        private void UpdateAnimationList()
        {
            int selected = ls_animation_sequence.SelectedIndex;
            ls_animation_sequence.Items.Clear();
            lb_animation_sequence.Items.Clear();
            ls_animation_sequence.Items.Add("<All frames>");
            foreach (KeyValuePair<string, Sprite.AnimationSequence> animationSequence in _currentSprite.SpriteAnimationSequence)
            {
                ls_animation_sequence.Items.Add(animationSequence.Key + '|' + animationSequence.Value.Name + ' ' +
                                                '(' + animationSequence.Value.FrameFrom + '-' +
                                                animationSequence.Value.FrameTo + ')');
                
                lb_animation_sequence.Items.Add(animationSequence.Key + '|' + animationSequence.Value.Name + ' ' +
                                                '(' + animationSequence.Value.FrameFrom + '-' +
                                                animationSequence.Value.FrameTo + ')');
            }
            //Restore previous selected 
            if(selected > 0 && ls_animation_sequence.Items.Count > 0)
            {
                ls_animation_sequence.SelectedIndex = selected - 1;
            }

        }

        #region control_box_hover
        private void br_play_MouseEnter(object sender, EventArgs e)
        {
            tb_control_box_info.Text = "Play";
        }

        private void bt_pause_MouseEnter(object sender, EventArgs e)
        {
            tb_control_box_info.Text = "Pause";
        }

        private void bt_stop_MouseEnter(object sender, EventArgs e)
        {
            tb_control_box_info.Text = "Stop";
        }

        private void bt_first_MouseEnter(object sender, EventArgs e)
        {
            tb_control_box_info.Text = "First";
        }

        private void bt_prev_MouseEnter(object sender, EventArgs e)
        {
            tb_control_box_info.Text = "Previous";
        }

        private void bt_next_MouseEnter(object sender, EventArgs e)
        {
            tb_control_box_info.Text = "Next";
        }

        private void bt_last_MouseEnter(object sender, EventArgs e)
        {
            tb_control_box_info.Text = "Last";
        }

        private void ControlBoxMouseLeave(object sender, EventArgs e)
        {
            tb_control_box_info.Text = "";
        }
        #endregion

        #region control_box_click

        private void RefreshCurrentFrame()
        {
            if (_imageList.Count > 0 && _controlBoxFrame < _imageList.Count)
            {
                sprite_preview.Image = _imageList[_controlBoxFrame];
            }
            lb_preview_info.Text = _controlBoxFrame.ToString() + "/" + _controlBoxAnimationTo.ToString();
            sprite_preview.Refresh();
        }

        private void br_play_Click(object sender, EventArgs e)
        {
            update_preview_timer.Start();
            RefreshCurrentFrame();
        }

        private void bt_pause_Click(object sender, EventArgs e)
        {
            update_preview_timer.Stop();
            RefreshCurrentFrame();
        }

        private void bt_stop_Click(object sender, EventArgs e)
        {
            _controlBoxFrame = _controlBoxAnimationFrom;
            update_preview_timer.Stop();
            RefreshCurrentFrame();
        }

        private void bt_first_Click(object sender, EventArgs e)
        {
            _controlBoxFrame = _controlBoxAnimationFrom;
            RefreshCurrentFrame();
        }

        private void bt_prev_Click(object sender, EventArgs e)
        {
            if (--_controlBoxFrame < _controlBoxAnimationFrom)
            {
                _controlBoxFrame = _controlBoxLoop ? _controlBoxAnimationTo : _controlBoxAnimationFrom;
            }
            RefreshCurrentFrame();
        }

        private void bt_next_Click(object sender, EventArgs e)
        {
            if (++_controlBoxFrame > _controlBoxAnimationTo)
            {
                _controlBoxFrame = _controlBoxLoop ? _controlBoxAnimationFrom : _controlBoxAnimationTo;
            }
            RefreshCurrentFrame();
        }

        private void bt_last_Click(object sender, EventArgs e)
        {
            _controlBoxFrame = _controlBoxAnimationTo;
            RefreshCurrentFrame();
        }

        private void cb_loop_CheckedChanged(object sender, EventArgs e)
        {
            _controlBoxLoop = cb_loop.Checked;
            RefreshCurrentFrame();
        }

        private void nm_fps_ValueChanged(object sender, EventArgs e)
        {
            _controlBoxFps = (int)nm_fps.Value;
            update_preview_timer.Interval = (1000 / _controlBoxFps);
            RefreshCurrentFrame();
        }
        
        // reset frame data, from-to frame max
        private void ResetFramesData()
        {
            if (ls_animation_sequence.SelectedItem == null) return;
            if (ls_animation_sequence.SelectedIndex == 0)
            {
                _controlBoxAnimationFrom = 0;
                _controlBoxAnimationTo = _currentSprite.SpriteFrames;
                _controlBoxFrame = _controlBoxAnimationFrom;
            }
            else
            {
                string animationSequence = ls_animation_sequence.SelectedItem.ToString()!.Split('|').First();
                if (animationSequence.Length < 2) return;

                if (!_currentSprite.SpriteAnimationSequence.ContainsKey(animationSequence)) return;

                _controlBoxAnimationFrom = _currentSprite.SpriteAnimationSequence[animationSequence].FrameFrom;
                _controlBoxAnimationTo = _currentSprite.SpriteAnimationSequence[animationSequence].FrameTo;
                _controlBoxFrame = _controlBoxAnimationFrom;
            }
            RefreshCurrentFrame();
        }

        private void ls_animation_sequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetFramesData();
        }
        #endregion

        #endregion

        #region sprite_center
        private void cb_show_sprite_center_CheckedChanged(object sender, EventArgs e)
        {
            sprite_preview.Refresh();
        }

        private void nm_center_w_ValueChanged(object sender, EventArgs e)
        {
            _currentSprite.SpriteCenterX = (int)nm_center_w.Value;
            if (cb_show_sprite_center.Checked)
                sprite_preview.Refresh();
            MakeChanges();
        }

        private void nm_center_h_ValueChanged(object sender, EventArgs e)
        {
            _currentSprite.SpriteCenterY = (int)nm_center_h.Value;
            if (cb_show_sprite_center.Checked)
                sprite_preview.Refresh();
            MakeChanges();
        }

        private void rb_center_00_CheckedChanged(object sender, EventArgs e)
        {
            nm_center_w.Value = 0;
            nm_center_h.Value = 0;
            nm_center_w.Enabled = false;
            nm_center_h.Enabled = false;

           // _currentSprite.SpriteCenterX = (int)nm_center_w.Value;
           // _currentSprite.SpriteCenterY = (int)nm_center_h.Value;

            if (cb_show_sprite_center.Checked)
                sprite_preview.Refresh();
            MakeChanges();
        }

        private void rb_center_center_CheckedChanged(object sender, EventArgs e)
        {
            nm_center_w.Value = Math.Round((decimal)_currentSprite.SpriteWidth / 2);
            nm_center_h.Value = Math.Round((decimal)_currentSprite.SpriteHeight / 2);
            nm_center_w.Enabled = false;
            nm_center_h.Enabled = false;
            if (cb_show_sprite_center.Checked)
                sprite_preview.Refresh();
            MakeChanges();
        }

        private void rb_center_custom_CheckedChanged(object sender, EventArgs e)
        {
            nm_center_w.Enabled = true;
            nm_center_h.Enabled = true;
            if (cb_show_sprite_center.Checked)
                sprite_preview.Refresh();
            MakeChanges();
        }
        #endregion

        #region sprite_mask

        private void RefreshMaskRegion()
        {
            switch (
                _currentSprite.CollisionMask)
            {
                case Sprite.CollisionMaskEnum.None:
                    lb_sprite_mask_width.Visible = false;
                    nm_sprite_mask_width.Visible = false;

                    lb_sprite_mask_height.Visible = false;
                    nm_sprite_mask_height.Visible = false;

                    cb_mask_ratio.Visible = false;

                    lb_sprite_mask_radius.Visible = false;
                    nm_sprite_mask_radius.Visible = false;
                    break;

                case Sprite.CollisionMaskEnum.Circle:
                    nm_sprite_mask_width.Visible = false;
                    lb_sprite_mask_width.Visible = false;

                    lb_sprite_mask_height.Visible = false;
                    nm_sprite_mask_height.Visible = false;

                    cb_mask_ratio.Visible = false;

                    lb_sprite_mask_radius.Visible = true;
                    nm_sprite_mask_radius.Visible = true;

                    nm_sprite_mask_radius.Maximum = (decimal) Math.Max(_currentSprite.SpriteHeight, _currentSprite.SpriteWidth) / 2;
                    nm_sprite_mask_radius.Value = Math.Min(_currentSprite.CollisionMaskValue1, nm_sprite_mask_radius.Maximum);
                    break;

                case Sprite.CollisionMaskEnum.Rectangle:
                    lb_sprite_mask_width.Visible = true;
                    nm_sprite_mask_width.Visible = true;

                    lb_sprite_mask_height.Visible = true;
                    nm_sprite_mask_height.Visible = true;

                    cb_mask_ratio.Visible = true;

                    lb_sprite_mask_radius.Visible = false;
                    nm_sprite_mask_radius.Visible = false;

                    nm_sprite_mask_width.Maximum = _currentSprite.SpriteWidth;
                    nm_sprite_mask_width.Value = Math.Min(nm_sprite_mask_width.Maximum, _currentSprite.CollisionMaskValue1);

                    nm_sprite_mask_height.Maximum = _currentSprite.SpriteHeight;
                    nm_sprite_mask_height.Value = Math.Min(nm_sprite_mask_height.Maximum, _currentSprite.CollisionMaskValue2);
                    break;
            }
            if (cb_show_sprite_mask.Checked)
                sprite_preview.Refresh();
        }

        private void cb_show_sprite_mask_CheckedChanged(object sender, EventArgs e)
        {
            sprite_preview.Refresh();
        }

        private void rb_mask_none_CheckedChanged(object sender, EventArgs e)
        {
            _currentSprite.CollisionMask = Sprite.CollisionMaskEnum.None;
            RefreshMaskRegion();
            MakeChanges();
        }

        private void rb_mask_rect_CheckedChanged(object sender, EventArgs e)
        {
            _currentSprite.CollisionMask = Sprite.CollisionMaskEnum.Rectangle;
            RefreshMaskRegion();
            MakeChanges();
        }

        private void rb_mask_circle_CheckedChanged(object sender, EventArgs e)
        {
            _currentSprite.CollisionMask = Sprite.CollisionMaskEnum.Circle;
            RefreshMaskRegion();
            MakeChanges();
        }

        private void nm_sprite_mask_h_ValueChanged(object sender, EventArgs e)
        {
            _currentSprite.CollisionMaskValue2 = (int)nm_sprite_mask_height.Value;
            if (cb_mask_ratio.Checked)
            {
                _currentSprite.CollisionMaskValue1 = (int)nm_sprite_mask_height.Value;
                nm_sprite_mask_width.Value = nm_sprite_mask_height.Value;
            }
            if (cb_show_sprite_mask.Checked)
                sprite_preview.Refresh();
            MakeChanges();
        }

        private void nm_sprite_mask_w_ValueChanged(object sender, EventArgs e)
        {
            _currentSprite.CollisionMaskValue1 = (int)nm_sprite_mask_width.Value;
            if (cb_mask_ratio.Checked)
            {
                _currentSprite.CollisionMaskValue2 = (int)nm_sprite_mask_width.Value;
                nm_sprite_mask_height.Value = nm_sprite_mask_width.Value;
            }
            if (cb_show_sprite_mask.Checked)
                sprite_preview.Refresh();
            MakeChanges();
        }
        private void nm_sprite_mask_radius_ValueChanged(object sender, EventArgs e)
        {
            _currentSprite.CollisionMaskValue1 = (int)nm_sprite_mask_radius.Value;
            if (cb_show_sprite_mask.Checked)
                sprite_preview.Refresh();
            MakeChanges();
        }
        #endregion

        #region sprite_animation_squence
        private void bt_as_delete_Click(object sender, EventArgs e)
        {
            if (lb_animation_sequence.SelectedItem == null) return;
            if (MessageBox.Show("Do You want to delete selected item?",
                    "Delete [" + lb_animation_sequence.SelectedItem.ToString()?.Split(']')[0].Split('[')[1] + "]",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            _currentSprite.SpriteAnimationSequence.Remove(lb_animation_sequence.SelectedItem.ToString()?.Split(']')[0].Split('[')[1] ?? string.Empty);
            UpdateAnimationList();
        }

        private void bt_as_edit_Click(object sender, EventArgs e)
        {
            if (lb_animation_sequence.SelectedItem == null) return;
            AnimationSequencerForm asf = new AnimationSequencerForm();
            asf.FFrameFromMin = 0;
            asf.FFrameFromMax = _imageList.Count - 1;
            asf.FFrameToMin = 0;
            asf.FFrameToMax = _imageList.Count - 1;

            string tmpIndex = lb_animation_sequence.SelectedItem.ToString()?.Split(']')[0].Split('[')[1];
            if (tmpIndex != null)
            {
                asf.FFullName = _currentSprite.SpriteAnimationSequence[tmpIndex].Name;
                asf.FIndexName = _currentSprite.SpriteAnimationSequence[tmpIndex].Index;
                asf.FFrameFrom = _currentSprite.SpriteAnimationSequence[tmpIndex].FrameFrom;
                asf.FFrameTo = _currentSprite.SpriteAnimationSequence[tmpIndex].FrameTo;
            }

            if (asf.ShowDialog() != DialogResult.OK) return;
            _currentSprite.SpriteAnimationSequence[asf.FIndexName].FrameFrom = asf.FFrameFrom;
            _currentSprite.SpriteAnimationSequence[asf.FIndexName].FrameTo = asf.FFrameTo;
            _currentSprite.SpriteAnimationSequence[asf.FIndexName].Name = asf.FFullName;
            UpdateAnimationList();
        }

        private void bt_as_add_Click(object sender, EventArgs e)
        {
            if (_currentSprite == null) return;
            AnimationSequencerForm asf = new AnimationSequencerForm();
            asf.FFrameFromMin = 0;
            asf.FFrameFromMax = _imageList.Count - 1;
            asf.FFrameToMin = 0;
            asf.FFrameToMax = _imageList.Count - 1;
            if (asf.ShowDialog() != DialogResult.OK) return;
            _currentSprite.SpriteAnimationSequence.Add(asf.FIndexName, new Sprite.AnimationSequence(asf.FFullName, asf.FIndexName, asf.FFrameFrom, asf.FFrameTo));
            UpdateAnimationList();
        }
        #endregion

        #region sprite_info_main

        private void LoadImageToList(string path)
        {
            if (!File.Exists(path)) return;
            using Bitmap bmpTemp = new Bitmap(path);
            _imageList.Add(new Bitmap(bmpTemp));
            _currentSprite.SpriteWidth = Math.Max(_currentSprite.SpriteWidth, _imageList.Last().Width);
            _currentSprite.SpriteHeight = Math.Max(_currentSprite.SpriteHeight, _imageList.Last().Height);

            UpdateSpriteCenterBox();

            control_box_region.Enabled = true;
            _currentSprite.SpriteFrames = _imageList.Count-1;
        }

        private void UpdateSpriteCenterBox()
        {
            
            nm_center_w.Maximum = _currentSprite.SpriteWidth;
            nm_center_h.Maximum = _currentSprite.SpriteHeight;

            nm_center_w.Value = Math.Min(nm_center_w.Value, nm_center_w.Maximum);
            nm_center_h.Value = Math.Min(nm_center_h.Value, nm_center_h.Maximum);
        }

        private void ClearImageList()
        {
            if (_imageList.Count == 0) return;
            foreach (Image image in _imageList)
            {
                image.Dispose();
            }
            _imageList.Clear();
            // delete all image data
#pragma warning disable S1215 // "GC.Collect" should not be called
            GC.Collect();
#pragma warning restore S1215 // "GC.Collect" should not be called
            _currentSprite.SpriteFrames = 0;
            control_box_region.Enabled = false;
        }
        private void bt_import_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "png";
            openFileDialog.Filter = "PNG|*.png";
            openFileDialog.Multiselect = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.AddExtension = true;
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            if (_imageList.Count > 0 && MessageBox.Show(
                       "Do You want to clear image list before adding new images?",
                       "Replace or add",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ClearImageList();
                MakeChanges();
            }

            foreach (string fileName in openFileDialog.FileNames)
            {
                LoadImageToList(fileName);
            }
            MakeChanges();
            UpdateSpriteInfo();

            // reset animation sequence to proper get first and last frame
            ResetFramesData();
        }

        private void UpdateSpriteInfo()
        {
            if (_imageList.Count == 0)
            {
                lb_sprite_info.Text = "No textures in sprite";
                return;
            }

            lb_sprite_info.Text = $"Textures: {_imageList.Count.ToString()}\n" +
                                  $"Sprite width: {_currentSprite.SpriteWidth}px\n" +
                                  $"Sprite height: {_currentSprite.SpriteHeight}px\n";
        }
        #endregion

        private void sprite_preview_Paint(object sender, PaintEventArgs e)
        {
            // if not have images do not draw anything
            // this can make problems with 0 width or height of sprite
            if (_imageList.Count == 0) return;

            if (cb_show_sprite_center.Checked)
            {
                using Pen redPen = new Pen(Color.Red, 1);
                using Pen bluePen = new Pen(Color.Blue, 1);
                int xCenter = Functions.Functions.Scale(_currentSprite.SpriteCenterX, 0, _currentSprite.SpriteWidth, 0, sprite_preview.Width);
                int yCenter = Functions.Functions.Scale(_currentSprite.SpriteCenterY, 0, _currentSprite.SpriteHeight, 0, sprite_preview.Height);
                e.Graphics.DrawLine(redPen, xCenter, 0, xCenter, sprite_preview.Width);
                e.Graphics.DrawLine(bluePen, 0, yCenter, sprite_preview.Height, yCenter);
            }

            if (cb_show_sprite_mask.Checked)
            {
                switch (_currentSprite.CollisionMask)
                {
                    case Sprite.CollisionMaskEnum.None:
                        break;
                    case Sprite.CollisionMaskEnum.Circle:
                        {
                            // every value must be scaled, original size -> picture box size
                            int xCenter = Functions.Functions.Scale(_currentSprite.SpriteCenterX, 0, _currentSprite.SpriteWidth, 0, sprite_preview.Width);
                            int yCenter = Functions.Functions.Scale(_currentSprite.SpriteCenterY, 0, _currentSprite.SpriteHeight, 0, sprite_preview.Height);

                            int rCircle = Functions.Functions.Scale(_currentSprite.CollisionMaskValue1, 0, (int)nm_sprite_mask_radius.Maximum, 0, sprite_preview.Width);

                            using Brush fillPen = new SolidBrush(Color.FromArgb(60, 132, 59, 98));
                            using Pen redPen = new Pen(Color.FromArgb(255, 132, 59, 98), 2);

                            Rectangle r = new Rectangle(
                                    xCenter - rCircle / 2,
                                    yCenter - rCircle / 2,
                                 rCircle,
                                 rCircle
                            );
                            e.Graphics.FillEllipse(fillPen, r);
                            e.Graphics.DrawEllipse(redPen, r);
                        }
                        break;
                    case Sprite.CollisionMaskEnum.Rectangle:
                        {
                            // every value must be scaled, original size -> picture box size
                            int xCenter = Functions.Functions.Scale(_currentSprite.SpriteCenterX, 0, _currentSprite.SpriteWidth, 0, sprite_preview.Width);
                            int yCenter = Functions.Functions.Scale(_currentSprite.SpriteCenterY, 0, _currentSprite.SpriteHeight, 0, sprite_preview.Height);

                            int rRectW = Functions.Functions.Scale(_currentSprite.CollisionMaskValue1, 0, (int)nm_sprite_mask_height.Maximum, 0, sprite_preview.Width);
                            int rRectH = Functions.Functions.Scale(_currentSprite.CollisionMaskValue2, 0, (int)nm_sprite_mask_width.Maximum, 0, sprite_preview.Height);

                            using Brush fillPen = new SolidBrush(Color.FromArgb(60, 132, 59, 98));
                            using Pen redPen = new Pen(Color.FromArgb(255, 132, 59, 98), 2);
                            Rectangle r = new Rectangle(
                                xCenter - rRectW / 2,
                                yCenter - rRectH / 2,
                                 rRectW,
                                 rRectH
                            );
                            e.Graphics.FillRectangle(fillPen, r);
                            e.Graphics.DrawRectangle(redPen, r);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void bt_accept_Click(object sender, EventArgs e)
        {
            SaveSprite();
            Close();
        }

        private void bt_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
