using System;

namespace CruZ.Components
{
    enum ComponentEnum
    {
        Sprite,
        AnimatedSprite,
        EntityScript
    }

    public static class ComponentManager
    {
        Type GetComponentType(ComponentEnum componentEnum)
        {
            switch (componentEnum)
            {
                case ComponentEnum.Sprite:
                    return typeof(SpriteComponent);
                    break;
                case ComponentEnum.AnimatedSprite:
                    break;
                case ComponentEnum.EntityScript:
                    break;
                default:
                    break;
            }
        }
    }
}