using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace DerpGL
{
    public abstract class QueryIndexer
    {
        // stores indices currently in use for each QueryTarget
        private static readonly Dictionary<QueryTarget, SortedSet<int>> TargetIndices = new Dictionary<QueryTarget, SortedSet<int>>();

        private static readonly List<QueryTarget> IndexableTargets = new List<QueryTarget>
        {
            QueryTarget.PrimitivesGenerated,
            QueryTarget.TransformFeedbackPrimitivesWritten
        };

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
            // remember index is used
            TargetIndices[target].Add(i);
            return i;
        }

        protected static void ReleaseIndex(QueryTarget target, int index)
        {
            TargetIndices[target].Remove(index);
        }
    }
}