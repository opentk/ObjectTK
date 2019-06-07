//
// CameraState.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

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