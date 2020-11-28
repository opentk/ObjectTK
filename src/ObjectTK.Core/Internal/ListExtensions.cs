using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ObjectTK.Internal {
    internal static class UnsafeListExtensions {
        private static class ArrayAccessor<T>
        {
            internal static readonly Func<List<T>, T[]> GetUnderlyingArray;

            static ArrayAccessor()
            {
                var dm = new DynamicMethod("get", MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, typeof(T[]), new[] { typeof(List<T>) }, typeof(ArrayAccessor<T>), true);
                var il = dm.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0); // Load List<T> argument
                il.Emit(OpCodes.Ldfld, typeof(List<T>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)!); // Replace argument by field
                il.Emit(OpCodes.Ret); // Return field
                GetUnderlyingArray = (Func<List<T>, T[]>)dm.CreateDelegate(typeof(Func<List<T>, T[]>));
            }
        }
            
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        internal static T[] GetInternalArray<T>(this List<T> list)
        {
            return ArrayAccessor<T>.GetUnderlyingArray(list);
        }
    }
}
