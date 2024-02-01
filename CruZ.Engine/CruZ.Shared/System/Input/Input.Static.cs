using CruZ.Exception;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruZ.Systems
{
    public interface IInputContextProvider
    {
        public event Action<GameTime> InputUpdate;
    }

    public partial class Input
    {
        public static event Action<InputInfo> MouseScroll;
        public static event Action<InputInfo> MouseMove;
        public static event Action<InputInfo> MouseDown;
        public static event Action<InputInfo> MouseUp;

        public static InputInfo Info => _Instance.GetInputInfo();

        static Input? _Instance;

        public static void CreateContext(IInputContextProvider contextProvider)
        {
            _Instance = new(contextProvider);
        }
    }
}
