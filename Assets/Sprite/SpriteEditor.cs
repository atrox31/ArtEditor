using System;
using System.Drawing;
using System.Windows.Forms;
using ArtCore_Editor.Pick_forms;

namespace ArtCore_Editor.Assets.Sprite
{
    public partial class SpriteEditor : Form
    {
        private bool _saved = true;
        public Sprite GlobalSprite;
        private int _pFirstFrame, _pLastFrame, _pCurrentFrame;

        private void CreateNewSprite()
        {
            _pCurrentFrame = _pFirstFrame;
            _pLastFrame = 0;
            GlobalSprite = new Sprite();
        }

        public SpriteEditor(string sprite = null)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            s_preview.SizeMode = PictureBoxSizeMode.StretchImage;
            if (sprite != null)
            {
                GlobalSprite = GameProject.GetInstance().Sprites[sprite];
                GlobalSprite.Load(GameProject.ProjectPath + GlobalSprite.ProjectPath + "\\" + GlobalSprite.FileName);
                _pFirstFrame = 0;
                _pLastFrame = (GlobalSprite.Textures == null ? 0 : GlobalSprite.Textures.Count - 1);
                _pCurrentFrame = _pFirstFrame;
            }
            else
            {
                CreateNewSprite();
            }
            UpdateForm();
            s_preview.Refresh();
        }


        private void SaveSprite()
        {
            while (s_spritename.Text.Length == 0)
            {
                s_spritename.Text = GetString.Get("Sprite name");
            }

            GlobalSprite.CollisionMask =
                (Sprite.CollisionMaskEnum)(s_collision_have_mask.Checked ? (s_col_mask_circle.Checked ? 1 : s_col_mask_rect.Checked ? 2 : s_col_mask_perpixel.Checked ? 3 : 0) : 0);
            GlobalSprite.CollisionMaskValue = s_col_mask_value.Value;
            GlobalSprite.SpriteCenter = (s_sprite_center_center.Checked ? Sprite.SpriteCenterEnum.Center : s_sprite_center_left.Checked ? Sprite.SpriteCenterEnum.LeftCorner : Sprite.SpriteCenterEnum.Custom);
            GlobalSprite.SpriteCenterX = (int)s_aprite_center_x.Value;
            GlobalSprite.SpriteCenterY = (int)s_sprite_center_y.Value;

            GlobalSprite.EditorFps = (int)s_preview_fps.Value;
            _pCurrentFrame = _pFirstFrame;
            _pLastFrame = (GlobalSprite.Textures == null ? 0 : GlobalSprite.Textures.Count - 1);

            GlobalSprite.Name = s_spritename.Text;
            GlobalSprite.Save();

            if (!MainWindow.GetInstance().GlobalProject.Sprites.ContainsKey(GlobalSprite.Name))
            {
                // add new
                MainWindow.GetInstance().GlobalProject.Sprites.Add(s_spritename.Text, new Sprite());
            }
            else
            {
                if (GlobalSprite.Name != MainWindow.GetInstance().GlobalProject.Sprites[GlobalSprite.Name].Name)
                {
                    MainWindow.GetInstance().GlobalProject.Sprites.RenameKey(GlobalSprite.Name, s_spritename.Text);
                }
            }
            GlobalSprite.Name = s_spritename.Text;
            MainWindow.GetInstance().GlobalProject.Sprites[GlobalSprite.Name] = GlobalSprite;
        }
        
