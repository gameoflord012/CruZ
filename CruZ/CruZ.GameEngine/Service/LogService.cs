using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CruZ.GameEngine.Utility
{
    public class LogManager
    {
        public static string LogRoot = ".";

        public static void SetMsg(string theMsg, string msgKey = DefaultString, bool flushToDebug = false, bool updateLogFile = false)
        {
            _msgs[msgKey] = theMsg;

            if (updateLogFile)
                FileHelper.WriteToFile(Path.Combine(LogRoot, msgKey + ".log"),
                    theMsg, false);

            if(flushToDebug) Debug.WriteLine(theMsg, msgKey);
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
            if (!_msgs.ContainsKey(key)) return "";
            return _msgs[key];
        }

        public static string GetMsgFormmated(string key = DefaultString)
        {
            return $"{key} : {GetMsg(key)}";
        }

        static Dictionary<string, string> _msgs = new();

        const string DefaultString = "Default";
    }
}