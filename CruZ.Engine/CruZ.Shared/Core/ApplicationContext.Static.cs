
using CruZ.Exception;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ
{
    public partial class ApplicationContext
    {
        private static ApplicationContext _instance;
        public static ApplicationContext Instance { get => _instance ?? throw new MissingContextException(typeof(ApplicationContext)); }

        public static void CreateContext(IApplicationContextProvider contextProvider)
        {
            _instance = new(contextProvider);
        }

        public static GraphicsDevice GraphicsDevice { get => Instance._contextProvider.GraphicsDevice; }
        public static ContentManager Content        { get => Instance._contextProvider.Content; }
    }
}
