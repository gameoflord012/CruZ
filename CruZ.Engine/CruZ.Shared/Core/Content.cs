using Microsoft.Xna.Framework.Content;

namespace CruZ
{
    public class Content
    {
        public static ContentManager GetManager()
        {
            return MGWrapper.Instance().Content;
        }

        public static T Load<T>(string resourceName)
        {
            return GetManager().Load<T>(resourceName);
        }
    }
}
