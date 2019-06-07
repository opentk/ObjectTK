//
// ShaderCompiler.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ObjectTK.Exceptions;
using ObjectTK.Shaders;
using ObjectTK.Shaders.Sources;
using OpenTK;

namespace ObjectTK.Compiler
{
    /// <summary>
    /// Compiles all shader programs contained in assemblies given per command line arguments and outputs errors in a MSBuild and Visual Studio friendly way.<br/>
    /// </summary>
    /// <remarks>
    /// The easiest way to use this feature is to add a reference to ObjectTKC so that the executable gets copied to the output folder and add the following Post-build event:<br/>
    /// "$(TargetDir)ObjectTKC.exe" "$(TargetPath)"
    /// </remarks>
    public class ShaderCompiler
    {
        // GLSL example error messages:
        // ERROR: Data/Shaders/ExampleShader.glsl:21: error(#132) Syntax error: "d" parse error
        // ERROR: error(#273) 1 compilation errors.  No code generated

        /// <summary>
        /// Matches essential parts of GLSL error messages.
        /// </summary>
        private static readonly Regex ErrorRegex = new Regex(@"^ERROR: (.+):(\d+): error\(#(\d+)\) (.*)$", RegexOptions.Multiline);

        /// <summary>
        /// Matches essential parts of GLSL warning messages.<br/>
        /// TODO: currently untested
        /// </summary>
        private static readonly Regex WarningRegex = new Regex(@"^WARNING: (.+):(\d+): (.*)$", RegexOptions.Multiline);

        /// <summary>
        /// Reformats OpenGL information log to a MSBuild and Visual Studio friendly format. Makes errors and warnings apprear correctly in the "Error List" of Visual Studio.
        /// </summary>
        /// <param name="infoLog">OpenGL information log.</param>
        private static string FormatInfoLog(string infoLog)
        {
            var log = ErrorRegex.Replace(infoLog, "$1($2): error $3: $4");
            return WarningRegex.Replace(log, "$1($2): warning 0: $3");
        }

        public static void Main(string[] args)
        {
            // create a hidden GameWindow to initialize an OpenGL context
            using (new GameWindow())
            {
                // iterate over given arguments
                foreach (var path in args)
                {
                    // check if file exists
                    if (!File.Exists(path))
                    {
                        Console.Out.WriteLine("{0}: error 0: ShaderCompiler: file not found", path);
                        continue;
                    }
                    Console.Out.WriteLine("Compiling shaders of: {0}", path);
                    // load assembly
                    var assembly = Assembly.LoadFrom(path);
                    // set working directory
                    Directory.SetCurrentDirectory(Path.GetDirectoryName(assembly.Location));
                    // iterate over all non-abstract shader programs
                    foreach (var type in assembly.GetTypes().Where(_ => !_.IsAbstract && typeof(Program).IsAssignableFrom(_)))
                    {
                        // check if the program has any shader sources tagged to it
                        if (ShaderSourceAttribute.GetShaderSources(type).Count == 0) continue;
                        Console.Out.WriteLine("Compiling: {0}", type.FullName);
                        // get generic program factory method
                        var method = typeof(ProgramFactory).GetMethod("Create");
                        var generic = method.MakeGenericMethod(type);
                        try
                        {
                            // invoke program factory
                            var program = (Program)generic.Invoke(null, null);
                            program.Dispose();
                        }
                        catch (TargetInvocationException ex)
                        {
                            Console.Out.WriteLine(ex.InnerException.Message);
                            // reformat OpenGL information log if existing
                            var exception = ex.InnerException as ProgramException;
                            if (exception != null) Console.Out.WriteLine(FormatInfoLog(exception.InfoLog));
                        }
                    }
                }
            }
        }
    }
}
