using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CruZ.Utility
{
    public class Logging
    {
        public static string OutputDir = ".";

        public static void SetMsg(string newMsg, string key = DefaultString, bool updateLogFile = false)
        {
            Main._msgs[key] = newMsg;

            if (updateLogFile)
                FileHelper.WriteToFile(Path.Combine(OutputDir, key + ".log"),
                    newMsg, false);
        }

        public static void PushMsg(string newMsg, string key = DefaultString, bool updateLogFile = false)
        {
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

        public static string GetMsg(string key = DefaultString)
        {
            if (!Main._msgs.ContainsKey(key)) return "";
            return Main._msgs[key];
        }

        //public static void FlushToDebug()
        //{
        //    Debug.WriteLine("===========================CRUZ_LOGGING===========================");
        //    foreach (var msg in Main._msgs)
        //    {
        //        Debug.WriteLine(msg);
        //    }
        //    Debug.WriteLine("===========================END_LOG============================");
        //}

        #region Privates
        Dictionary<string, string> _msgs = new();
        static Logging _main;
        static Logging Main { get => _main ??= new Logging(); }
        #endregion

        const string DefaultString = "Default"; 
    }
}