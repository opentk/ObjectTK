#region License
// DerpGL License
// Copyright (C) 2013-2014 J.C.Bernack
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion
using DerpGL.Exceptions;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Represents a shader object.
    /// </summary>
    public class Shader
        : GLResource
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Shader));

        /// <summary>
        /// Specifies the type of this shader.
        /// </summary>
        public readonly ShaderType Type;

        /// <summary>
        /// Initializes a new shader object of the given type.
        /// </summary>
        /// <param name="type"></param>
        public Shader(ShaderType type)
            : base(GL.CreateShader(type))
        {
            Type = type;
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteShader(Handle);
        }

        /// <summary>
        /// Loads the given source file and compiles the shader.
        /// </summary>
        public void CompileSource(string source)
        {
            GL.ShaderSource(Handle, source);
            GL.CompileShader(Handle);
            CheckCompileStatus();
        }

        /// <summary>
        /// Assert that no compile error occured.
        /// </summary>
        private void CheckCompileStatus()
        {
            // check compile status
            int compileStatus;
            GL.GetShader(Handle, ShaderParameter.CompileStatus, out compileStatus);
            Logger.DebugFormat("Compile status: {0}", compileStatus);
            // check shader info log
            var info = GL.GetShaderInfoLog(Handle);
            if (!string.IsNullOrEmpty(info)) Logger.InfoFormat("Compile log:\n{0}", info);
            // log message and throw exception on compile error
            if (compileStatus == 1) return;
            const string msg = "Error compiling shader.";
            Logger.Error(msg);
            throw new ShaderCompileException(msg, info);
        }
    }
}
