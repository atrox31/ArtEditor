using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.Functions;
using ArtCore_Editor.Pick_forms;
using Bitmap = System.Drawing.Bitmap;

namespace ArtCore_Editor.AdvancedAssets.SpriteManager
{
    public partial class SpriteEditor : Form
    {
        private bool _saved = true;
        private Sprite _globalSprite;
        private int _pFirstFrame, _pLastFrame, _pCurrentFrame;
        
        private string _aid;

        private void CreateNewSprite()
        {
            _pCurrentFrame = _pFirstFrame;
            _pLastFrame = 0;
            _globalSprite = new Sprite();
        }

        public SpriteEditor(string sprite = null)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            s_preview.SizeMode = PictureBoxSizeMode.StretchImage;
            _aid = sprite;
            if (sprite != null)
            {
                _globalSprite = (Sprite)GameProject.GetInstance().Sprites[sprite].Clone();
                _globalSprite.Load();
                _pFirstFrame = 0;
                _pLastFrame = (_globalSprite.Textures == null ? 0 : _globalSprite.Textures.Count - 1);
                _pCurrentFrame = _pFirstFrame;
            }
            else
            {
                CreateNewSprite();
            }
            
            UpdateForm();
        }


        private void SaveSprite()
        {
            // check if asset have name
            if (Functions.Functions.ErrorCheck(s_spritename.Text.Length > 0,
                    $"Asset must have name!")) return;

            // check if asset name is longer than 3 chars
            if (Functions.Functions.ErrorCheck(s_spritename.Text.Length > 3,
                    $"Asset name must have more that 3 chars ({s_spritename.Text.Length} current)")) return;

            _globalSprite.CollisionMask =
                (Sprite.CollisionMaskEnum)(s_collision_have_mask.Checked ? (s_col_mask_circle.Checked ? 1 : s_col_mask_rect.Checked ? 2 : 0) : 0);
            _globalSprite.CollisionMaskValue = s_col_mask_value.Value;
            _globalSprite.SpriteCenter = (s_sprite_center_center.Checked ? Sprite.SpriteCenterEnum.Center : s_sprite_center_left.Checked ? Sprite.SpriteCenterEnum.LeftCorner : Sprite.SpriteCenterEnum.Custom);
            _globalSprite.SpriteCenterX = (int)s_sprite_center_x.Value;
            _globalSprite.SpriteCenterY = (int)s_sprite_center_y.Value;

            _globalSprite.EditorFps = (int)s_preview_fps.Value;
            _pCurrentFrame = _pFirstFrame;
            _pLastFrame = (_globalSprite.Textures == null ? 0 : _globalSprite.Textures.Count - 1);

            _globalSprite.Name = s_spritename.Text;
            _globalSprite.ProjectPath = $"\\assets\\sprite\\{_globalSprite.Name}";
            _globalSprite.FileName = $"{_globalSprite.Name}.spr";

            // fresh sprite
            if (_aid == null)
            {
                _aid = _globalSprite.Name;
                _globalSprite.Save();
                GameProject.GetInstance().Sprites.Add(_aid, (Sprite)_globalSprite.Clone());
                return;
            }
            // edited sprite
            if (_globalSprite.Name != _aid)
            {// change name
                string oldProjectPath = $"\\assets\\sprite\\{_aid}";
                if (Directory.Exists(GameProject.ProjectPath + oldProjectPath))
                {
                    Directory.Delete(GameProject.ProjectPath + oldProjectPath, true);
                }
                _globalSprite.Save();
                GameProject.GetInstance().Sprites.Remove(_aid);
                _aid = _globalSprite.Name;
                GameProject.GetInstance().Sprites.Add(_aid, (Sprite)_globalSprite.Clone());
                return;
            }
            else
            {
                // the same name
                _globalSprite.Save();
                GameProject.GetInstance().Sprites[_aid] = (Sprite)_globalSprite.Clone();
            }
        }

        private void SetCollisionMaskSliderValues()
        {
            switch (_globalSprite?.CollisionMask)
            {
                case Sprite.CollisionMaskEnum.Circle:
                    s_col_mask_value.Value = Math.Max(1,_globalSprite.CollisionMaskValue);
                    s_col_mask_value.Maximum = Math.Max(_globalSprite.SpriteHeight, _globalSprite.SpriteWidth) / 2;
                    break;
                case Sprite.CollisionMaskEnum.Rectangle:
                    s_col_mask_value.Value = Math.Max(1, _globalSprite.CollisionMaskValue);
                    s_col_mask_value.Maximum = Math.Max(_globalSprite.SpriteHeight, _globalSprite.SpriteWidth) / 2;
                    break;
            }
        }

