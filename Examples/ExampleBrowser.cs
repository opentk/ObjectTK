using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
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
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(_ => _.IsDefined(typeof(ExampleProjectAttribute)));
            // add tree nodes for example projects
            _examples = new Dictionary<string, Type>();
            foreach (var type in types)
            {
                var node = treeViewExamples.Nodes.Add(type.Namespace, type.Namespace);
                node.Nodes.Add(type.Name, type.Name);
                _examples.Add(type.Name, type);
            }
            treeViewExamples.ExpandAll();
        }

        private void RunExample(Type exampleType)
        {
            // hide browser
            Hide();
            // run the example
            using (var exampleWindow = (GameWindow)Activator.CreateInstance(exampleType))
            {
                exampleWindow.Run();
            }
            // show the browser again
            Show();
        }

        private void TreeViewExamples_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Type type;
            if (!_examples.TryGetValue(e.Node.Name, out type)) return;
            RunExample(type);
        }
    }
}
