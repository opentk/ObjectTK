using System;
using System.Windows.Forms;
using log4net.Config;

namespace Examples
{
    public static class ExampleBrowserEntry
    {
        [STAThread]
        public static void Main()
        {
            // initialize log4net via app.config if available
            if (ObjectTK.Logging.LogFactory.IsAvailable)
                ConfigureLogging();

            // show example browser
            using (var browser = new ExampleBrowser())
            {
                Application.Run(browser);
            }
        }

        public static void ConfigureLogging()
        {
            XmlConfigurator.Configure();
        }
    }
}
