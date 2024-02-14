using CruZ.Components;
using System;

namespace CruZ.Utility
{
    public class ComponentHelper
    {
        public static IComponent GetDefaultComponentInstance(Type ty)
        {
            var consInfo = ty.GetConstructor(Type.EmptyTypes);
            if (consInfo == null)
            {
                throw new ArgumentException($"Component of type {ty} need to have default constructor to create default instannce");
            }

            var iCom = (IComponent)consInfo.Invoke(null);
            return iCom;
        }
    }
}