using System;
using System.Diagnostics;
using System.Reflection;
using log4net;

namespace DerpGL
{
    public abstract class ContextResource
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ContextResource));

        /// <summary>
        /// Specifies whether leaks should be ignored.
        /// </summary>
        public static bool IgnoreLeaks { get; set; }

        /// <summary>
        /// Specifies whether the object currently holds references which must be properly released.
        /// </summary>
        protected bool ReleaseRequired { get; set; }

        private bool _isReleased;

        protected ContextResource()
        {
            _isReleased = false;
            ReleaseRequired = true;
        }

        ~ContextResource()
        {
            if (IgnoreLeaks) return;
            if (ReleaseRequired && !_isReleased) throw new ApplicationException("Some OpenGL resource was not properly released and leaked out of scope.. :/");
        }

        /// <summary>
        /// Advises the object to properly release all its handles.
        /// </summary>
        public void Release()
        {
#if DEBUG
            Trace.WriteLine(string.Format("Releasing a context resource: {0}", this));
#endif
            Logger.DebugFormat("Releasing a context resource: {0}", this);
            OnRelease();
            _isReleased = true;
        }

        public bool IsReleased()
        {
            return _isReleased;
        }

        protected abstract void OnRelease();

        protected void ReleaseAll()
        {
            // get all fields, including backing fields for properties
            foreach (var field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                // check if it should be released
                if (typeof (ContextResource).IsAssignableFrom(field.FieldType))
                {
                    // and release it
                    ((ContextResource)field.GetValue(this)).Release();
                }
            }
        }
    }
}