        private void UpdateForm()
        {
            s_spritename.Text = _globalSprite.Name;

            s_sprite_center_y.Maximum = _globalSprite.SpriteWidth;
            s_sprite_center_x.Maximum = _globalSprite.SpriteHeight;
            
            s_col_mask_show.Checked = _globalSprite.EditorShowMask;
            if (_globalSprite.CollisionMask == Sprite.CollisionMaskEnum.None)
            {
                s_collision_have_mask.Checked = false;
            }
            else
            {
                s_collision_have_mask.Checked = true;

                switch (_globalSprite.CollisionMask)
                {
                    case Sprite.CollisionMaskEnum.Circle:
                        s_col_mask_circle.Checked = true;
                        break;
                    case Sprite.CollisionMaskEnum.Rectangle:
                        s_col_mask_rect.Checked = true;
                        break;
                }
            }
            SetCollisionMaskSliderValues();

            s_sprite_center_show.Checked = _globalSprite.EditorShowCenter;
            switch (_globalSprite.SpriteCenter)
            {
                case Sprite.SpriteCenterEnum.Center:
                    s_sprite_center_center.Checked = true;
                    break;
                case Sprite.SpriteCenterEnum.LeftCorner:
                    s_sprite_center_left.Checked = true;
                    break;
                case Sprite.SpriteCenterEnum.Custom:
                    s_sprite_center_custom.Checked = true;
                    break;
            }

            s_preview_fps.Value = _globalSprite.EditorFps;
            _pFirstFrame = 0;
            _pLastFrame = (_globalSprite.Textures == null ? 0 : _globalSprite.Textures.Count - 1);
            //_pCurrentFrame = _pFirstFrame;
            s_preview_loop.Checked = _globalSprite.EditorPreviewLoop;
            label4.Text = "Current: " + (_globalSprite.Textures?.Count ?? 0).ToString();
            label5.Text = _pCurrentFrame.ToString() + "/" + _pLastFrame.ToString();
            s_preview.Image = (_globalSprite.Textures == null || _globalSprite.Textures.Count == 0 ? null : _globalSprite.Textures[_pCurrentFrame]);

            s_animationSequencePreview.Items.Clear();
            s_animationSequencePreview.Items.Add("<All frames>");
            s_animationSequencePreview.SelectedIndex = 0;
            listBox1.Items.Clear();
            foreach (KeyValuePair<string, Sprite.AnimationSequence> item in _globalSprite.SpriteAnimationSequence)
            {
                s_animationSequencePreview.Items.Add("[" + item.Value.Index + "] " + item.Value.Name);
                listBox1.Items.Add("[" + item.Value.Index + "] " + item.Value.Name + "( " + item.Value.FrameFrom.ToString() + ":" + item.Value.FrameTo.ToString() + " )");
            }

            s_preview.Refresh();
        }

