#region License

// ObjectTK License
// Copyright (C) 2013-2015 J.C.Bernack
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
// along with this program. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Reflection;
using ObjectTK.Data.Variables;

namespace ObjectTK.Extensions.Shaders {
    public class Material<T> where T : class, new() {
        public ShaderProgram<T> ShaderProgram { get; set; }
        public T Variables { get; set; }

        public Material() {

            foreach (PropertyInfo Prop in ShaderProgram.UniformInfoProperties) {
                Type UniformType = Prop.PropertyType.GetGenericArguments()[0];
                object DefaultValue = Activator.CreateInstance(UniformType);
                ShaderUniformInfo PropValue = Prop.GetValue(ShaderProgram.Variables) as ShaderUniformInfo;
            }
        }

        public void Use() {
            ShaderProgram.Use();
        }

    }
}