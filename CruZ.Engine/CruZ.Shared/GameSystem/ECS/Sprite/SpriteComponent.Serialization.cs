using System.Runtime.Serialization;

namespace CruZ.Common.ECS
{
    public partial class SpriteComponent
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