        private void ImportSpriteImages(bool clearList)
        {
            if (s_spritename.Text.Length == 0)
            {
                s_spritename.Text = "Unnamed_" + GameProject.GetInstance().Sprites
                    .Select(s => s.Value.Name.Contains("Unnamed_")).Count().ToString();
            }
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            if (clearList)
            {
                _globalSprite.ClearImages();
                GC.Collect();
                _globalSprite.Textures = new List<Image>(openFileDialog1.FileNames.Length);
            }
            
            int maxX = 0;
            int maxY = 0;
            int i = 0;
            foreach (string file in openFileDialog1.FileNames)
            {
                _globalSprite.AddImage(file);
                maxX = Math.Max(_globalSprite.Textures[i].Width, maxX);
                maxY = Math.Max(_globalSprite.Textures[i].Height, maxY);
                i++;
            }
            s_preview.Image = _globalSprite.Textures[0];
            label4.Text = "Current: " + _globalSprite.Textures.Count.ToString();
            _pLastFrame = _globalSprite.Textures.Count - 1;
            _pCurrentFrame = _pFirstFrame;

            _globalSprite.SpriteWidth = maxX;
            _globalSprite.SpriteHeight = maxY;

            s_sprite_center_y.Maximum = _globalSprite.SpriteWidth;
            s_sprite_center_x.Maximum = _globalSprite.SpriteHeight;
            SetCollisionMaskSliderValues();
            _saved = false;
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            // import textures
            ImportSpriteImages(MessageBox.Show("Do You want to clear image list before adding new images?", "Replace or add", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
            _saved = false;
            UpdateForm();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ImageListViewer ilv = new ImageListViewer(_globalSprite);
            ilv.ShowDialog();
            ilv.Dispose();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SaveSprite();
            _saved = true;
        }


        private void SpriteAddForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_saved)
            {
                switch (MessageBox.Show("Do You want to save?", "Data changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                    case DialogResult.Yes:
                    {
                        while (s_spritename.Text.Length == 0)
                        {
                            s_spritename.Text = GetString.Get("Sprite name");
                        }
                        _globalSprite.Save();
                        break;
                    }
                }

                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = _globalSprite.Name.Length == 0 ? DialogResult.No : DialogResult.OK;
            }

        }

        private void s_preview_play_Click(object sender, EventArgs e)
        {
            timer1.Interval = (int)(1000 / s_preview_fps.Value);
            timer1.Start();
            s_preview_play.Enabled = false;
            s_preview_stop.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _pCurrentFrame++;
            if (_pCurrentFrame > _pLastFrame)
            {
                if (_globalSprite.EditorPreviewLoop)
                {
                    _pCurrentFrame = _pFirstFrame;
                }
                else
                {
                    _pCurrentFrame = _pLastFrame;
                    timer1.Stop();
                    s_preview_play.Enabled = true;
                    s_preview_stop.Enabled = false;
                }
            }
            label5.Text = _pCurrentFrame.ToString() + "/" + _pLastFrame.ToString();
            s_preview.Image = _globalSprite.Textures[_pCurrentFrame];
        }

        private void s_preview_stop_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            s_preview_play.Enabled = true;
            s_preview_stop.Enabled = false;
        }

        private void s_preview_loop_CheckedChanged(object sender, EventArgs e)
        {
            _globalSprite.EditorPreviewLoop = s_preview_loop.Checked;
        }

