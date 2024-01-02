using System;

namespace CruZ.Components
{
    public class ComponentManager
    {
        public static bool IsComponent(Type ty)
        {
            return typeof(IComponent).IsAssignableFrom(ty);
        }
    }
}