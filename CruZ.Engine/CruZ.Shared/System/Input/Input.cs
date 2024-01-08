using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CruZ.Systems
{
    public partial class Input
    {
        public Input(IInputContextProvider contextProvider)
        {
            contextProvider.UpdateInputEvent += InputUpdate;
        }

        public void InputUpdate(GameTime gameTime)
        {
            _prevMouseState = _curMouseState;
            _curMouseState = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
        }

        public int ScrollDeltaValue()
        {
            return _curMouseState.ScrollWheelValue - _prevMouseState.ScrollWheelValue;
        }

        MouseState _prevMouseState;
        MouseState _curMouseState;
        KeyboardState _keyboardState;
    }
}
