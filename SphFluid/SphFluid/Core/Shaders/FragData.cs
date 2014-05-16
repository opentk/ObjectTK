using log4net;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Shaders
{
    public class FragData
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FragData));

        public readonly string Name;
        public readonly int Location;

        public FragData(int program, string name)
        {
            Name = name;
            //TODO: find out what GL.GetFragDataIndex(); does
            Location = GL.GetFragDataLocation(program, name);
            if (Location == -1) Logger.WarnFormat("Output variable not found or not active: {0}", name);
        }
    }
}