        private void s_preview_fps_ValueChanged(object sender, EventArgs e)
        {
            _globalSprite.EditorFps = (int)s_preview_fps.Value;
            timer1.Interval = (int)(1000 / s_preview_fps.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            _pCurrentFrame = _pFirstFrame;
            label5.Text = _pCurrentFrame.ToString() + "/" + _pLastFrame.ToString();
            s_preview.Image = _globalSprite.Textures[_pCurrentFrame];
        }

        private void s_preview_next_Click(object sender, EventArgs e)
        {
            _pCurrentFrame++;
            if (_pCurrentFrame > _pLastFrame)
            {
                _pCurrentFrame = _globalSprite.EditorPreviewLoop ? _pFirstFrame : _pLastFrame;
            }
            label5.Text = _pCurrentFrame.ToString() + "/" + _pLastFrame.ToString();
            s_preview.Image = _globalSprite.Textures[_pCurrentFrame];
        }

        private void s_preview_prev_Click(object sender, EventArgs e)
        {
            _pCurrentFrame--;
            if (_pCurrentFrame < 0)
            {
                _pCurrentFrame = _globalSprite.EditorPreviewLoop ? _pLastFrame : _pFirstFrame;
            }
            label5.Text = _pCurrentFrame.ToString() + "/" + _pLastFrame.ToString();
            s_preview.Image = _globalSprite.Textures[_pCurrentFrame];
        }


        private void s_sprite_center_x_ValueChanged(object sender, EventArgs e)
        {
            _globalSprite.SpriteCenterX = Convert.ToInt32(s_sprite_center_x.Value);
            s_preview.Refresh();
        }

        private void s_sprite_center_y_ValueChanged(object sender, EventArgs e)
        {
            _globalSprite.SpriteCenterY = Convert.ToInt32(s_sprite_center_y.Value);
            s_preview.Refresh();
        }
        
        private void s_preview_Paint(object sender, PaintEventArgs e)
        {
            // move to UpdatePreview to easier find by name
            UpdatePreview(e);
        }

        private void UpdatePreview(PaintEventArgs e)
        {
            //s_col_mask_value.Maximum = Math.Max(_globalSprite.SpriteHeight, _globalSprite.SpriteWidth) / 2;
            if (_globalSprite.Textures == null) return;
            if (_globalSprite.EditorShowCenter)
            {
                using Pen redPen = new Pen(Color.Red, 1);
                using Pen bluePen = new Pen(Color.Blue, 1);
                int xCenter = Functions.Functions.Scale(_globalSprite.SpriteCenterX, 0, _globalSprite.SpriteWidth, 0, s_preview.Width);
                int yCenter = Functions.Functions.Scale(_globalSprite.SpriteCenterY, 0, _globalSprite.SpriteHeight, 0, s_preview.Height);
                e.Graphics.DrawLine(redPen, xCenter, 0, xCenter, s_preview.Width);
                e.Graphics.DrawLine(bluePen, 0, yCenter, s_preview.Height, yCenter);
            }

            if (!_globalSprite.EditorShowMask) return;
            {
                switch (_globalSprite.CollisionMask)
                {
                    case Sprite.CollisionMaskEnum.None:
                        break;
                    case Sprite.CollisionMaskEnum.Circle:
                        {
                            // every value must be scaled, original size -> picture box size
                            int xCenter = Functions.Functions.Scale(_globalSprite.SpriteCenterX, 0, _globalSprite.SpriteWidth, 0, s_preview.Width);
                            int yCenter = Functions.Functions.Scale(_globalSprite.SpriteCenterY, 0, _globalSprite.SpriteHeight, 0, s_preview.Height);

                            int rCircle = Functions.Functions.Scale(s_col_mask_value.Value, 0, s_col_mask_value.Maximum, 0, s_preview.Width);

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
                            redPen.Dispose();
                        }
                        break;
                    case Sprite.CollisionMaskEnum.Rectangle:
                        {
                            // every value must be scaled, original size -> picture box size
                            int xCenter = Functions.Functions.Scale(_globalSprite.SpriteCenterX, 0, _globalSprite.SpriteWidth, 0, s_preview.Width);
                            int yCenter = Functions.Functions.Scale(_globalSprite.SpriteCenterY, 0, _globalSprite.SpriteHeight, 0, s_preview.Height);

                            int rRect = Functions.Functions.Scale(s_col_mask_value.Value, 0, s_col_mask_value.Maximum, 0, s_preview.Width);

                            using Brush fillPen = new SolidBrush(Color.FromArgb(60, 132, 59, 98));
                            using Pen redPen = new Pen(Color.FromArgb(255, 132, 59, 98), 2);
                            Rectangle r = new Rectangle(
                                xCenter - rRect / 2,
                                yCenter - rRect / 2,
                                 rRect,
                                 rRect
                            );
                            e.Graphics.FillRectangle(fillPen, r);
                            e.Graphics.DrawRectangle(redPen, r);
                            redPen.Dispose();
                        }
                        break;
                    default:
                        break;
                }
            }
        }


        private void s_sprite_center_center_CheckedChanged(object sender, EventArgs e)
        {
            if (!s_sprite_center_center.Checked) return;
            _globalSprite.SpriteCenterX = _globalSprite.SpriteWidth / 2;
            _globalSprite.SpriteCenterY = _globalSprite.SpriteHeight / 2;
            s_sprite_center_x.Value = _globalSprite.SpriteCenterX;
            s_sprite_center_y.Value = _globalSprite.SpriteCenterY;
            s_preview.Refresh();
            s_sprite_center_x.Enabled = false;
            s_sprite_center_y.Enabled = false;
        }

        private void s_sprite_center_left_CheckedChanged(object sender, EventArgs e)
        {
            if (!s_sprite_center_left.Checked) return;
            _globalSprite.SpriteCenterX = 0;
            _globalSprite.SpriteCenterY = 0;
            s_sprite_center_x.Value = 0;
            s_sprite_center_y.Value = 0;
            s_preview.Refresh();
            s_sprite_center_x.Enabled = false;
            s_sprite_center_y.Enabled = false;
        }

        private void s_sprite_center_custom_CheckedChanged(object sender, EventArgs e)
        {
            if (!s_sprite_center_custom.Checked) return;
            _globalSprite.SpriteCenterX = Convert.ToInt32(s_sprite_center_x.Value);
            _globalSprite.SpriteCenterY = Convert.ToInt32(s_sprite_center_y.Value);
            s_preview.Refresh();
            s_sprite_center_x.Enabled = true;
            s_sprite_center_y.Enabled = true;
        }

        private void s_sprite_center_show_CheckedChanged(object sender, EventArgs e)
        {
            _globalSprite.EditorShowCenter = s_sprite_center_show.Checked;
            s_preview.Refresh();
        }

        private void s_collision_have_mask_CheckedChanged(object sender, EventArgs e)
        {
            s_col_mask_circle.Enabled = s_collision_have_mask.Checked;
            s_col_mask_rect.Enabled = s_collision_have_mask.Checked;
            s_col_mask_show.Enabled = s_collision_have_mask.Checked;
            s_col_mask_value.Enabled = s_collision_have_mask.Checked;
            SetCollisionMaskSliderValues();
            s_preview.Refresh();
        }

        private void s_col_mask_circle_CheckedChanged(object sender, EventArgs e)
        {
            _globalSprite.CollisionMask = Sprite.CollisionMaskEnum.Circle;
            SetCollisionMaskSliderValues();
            s_preview.Refresh();
        }

        private void s_col_mask_rect_CheckedChanged(object sender, EventArgs e)
        {
            _globalSprite.CollisionMask = Sprite.CollisionMaskEnum.Rectangle;
            SetCollisionMaskSliderValues();
            s_preview.Refresh();
        }

        private void s_col_mask_show_CheckedChanged(object sender, EventArgs e)
        {
            _globalSprite.EditorShowMask = s_col_mask_show.Checked;
            SetCollisionMaskSliderValues();
            s_preview.Refresh();
        }

        private void s_col_mask_value_Scroll(object sender, EventArgs e)
        {
            _globalSprite.CollisionMaskValue = s_col_mask_value.Value;
            label3.Text = "Value ( " + _globalSprite.CollisionMaskValue + " )";
            s_preview.Refresh();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            AnimationSequencerForm asf = new AnimationSequencerForm();
            asf.FFrameFromMin = 0;
            asf.FFrameFromMax = _globalSprite.Textures.Count - 1;
            asf.FFrameToMin = 0;
            asf.FFrameToMax = _globalSprite.Textures.Count - 1;
            asf.ShowDialog();
            if (asf.DialogResult == DialogResult.OK)
            {
                _globalSprite.SpriteAnimationSequence.Add(asf.FIndexName, new Sprite.AnimationSequence(asf.FFullName, asf.FIndexName, asf.FFrameFrom, asf.FFrameTo));
                UpdateForm();
            }
        }

        private void s_animationSequencePreview_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (s_animationSequencePreview.SelectedIndex == 0)
            {
                _pFirstFrame = 0;
                _pLastFrame = (_globalSprite.Textures == null ? 0 : _globalSprite.Textures.Count - 1);

                _pCurrentFrame = _pFirstFrame;
            }
            else
            {
                string tmpIndex = s_animationSequencePreview.SelectedItem.ToString()?.Split(']')[0].Split('[')[1];
                if (tmpIndex != null)
                {
                    _pFirstFrame = _globalSprite.SpriteAnimationSequence[tmpIndex].FrameFrom;
                    _pLastFrame = _globalSprite.SpriteAnimationSequence[tmpIndex].FrameTo;
                }

                _pCurrentFrame = _pFirstFrame;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                AnimationSequencerForm asf = new AnimationSequencerForm();
                asf.FFrameFromMin = 0;
                asf.FFrameFromMax = _globalSprite.Textures.Count - 1;
                asf.FFrameToMin = 0;
                asf.FFrameToMax = _globalSprite.Textures.Count - 1;

                string tmpIndex = listBox1.SelectedItem.ToString()?.Split(']')[0].Split('[')[1];
                if (tmpIndex != null)
                {
                    asf.FFullName = _globalSprite.SpriteAnimationSequence[tmpIndex].Name;
                    asf.FIndexName = _globalSprite.SpriteAnimationSequence[tmpIndex].Index;
                    asf.FFrameFrom = _globalSprite.SpriteAnimationSequence[tmpIndex].FrameFrom;
                    asf.FFrameTo = _globalSprite.SpriteAnimationSequence[tmpIndex].FrameTo;
                }

                asf.ShowDialog();
                if (asf.DialogResult != DialogResult.OK) return;
                _globalSprite.SpriteAnimationSequence[asf.FIndexName].FrameFrom = asf.FFrameFrom;
                _globalSprite.SpriteAnimationSequence[asf.FIndexName].FrameTo = asf.FFrameTo;
                _globalSprite.SpriteAnimationSequence[asf.FIndexName].Name = asf.FFullName;
                UpdateForm();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            if (MessageBox.Show("Do You want to delete selected item?",
                    "Delete [" + listBox1.SelectedItem.ToString()?.Split(']')[0].Split('[')[1] + "]",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            _globalSprite.SpriteAnimationSequence.Remove(listBox1.SelectedItem.ToString()?.Split(']')[0].Split('[')[1] ?? string.Empty);
            UpdateForm();
        }
    }
}
