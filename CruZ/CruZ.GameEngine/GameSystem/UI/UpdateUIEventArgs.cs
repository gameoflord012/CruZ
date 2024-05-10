using CruZ.GameEngine.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.UI
{
    public class UpdateUIEventArgs
    {
        public UpdateUIEventArgs(GameTime gameTime, IInputInfo inputInfo)
        {
            GameTime = gameTime;
            InputInfo = inputInfo;
        }

        public GameTime GameTime
        { get; private set; }

        public IInputInfo InputInfo
        { get; private set; }
    }
}
