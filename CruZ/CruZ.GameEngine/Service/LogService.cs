using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CruZ.Framework.Utility;

namespace CruZ.Framework.Service
{
    public class LogManager
    {
        public static string LogRoot = ".";

        public static void SetMsg(string theMsg, string msgKey = DefaultString, bool updateLogFile = false)
        {
            Main._msgs[msgKey] = theMsg;

            if (updateLogFile)
                FileHelper.WriteToFile(Path.Combine(LogRoot, msgKey + ".log"),
                    theMsg, false);
        }

        public static void AppendMsg(string theMsg, string msgKey = DefaultString, bool updateLogFile = false)
        {
            StringBuilder sb = new();

            var prevMsg = GetMsg(msgKey);

            if (!string.IsNullOrEmpty(prevMsg))
            {
                sb.Append(prevMsg);
                sb.Append(Environment.NewLine);
            }

            sb.Append(theMsg);

            SetMsg(sb.ToString(), msgKey, updateLogFile);
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
        static LogManager _main;
        static LogManager Main { get => _main ??= new LogManager(); }
        #endregion

        const string DefaultString = "Default";
    }
}