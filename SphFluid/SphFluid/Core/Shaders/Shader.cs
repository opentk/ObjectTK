using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SphFluid.Properties;

namespace SphFluid.Core.Shaders
{
    public class Shader
        : ContextResource
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Shader));

        private interface IMapping
        {
            Type MappedType { get; }
            object Create(int program, MemberInfo info);
        }

        private class Mapping<T>
            : IMapping
        {
            private readonly Func<int, MemberInfo, T> _creator;
            public Type MappedType { get { return typeof(T); } }
            public Mapping(Func<int, MemberInfo, T> creator) { _creator = creator; }
            public object Create(int program, MemberInfo info) { return _creator(program, info); }
        }

        //TODO: refactor to use GL.ProgramUniform* (check out EXT_direct_state_access)
        private static readonly List<IMapping> TypeMapping = new List<IMapping>
        {
            new Mapping<VertexAttrib>(VertexAttribHelper),
            new Mapping<TextureUniform>((p,i) => new TextureUniform(p, i.Name)),
            new Mapping<ImageUniform>((p,i) => new ImageUniform(p, i.Name)),
            new Mapping<Uniform<bool>>((p,i) => new Uniform<bool>(p, i.Name, (l,b) => GL.Uniform1(l, b?1:0))),
            new Mapping<Uniform<int>>((p,i) => new Uniform<int>(p, i.Name, GL.Uniform1)),
            new Mapping<Uniform<float>>((p,i) => new Uniform<float>(p, i.Name, GL.Uniform1)),
            new Mapping<Uniform<Vector2>>((p,i) => new Uniform<Vector2>(p, i.Name, GL.Uniform2)),
            new Mapping<Uniform<Vector3>>((p,i) => new Uniform<Vector3>(p, i.Name, GL.Uniform3)),
            new Mapping<Uniform<Vector4>>((p,i) => new Uniform<Vector4>(p, i.Name, GL.Uniform4)),
            new Mapping<Uniform<Matrix4>>((p,i) => new Uniform<Matrix4>(p, i.Name, (_, matrix) => GL.UniformMatrix4(_, false, ref matrix))),
            new Mapping<FragData>((p,i) => new FragData(p, i.Name))
        };

        private static VertexAttrib VertexAttribHelper(int program, MemberInfo info)
        {
            var attrib = info.GetCustomAttributes<VertexAttribAttribute>(false).FirstOrDefault();
            if (attrib == null) throw new ApplicationException("VertexAttribAttribute missing!");
            return new VertexAttrib(program, info.Name, attrib);
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
            var shaderSources = ShaderSourceAttribute.GetShaderSources(this);
            if (shaderSources.Count == 0) throw new ApplicationException("ShaderSourceAttribute(s) missing!");
            CreateProgram(shaderSources);
        }

        private void CreateProgram(Dictionary<ShaderType, string> shaders)
        {
            Logger.InfoFormat("Creating shader program: {0}", GetType().Name);
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
            Logger.DebugFormat("Linking shader program: {0}", GetType().Name);
            GL.LinkProgram(Program);
            // assert that no link errors occured
            int linkStatus;
            GL.GetProgram(Program, GetProgramParameterName.LinkStatus, out linkStatus);
            var info = GL.GetProgramInfoLog(Program);
            Logger.DebugFormat("Link status: {0}", linkStatus);
            if (!string.IsNullOrEmpty(info)) Logger.InfoFormat("Link log:\n{0}", info);
            Utility.Assert(linkStatus, 1, string.Format("Error linking program: {0}", GetType().Name));
            // initialize shader properties
            Initialize();
        }

        private void AttachShader(ShaderType type, string name)
        {
            Logger.DebugFormat("Compiling {0}: {1}", type, name);
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
            Logger.DebugFormat("Compiling status: {0}", compileStatus);
            var info = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(info)) Logger.InfoFormat("Compile log for {0}:\n{1}", name, info);
            Utility.Assert(compileStatus, 1, string.Format("Error compiling shader: {0}", filename));
            // attach shader to program
            GL.AttachShader(Program, shader);
            // remember shader to be able to properly release it
            _shaders.Add(shader);
        }

        /// <summary>
        /// Initializes all properties of the current shader which are of type TransformOut.
        /// Is called before linking the shader to initialize transform feedback outputs and store their index to the related property.
        /// </summary>
        /// <returns></returns>
        private List<string> InitializeTransformOut()
        {
            var outs = new List<string>();
            var index = 0;
            foreach (var property in GetType().GetProperties().Where(_ => _.PropertyType == typeof(TransformOut)))
            {
                property.SetValue(this, new TransformOut(index++), null);
                outs.Add(property.Name);
            }
            return outs;
        }

        /// <summary>
        /// Initializes all properties of the current shader instance which are of a type contained in the TypeMapping.
        /// Is called after linking the shader to initialize vertex attributes and uniforms.
        /// </summary>
        private void Initialize()
        {
            foreach (var property in GetType().GetProperties())
            {
                var mapping = TypeMapping.FirstOrDefault(_ => _.MappedType == property.PropertyType);
                if (mapping == null) continue;
                Logger.DebugFormat("Creating property mapping: {0}", property.Name);
                property.SetValue(this, mapping.Create(Program, property), null);
            }
        }

        public void Use()
        {
            GL.UseProgram(Program);
        }

        protected override void OnRelease()
        {
            GL.DeleteProgram(Program);
            foreach (var shader in _shaders)
            {
                GL.DeleteShader(shader);
            }
        }
    }
}