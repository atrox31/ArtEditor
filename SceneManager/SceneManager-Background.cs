using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static ArtCore_Editor.GameProject;

namespace ArtCore_Editor
{
    public partial class SceneManager
    {
        // 
        private void button4_Click(object sender, EventArgs e)
        {
            PicFromList picFromList = new PicFromList(GameProject.GetInstance().Textures.Keys.ToList());
            if (picFromList.ShowDialog() == DialogResult.OK)
            {
                bc_selected_preview.BackgroundImage = BcPreviewList.Images[picFromList.SelectedIndex];
                cScene.BackGroundTexture_name = picFromList.Selected;
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (cScene.BackGroundTexture_name == null) return;
            r_bc_texture.Select();

            BcTexture?.Dispose();
            GC.Collect();

            cScene.BackGroundType = Scene.BackGroundTypeEnum.DrawTexture;

            cScene.BackGroundTexture = GameProject.GetInstance().Textures[cScene.BackGroundTexture_name].ProjectPath + GameProject.GetInstance().Textures[cScene.BackGroundTexture_name].FileName;
            BcTexture = Image.FromFile(GameProject.ProjectPath + "\\" + cScene.BackGroundTexture);

            if (rb_td_normal.Checked) cScene.BackGroundWrapMode = WrapMode.Tile;
            if (rb_td_w.Checked) cScene.BackGroundWrapMode = WrapMode.TileFlipX;
            if (rb_td_h.Checked) cScene.BackGroundWrapMode = WrapMode.TileFlipY;
            if (rb_td_w_h.Checked) cScene.BackGroundWrapMode = WrapMode.TileFlipXY;

            MakeChange();
            RedrawScene();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                bc_color_pick_value.Text = Functions.ColorToHex(colorDialog.Color);
                bc_color_box.BackColor = colorDialog.Color;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            r_bc_solidcolor.Select();
            if (!bc_color_pick_value.Text.StartsWith('#'))
            {
                bc_color_pick_value.Text.Insert(0, "#");
            }

            if (!Functions.ErrorCheck(bc_color_pick_value.Text.Length > 0, "Hex color value is empty"))
            {
                if (!Functions.ErrorCheck(Regex.IsMatch(bc_color_pick_value.Text, @"[#][0-9A-Fa-f]{6}\b"), "Hex color value is inwalid"))
                {
                    cScene.BackGroundColor = Functions.HexToColor(bc_color_pick_value.Text);
                    cScene.BackGroundType = Scene.BackGroundTypeEnum.DrawColor;
                    RedrawScene();
                    MakeChange();

                }
            }

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            MakeChange();
        }

        private void bc_color_pick_value_TextChanged(object sender, EventArgs e)
        {
            if (bc_color_pick_value.Text.Length == 6)
            {
                if (!bc_color_pick_value.Text.StartsWith('#'))
                {
                    bc_color_pick_value.Text.Insert(0, "#");
                }
            }
            if (bc_color_pick_value.Text.Length == 7)
            {
                Color tmp = Functions.HexToColor(bc_color_pick_value.Text);
                if (!tmp.IsEmpty)
                {
                    bc_color_box.BackColor = tmp;
                    MakeChange();
                }
            }
        }
        private void r_bc_texture_CheckedChanged(object sender, EventArgs e)
        {
            gb_bc_color_pic.Enabled = false;
            gb_bc_tex.Enabled = true;
        }

        private void r_bc_solidcolor_CheckedChanged(object sender, EventArgs e)
        {
            gb_bc_tex.Enabled = false;
            gb_bc_color_pic.Enabled = true;
        }



    }
}
