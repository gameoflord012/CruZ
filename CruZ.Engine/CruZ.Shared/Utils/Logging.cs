using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace CruZ.Utility
{
    public class Logging
    {
        private static string LOGGING_DIR = ".\\logs\\";

        Dictionary<string, string> _msgs = new();
        int _maxMsg = 10;

        //public static void SetMsg(string msg, string key = DefaultString)
        //{

        //}

        public static void SetMsg(string newMsg, string key = DefaultString, bool updateLogFile = false)
        {
            Main._msgs[key] = newMsg;

            if (updateLogFile)
                FileHelper.WriteToFile($"{LOGGING_DIR}{key}.log",
                    newMsg, false);
        }

        public static void PushMsg(string newMsg, string key = DefaultString, bool updateLogFile = false)
        {
            //while (Main._msgs.Count > Main._maxMsg) Main._msgs.RemoveAt(0);

            StringBuilder sb = new();

            var prevMsg = GetMsg(key);

            if (!string.IsNullOrEmpty(prevMsg))
            {
                sb.Append(prevMsg);
                sb.Append(Environment.NewLine);
            }

            sb.Append(newMsg);

            SetMsg(sb.ToString(), key, updateLogFile);
        }

        //public static void PushMsg(string fmt, string key = DefaultString, params object[] args)
        //{
        //    var msg = string.Format(fmt, args);
        //    PushMsg(msg, key);
        //}

        public static string GetMsg(string key = DefaultString)
        {
            if (!Main._msgs.ContainsKey(key)) return "";
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
        //public Dictionary<string, string> Msgs { get => _msgs; set => _msgs = value; }
    }
}