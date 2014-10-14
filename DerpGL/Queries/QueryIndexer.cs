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
using System.Collections.Generic;
using DerpGL.Exceptions;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Queries
{
    /// <summary>
    /// Base class for <see cref="QueryMapping{T}"/>.<br/>
    /// Provides methods to prevent query collisions on standard and indexable <see cref="QueryTarget"/>s.
    /// </summary>
    public abstract class QueryIndexer
    {
        /// <summary>
        /// Stores indices currently in use for each QueryTarget.
        /// </summary>
        private static readonly Dictionary<QueryTarget, SortedSet<int>> TargetIndices = new Dictionary<QueryTarget, SortedSet<int>>();

        /// <summary>
        /// Defines which <see cref="QueryTarget"/> is indexable.
        /// </summary>
        private static readonly List<QueryTarget> IndexableTargets = new List<QueryTarget>
        {
            QueryTarget.PrimitivesGenerated,
            QueryTarget.TransformFeedbackPrimitivesWritten
        };

        /// <summary>
        /// Acquires an unused index for the given <see cref="QueryTarget"/>.
        /// </summary>
        /// <param name="target">The QueryTarget to acquire an index for.</param>
        /// <returns>Unused index.</returns>
        protected static int AcquireIndex(QueryTarget target)
        {
            if (!TargetIndices.ContainsKey(target)) TargetIndices.Add(target, new SortedSet<int>());
            // find first free index
            var i = 0;
            foreach (var index in TargetIndices[target])
            {
                if (index > i) break;
                i++;
            }
            if (!IndexableTargets.Contains(target) && i > 0) throw new QueryException(
                string.Format("Query target not indexable and the single target it already in use: {0}", target));
            // remember index is in use
            TargetIndices[target].Add(i);
            return i;
        }

        /// <summary>
        /// Releases a previously acquired index for the given <see cref="QueryTarget"/>.
        /// </summary>
        /// <param name="target">The QueryTarget to release the index from.</param>
        /// <param name="index">The index to release.</param>
        protected static void ReleaseIndex(QueryTarget target, int index)
        {
            TargetIndices[target].Remove(index);
        }
    }
}