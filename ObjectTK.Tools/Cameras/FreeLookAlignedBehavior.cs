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
    public class FreeLookAlignedBehavior
        : FreeLookBehavior
    {
        public Vector3 AlignmentPoint;

        public override void UpdateFrame(CameraState state, float step)
        {
            base.UpdateFrame(state, step);
            // update the up direction to always point away from the alignment point
            var up = state.Position - AlignmentPoint;
            up.Normalize();
            // if the angle between the old and new up directions is larger than 90°
            // we assume that the camera is upside down and keep it that way
            state.Up = Vector3.Dot(state.Up, up) < 0 ? -up : up;
        }
    }
}