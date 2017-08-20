using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace XmlViewer
{
    public partial class MainForm : Form
    {
        public MainForm(string filename = "")
        {
            InitializeComponent();
            Text = filename;
        }

        private static TreeNode Explore(XElement element)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            if (element.HasAttributes)
                nodes.AddRange(
                    element.Attributes()
                        .Select(
                            attribute =>
                                    new TreeNode("Attribute " + attribute.Name.LocalName + ": " + attribute.Value)));
            if (element.HasElements)
                nodes.AddRange(element.Elements().Select(Explore));
            return new TreeNode(element.Name.LocalName + (element.HasElements ? "" : ": " + element.Value),
                nodes.ToArray());
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            if (Text == "")
            {
                Text = "XML Viewer";
                return;
            }
            string filename = Text;
            var document = XDocument.Load(filename);
            Text = "Loading...";
            if (document.Root != null) treeView.Nodes.Add(await Task.Run(() => Explore(document.Root)));
            treeView.CollapseAll();
            Text = filename;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "XML Files (*.xml) | *.xml" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Text = dialog.FileName;
                MainForm_Load(sender, null);
            }
        }
    }
}
