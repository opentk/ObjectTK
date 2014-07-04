using System;
using System.Reflection;
using log4net;

namespace DerpGL
{
    public abstract class GLResource
        : GLObject
        , IDisposable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GLResource));
        
        protected GLResource(int handle)
            : base(handle)
        {
        }

        ~GLResource()
        {
            Logger.WarnFormat("GLResource leaked: {0}", this);
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            GC.KeepAlive(this);
        }

        protected abstract void Dispose(bool manual);

        public static void DisposeAll(object obj)
        {
            // get all fields, including backing fields for properties
            foreach (var field in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                // check if it should be released
                if (typeof (GLResource).IsAssignableFrom(field.FieldType))
                {
                    // and release it
                    ((GLResource)field.GetValue(obj)).Dispose();
                }
            }
        }
    }
}