﻿using System;
using System.Runtime.CompilerServices;

using CruZ.Common.ECS;
using CruZ.Framework.GameSystem.ECS;

namespace CruZ.Framework.Utility
{
    public class ComponentHelper
    {
        public static Component GetDefaultComponentInstance(Type ty)
        {
            var consInfo = ty.GetConstructor(Type.EmptyTypes) ?? throw new ArgumentException($"Component of type {ty} need to have default constructor to create default instannce");
            var comp = (Component)consInfo.Invoke(null);
            return comp;
        }

        //public static Type GetComponentType(Type ty)
        //{
        //    CheckTypeValid(ty);

        //    return 
        //        ((Component)RuntimeHelpers.GetUninitializedObject(ty)).ComponentType;
        //}

        //private static void CheckTypeValid(Type ty)
        //{
        //    if (ty.IsAbstract)
        //    {
        //        throw new ArgumentException($"Type {ty} can't be abstract");
        //    }

        //    if (!ty.IsAssignableTo(typeof(Component)))
        //    {
        //        throw new ArgumentException($"Type {ty} is not a component type");
        //    }
        //}
    }
}