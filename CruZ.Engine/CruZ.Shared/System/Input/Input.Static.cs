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
        public static event Action<InputInfo>? MouseScrolled;
        public static event Action<InputInfo>? MouseMoved;
        public static event Action<InputInfo>? MouseStateChanged;

        public static InputInfo Info => _Instance._info;

        static Input? _Instance;

        public static void CreateContext(IInputContextProvider contextProvider)
        {
            _Instance = new(contextProvider);
        }
    }
}
