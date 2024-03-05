using CruZ.Exception;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruZ.GameSystem
{
    public interface IInputContextProvider
    {
        public event Action<GameTime> InputUpdate;
    }

    public partial class Input
    {
        public static event Action<IInputInfo>? MouseScrolled;
        public static event Action<IInputInfo>? MouseMoved;
        public static event Action<IInputInfo>? MouseStateChanged;

        public static event Action<IInputInfo>? KeyStateChanged;
        //public static event Action<IInputInfo>? MouseClicked;

        public static IInputInfo Info => _instance._info;

        static Input? _instance;

        public static void CreateContext(IInputContextProvider contextProvider)
        {
            _instance = new(contextProvider);
        }
    }
}
