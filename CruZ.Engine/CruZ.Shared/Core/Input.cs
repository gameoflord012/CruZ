using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;

namespace CruZ
{
    public class Input
    {
        static Input? _instance;
        public static Input Instance { get => _instance ??= new Input(); }

        public Input()
        {
            Core.OnUpdate += InputUpdate;
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

        public static KeyboardState KeyboardState { get => Instance._keyboardState; }
    }
}
