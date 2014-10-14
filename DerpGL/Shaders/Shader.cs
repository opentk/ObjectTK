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
using System.Collections.Generic;
using System.IO;
using System.Text;
using DerpGL.Exceptions;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Represents a shader object.
    /// </summary>
    public sealed class Shader
        : GLResource
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Shader));

        /// <summary>
        /// Specifies the type of this shader.
        /// </summary>
        public readonly ShaderType Type;

        /// <summary>
        /// Specifies the path to the last compiled source file.
        /// </summary>
        public string Path { get; private set; }

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
        /// <param name="path">Specifies the path to the glsl source file.</param>
        public void CompileSource(string path)
        {
            Path = path;
            Logger.DebugFormat("Compiling {0}: {1}", Type, path);
            GL.ShaderSource(Handle, RetrieveShaderSource(path));
            GL.CompileShader(Handle);
            CheckCompileStatus();
        }

        /// <summary>
        /// Load shader source file(s).<br/>
        /// Supports multiple source files via "#include xx" directives and corrects the line numbering by using the glsl standard #line directive.
        /// </summary>
        /// <param name="path">Specifies the path to the glsl source file to load.</param>
        /// <param name="includedNames">Holds the filename of all files already loaded to prevent multiple inclusions.</param>
        /// <returns>The fully assembled shader source.</returns>
        private static string RetrieveShaderSource(string path, List<string> includedNames = null)
        {
            if (includedNames == null) includedNames = new List<string>();
            // check for multiple includes of the same file
            if (includedNames.Contains(path))
            {
                Logger.WarnFormat("File already included: {0} (included files: {1})", path, string.Join(", ", includedNames));
                return "";
            }
            includedNames.Add(path);
            // load shaders source
            var source = new StringBuilder();
            StreamReader reader;
            try
            {
                reader = new StreamReader(string.Format("{0}.glsl", System.IO.Path.Combine(Program.BasePath, path)));
            }
            catch (FileNotFoundException ex)
            {
                Logger.Error("Shader source file not found.", ex);
                throw;
            }
            // parse this file
            using (reader)
            {
                var fileNumber = includedNames.Count - 1;
                var lineNumber = 1;
                while (!reader.EndOfStream)
                {
                    // read the file line by line
                    var code = reader.ReadLine();
                    if (code == null) break;
                    // check if it is an include statement
                    const string includeKeyword = "#include";
                    if (!code.StartsWith(includeKeyword))
                    {
                        source.AppendLine(code);
                    }
                    else
                    {
                        // parse the included filename
                        var includedFile = code.Remove(0, includeKeyword.Length).Trim();
                        // insert #line statement to correct line numbers
                        source.AppendLine(string.Format("#line 1 {0}", includedNames.Count));
                        // replace current line with the source of that file
                        includedFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path) ?? "", includedFile);
                        source.Append(RetrieveShaderSource(includedFile, includedNames));
                        // the #line statement defines the line number of the *next* line
                        source.AppendLine(string.Format("#line {0} {1}", lineNumber + 1, fileNumber));
                    }
                    lineNumber++;
                }
            }
            return source.ToString();
        }

        /// <summary>
        /// Assert that no compile error occured.
        /// </summary>
        private void CheckCompileStatus()
        {
            // check compile status
            int compileStatus;
            GL.GetShader(Handle, ShaderParameter.CompileStatus, out compileStatus);
            Logger.DebugFormat("Compiling status: {0}", compileStatus);
            // check shader info log
            var info = GL.GetShaderInfoLog(Handle);
            if (!string.IsNullOrEmpty(info)) Logger.InfoFormat("Compile log for {0}:\n{1}", Path, info);
            // log message and throw exception on compile error
            if (compileStatus == 1) return;
            var msg = string.Format("Error compiling shader: {0}", Path);
            Logger.Error(msg);
            throw new ShaderCompileException(msg, info);
        }
    }
}