        private void UpdateForm()
        {
            s_spritename.Text = GlobalSprite.Name;

            s_sprite_center_y.Maximum = GlobalSprite.SpriteWidth;
            s_aprite_center_x.Maximum = GlobalSprite.SpriteHeight;

            s_col_mask_value.Maximum = Math.Max(GlobalSprite.SpriteHeight, GlobalSprite.SpriteWidth) / 2;

            s_aprite_center_x.Value = GlobalSprite.SpriteCenterX;
            s_sprite_center_y.Value = GlobalSprite.SpriteCenterY;

            s_collision_have_mask.Checked = true;
            switch (GlobalSprite.CollisionMask)
            {
                case Sprite.CollisionMaskEnum.None:
                    s_collision_have_mask.Checked = false;
                    break;
                case Sprite.CollisionMaskEnum.Circle:
                    s_col_mask_circle.Checked = true;
                    break;
                case Sprite.CollisionMaskEnum.Rectangle:
                    s_col_mask_rect.Checked = true;
                    break;
                case Sprite.CollisionMaskEnum.Perpixel:
                    s_col_mask_perpixel.Checked = true;
                    break;
            }
            s_col_mask_value.Value = GlobalSprite.CollisionMaskValue;

            switch (GlobalSprite.SpriteCenter)
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
            s_preview_fps.Value = GlobalSprite.EditorFps;

            _pFirstFrame = 0;
            _pLastFrame = (GlobalSprite.Textures == null ? 0 : GlobalSprite.Textures.Count - 1);


            _pCurrentFrame = _pFirstFrame;

            s_preview_loop.Checked = GlobalSprite.EditorPreviewLoop;
            s_sprite_center_show.Checked = GlobalSprite.EditorShowCenter;
            s_col_mask_show.Checked = GlobalSprite.EditorShowMask;
            label4.Text = "Current: " + (GlobalSprite.Textures?.Count ?? 0).ToString();
            label5.Text = _pCurrentFrame.ToString() + "/" + _pLastFrame.ToString();
            s_preview.Image = (GlobalSprite.Textures == null || GlobalSprite.Textures.Count == 0 ? null : GlobalSprite.Textures[_pCurrentFrame]);

            s_animationSequencePreview.Items.Clear();
            s_animationSequencePreview.Items.Add("<All frames>");
            s_animationSequencePreview.SelectedIndex = 0;
            listBox1.Items.Clear();
            foreach (var item in GlobalSprite.SpriteAnimationSequence)
            {
                s_animationSequencePreview.Items.Add("[" + item.Value.Index + "] " + item.Value.Name);
                listBox1.Items.Add("[" + item.Value.Index + "] " + item.Value.Name + "( " + item.Value.FrameFrom.ToString() + ":" + item.Value.FrameTo.ToString() + " )");
            }

        }

        private void ImportSpriteImages(bool clearList)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            if (clearList)
            {
                GlobalSprite.ClearImages();
                GC.Collect();
                GlobalSprite.Textures = new System.Collections.Generic.List<Image>(openFileDialog1.FileNames.Length);
            }
            
            var maxX = 0;
            var maxY = 0;
            var i = 0;
            foreach (var file in openFileDialog1.FileNames)
            {
                GlobalSprite.AddImage(file);
                maxX = Math.Max(GlobalSprite.Textures[i].Width, maxX);
                maxY = Math.Max(GlobalSprite.Textures[i].Height, maxY);
                i++;
            }
            s_preview.Image = GlobalSprite.Textures[0];
            label4.Text = "Current: " + GlobalSprite.Textures.Count.ToString();
            _pLastFrame = GlobalSprite.Textures.Count - 1;
            _pCurrentFrame = _pFirstFrame;

            GlobalSprite.SpriteWidth = maxX;
            GlobalSprite.SpriteHeight = maxY;

