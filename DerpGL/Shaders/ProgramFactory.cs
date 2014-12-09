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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DerpGL.Exceptions;
using log4net;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Contains methods to automatically initialize program objects.
    /// </summary>
    public static class ProgramFactory
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ProgramFactory));

        /// <summary>
        /// The base path used when looking for shader files.<br/>
        /// Default is: Data/Shaders/
        /// </summary>
        public static string BasePath { get; set; }
        
        /// <summary>
        /// Specifies the default extension appended to effect names.<br/>
        /// Default is: glsl
        /// </summary>
        public static string Extension { get; set; }

        static ProgramFactory()
        {
            BasePath = "Data/Shaders/";
            Extension = "glsl";
        }

        /// <summary>
        /// Initializes a program object using the shader sources tagged to the type with <see cref="ShaderSourceAttribute"/>.
        /// </summary>
        /// <typeparam name="T">Specifies the program type to create.</typeparam>
        /// <returns>A compiled and linked program.</returns>
        public static T Create<T>()
            where T : Program
        {
            // retrieve shader types and filenames from attributes
            var shaders = ShaderSourceAttribute.GetShaderSources(typeof(T));
            if (shaders.Count == 0) throw new DerpGLException("ShaderSourceAttribute(s) missing!");
            // create program instance
            var program = (T)Activator.CreateInstance(typeof(T));
            // compile and attach all shaders
            foreach (var attribute in shaders)
            {
                // create a new shader of the appropriate type
                using (var shader = new Shader(attribute.Type))
                {
                    Logger.DebugFormat("Compiling {0}: {1}", attribute.Type, attribute.EffectKey);
                    // load the source from effect(s)
                    var included = new List<Effect.Section>();
                    var source = GetShaderSource(attribute.EffectKey, included);
                    // assign source filenames for proper information log output
                    shader.SourceFiles = included.Select(_ => _.Effect.Path).ToList();
                    // compile shader source
                    shader.CompileSource(source);
                    // attach shader to the program
                    program.Attach(shader);
                }
            }
            // link and return the program
            program.Link();
            return program;
        }

        /// <summary>
        /// Load shader source file(s).<br/>
        /// Supports multiple source files via "#include xx" directives and corrects the line numbering by using the glsl standard #line directive.
        /// </summary>
        /// <param name="effectKey">Specifies the effect key to load.</param>
        /// <param name="included">Holds the effectKeys of all shaders already loaded to prevent multiple inclusions.</param>
        /// <returns>The fully assembled shader source.</returns>
        private static string GetShaderSource(string effectKey, List<Effect.Section> included)
        {
            // retrieve effect section
            Effect.Section section;
            try
            {
                var directory = Path.GetDirectoryName(effectKey);
                var filename = Path.GetFileName(effectKey);
                var separator = filename.IndexOf('.');
                var effectPath = Path.ChangeExtension(Path.Combine(BasePath, directory, filename.Substring(0, separator)), Extension);
                var shaderKey = filename.Substring(separator + 1);
                section = Effect.GetSection(effectPath, shaderKey);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Invalid effect key: {0}", effectKey), ex);
            }
            if (section == null) throw new Exception(string.Format("Shader source not found: {0}", effectKey));
            // check for multiple includes of the same section
            if (included.Contains(section))
            {
                Logger.WarnFormat("Shader already included: {0}", effectKey);
                return "";
            }
            included.Add(section);
            // parse source for #include directives and insert proper #line annotations
            using (var reader = new StringReader(section.Source))
            {
                var source = new StringBuilder();
                var lineNumber = section.FirstLineNumber;
                var fileNumber = included.Count-1;
                var fixLine = true;
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null) break;
                    // check if it is an include statement
                    const string includeKeyword = "#include";
                    if (!line.StartsWith(includeKeyword))
                    {
                        // add correct line number offset to the corresponding section within the effect file
                        if (fixLine)
                        {
                            source.AppendLine(string.Format("#line {0} {1}", lineNumber, fileNumber));
                            fixLine = false;
                        }
                        source.AppendLine(line);
                    }
                    else
                    {
                        // parse the included filename
                        var includedEffectKey = line.Remove(0, includeKeyword.Length).Trim();
                        // make the include path relative to the current file
                        includedEffectKey = Path.Combine(Path.GetDirectoryName(effectKey) ?? "", includedEffectKey);
                        // replace current line with the source of the included section
                        source.Append(GetShaderSource(includedEffectKey, included));
                        // remember to fix the line numbering on the next line
                        fixLine = true;
                    }
                    lineNumber++;
                }
                return source.ToString();
            }
        }
    }
}