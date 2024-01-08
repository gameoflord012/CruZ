using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruZ
{
    public interface IApplicationContextProvider
    {
        GraphicsDevice GraphicsDevice { get; }
        ContentManager Content { get; }
    }

    public partial class ApplicationContext
    {
        ApplicationContext(IApplicationContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        IApplicationContextProvider _contextProvider;
    }
}
