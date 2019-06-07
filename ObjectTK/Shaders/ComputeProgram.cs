//
// ComputeProgram.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.Linq;
using ObjectTK.Shaders.Sources;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Shaders
{
    /// <summary>
    /// Represents a program object which contains compute shaders.
    /// </summary>
    public class ComputeProgram
        : Program
    {
        /// <summary>
        /// The work group size of the compute shader.
        /// </summary>
        public Vector3i WorkGroupSize { get; protected set; }

        /// <summary>
        /// The total number of work groups of this compute shader.
        /// </summary>
        public int WorkGroupTotalSize { get; protected set; }

        /// <summary>
        /// The maximum work group size of compute shaders.<br/>
        /// The values are hardware dependent and queried from OpenGL.
        /// </summary>
        public static Vector3i MaximumWorkGroupSize { get; protected set; }

        static ComputeProgram()
        {
            int x,y,z;
            GL.GetInteger((GetIndexedPName)All.MaxComputeWorkGroupCount, 0, out x);
            GL.GetInteger((GetIndexedPName)All.MaxComputeWorkGroupCount, 1, out y);
            GL.GetInteger((GetIndexedPName)All.MaxComputeWorkGroupCount, 2, out z);
            MaximumWorkGroupSize = new Vector3i(x,y,z);
        }

        /// <summary>
        /// Initializes a new instance of this compute shader.<br/>
        /// Retrieves shader source filenames from ShaderSourceAttributes tagged to this type.
        /// </summary>
        protected ComputeProgram()
        {
            if (ShaderSourceAttribute.GetShaderSources(GetType()).Any(_ => _.Type != ShaderType.ComputeShader))
                throw new ApplicationException("Invalid ShaderType supplied to compute shader via ShaderSourceAttribute(s).");
        }

        public override void Link()
        {
            base.Link();
            // query the work group size
            var sizes = new int[3];
            GL.GetProgram(Handle, (GetProgramParameterName)All.ComputeWorkGroupSize, sizes);
            WorkGroupSize = new Vector3i(sizes[0], sizes[1], sizes[2]);
            WorkGroupTotalSize = sizes[0] * sizes[1] * sizes[2];
        }

        /// <summary>
        /// Splits a given number of work groups up to the three dimensions.
        /// The number of work groups in any dimensions is kept less or equal to the supported maximum.
        /// The resulting total number of work groups may be larger than the given number.
        /// </summary>
        public static Vector3i SplitWorkGroups(long groups)
        {
            //TODO: add missing limitations. there is not only the limitation on workgroup size in each dimension but also for the total number of work groups
            if (groups > (long)MaximumWorkGroupSize.X * MaximumWorkGroupSize.Y * MaximumWorkGroupSize.Z)
                throw new ArgumentOutOfRangeException("groups", groups, "Maximum number of work groups exceeded.");
            double n = groups;
            // determine number of layers needed
            var z = (int)Math.Ceiling(n / ((long)MaximumWorkGroupSize.X * MaximumWorkGroupSize.Y));
            // determine the number of items per layer
            n = Math.Ceiling(n / z);
            // determine x and y to have the smallest number of items equal or larger to n
            var y = (int)Math.Ceiling(n / MaximumWorkGroupSize.X);
            var x = (int)Math.Ceiling(n / y);
            return new Vector3i(x, y, z);
        }

        /// <summary>
        /// Dispatches the currently active compute shader.
        /// </summary>
        public static void Dispatch(int x, int y, int z)
        {
            GL.DispatchCompute(x, y, z);
        }

        /// <summary>
        /// Dispatches the currently active compute shader.
        /// </summary>
        /// <param name="workGroups">The number of work groups to be launched for each dimension.</param>
        public static void Dispatch(Vector3i workGroups)
        {
            GL.DispatchCompute(workGroups.X, workGroups.Y, workGroups.Z);
        }

        /// <summary>
        /// Dispatches the currently active compute shader.
        /// May split the number of work groups up to more dimensions.
        /// </summary>
        public static void Dispatch(int total)
        {
            Dispatch(SplitWorkGroups(total));
        }

        /// <summary>
        /// Dispatches the currently active compute shader.
        /// Uses the shaders work group size to launch as many work groups as necessary to reach the given number of invocations.
        /// May also split the number of work groups up to more dimensions.
        /// </summary>
        public void DispatchInvocations(int invocations)
        {
            Dispatch((int)Math.Ceiling((float)invocations / WorkGroupTotalSize));
        }
    }
}