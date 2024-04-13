using System.Runtime.Serialization;

namespace CruZ.Framework.GameSystem.ECS
{
    public partial class SpriteRendererComponent
    {
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if(_spriteResInfo != null)
            {
                LoadTexture(_spriteResInfo.ResourceName);
            }
        }
    }
}