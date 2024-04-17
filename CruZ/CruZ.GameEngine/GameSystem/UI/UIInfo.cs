using CruZ.GameEngine.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.UI
{
    public struct UIInfo
    {
        public GameTime GameTime;
        public IInputInfo InputInfo;
        public SpriteBatch SpriteBatch;

        public Point MousePos()
        {
            return new(
                InputInfo.CurMouse.Position.X,
                InputInfo.CurMouse.Position.Y);
        }
    }
}
