using System;

namespace CruZ.Editor.Utility
{
    class Helper
    {
        static public Func<object> GetPropertyGetter(object target, string propertyName, Type ty)
        {
            var method = target.GetType().GetProperty(propertyName, ty).GetGetMethod();
            var getter = () => method.Invoke(target, []);

            return getter;
        }

        static public Action<object> GetPropertySetter(object target, string propertyName, Type ty)
        {
            var method = target.GetType().GetProperty(propertyName, ty).GetSetMethod();
            var setter = (object arg) => { method.Invoke(target, [arg]); };

            return setter;
        }

        //static public Func<T> GetPropertyGetter<T>(object target, string propertyName)
        //{
        //    return GetPropertyGetter(target, propertyName, typeof(T));
        //}

        //static public Action<T> GetPropertySetter<T>(object target, string propertyName)
        //{
        //    return GetPropertySetter(target, propertyName, typeof(T));
        //}
    }
}