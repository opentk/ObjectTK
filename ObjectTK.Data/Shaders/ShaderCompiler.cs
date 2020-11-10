using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ObjectTK.Data.Variables;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Shaders {
    
    public static class ShaderCompiler {
        
        /// Helper method that compiles a shader stage (part of a ShaderProgram) or throws an exception if failure occurs.
        [MustUseReturnValue]
        [NotNull]
        public static ShaderStage CompileShaderStage(ShaderType shaderType, string name, string source)
        {
            var handle = GL.CreateShader(shaderType);
            var objLabel = $"ShaderStage: {name}-{shaderType}";
            GL.ObjectLabel(ObjectLabelIdentifier.Shader, handle, objLabel.Length, objLabel);
            GL.ShaderSource(handle, source);
            GL.CompileShader(handle);
            GL.GetShader(handle, ShaderParameter.CompileStatus, out var compileStatus);
            if (compileStatus == 0)
            {
                var vertLog = GL.GetShaderInfoLog(handle);
                //TODO: needs a proper exception type.
                var msg = $"Failed to compile shader '{name}' of type {shaderType}. Error:\n{vertLog}";
                throw new Exception(msg);
            }
            return new ShaderStage(shaderType, handle, source);
        }
        
        [NotNull]
        [MustUseReturnValue]
        public static ShaderProgram Introspect(int prog, ShaderStage[] stages) {
            // inspect the attributes:
            var attributes = new Dictionary<string, ShaderAttributeInfo>(StringComparer.Ordinal);

            GL.GetProgram(prog, GetProgramParameterName.ActiveAttributes, out var attribCount);
            for (var i = 0; i < attribCount; i++)
            {
                var attrName = GL.GetActiveAttrib(prog, i, out var count, out var attrType);
                var attrLoc = GL.GetAttribLocation(prog, attrName);
                attributes[attrName] = new ShaderAttributeInfo() {
                    Name = attrName,
                    Location = attrLoc,
                    Size = count,
                    ActiveAttribType = attrType,
                };
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            }
            
            // inspect the uniforms
            var uniforms = new Dictionary<string, ShaderUniformInfo>(StringComparer.Ordinal);

            GL.GetProgram(prog, GetProgramParameterName.ActiveUniforms, out var uniformCount);
            for (var i = 0; i < uniformCount; i++)
            {
                var uniName = GL.GetActiveUniform(prog, i, out var uniSize, out var uniType);
                var uniLoc = GL.GetUniformLocation(prog, uniName);
                uniforms[uniName] = new ShaderUniformInfo(uniName, uniLoc, uniSize, uniType);
            }
            
            return new ShaderProgram(prog, stages, uniforms, attributes);
        }

        [MustUseReturnValue]
        [NotNull]
        public static ShaderProgram VertexFrag(string name, string vertSource, string fragSource) {

            var vert = CompileShaderStage(ShaderType.VertexShader, name, vertSource);
            var frag = CompileShaderStage(ShaderType.FragmentShader, name, fragSource);
            var stages = new[] {vert, frag};
            
            int handle = GL.CreateProgram();
            var label = $"ShaderProgram: {name}";
            GL.ObjectLabel(ObjectLabelIdentifier.Program, handle, label.Length, label);

            GL.AttachShader(handle, vert.Handle);
            GL.AttachShader(handle, frag.Handle);
            GL.LinkProgram(handle);
            
            GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int linkStatus);
            if (linkStatus == 0)
            {
                var linkLog = GL.GetProgramInfoLog(handle);
                //TODO: needs a proper exception type.
                var msg = $"Failed to link shader '{name}'. Error:\n{linkLog}";
                throw new Exception(msg);
            }

            return Introspect(handle, stages);
        }
    }
}
