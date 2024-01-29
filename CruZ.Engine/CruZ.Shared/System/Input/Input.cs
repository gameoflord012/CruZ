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
            _prevMouseState =   _curMouseState;
            _curMouseState =    Mouse.GetState();
            _keyboardState =    Keyboard.GetState();
        }

        public int ScrollDeltaValue()
        {
            return _curMouseState.ScrollWheelValue - _prevMouseState.ScrollWheelValue;
        }

        public InputInfo GetInfo()
        {
            InputInfo info = new();
            info.SrollDelta = ScrollDeltaValue();
            info.PrevMouseState = _prevMouseState;
            info.CurMouseState = _curMouseState;
            info.KeyboardState = _keyboardState;
            return info;
        }

        MouseState      _prevMouseState;
        MouseState      _curMouseState;
        KeyboardState   _keyboardState;
    }

    public struct InputInfo
    {
        public int              SrollDelta;
        public MouseState       PrevMouseState;
        public MouseState       CurMouseState;
        public KeyboardState    KeyboardState;
    }
}
