using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static ArtCore_Editor.GameProject;
using ListViewItem = System.Windows.Forms.ListViewItem;

namespace ArtCore_Editor
{
    public partial class SceneManager
    {
        private void Content_MouseMove(object sender, MouseEventArgs e)
        {
            if(selected_sceneInstance != null)
            {
                if(e.Button == MouseButtons.Left)
                {
                    Point point = new Point(e.X, e.Y);
                    if (SnapGrid)
                    {
                        point.X = (int)(Math.Round((decimal)point.X / Grid.X) * Grid.X);
                        point.Y = (int)(Math.Round((decimal)point.Y / Grid.Y) * Grid.Y);
                    }
                    selected_sceneInstance.x = point.X;
                    selected_sceneInstance.y = point.Y;
                    RedrawScene();
                }
            }
        }
        private void Content_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            selected_sceneInstance = null;
            foreach (var item in cScene.SceneInstances)
            {
                if (Functions.GetDistance(point, new Point(item.x, item.y)) < item.editorMask)
                {
                    selected_sceneInstance = item;
                    //RedrawScene();
                    //return;
                }
            }
            UpdateProperties(selected_sceneInstance);
            RedrawScene();

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