            s_sprite_center_y.Maximum = GlobalSprite.SpriteWidth;
            s_aprite_center_x.Maximum = GlobalSprite.SpriteHeight;
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
            ImageListViewer ilv = new ImageListViewer(GlobalSprite);
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
                        GlobalSprite.Save();
                        break;
                    }
                }

                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = GlobalSprite.Name.Length == 0 ? DialogResult.No : DialogResult.OK;
            }

        }

        private void s_spritename_MouseEnter(object sender, EventArgs e)
        {
            s_spritename.BackColor = Color.White;
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
                if (GlobalSprite.EditorPreviewLoop)
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
            s_preview.Image = GlobalSprite.Textures[_pCurrentFrame];
        }

        private void s_preview_stop_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            s_preview_play.Enabled = true;
            s_preview_stop.Enabled = false;
        }

        private void s_preview_loop_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSprite.EditorPreviewLoop = s_preview_loop.Checked;
        }

        private void s_preview_fps_ValueChanged(object sender, EventArgs e)
        {
            GlobalSprite.EditorFps = (int)s_preview_fps.Value;
            timer1.Interval = (int)(1000 / s_preview_fps.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            _pCurrentFrame = _pFirstFrame;
            label5.Text = _pCurrentFrame.ToString() + "/" + _pLastFrame.ToString();
            s_preview.Image = GlobalSprite.Textures[_pCurrentFrame];
        }

        private void s_preview_next_Click(object sender, EventArgs e)
        {
            _pCurrentFrame++;
            if (_pCurrentFrame > _pLastFrame)
            {
                _pCurrentFrame = GlobalSprite.EditorPreviewLoop ? _pFirstFrame : _pLastFrame;
            }
            label5.Text = _pCurrentFrame.ToString() + "/" + _pLastFrame.ToString();
            s_preview.Image = GlobalSprite.Textures[_pCurrentFrame];
        }

        private void s_preview_prev_Click(object sender, EventArgs e)
        {
            _pCurrentFrame--;
            if (_pCurrentFrame < 0)
            {
                _pCurrentFrame = GlobalSprite.EditorPreviewLoop ? _pLastFrame : _pFirstFrame;
            }
            label5.Text = _pCurrentFrame.ToString() + "/" + _pLastFrame.ToString();
            s_preview.Image = GlobalSprite.Textures[_pCurrentFrame];
        }


        private void s_sprite_center_x_ValueChanged(object sender, EventArgs e)
        {
            GlobalSprite.SpriteCenterX = Convert.ToInt32(s_aprite_center_x.Value);
            s_preview.Refresh();
        }

        private void s_sprite_center_y_ValueChanged(object sender, EventArgs e)
        {
            GlobalSprite.SpriteCenterY = Convert.ToInt32(s_sprite_center_y.Value);
            s_preview.Refresh();
        }

        public static int Remap(float value, float from1, float to1, float from2, float to2)
        {
            return Convert.ToInt32((value - from1) / (to1 - from1) * (to2 - from2) + from2);
        }

        private void s_preview_Paint(object sender, PaintEventArgs e)
        {
            if (GlobalSprite.Textures == null) return;
            if (GlobalSprite.EditorShowCenter)
            {
                Pen redPen = new Pen(Color.Red, 1);
                Pen bluePen = new Pen(Color.Blue, 1);
                int x1 = Remap(GlobalSprite.SpriteCenterX, 0, GlobalSprite.SpriteWidth, 0, s_preview.Width);
                int y2 = Remap(GlobalSprite.SpriteCenterY, 0, GlobalSprite.SpriteHeight, 0, s_preview.Height);
                e.Graphics.DrawLine(redPen, x1, 0, x1, s_preview.Width);
                e.Graphics.DrawLine(bluePen, 0, y2, s_preview.Height, y2);

                //Dispose of objects
                redPen.Dispose();
                bluePen.Dispose();
            }

            if (!GlobalSprite.EditorShowMask) return;
            {
                switch (GlobalSprite.CollisionMask)
                {
                    case Sprite.CollisionMaskEnum.None:
                        break;
                    case Sprite.CollisionMaskEnum.Circle:
                    {
                        int x = Remap(GlobalSprite.SpriteCenterX, 0, GlobalSprite.SpriteWidth, 0, s_preview.Width);
                        int y = Remap(GlobalSprite.SpriteCenterY, 0, GlobalSprite.SpriteHeight, 0, s_preview.Height);
                        Brush fillPen = new SolidBrush(Color.FromArgb(60, 132, 59, 98));
                        Pen redPen = new Pen(Color.FromArgb(255, 132, 59, 98), 2);
                        Rectangle r = new Rectangle(
                            x - Remap(GlobalSprite.CollisionMaskValue / 2, 0, (GlobalSprite.SpriteWidth + GlobalSprite.SpriteHeight) / 2, 0, s_preview.Width),
                            y - Remap(GlobalSprite.CollisionMaskValue / 2, 0, (GlobalSprite.SpriteWidth + GlobalSprite.SpriteHeight) / 2, 0, s_preview.Width),
                            Remap(GlobalSprite.CollisionMaskValue, 0, (GlobalSprite.SpriteWidth + GlobalSprite.SpriteHeight) / 2, 0, s_preview.Width),
                            Remap(GlobalSprite.CollisionMaskValue, 0, (GlobalSprite.SpriteWidth + GlobalSprite.SpriteHeight) / 2, 0, s_preview.Width)
                        );
                        e.Graphics.FillEllipse(fillPen, r);
                        e.Graphics.DrawEllipse(redPen, r);
                        redPen.Dispose();
                    }
                        break;
                    case Sprite.CollisionMaskEnum.Rectangle:
                    {
                        int x = Remap(GlobalSprite.SpriteCenterX, 0, GlobalSprite.SpriteWidth, 0, s_preview.Width);
                        int y = Remap(GlobalSprite.SpriteCenterY, 0, GlobalSprite.SpriteHeight, 0, s_preview.Height);
                        Brush fillPen = new SolidBrush(Color.FromArgb(60, 132, 59, 98));
                        Pen redPen = new Pen(Color.FromArgb(255, 132, 59, 98), 2);
                        Rectangle r = new Rectangle(
                            x - GlobalSprite.CollisionMaskValue / 2,
                            y - GlobalSprite.CollisionMaskValue / 2,
                            GlobalSprite.CollisionMaskValue,
                            GlobalSprite.CollisionMaskValue
                        );
                        e.Graphics.FillRectangle(fillPen, r);
                        e.Graphics.DrawRectangle(redPen, r);
                        redPen.Dispose();
                    }
                        break;
                    case Sprite.CollisionMaskEnum.Perpixel:
                        // draw nothing
                        break;
                    default:
                        break;
                }
            }
        }

        private void s_sprite_center_center_CheckedChanged(object sender, EventArgs e)
        {
            if (!s_sprite_center_center.Checked) return;
            GlobalSprite.SpriteCenterX = GlobalSprite.SpriteWidth / 2;
            GlobalSprite.SpriteCenterY = GlobalSprite.SpriteHeight / 2;
            s_aprite_center_x.Value = GlobalSprite.SpriteCenterX;
            s_sprite_center_y.Value = GlobalSprite.SpriteCenterY;
            s_preview.Refresh();
            s_aprite_center_x.Enabled = false;
            s_sprite_center_y.Enabled = false;
        }

        private void s_sprite_center_left_CheckedChanged(object sender, EventArgs e)
        {
            if (!s_sprite_center_left.Checked) return;
            GlobalSprite.SpriteCenterX = 0;
            GlobalSprite.SpriteCenterY = 0;
            s_aprite_center_x.Value = 0;
            s_sprite_center_y.Value = 0;
            s_preview.Refresh();
            s_aprite_center_x.Enabled = false;
            s_sprite_center_y.Enabled = false;
        }

        private void s_sprite_center_custom_CheckedChanged(object sender, EventArgs e)
        {
            if (!s_sprite_center_custom.Checked) return;
            GlobalSprite.SpriteCenterX = Convert.ToInt32(s_aprite_center_x.Value);
            GlobalSprite.SpriteCenterY = Convert.ToInt32(s_sprite_center_y.Value);
            s_preview.Refresh();
            s_aprite_center_x.Enabled = true;
            s_sprite_center_y.Enabled = true;
        }

        private void s_sprite_center_show_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSprite.EditorShowCenter = s_sprite_center_show.Checked;
            s_preview.Refresh();
        }

        private void s_collision_have_mask_CheckedChanged(object sender, EventArgs e)
        {
            s_col_mask_circle.Enabled = s_collision_have_mask.Checked;
            s_col_mask_perpixel.Enabled = s_collision_have_mask.Checked;
            s_col_mask_rect.Enabled = s_collision_have_mask.Checked;
            s_col_mask_show.Enabled = s_collision_have_mask.Checked;
            s_col_mask_value.Enabled = s_collision_have_mask.Checked;
            s_preview.Refresh();
        }

        private void s_col_mask_circle_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSprite.CollisionMask = Sprite.CollisionMaskEnum.Circle;
            s_preview.Refresh();
        }

        private void s_col_mask_rect_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSprite.CollisionMask = Sprite.CollisionMaskEnum.Rectangle;
            s_preview.Refresh();
        }

        private void s_col_mask_perpixel_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSprite.CollisionMask = Sprite.CollisionMaskEnum.Perpixel;
            s_preview.Refresh();
        }

        private void s_col_mask_show_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSprite.EditorShowMask = s_col_mask_show.Checked;
            s_preview.Refresh();
        }

        private void s_col_mask_value_Scroll(object sender, EventArgs e)
        {
            GlobalSprite.CollisionMaskValue = Convert.ToInt32((Convert.ToDecimal(s_col_mask_value.Value) / 100) * Convert.ToDecimal((GlobalSprite.SpriteWidth + GlobalSprite.SpriteHeight) / 2));
            label3.Text = "Value ( " + GlobalSprite.CollisionMaskValue + " px)";
            s_preview.Refresh();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            AnimationSequencerForm asf = new AnimationSequencerForm();
            asf.FFrameFromMin = 0;
            asf.FFrameFromMax = GlobalSprite.Textures.Count - 1;
            asf.FFrameToMin = 0;
            asf.FFrameToMax = GlobalSprite.Textures.Count - 1;
            asf.ShowDialog();
            if (asf.DialogResult == DialogResult.OK)
            {
                GlobalSprite.SpriteAnimationSequence.Add(asf.FIndexName, new Sprite.AnimationSequence(asf.FFullName, asf.FIndexName, asf.FFrameFrom, asf.FFrameTo));
                UpdateForm();
            }
        }

        private void s_animationSequencePreview_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (s_animationSequencePreview.SelectedIndex == 0)
            {
                _pFirstFrame = 0;
                _pLastFrame = (GlobalSprite.Textures == null ? 0 : GlobalSprite.Textures.Count - 1);

                _pCurrentFrame = _pFirstFrame;
            }
            else
            {
                string tmpIndex = s_animationSequencePreview.SelectedItem.ToString()?.Split(']')[0].Split('[')[1];
                if (tmpIndex != null)
                {
                    _pFirstFrame = GlobalSprite.SpriteAnimationSequence[tmpIndex].FrameFrom;
                    _pLastFrame = GlobalSprite.SpriteAnimationSequence[tmpIndex].FrameTo;
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
                asf.FFrameFromMax = GlobalSprite.Textures.Count - 1;
                asf.FFrameToMin = 0;
                asf.FFrameToMax = GlobalSprite.Textures.Count - 1;

                var tmpIndex = listBox1.SelectedItem.ToString()?.Split(']')[0].Split('[')[1];
                if (tmpIndex != null)
                {
                    asf.FFullName = GlobalSprite.SpriteAnimationSequence[tmpIndex].Name;
                    asf.FIndexName = GlobalSprite.SpriteAnimationSequence[tmpIndex].Index;
                    asf.FFrameFrom = GlobalSprite.SpriteAnimationSequence[tmpIndex].FrameFrom;
                    asf.FFrameTo = GlobalSprite.SpriteAnimationSequence[tmpIndex].FrameTo;
                }

                asf.ShowDialog();
                if (asf.DialogResult != DialogResult.OK) return;
                GlobalSprite.SpriteAnimationSequence[asf.FIndexName].FrameFrom = asf.FFrameFrom;
                GlobalSprite.SpriteAnimationSequence[asf.FIndexName].FrameTo = asf.FFrameTo;
                GlobalSprite.SpriteAnimationSequence[asf.FIndexName].Name = asf.FFullName;
                UpdateForm();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            if (MessageBox.Show("Do You want to delete selected item?",
                    "Delete [" + listBox1.SelectedItem.ToString()?.Split(']')[0].Split('[')[1] + "]",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            GlobalSprite.SpriteAnimationSequence.Remove(listBox1.SelectedItem.ToString()?.Split(']')[0].Split('[')[1] ?? string.Empty);
            UpdateForm();
        }
    }
}
