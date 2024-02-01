
using CruZ.Exception;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ
{
    public partial class ApplicationContext
    {
        private static ApplicationContext? _instance;

        public static void CreateContext(IApplicationContextProvider contextProvider)
        {
            _instance = new(contextProvider);
        }

        public static GraphicsDevice GraphicsDevice 
        {
            get => _instance._contextProvider.GraphicsDevice; 
        }
        public static ContentManager Content { get => _instance._contextProvider.Content; }
    }
}
