using System;
using System.Windows;
using System.Windows.Forms;
using DragDropEffects = System.Windows.Forms.DragDropEffects;
using DragEventArgs = System.Windows.Forms.DragEventArgs;
using Point = System.Drawing.Point;

namespace ArtCore_Editor
{
    public partial class SceneManager
    {
        bool mouseDrag = false;
        ArtCore_Editor.GameProject.Scene.SceneInstance GetInstance(Point point, bool first)
        {
            ArtCore_Editor.GameProject.Scene.SceneInstance selected = null;
            foreach (var item in cScene.SceneInstances)
            {
                if (Functions.GetDistance(point, new (item.x, item.y)) < item.editorMask)
                {
                    selected = item;
                    if (first)
                    {
                        return selected;
                    }
                }
            }
            return selected;
        }
        private void Content_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDrag)
            {
                if (selected_sceneInstance != null)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        Point point = new Point(e.X, e.Y);
                        if (selected_sceneInstance != GetInstance(point, false))
                        {
                            selected_sceneInstance = null;
                            mouseDrag = false;
                            return;
                        }
                        if (SnapGrid)
                        {
                            point.X = (int)(Math.Round((decimal)point.X / Grid.X) * Grid.X);
                            point.Y = (int)(Math.Round((decimal)point.Y / Grid.Y) * Grid.Y);
                        }

                        point.X = (point.X < 0 ? 0 : (point.X > cScene.Width ? cScene.Width : point.X));
                        point.Y = (point.Y < 0 ? 0 : (point.Y > cScene.Height ? cScene.Height : point.Y));

                        selected_sceneInstance.x = point.X;
                        selected_sceneInstance.y = point.Y;
                        RedrawScene();
                    }
                }
            }
        }


        private void Content_MouseLeave(object sender, EventArgs e)
        {

        }

        private void Content_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void Content_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            if(e.Button== MouseButtons.Left)
            {
                if(selected_sceneInstance != GetInstance(point, false))
                {
                    selected_sceneInstance = null;
                }
                selected_sceneInstance = GetInstance(point, false);
                if(selected_sceneInstance != null)
                {
                    mouseDrag = true;
                }
                else
                {
                    mouseDrag = false;
                }
                UpdateProperties(selected_sceneInstance);
                RedrawScene();
            }
            if(e.Button== MouseButtons.Right)
            {
                if (selected_sceneInstance != null)
                {
                    ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                    contextMenuStrip.Items.Add("Delete");
                    contextMenuStrip.ItemClicked += (object sender, ToolStripItemClickedEventArgs e) =>
                    {
                        if (System.Windows.Forms.MessageBox.Show("Delete instance '" + selected_sceneInstance.instance.Name + "'?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            cScene.SceneInstances.Remove(selected_sceneInstance);
                            UpdateProperties(selected_sceneInstance);
                            RedrawScene();
                        }
                    };
                    contextMenuStrip.Show(Cursor.Position);
                }
            }
        }
        private void instanceListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            instanceListView.DoDragDrop(e.Item, DragDropEffects.Move);

            //shadow_sceneInstance = GameProject.GetInstance().Instances[((ListViewItem)e.Item).Text];
        }

        private void Content_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Content_DragDrop(object sender, DragEventArgs e)
        {
            var item = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            GameProject.Instance t_ins = GameProject.GetInstance().Instances[item.Text];

            Point point = Content.PointToClient(new Point(e.X, e.Y));

            // grid
            if (SnapGrid)
            {
                point.X = (int)(Math.Round((decimal)point.X / Grid.X) * Grid.X);
                point.Y = (int)(Math.Round((decimal)point.Y / Grid.Y) * Grid.Y);
            }

            cScene.SceneInstances.Add(new GameProject.Scene.SceneInstance(point.X, point.Y, t_ins));
            saved = false;
            RedrawScene();
        }

    }
}
