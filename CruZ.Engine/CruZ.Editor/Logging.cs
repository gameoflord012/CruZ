using System.Runtime.Serialization;

namespace CruZ_Engine.Editor
{
    public class Logging
    {
        private static Logging _mainLogging;

        public static void ChangeMain(Logging main)
        {
            _mainLogging = main;
        }

        public static Logging Main()
        {
            if (_mainLogging == null) _mainLogging = new Logging();
            return _mainLogging;
        }

        public static void PushMsg(string msg)
        {
            while (Main()._msgs.Count > Main()._maxMsg) Main()._msgs.RemoveAt(0);
            Main()._msgs.Add(msg);
        }

        public static void PushMsg(string fmt, params object[] args)
        {
            var msg = string.Format(fmt, args);
            PushMsg(msg);
        }

        public static string[] GetMsgs()
        {
            return Main()._msgs.ToArray();
        }

        List<string> _msgs = new();
        int _maxMsg = 10;

        public List<string> Msgs { get => _msgs; set => _msgs = value; }
    }
}