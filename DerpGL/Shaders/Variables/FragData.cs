using log4net;
using OpenTK.Graphics.OpenGL4;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a fragment shader output.<br/>
    /// TODO: implement methods to bind output to a specific attachment
    /// see glBindFragDataLocation, glDrawBuffers and http://stackoverflow.com/questions/1733838/fragment-shaders-output-variables
    /// </summary>
    public sealed class FragData
        : ShaderVariable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FragData));

        /// <summary>
        /// The location of the output.
        /// </summary>
        public readonly int Location;

        internal FragData(int program, string name)
            : base(program, name)
        {
            //TODO: find out what GL.GetFragDataIndex(); does
            Location = GL.GetFragDataLocation(program, name);
            if (Location == -1) Logger.WarnFormat("Output variable not found or not active: {0}", name);
        }
    }
}