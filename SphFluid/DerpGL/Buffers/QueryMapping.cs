using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    public class QueryMapping<T>
        : QueryIndexer
        where T : struct, IConvertible
    {
        private class QueryMap
        {
            internal readonly int Handle;

            internal bool Active;
            internal QueryTarget Target;
            internal int Index;

            public int Value;
            public int Average;

            public QueryMap(int handle)
            {
                Handle = handle;
            }
        }

        public float this[T key]
        {
            get
            {
                return _queries[key].Average;
            }
        }

        private readonly Dictionary<T, QueryMap> _queries;
        private const float AveragingFactor = 0.05f;
        private const int ElapsedTimeFactor = 1000;

        public QueryMapping()
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");
            // create a query object for each enum entry
            _queries = Enum.GetValues(typeof (T)).Cast<T>().ToDictionary(_ => _, _ => new QueryMap(GL.GenQuery()));
        }

        public void Begin(T mapping, QueryTarget target)
        {
            var map = _queries[mapping];
            if (map.Active) throw new ApplicationException(string.Format("Query already active: {0} {1}", target, mapping));
            map.Active = true;
            map.Target = target;
            map.Index = AcquireIndex(target);
            GL.BeginQueryIndexed(target, map.Index, map.Handle);
        }

        public void End(T mapping)
        {
            var map = _queries[mapping];
            GL.EndQueryIndexed(map.Target, map.Index);
            ReleaseIndex(map.Target, map.Index);
            map.Active = false;
        }

        public void Update()
        {
            foreach (var map in _queries.Values)
            {
                // get current value
                GL.GetQueryObject(map.Handle, GetQueryObjectParam.QueryResult, out map.Value);
                // scale elapsed time
                if (map.Target == QueryTarget.TimeElapsed) map.Value /= ElapsedTimeFactor;
                // calculate averaged value
                map.Average = (int)(map.Value * AveragingFactor + map.Average * (1 - AveragingFactor));
            }
        }
    }
}