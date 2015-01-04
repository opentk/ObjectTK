using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ObjectTK;
using OpenTK;

namespace Examples
{
    public partial class ExampleBrowser
        : Form
    {
        private Dictionary<string, Type> _examples;

        public ExampleBrowser()
        {
            InitializeComponent();
        }

        private void ExampleBrowser_Load(object sender, EventArgs e)
        {
            // find example projects
            var baseType = typeof (ExampleWindow);
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(_ => _ != baseType && baseType.IsAssignableFrom(_));
            // add tree nodes for example projects
            _examples = new Dictionary<string, Type>();
            foreach (var type in types)
            {
                // find or create node for namespace
                var existingNodes = treeViewExamples.Nodes.Find(type.Namespace, false);
                var node = existingNodes.Length > 0
                    ? existingNodes[0]
                    : treeViewExamples.Nodes.Add(type.Namespace, type.Namespace);
                // add node for this example and get the caption from the attribute
                var captionAttribute = type.GetCustomAttributes<ExampleProjectAttribute>(false).FirstOrDefault();
                node.Nodes.Add(type.Name, captionAttribute == null ? type.Name : captionAttribute.Caption);
                // remember example type
                _examples.Add(type.Name, type);
            }
            // show all examples
            treeViewExamples.ExpandAll();
        }

        private void ExampleBrowser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void TreeViewExamples_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) RunExample(treeViewExamples.SelectedNode);
        }

        private void TreeViewExamples_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            RunExample(e.Node);
        }

        private void RunExample(TreeNode node)
        {
            Type type;
            if (!_examples.TryGetValue(node.Name, out type)) return;
            // hide browser
            Hide();
            // run the example
            using (var exampleWindow = (GameWindow)Activator.CreateInstance(type))
            {
                exampleWindow.Title = node.Text;
                exampleWindow.Run();
            }
            // show the browser again
            Show();
        }
    }
}
