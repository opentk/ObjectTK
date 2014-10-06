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
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shapes
{
    public class TexturedQuad
        : TexturedShape
    {
        public TexturedQuad()
        {
            DefaultMode = PrimitiveType.TriangleStrip;
            
            // Source: http://www.opengl.org/archives/resources/faq/technical/transformations.htm (9.090 How do I draw a full-screen quad?)
            // Your rectangle or quad's Z value should be in the range of –1.0 to 1.0, with –1.0 mapping to the zNear clipping plane, and 1.0 to the zFar clipping plane.
            const int z = 0;
            Vertices = new[]
            {
                new Vector3(-1, -1, z),
                new Vector3( 1, -1, z),
                new Vector3(-1,  1, z),
                new Vector3( 1,  1, z)
            };
            
            TexCoords = new[]
            {
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(0,1),
                new Vector2(1,1)
            };
        }
    }
}