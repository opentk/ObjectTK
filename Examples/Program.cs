using System;
using System.Windows.Forms;
using log4net.Config;

namespace Examples
{
    static class Program
    {
        [STAThread]
        public static void Main()
        {
            // initialize log4net via app.config
            XmlConfigurator.Configure();
            // show example browser
            using (var browser = new ExampleBrowser())
            {
                Application.Run(browser);
            }
        }
    }
}
