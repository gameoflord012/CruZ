using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CruZ.GameEngine.Input
{
    public interface IInputInfo
    {
        int SrollDelta { get; }

        MouseState CurMouse { get; }
        MouseState PreMouse { get; }
        KeyboardState PreKeyboard { get; }
        KeyboardState Keyboard { get; }

        bool MouseStateChanges { get; }
        bool MouseClick { get; }
        bool MouseMoving { get; }

        bool IsMouseHeldDown(MouseKey key);
        bool IsMouseHeldUp(MouseKey key);
        bool IsMouseJustDown(MouseKey key);
        bool IsMouseJustUp(MouseKey key);

        bool IsKeyJustDown(Keys key)
        {
            return PreKeyboard.IsKeyUp(key) && Keyboard.IsKeyDown(key);
        }

        bool IsKeyHeldDown(Keys key)
        {
            return Keyboard.IsKeyDown(key);
        }

        Point MousePos()
        {
            return new(CurMouse.X, CurMouse.Y);
        }
    }
}
