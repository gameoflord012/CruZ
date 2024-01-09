using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace CruZ.Utility
{
    public class Logging
    {
        private static Logging _main;
        public static Logging Main { get => _main ??= new Logging(); }

        public static void PushMsg(string msg)
        {
            while (Main._msgs.Count > Main._maxMsg) Main._msgs.RemoveAt(0);
            Main._msgs.Add(msg);
        }

        public static void PushMsg(string fmt, params object[] args)
        {
            var msg = string.Format(fmt, args);
            PushMsg(msg);
        }

        public static string[] GetMsgs()
        {
            return Main._msgs.ToArray();
        }

        public static void FlushToDebug()
        {
            Debug.WriteLine("===========================CRUZ_LOGGING===========================");
            foreach (var msg in Main._msgs)
            {
                Debug.WriteLine(msg);
            }
            Debug.WriteLine("===========================END_LOG============================");
        }

        List<string> _msgs = new();
        int _maxMsg = 10;

        public List<string> Msgs { get => _msgs; set => _msgs = value; }
    }
}