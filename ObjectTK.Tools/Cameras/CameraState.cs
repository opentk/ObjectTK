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
using OpenTK;

namespace ObjectTK.Tools.Cameras
{
    /// <summary>
    /// Represents the state of a camera.<br/>
    /// TODO: Maybe also add field-of-view and z-near/far plane parameter
    /// </summary>
    public class CameraState
    {
        public Vector3 Position;
        public Vector3 LookAt;
        public Vector3 Up;

        public CameraState()
        {
            LookAt.Z = -1;
            Up.Y = 1;
        }

        public override string ToString()
        {
            return string.Format((string) "({0},{1},{2})", (object) Position, (object) LookAt, (object) Up);
        }
    }
}