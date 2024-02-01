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

        public static InputInfo Info => Instance.GetInputInfo();

        static Input? _instance;
        public static Input Instance { get => _instance ?? throw new MissingContextException(typeof(Input)); }

        public static void CreateContext(IInputContextProvider contextProvider)
        {
            _instance = new(contextProvider);
        }
    }
}
