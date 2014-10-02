using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace DerpGL
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
            if (!IndexableTargets.Contains(target) && i > 0) throw new ApplicationException(string.Format("Query target already in use and not indexable: {0}", target));
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