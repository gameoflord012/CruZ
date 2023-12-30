using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CruZ
{
    public class MainInput
    {
        MGWrapper _core;
        public MainInput(MGWrapper core)
        {
            _core = core;
            _core.OnUpdate += InputUpdate;
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

        public KeyboardState KeyboardState { get => _keyboardState; }
    }
}
