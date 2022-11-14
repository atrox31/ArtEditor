using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class SpriteEditor : Form
    {
        bool saved = true;
        public Sprite global_sprite;
        int p_firstFrame, p_lastFrame, p_currentFrame;
        void createNewSprite()
        {
            p_currentFrame = p_firstFrame;
            p_lastFrame = 0;
            global_sprite = new Sprite();
        }

        public SpriteEditor(string sprite)
        {
            //this.parrent = parrent;
            InitializeComponent(); Program.ApplyTheme(this);
            s_preview.SizeMode = PictureBoxSizeMode.StretchImage;
            if (sprite != null)
            {
                global_sprite = GameProject.GetInstance().Sprites[sprite];
                global_sprite.Load(GameProject.ProjectPath + global_sprite.ProjectPath + "\\" + global_sprite.FileName);
                p_firstFrame = 0;
                p_lastFrame = (global_sprite.textures == null ? 0 : global_sprite.textures.Count - 1);

                p_currentFrame = p_firstFrame;
                
            }
            else
            {
                createNewSprite();
            }
            //importSpriteImages();
            updateForm();
            s_preview.Refresh();

        }


        void saveSprite()
        {
            while (s_spritename.Text.Length == 0)
            {
                s_spritename.Text = GetString.Get("Sprite name");
            }

            global_sprite.type = (Sprite.Type)s_sprite_type.SelectedIndex;
            global_sprite.collision_mask =
                (Sprite.CollisionMask)(s_collision_have_mask.Checked ? (s_col_mask_circle.Checked ? 1 : s_col_mask_rect.Checked ? 2 : s_col_mask_perpixel.Checked ? 3 : 0) : 0);
            global_sprite.collision_mask_value = s_col_mask_value.Value;
            global_sprite.sprite_center = (s_sprite_center_center.Checked ? Sprite.SpriteCenter.center : s_sprite_center_left.Checked ? Sprite.SpriteCenter.leftcorner : Sprite.SpriteCenter.custom);
            global_sprite.sprite_center_x = (int)s_aprite_center_x.Value;
            global_sprite.sprite_center_y = (int)s_sprite_center_y.Value;

            global_sprite.editor_fps = (int)s_preview_fps.Value;
            p_currentFrame = p_firstFrame;
            p_lastFrame = (global_sprite.textures == null ? 0 : global_sprite.textures.Count - 1);

            global_sprite.Name = s_spritename.Text;
            global_sprite.Save();

            if (!MainWindow.GetInstance().Game_Project.Sprites.ContainsKey(global_sprite.Name))
            {
                // add new
                MainWindow.GetInstance().Game_Project.Sprites.Add(s_spritename.Text, new Sprite());
            }
            else
            {
                if (global_sprite.Name != MainWindow.GetInstance().Game_Project.Sprites[global_sprite.Name].Name)
                {
                    Functions.RenameKey(MainWindow.GetInstance().Game_Project.Sprites, global_sprite.Name, s_spritename.Text);
                }
            }

            global_sprite.Name = s_spritename.Text;
            MainWindow.GetInstance().Game_Project.Sprites[global_sprite.Name] = global_sprite;

        }


        void updateForm()
        {
            s_spritename.Text = global_sprite.Name;
            s_sprite_type.SelectedIndex = (int)global_sprite.type;

            s_sprite_center_y.Maximum = global_sprite.sprite_width;
            s_aprite_center_x.Maximum = global_sprite.sprite_height;

            s_col_mask_value.Maximum = Math.Max(global_sprite.sprite_height, global_sprite.sprite_width) / 2;

            s_aprite_center_x.Value = global_sprite.sprite_center_x;
            s_sprite_center_y.Value = global_sprite.sprite_center_y;

            s_collision_have_mask.Checked = true;
            switch (global_sprite.collision_mask)
            {
                case Sprite.CollisionMask.none:
                    s_collision_have_mask.Checked = false;
                    break;
                case Sprite.CollisionMask.circle:
                    s_col_mask_circle.Checked = true;
                    break;
                case Sprite.CollisionMask.rectangle:
                    s_col_mask_rect.Checked = true;
                    break;
                case Sprite.CollisionMask.perpixel:
                    s_col_mask_perpixel.Checked = true;
                    break;
            }
            s_col_mask_value.Value = global_sprite.collision_mask_value;

            switch (global_sprite.sprite_center)
            {
                case Sprite.SpriteCenter.center:
                    s_sprite_center_center.Checked = true;
                    break;
                case Sprite.SpriteCenter.leftcorner:
                    s_sprite_center_left.Checked = true;
                    break;
                case Sprite.SpriteCenter.custom:
                    s_sprite_center_custom.Checked = true;
                    break;
            }
            s_preview_fps.Value = global_sprite.editor_fps;

            p_firstFrame = 0;
            p_lastFrame = (global_sprite.textures == null ? 0 : global_sprite.textures.Count - 1);


            p_currentFrame = p_firstFrame;

            s_preview_loop.Checked = global_sprite.editor_preview_loop;
            s_sprite_center_show.Checked = global_sprite.editor_show_center;
            s_col_mask_show.Checked = global_sprite.editor_show_mask;
            label4.Text = "Current: " + (global_sprite.textures == null ? 0 : global_sprite.textures.Count).ToString();
            label5.Text = p_currentFrame.ToString() + "/" + p_lastFrame.ToString();
            s_preview.Image = (global_sprite.textures == null || global_sprite.textures.Count == 0 ? null : global_sprite.textures[p_currentFrame]);

            s_animationSequencePreview.Items.Clear();
            s_animationSequencePreview.Items.Add("<All frames>");
            s_animationSequencePreview.SelectedIndex = 0;
            listBox1.Items.Clear();
            foreach (var item in global_sprite.sprite_animationSquence)
            {
                s_animationSequencePreview.Items.Add("[" + item.Value.index + "] " + item.Value.name);
                listBox1.Items.Add("[" + item.Value.index + "] " + item.Value.name + "( " + item.Value.frameFrom.ToString() + ":" + item.Value.frameTo.ToString() + " )");
            }

        }

        bool importSpriteImages(bool clearList)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (clearList)
                {
                    global_sprite.ClearImages();
                    GC.Collect();
                    global_sprite.textures = new System.Collections.Generic.List<Image>(openFileDialog1.FileNames.Length);
                }


                int max_x = 0;
                int max_y = 0;
                int i = 0;
                foreach (string file in openFileDialog1.FileNames)
                {
                    global_sprite.AddImage(file);
                    max_x = Math.Max(global_sprite.textures[i].Width, max_x);
                    max_y = Math.Max(global_sprite.textures[i].Height, max_y);
                    i++;
                }
                s_preview.Image = global_sprite.textures[0];
                label4.Text = "Current: " + global_sprite.textures.Count.ToString();
                p_lastFrame = global_sprite.textures.Count - 1;
                p_currentFrame = p_firstFrame;

                global_sprite.sprite_width = max_x;
                global_sprite.sprite_height = max_y;

                s_sprite_center_y.Maximum = global_sprite.sprite_width;
                s_aprite_center_x.Maximum = global_sprite.sprite_height;
                saved = false;
                return true;
            }
            return false;
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            // inport textures
            importSpriteImages(MessageBox.Show("Do You want to clear image list before adding new images?", "Replace or add", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
            saved = false;
            updateForm();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ImageListViewer ilv = new ImageListViewer(global_sprite);
            ilv.ShowDialog();
            ilv.Dispose();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            saveSprite();
            saved = true;
        }


        private void SpriteAddForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saved)
            {
                DialogResult q = MessageBox.Show("Do You want to save?", "Data changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (q == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                if (q == DialogResult.Yes)
                {
                    while (s_spritename.Text.Length == 0)
                    {
                        s_spritename.Text = GetString.Get("Sprite name");
                    }
                    global_sprite.Save();
                }
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                if (global_sprite.Name.Length == 0)
                {
                    this.DialogResult = DialogResult.No;
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                }
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
            p_currentFrame++;
            if (p_currentFrame > p_lastFrame)
            {
                if (global_sprite.editor_preview_loop)
                {
                    p_currentFrame = p_firstFrame;
                }
                else
                {
                    p_currentFrame = p_lastFrame;
                    timer1.Stop();
                    s_preview_play.Enabled = true;
                    s_preview_stop.Enabled = false;
                }
            }
            label5.Text = p_currentFrame.ToString() + "/" + p_lastFrame.ToString();
            s_preview.Image = global_sprite.textures[p_currentFrame];
        }

        private void s_preview_stop_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            s_preview_play.Enabled = true;
            s_preview_stop.Enabled = false;
        }

        private void s_preview_loop_CheckedChanged(object sender, EventArgs e)
        {
            global_sprite.editor_preview_loop = s_preview_loop.Checked;
        }

        private void s_preview_fps_ValueChanged(object sender, EventArgs e)
        {
            global_sprite.editor_fps = (int)s_preview_fps.Value;
            timer1.Interval = (int)(1000 / s_preview_fps.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            p_currentFrame = p_firstFrame;
            label5.Text = p_currentFrame.ToString() + "/" + p_lastFrame.ToString();
            s_preview.Image = global_sprite.textures[p_currentFrame];
        }

        private void s_preview_next_Click(object sender, EventArgs e)
        {
            p_currentFrame++;
            if (p_currentFrame > p_lastFrame)
            {
                if (global_sprite.editor_preview_loop)
                {
                    p_currentFrame = p_firstFrame;
                }
                else
                {
                    p_currentFrame = p_lastFrame;
                }
            }
            label5.Text = p_currentFrame.ToString() + "/" + p_lastFrame.ToString();
            s_preview.Image = global_sprite.textures[p_currentFrame];
        }

        private void s_preview_prev_Click(object sender, EventArgs e)
        {
            p_currentFrame--;
            if (p_currentFrame < 0)
            {
                if (global_sprite.editor_preview_loop)
                {
                    p_currentFrame = p_lastFrame;
                }
                else
                {
                    p_currentFrame = p_firstFrame;
                }
            }
            label5.Text = p_currentFrame.ToString() + "/" + p_lastFrame.ToString();
            s_preview.Image = global_sprite.textures[p_currentFrame];
        }


        private void s_aprite_center_x_ValueChanged(object sender, EventArgs e)
        {
            global_sprite.sprite_center_x = Convert.ToInt32(s_aprite_center_x.Value);
            s_preview.Refresh();
        }

        private void s_sprite_center_y_ValueChanged(object sender, EventArgs e)
        {
            global_sprite.sprite_center_y = Convert.ToInt32(s_sprite_center_y.Value);
            s_preview.Refresh();
        }

        public static int Remap(float value, float from1, float to1, float from2, float to2)
        {
            return Convert.ToInt32((value - from1) / (to1 - from1) * (to2 - from2) + from2);
        }

        private void s_preview_Paint(object sender, PaintEventArgs e)
        {
            if (global_sprite.textures != null)
            {
                if (global_sprite.editor_show_center)
                {
                    Pen redPen = new Pen(Color.Red, 1);
                    Pen bluePen = new Pen(Color.Blue, 1);
                    int x1 = Remap(global_sprite.sprite_center_x, 0, global_sprite.sprite_width, 0, s_preview.Width);
                    int y2 = Remap(global_sprite.sprite_center_y, 0, global_sprite.sprite_height, 0, s_preview.Height);
                    e.Graphics.DrawLine(redPen, x1, 0, x1, s_preview.Width);
                    e.Graphics.DrawLine(bluePen, 0, y2, s_preview.Height, y2);

                    //Dispose of objects
                    redPen.Dispose();
                    bluePen.Dispose();
                }
                if (global_sprite.editor_show_mask)
                {
                    switch (global_sprite.collision_mask)
                    {
                        case Sprite.CollisionMask.none:
                            break;
                        case Sprite.CollisionMask.circle:
                            {
                                int x = Remap(global_sprite.sprite_center_x, 0, global_sprite.sprite_width, 0, s_preview.Width);
                                int y = Remap(global_sprite.sprite_center_y, 0, global_sprite.sprite_height, 0, s_preview.Height);
                                Brush fillPen = new SolidBrush(Color.FromArgb(60, 132, 59, 98));
                                Pen redPen = new Pen(Color.FromArgb(255, 132, 59, 98), 2);
                                Rectangle r = new Rectangle(
                                    x - Remap(global_sprite.collision_mask_value / 2, 0, (global_sprite.sprite_width + global_sprite.sprite_height) / 2, 0, s_preview.Width),
                                    y - Remap(global_sprite.collision_mask_value / 2, 0, (global_sprite.sprite_width + global_sprite.sprite_height) / 2, 0, s_preview.Width),
                                    Remap(global_sprite.collision_mask_value, 0, (global_sprite.sprite_width + global_sprite.sprite_height) / 2, 0, s_preview.Width),
                                    Remap(global_sprite.collision_mask_value, 0, (global_sprite.sprite_width + global_sprite.sprite_height) / 2, 0, s_preview.Width)
                                    );
                                e.Graphics.FillEllipse(fillPen, r);
                                e.Graphics.DrawEllipse(redPen, r);
                                redPen.Dispose();
                            }
                            break;
                        case Sprite.CollisionMask.rectangle:
                            {
                                int x = Remap(global_sprite.sprite_center_x, 0, global_sprite.sprite_width, 0, s_preview.Width);
                                int y = Remap(global_sprite.sprite_center_y, 0, global_sprite.sprite_height, 0, s_preview.Height);
                                Brush fillPen = new SolidBrush(Color.FromArgb(60, 132, 59, 98));
                                Pen redPen = new Pen(Color.FromArgb(255, 132, 59, 98), 2);
                                Rectangle r = new Rectangle(
                                    x - global_sprite.collision_mask_value / 2,
                                    y - global_sprite.collision_mask_value / 2,
                                    global_sprite.collision_mask_value,
                                    global_sprite.collision_mask_value
                                    );
                                e.Graphics.FillRectangle(fillPen, r);
                                e.Graphics.DrawRectangle(redPen, r);
                                redPen.Dispose();
                            }
                            break;
                        case Sprite.CollisionMask.perpixel:

                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void s_sprite_center_center_CheckedChanged(object sender, EventArgs e)
        {
            if (s_sprite_center_center.Checked)
            {
                global_sprite.sprite_center_x = global_sprite.sprite_width / 2;
                global_sprite.sprite_center_y = global_sprite.sprite_height / 2;
                s_aprite_center_x.Value = global_sprite.sprite_center_x;
                s_sprite_center_y.Value = global_sprite.sprite_center_y;
                s_preview.Refresh();
                s_aprite_center_x.Enabled = false;
                s_sprite_center_y.Enabled = false;
            }
        }

        private void s_sprite_center_left_CheckedChanged(object sender, EventArgs e)
        {
            if (s_sprite_center_left.Checked)
            {
                global_sprite.sprite_center_x = 0;
                global_sprite.sprite_center_y = 0;
                s_aprite_center_x.Value = 0;
                s_sprite_center_y.Value = 0;
                s_preview.Refresh();
                s_aprite_center_x.Enabled = false;
                s_sprite_center_y.Enabled = false;
            }
        }

        private void s_sprite_center_custom_CheckedChanged(object sender, EventArgs e)
        {
            if (s_sprite_center_custom.Checked)
            {
                global_sprite.sprite_center_x = Convert.ToInt32(s_aprite_center_x.Value);
                global_sprite.sprite_center_y = Convert.ToInt32(s_sprite_center_y.Value);
                s_preview.Refresh();
                s_aprite_center_x.Enabled = true;
                s_sprite_center_y.Enabled = true;
            }
        }

        private void s_sprite_center_show_CheckedChanged(object sender, EventArgs e)
        {
            global_sprite.editor_show_center = s_sprite_center_show.Checked;
            s_preview.Refresh();
        }

        private void s_collision_have_mask_CheckedChanged(object sender, EventArgs e)
        {
            if (s_collision_have_mask.Checked == false)
            {
                //global_sprite.collision_mask = Sprite.CollisionMask.none;
            }
            else
            {
                //global_sprite.collision_mask = (Sprite.CollisionMask)(s_collision_have_mask.Checked ? 0 : s_col_mask_circle.Checked ? 1 : s_col_mask_rect.Checked ? 2 : s_col_mask_perpixel.Checked ? 3 : 0);

            }
            s_col_mask_circle.Enabled = s_collision_have_mask.Checked;
            s_col_mask_perpixel.Enabled = s_collision_have_mask.Checked;
            s_col_mask_rect.Enabled = s_collision_have_mask.Checked;
            s_col_mask_show.Enabled = s_collision_have_mask.Checked;
            s_col_mask_value.Enabled = s_collision_have_mask.Checked;
            s_preview.Refresh();
        }

        private void SpriteAddForm_Load_1(object sender, EventArgs e)
        {
            //s_preview.SizeMode = PictureBoxSizeMode.StretchImage;
            //p_firstFrame = 0;
            //p_lastFrame = 0;
            //p_currentFrame = p_firstFrame;
        }

        private void s_col_mask_circle_CheckedChanged(object sender, EventArgs e)
        {
            global_sprite.collision_mask = Sprite.CollisionMask.circle;
            s_preview.Refresh();
        }

        private void s_col_mask_rect_CheckedChanged(object sender, EventArgs e)
        {
            global_sprite.collision_mask = Sprite.CollisionMask.rectangle;
            s_preview.Refresh();
        }

        private void s_col_mask_perpixel_CheckedChanged(object sender, EventArgs e)
        {
            global_sprite.collision_mask = Sprite.CollisionMask.perpixel;
            s_preview.Refresh();
        }

        private void s_col_mask_show_CheckedChanged(object sender, EventArgs e)
        {
            global_sprite.editor_show_mask = s_col_mask_show.Checked;
            s_preview.Refresh();
        }

        private void s_col_mask_value_Scroll(object sender, EventArgs e)
        {
            global_sprite.collision_mask_value = Convert.ToInt32((Convert.ToDecimal(s_col_mask_value.Value) / 100) * Convert.ToDecimal((global_sprite.sprite_width + global_sprite.sprite_height) / 2));
            label3.Text = "Value ( " + global_sprite.collision_mask_value + " px)";
            s_preview.Refresh();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            animationSequencerForm asf = new animationSequencerForm();
            asf.f_frameFromMin = 0;
            asf.f_frameFromMax = global_sprite.textures.Count - 1;
            asf.f_frameToMin = 0;
            asf.f_frameToMax = global_sprite.textures.Count - 1;
            asf.ShowDialog();
            if (asf.DialogResult == DialogResult.OK)
            {
                global_sprite.sprite_animationSquence.Add(asf.f_indexName, new Sprite.animationSequence(asf.f_fullName, asf.f_indexName, asf.f_frameFrom, asf.f_frameTo));
                updateForm();
            }
        }

        private void s_animationSequencePreview_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (s_animationSequencePreview.SelectedIndex == 0)
            {
                p_firstFrame = 0;
                p_lastFrame = (global_sprite.textures == null ? 0 : global_sprite.textures.Count - 1);

                p_currentFrame = p_firstFrame;
            }
            else
            {
                string tmp_index = s_animationSequencePreview.SelectedItem.ToString().Split(']')[0].Split('[')[1];
                p_firstFrame = global_sprite.sprite_animationSquence[tmp_index].frameFrom;
                p_lastFrame = global_sprite.sprite_animationSquence[tmp_index].frameTo;
                p_currentFrame = p_firstFrame;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                animationSequencerForm asf = new animationSequencerForm();
                asf.f_frameFromMin = 0;
                asf.f_frameFromMax = global_sprite.textures.Count - 1;
                asf.f_frameToMin = 0;
                asf.f_frameToMax = global_sprite.textures.Count - 1;

                string tmp_index = listBox1.SelectedItem.ToString().Split(']')[0].Split('[')[1];
                asf.f_fullName = global_sprite.sprite_animationSquence[tmp_index].name;
                asf.f_indexName = global_sprite.sprite_animationSquence[tmp_index].index;
                asf.f_frameFrom = global_sprite.sprite_animationSquence[tmp_index].frameFrom;
                asf.f_frameTo = global_sprite.sprite_animationSquence[tmp_index].frameTo;

                asf.ShowDialog();
                if (asf.DialogResult == DialogResult.OK)
                {
                    global_sprite.sprite_animationSquence[asf.f_indexName].frameFrom = asf.f_frameFrom;
                    global_sprite.sprite_animationSquence[asf.f_indexName].frameTo = asf.f_frameTo;
                    global_sprite.sprite_animationSquence[asf.f_indexName].name = asf.f_fullName;
                    updateForm();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (MessageBox.Show("Do You want to delete selected item?", "Delete [" + listBox1.SelectedItem.ToString().Split(']')[0].Split('[')[1] + "]", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    global_sprite.sprite_animationSquence.Remove(listBox1.SelectedItem.ToString().Split(']')[0].Split('[')[1]);
                    updateForm();
                }
            }
        }
    }
}
