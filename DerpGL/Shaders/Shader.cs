using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using DerpGL.Shaders.Variables;
using DerpGL.Textures;
using log4net;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Provides a base class for shader programs.<br/>
    /// Automatically maps properties to shader variables by name.<br/>
    /// For shader variable types see the DerpGL.Shaders.Variables namespace:<br/>
    /// <see cref="VertexAttrib"/>, <see cref="Uniform{T}"/>, <see cref="TextureUniform"/>, <see cref="ImageUniform"/>,
    /// <see cref="UniformBuffer"/>, <see cref="TransformOut"/>, <see cref="ShaderStorage"/>
    /// </summary>
    public class Shader
        : GLResource
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Shader));

        /// <summary>
        /// Path used when looking for shader files.
        /// </summary>
        public static string BasePath { get; set; }

        /// <summary>
        /// Handle of the currently active shader program.
        /// </summary>
        public static int ActiveProgramHandle { get; protected set; }

        /// <summary>
        /// Defines the <see cref="TransformFeedbackMode"/> which is used to specify how data should be written to the output buffer(s).
        /// May be overwritten by derived classes to be changed.
        /// </summary>
        protected virtual TransformFeedbackMode TransformOutMode
        {
            get { return TransformFeedbackMode.SeparateAttribs; }
        }

        static Shader()
        {
            BasePath = "Data/Shaders/";
        }

        /// <summary>
        /// Initializes a new instance of this shader.<br/>
        /// Retrieves shader source filenames from ShaderSourceAttributes tagged to this type.
        /// </summary>
        protected Shader()
            : base(GL.CreateProgram())
        {
            var shaderSources = ShaderSourceAttribute.GetShaderSources(this);
            if (shaderSources.Count == 0) throw new ApplicationException("ShaderSourceAttribute(s) missing!");
            CreateProgram(shaderSources);
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteProgram(Handle);
        }

        /// <summary>
        /// Activates the shader program.
        /// </summary>
        public void Use()
        {
            // remember handle of the active program
            ActiveProgramHandle = Handle;
            // activate program
            GL.UseProgram(Handle);
            // disable all vertex attributes
            //TODO: maybe dont ever do that
            VertexAttrib.DisableVertexAttribArrays();
        }

        private void CreateProgram(Dictionary<ShaderType, string> shaders)
        {
            Logger.InfoFormat("Creating shader program: {0}", GetType().Name);
            // load and attach all specified shaders
            var shaderObjects = shaders.Select(_ => AttachShader(_.Key, _.Value)).ToList();
            // bind transform feedback varyings if any
            var outs = InitializeTransformOut();
            if (outs.Count > 0)
            {
                GL.TransformFeedbackVaryings(Handle, outs.Count, outs.ToArray(), TransformOutMode);
            }
            // link program
            Logger.DebugFormat("Linking shader program: {0}", GetType().Name);
            GL.LinkProgram(Handle);
            // release shader objects, after they are linked into the program
            shaderObjects.ForEach(GL.DeleteShader);
            // assert that no link errors occured
            int linkStatus;
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out linkStatus);
            var info = GL.GetProgramInfoLog(Handle);
            Logger.DebugFormat("Link status: {0}", linkStatus);
            if (!string.IsNullOrEmpty(info)) Logger.InfoFormat("Link log:\n{0}", info);
            if (linkStatus != 1)
            {
                var msg = string.Format("Error linking program: {0}", GetType().Name);
                Logger.Error(msg);
                throw new ApplicationException(msg);
            }
            // initialize shader properties
            InitializePropertyMapping();
        }

        private int AttachShader(ShaderType type, string name)
        {
            Logger.DebugFormat("Compiling {0}: {1}", type, name);
            // create shader
            var shader = GL.CreateShader(type);
            // load shaders source
            GL.ShaderSource(shader, RetrieveShaderSource(Path.Combine(BasePath, name)));
            // compile shader
            GL.CompileShader(shader);
            // assert that no compile error occured
            int compileStatus;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out compileStatus);
            Logger.DebugFormat("Compiling status: {0}", compileStatus);
            var info = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(info)) Logger.InfoFormat("Compile log for {0}:\n{1}", name, info);
            if (compileStatus != 1)
            {
                var msg = string.Format("Error compiling shader: {0}", name);
                Logger.Error(msg);
                throw new ApplicationException(msg);
            }
            // attach shader to program
            GL.AttachShader(Handle, shader);
            return shader;
        }

        private static string RetrieveShaderSource(string file, List<string> includedNames = null)
        {
            const string includeKeyword = "#include";
            if (includedNames == null) includedNames = new List<string>();
            // check for multiple includes of the same file
            if (includedNames.Contains(file))
            {
                Logger.WarnFormat("File already included: {0} (included files: {1})", file, string.Join(", ", includedNames));
                return "";
            }
            includedNames.Add(file);
            // load shaders source
            var source = new StringBuilder();
            StreamReader reader;
            try
            {
                reader = new StreamReader(file + ".glsl");
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
                        includedFile = Path.Combine(Path.GetDirectoryName(file) ?? "", includedFile);
                        source.Append(RetrieveShaderSource(includedFile, includedNames));
                        // the #line statement defines the line number of the *next* line
                        source.AppendLine(string.Format("#line {0} {1}", lineNumber+1, fileNumber));
                    }
                    lineNumber++;
                }
            }
            return source.ToString();
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
        private void InitializePropertyMapping()
        {
            foreach (var property in GetType().GetProperties())
            {
                var mapping = TypeMapping.FirstOrDefault(_ => _.MappedType == property.PropertyType);
                if (mapping == null) continue;
                Logger.DebugFormat("Creating property mapping: {0}", property.Name);
                property.SetValue(this, mapping.Create(Handle, property), null);
            }
        }

        /// <summary>
        /// Defines available property mappings.<br/>
        /// NOTE: maybe refactor to use GL.ProgramUniform* later (check out EXT_direct_state_access)
        /// </summary>
        private static readonly List<IMapping> TypeMapping = new List<IMapping>
        {
            new Mapping<VertexAttrib>((p,i) => new VertexAttrib(p, i.Name, i.GetCustomAttributes<VertexAttribAttribute>(false).FirstOrDefault() ?? new VertexAttribAttribute())),
            new Mapping<ImageUniform>((p,i) => new ImageUniform(p, i.Name)),
            new Mapping<TextureUniform>((p,i) => new TextureUniform(p, i.Name)),
            new Mapping<TextureUniform<Texture>>((p,i) => new TextureUniform(p, i.Name)),
            new Mapping<TextureUniform<Texture1D>>((p,i) => new TextureUniform<Texture1D>(p, i.Name)),
            new Mapping<TextureUniform<Texture2D>>((p,i) => new TextureUniform<Texture2D>(p, i.Name)),
            new Mapping<TextureUniform<Texture3D>>((p,i) => new TextureUniform<Texture3D>(p, i.Name)),
            new Mapping<TextureUniform<Texture1DArray>>((p,i) => new TextureUniform<Texture1DArray>(p, i.Name)),
            new Mapping<TextureUniform<Texture2DArray>>((p,i) => new TextureUniform<Texture2DArray>(p, i.Name)),
            new Mapping<TextureUniform<TextureCubemap>>((p,i) => new TextureUniform<TextureCubemap>(p, i.Name)),
            new Mapping<TextureUniform<TextureCubemapArray>>((p,i) => new TextureUniform<TextureCubemapArray>(p, i.Name)),
            new Mapping<TextureUniform<Texture2DMultisample>>((p,i) => new TextureUniform<Texture2DMultisample>(p, i.Name)),
            new Mapping<TextureUniform<Texture2DMultisampleArray>>((p,i) => new TextureUniform<Texture2DMultisampleArray>(p, i.Name)),
            new Mapping<TextureUniform<TextureRectangle>>((p,i) => new TextureUniform<TextureRectangle>(p, i.Name)),
            new Mapping<TextureUniform<TextureBuffer>>((p,i) => new TextureUniform<TextureBuffer>(p, i.Name)),
            new Mapping<Uniform<bool>>((p,i) => new Uniform<bool>(p, i.Name, (l,b) => GL.Uniform1(l, b?1:0))),
            new Mapping<Uniform<int>>((p,i) => new Uniform<int>(p, i.Name, GL.Uniform1)),
            new Mapping<Uniform<uint>>((p,i) => new Uniform<uint>(p, i.Name, GL.Uniform1)),
            new Mapping<Uniform<float>>((p,i) => new Uniform<float>(p, i.Name, GL.Uniform1)),
            new Mapping<Uniform<double>>((p,i) => new Uniform<double>(p, i.Name, GL.Uniform1)),
            new Mapping<Uniform<Half>>((p,i) => new Uniform<Half>(p, i.Name, (_, half) => GL.Uniform1(_, half))),
            new Mapping<Uniform<Color>>((p,i) => new Uniform<Color>(p, i.Name, (_, color) => GL.Uniform4(_, color))),
            new Mapping<Uniform<Vector2>>((p,i) => new Uniform<Vector2>(p, i.Name, GL.Uniform2)),
            new Mapping<Uniform<Vector3>>((p,i) => new Uniform<Vector3>(p, i.Name, GL.Uniform3)),
            new Mapping<Uniform<Vector4>>((p,i) => new Uniform<Vector4>(p, i.Name, GL.Uniform4)),
            new Mapping<Uniform<Vector2d>>((p,i) => new Uniform<Vector2d>(p, i.Name, (_, vector) => GL.Uniform2(_, vector.X, vector.Y))),
            new Mapping<Uniform<Vector2h>>((p,i) => new Uniform<Vector2h>(p, i.Name, (_, vector) => GL.Uniform2(_, vector.X, vector.Y))),
            new Mapping<Uniform<Vector3d>>((p,i) => new Uniform<Vector3d>(p, i.Name, (_, vector) => GL.Uniform3(_, vector.X, vector.Y, vector.Z))),
            new Mapping<Uniform<Vector3h>>((p,i) => new Uniform<Vector3h>(p, i.Name, (_, vector) => GL.Uniform3(_, vector.X, vector.Y, vector.Z))),
            new Mapping<Uniform<Vector4d>>((p,i) => new Uniform<Vector4d>(p, i.Name, (_, vector) => GL.Uniform4(_, vector.X, vector.Y, vector.Z, vector.W))),
            new Mapping<Uniform<Vector4h>>((p,i) => new Uniform<Vector4h>(p, i.Name, (_, vector) => GL.Uniform4(_, vector.X, vector.Y, vector.Z, vector.W))),
            new Mapping<Uniform<Matrix2>>((p,i) => new Uniform<Matrix2>(p, i.Name, (_, matrix) => GL.UniformMatrix2(_, false, ref matrix))),
            new Mapping<Uniform<Matrix3>>((p,i) => new Uniform<Matrix3>(p, i.Name, (_, matrix) => GL.UniformMatrix3(_, false, ref matrix))),
            new Mapping<Uniform<Matrix4>>((p,i) => new Uniform<Matrix4>(p, i.Name, (_, matrix) => GL.UniformMatrix4(_, false, ref matrix))),
            new Mapping<Uniform<Matrix2x3>>((p,i) => new Uniform<Matrix2x3>(p, i.Name, (_, matrix) => GL.UniformMatrix2x3(_, false, ref matrix))),
            new Mapping<Uniform<Matrix2x4>>((p,i) => new Uniform<Matrix2x4>(p, i.Name, (_, matrix) => GL.UniformMatrix2x4(_, false, ref matrix))),
            new Mapping<Uniform<Matrix3x2>>((p,i) => new Uniform<Matrix3x2>(p, i.Name, (_, matrix) => GL.UniformMatrix3x2(_, false, ref matrix))),
            new Mapping<Uniform<Matrix3x4>>((p,i) => new Uniform<Matrix3x4>(p, i.Name, (_, matrix) => GL.UniformMatrix3x4(_, false, ref matrix))),
            new Mapping<Uniform<Matrix4x2>>((p,i) => new Uniform<Matrix4x2>(p, i.Name, (_, matrix) => GL.UniformMatrix4x2(_, false, ref matrix))),
            new Mapping<Uniform<Matrix4x3>>((p,i) => new Uniform<Matrix4x3>(p, i.Name, (_, matrix) => GL.UniformMatrix4x3(_, false, ref matrix))),
            new Mapping<UniformBuffer>((p,i) => new UniformBuffer(p, i.Name)),
            new Mapping<ShaderStorage>((p,i) => new ShaderStorage(p, i.Name)),
            new Mapping<FragData>((p,i) => new FragData(p, i.Name))
        };

        /// <summary>
        /// Enables registration of additional property mappings.<br/>
        /// Mapped types provide automatic property initialization on instantiation.
        /// </summary>
        /// <param name="mapping">The property mapping to add.</param>
        public void AddPropertyMapping(IMapping mapping)
        {
            TypeMapping.Add(mapping);
        }
    }
}