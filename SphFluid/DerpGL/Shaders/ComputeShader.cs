using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    public class ComputeShader
        : Shader
    {
        /// <summary>
        /// The work group size of the compute shader.
        /// </summary>
        public Vector3i WorkGroupSize { get; protected set; }

        protected ComputeShader()
        {
            // query the work group size
            var sizes = new int[3];
            GL.GetProgram(Program, (GetProgramParameterName)All.ComputeWorkGroupSize, sizes);
            WorkGroupSize = new Vector3i(sizes[0], sizes[1], sizes[2]);
        }
    }
}