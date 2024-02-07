using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace CruZ.Utility
{
    public class Logging
    {
        Dictionary<string, string> _msgs = new();
        int _maxMsg = 10;

        public static void SetMsg(string msg, string key = DefaultString)
        {
            Main._msgs [key] = msg;
        }

        #region Static
        public static void PushMsg(string newMsg, string key = DefaultString)
        {
            //while (Main._msgs.Count > Main._maxMsg) Main._msgs.RemoveAt(0);
            var msg = GetMsg(key);
            var prefix = string.IsNullOrEmpty(msg) ? "" : msg + Environment.NewLine;
            SetMsg(prefix + newMsg);
        }

        public static void PushMsg(string fmt, string key = DefaultString, params object[] args)
        {
            var msg = string.Format(fmt, args);
            PushMsg(msg, key);
        }

        public static string GetMsg(string key = DefaultString)
        {
            if(!Main._msgs.ContainsKey(key)) return "";
            return Main._msgs[key];
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

        private static Logging _main;
        private static Logging Main { get => _main ??= new Logging(); } 
        private const string DefaultString = "Default";
        #endregion
        //public Dictionary<string, string> Msgs { get => _msgs; set => _msgs = value; }
    }
}