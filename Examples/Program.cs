using System;
using Examples.RenderVboWithShader;
using log4net.Config;
using OpenTK.Graphics;

namespace Examples
{
    static class Program
    {
        [STAThread]
        public static void Main()
        {
            // initialize log4net via app.config
            XmlConfigurator.Configure();
            // start the game loop
            using (var game = new RenderVboWithShaderWindow(800, 600, GraphicsMode.Default, "DerpGL example"))
            {
                game.Run();
            }
        }
    }
}
