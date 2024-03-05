using System.Runtime.CompilerServices;
using System;

namespace CruZ.Common.Utility
{
    class Helper
    {
        public static T GetUnitializeObject<T>()
        {
            return (T)GetUnitializeObject(typeof(T));
        }

        public static object GetUnitializeObject(Type type)
        {
            return RuntimeHelpers.GetUninitializedObject(type);
        }
    }
}