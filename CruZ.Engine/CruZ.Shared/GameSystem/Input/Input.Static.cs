using Microsoft.Xna.Framework;

using System;

namespace CruZ.Common.Input
{
    public interface IInputContextProvider
    {
        public event Action<GameTime> InputUpdate;
    }

    public partial class InputManager
    {
        public static event Action<IInputInfo>? MouseScrolled;
        public static event Action<IInputInfo>? MouseMoved;
        public static event Action<IInputInfo>? MouseStateChanged;

        public static event Action<IInputInfo>? KeyStateChanged;
        //public static event Action<IInputInfo>? MouseClicked;

        public static IInputInfo Info => _instance._info;

        static InputManager? _instance;

        public static void CreateContext(IInputContextProvider contextProvider)
        {
            _instance = new(contextProvider);
        }
    }
}
