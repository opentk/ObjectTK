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
using System.Linq;
using System.Reflection;
using DerpGL.Exceptions;
using DerpGL.Shaders.Variables;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Represents a program object.
    /// </summary>
    public class Program
        : GLResource
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// Path used when looking for shader files.
        /// </summary>
        public static string BasePath { get; set; }

        public string Name { get { return GetType().Name; } }

        private List<ProgramVariable> _variables;

        static Program()
        {
            BasePath = "Data/Shaders/";
        }

        protected Program()
            : base(GL.CreateProgram())
        {
            Logger.InfoFormat("Creating shader program: {0}", Name);
            InitializeShaderVariables();
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteProgram(Handle);
        }

        private void InitializeShaderVariables()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
            _variables = new List<ProgramVariable>();
            foreach (var property in GetType().GetProperties(flags).Where(_ => typeof(ProgramVariable).IsAssignableFrom(_.PropertyType)))
            {
                var instance = (ProgramVariable)Activator.CreateInstance(property.PropertyType, true);
                instance.Initialize(this, property);
                property.SetValue(this, instance, null);
                _variables.Add(instance);
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void Attach(ShaderType type, string filename)
        {
            using (var shader = new Shader(type))
            {
                shader.CompileSource(filename);
                Attach(shader);
            }
        }
        
        public void Attach(Shader shader)
        {
            GL.AttachShader(Handle, shader.Handle);
        }

        public void Detach(Shader shader)
        {
            GL.DetachShader(Handle, shader.Handle);
        }

        public virtual void Link()
        {
            Logger.DebugFormat("Linking program: {0}", Name);
            GL.LinkProgram(Handle);
            CheckLinkStatus();
            // call OnLink() on all ShaderVariables
            _variables.ForEach(_ => _.OnLink());
        }

        private void CheckLinkStatus()
        {
            // check link status
            int linkStatus;
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out linkStatus);
            Logger.DebugFormat("Link status: {0}", linkStatus);
            // check program info log
            var info = GL.GetProgramInfoLog(Handle);
            if (!string.IsNullOrEmpty(info)) Logger.InfoFormat("Link log:\n{0}", info);
            // log message and throw exception on link error
            if (linkStatus == 1) return;
            var msg = string.Format("Error linking program: {0}", Name);
            Logger.Error(msg);
            throw new ProgramLinkException(msg, info);
        }

        public void AssertActive()
        {
#if DEBUG
            int activeHandle;
            GL.GetInteger(GetPName.CurrentProgram, out activeHandle);
            if (activeHandle != Handle) throw new ObjectNotBoundException("Program object is not currently active.");
#endif
        }
    }
}