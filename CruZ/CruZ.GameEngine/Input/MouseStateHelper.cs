using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;

namespace CruZ.GameEngine.Input
{
    internal static class MouseStateHelper
    {
        internal static ButtonState ButtonState(this MouseState state, MouseKey key)
        {
            return key switch
            {
                MouseKey.Left => state.LeftButton,
                MouseKey.Middle => state.MiddleButton,
                MouseKey.Right => state.RightButton,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
