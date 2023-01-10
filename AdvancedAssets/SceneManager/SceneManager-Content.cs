using System;
using System.Windows.Forms;
using ArtCore_Editor.AdvancedAssets.Instance_Manager;
using ArtCore_Editor.Main;
using DragDropEffects = System.Windows.Forms.DragDropEffects;
using DragEventArgs = System.Windows.Forms.DragEventArgs;
using Point = System.Drawing.Point;

namespace ArtCore_Editor.AdvancedAssets.SceneManager
{    /// <summary>
     /// scene content, entire drawing area
     /// </summary>
    public partial class SceneManager
    {
        private bool _mouseDrag;
        SceneInstance GetInstance(Point point, bool first)
        {
            SceneInstance selected = null;
            foreach (SceneInstance item in _cScene.SceneInstances)
            {
                if (Functions.Functions.GetDistance(point, new Point(item.X, item.Y)) < item.EditorMask)
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
            if (!_mouseDrag) return;
            if (_selectedSceneInstance == null) return;
            if (e.Button != MouseButtons.Left) return;
            Point point = new Point(e.X, e.Y);

            if (_selectedSceneInstance != GetInstance(point, false))
            {
                _selectedSceneInstance = null;
                _mouseDrag = false;
                return;
            }

            if (_snapGrid)
            {
                point.X = (int)(Math.Round((decimal)point.X / _grid.X) * _grid.X);
                point.Y = (int)(Math.Round((decimal)point.Y / _grid.Y) * _grid.Y);
            }

            point.X = (point.X < 0 ? 0 : (point.X > _cScene.Width ? _cScene.Width : point.X));
            point.Y = (point.Y < 0 ? 0 : (point.Y > _cScene.Height ? _cScene.Height : point.Y));

            _selectedSceneInstance.X = point.X;
            _selectedSceneInstance.Y = point.Y;
            RedrawScene();
        }

        private void Content_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            if(e.Button== MouseButtons.Left)
            {
                if(_selectedSceneInstance != GetInstance(point, false))
                {
                    _selectedSceneInstance = null;
                }
                _selectedSceneInstance = GetInstance(point, false);
                _mouseDrag = _selectedSceneInstance != null;
                UpdateProperties(_selectedSceneInstance);
                RedrawScene();
            }

            if (e.Button != MouseButtons.Right) return;
            if (_selectedSceneInstance == null) return;

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Delete");
            contextMenuStrip.ItemClicked += (object sender, ToolStripItemClickedEventArgs e) =>
            {
                if (System.Windows.Forms.MessageBox.Show("Delete instance '" + _selectedSceneInstance.Instance.Name + "'?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _cScene.SceneInstances.Remove(_selectedSceneInstance);
                    UpdateProperties(_selectedSceneInstance);
                    RedrawScene();
                }
            };
            contextMenuStrip.Show(Cursor.Position);
        }
        private void instanceListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item != null) instanceListView.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void Content_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(typeof(ListViewItem)))
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
            if (e.Data != null)
            {
                ListViewItem selectedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                if (selectedItem == null) return;

                Instance draggedObject = GameProject.GetInstance().Instances[selectedItem.Text];
                if (draggedObject == null) return;

                Point point = Content.PointToClient(new Point(e.X, e.Y));
                if (_snapGrid)
                {
                    point.X = (int)(Math.Round((decimal)point.X / _grid.X) * _grid.X);
                    point.Y = (int)(Math.Round((decimal)point.Y / _grid.Y) * _grid.Y);
                }

                _cScene.SceneInstances.Add(new SceneInstance(point.X, point.Y, draggedObject));
            }

            _saved = false;
            RedrawScene();
        }

    }
}
