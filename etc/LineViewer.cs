using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class LineViewer : Form
    {
        public LineViewer(string message)
        {
            InitializeComponent();
            richTextBox1.Text = message;
        }
    }
}
