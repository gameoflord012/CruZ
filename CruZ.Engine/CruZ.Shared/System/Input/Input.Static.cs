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
        public event Action<GameTime> UpdateInputEvent;
    }

    public partial class Input
    {
        static Input? _instance;
        public static Input Instance { get => _instance ?? throw new MissingContextException(typeof(Input)); }

        public static void SetContext(IInputContextProvider contextProvider)
        {
            _instance = new(contextProvider);
        }

        public static KeyboardState KeyboardState { get => Instance._keyboardState; }
    }
}
