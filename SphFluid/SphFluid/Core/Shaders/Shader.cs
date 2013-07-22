using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SphFluid.Properties;

namespace SphFluid.Core.Shaders
{
    public class Shader
        : IReleasable
    {
        private static readonly Dictionary<Type, Func<int, MemberInfo, object>> TypeMapping = new Dictionary<Type, Func<int, MemberInfo, object>>
        {
            { typeof(VertexAttrib), VertexAttribHelper },
            { typeof(TextureUniform), (program, info) => new TextureUniform(program, info.Name) },
            { typeof(Uniform<int>), (program, info) => new Uniform<int>(program, info.Name, GL.Uniform1) },
            { typeof(Uniform<float>), (program, info) => new Uniform<float>(program, info.Name, GL.Uniform1) },
            { typeof(Uniform<Vector3>), (program, info) => new Uniform<Vector3>(program, info.Name, GL.Uniform3) },
            { typeof(Uniform<Matrix4>), (program, info) => new Uniform<Matrix4>(program, info.Name, (_, matrix) => GL.UniformMatrix4(_, false, ref matrix)) }
        };

        private static object VertexAttribHelper(int program, MemberInfo info)
        {
            var attrib = info.GetCustomAttributes<VertexAttribAttribute>(false).FirstOrDefault();
            if (attrib == null) throw new ApplicationException("VertexAttribAttribute missing!");
            return new VertexAttrib(program, info.Name, attrib.Components, attrib.Type);
        }

        /// <summary>
        /// Shader program handle
        /// </summary>
        public int Program { get; private set; }

        /// <summary>
        /// Stores all shader handles to properly release them later
        /// </summary>
        private readonly List<int> _shaders;

        protected Shader()
        {
            _shaders = new List<int>();
            var shaderSources = new Dictionary<ShaderType, string>();
            var attributes = GetType().GetCustomAttributes<ShaderSourceAttribute>(true).ToList();
            attributes.Where(_ => _.GetType() == typeof(VertexShaderSourceAttribute)).ToList().ForEach(_ => shaderSources.Add(ShaderType.VertexShader, _.File));
            attributes.Where(_ => _.GetType() == typeof(GeometryShaderSourceAttribute)).ToList().ForEach(_ => shaderSources.Add(ShaderType.GeometryShader, _.File));
            attributes.Where(_ => _.GetType() == typeof(FragmentShaderSourceAttribute)).ToList().ForEach(_ => shaderSources.Add(ShaderType.FragmentShader, _.File));
            if (shaderSources.Count == 0) throw new ApplicationException("ShaderSourceAttribute(s) missing!");
            CreateProgram(shaderSources);
        }

        private void CreateProgram(Dictionary<ShaderType, string> shaders)
        {
            Trace.TraceInformation("Creating shader program:");
            // create program
            Program = GL.CreateProgram();
            // load and attach all specified shaders
            foreach (var pair in shaders)
            {
                AttachShader(pair.Key, pair.Value);
            }
            // bind transform feedback varyings if any
            var outs = InitializeTransformOut();
            if (outs.Count > 0)
            {
                GL.TransformFeedbackVaryings(Program, outs.Count, outs.ToArray(), TransformFeedbackMode.SeparateAttribs);
            }
            // link program
            GL.LinkProgram(Program);
            // assert that no link errors occured
            int linkStatus;
            GL.GetProgram(Program, ProgramParameter.LinkStatus, out linkStatus);
            var info = GL.GetProgramInfoLog(Program);
            Trace.TraceInformation("Link status: {0}", linkStatus);
            if (!string.IsNullOrEmpty(info)) Trace.TraceInformation("Info:\n{0}", info);
            Utility.Assert(() => linkStatus, 1, string.Format("Error linking program: {0}", info));
            // initialize shader properties
            Initialize();
        }

        private void AttachShader(ShaderType type, string name)
        {
            Trace.TraceInformation(name);
            // create shader
            var shader = GL.CreateShader(type);
            // load shaders source
            var filename = Path.Combine(Settings.Default.ShaderDir, name + ".glsl");
            using (var reader = new StreamReader(filename))
            {
                GL.ShaderSource(shader, reader.ReadToEnd());
            }
            // compile shader
            GL.CompileShader(shader);
            // assert that no compile error occured
            int compileStatus;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out compileStatus);
            var info = GL.GetShaderInfoLog(shader);
            Trace.TraceInformation("Compile status: {0}", compileStatus);
            if (!string.IsNullOrEmpty(info)) Trace.TraceInformation("Info:\n{0}", info);
            Utility.Assert(() => compileStatus, 1, string.Format("Error compiling shader: {0}\n{1}", filename, info));
            // attach shader to program
            GL.AttachShader(Program, shader);
            // remember shader to be able to properly release it
            _shaders.Add(shader);
        }

        private List<string> InitializeTransformOut()
        {
            var outs = new List<string>();
            var counter = 0;
            foreach (var property in GetType().GetProperties().Where(_ => _.PropertyType == typeof(TransformOut)))
            {
                property.SetValue(this, new TransformOut(counter++), null);
                outs.Add(property.Name);
            }
            return outs;
        }

        private void Initialize()
        {
            foreach (var property in GetType().GetProperties().Where(_ => TypeMapping.ContainsKey(_.PropertyType)))
            {
                property.SetValue(this, TypeMapping[property.PropertyType].Invoke(Program, property), null);
            }
        }

        public void Use()
        {
            GL.UseProgram(Program);
        }

        public void Release()
        {
            GL.DeleteProgram(Program);
            foreach (var shader in _shaders)
            {
                GL.DeleteShader(shader);
            }
        }
    }
}