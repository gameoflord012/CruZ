using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.Utility
{
    public class TextureHelper
    {
        public static T[] GetTextureData<T>(Texture2D tex) where T : struct
        {
            T[] data = new T[tex.Width * tex.Height];
            tex.GetData(data);
            return data;
        }
    }
}
