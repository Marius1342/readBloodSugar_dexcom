using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using diabetesApp.Classes.Logging;
namespace diabetesApp.Classes
{
    public static class Logger
    {
         //All data
         private static List<Log> text = new List<Log>();

        private const int MAX_SIZE = 50;

        public static int LogCount { get; internal set; }

        private static void AddLog(Log log)
        {
            if(text.Count> MAX_SIZE)
            {
                text.RemoveAt(0);
            }
            text.Add(log);

            LogCount = text.Count;
        }

        public static void Log(string Text, [CallerMemberName] string memberName = "",
[CallerLineNumber] int lineNumber = 0)
        {
            Log log = new Log(Logging.Log.levels.Info, Text, lineNumber, 
              memberName);
            AddLog(log);
        }
        public static void Warning(string Text, [CallerMemberName] string memberName = "",
[CallerLineNumber] int lineNumber = 0)
        {
            Log log = new Log(Logging.Log.levels.Warning, Text, lineNumber, memberName);
            AddLog(log);
        }
        public static void Error(string Text, [CallerMemberName] string memberName = "",
[CallerLineNumber] int lineNumber = 0)
        { 
            Log log = new Log(Logging.Log.levels.Error, Text, lineNumber, memberName);
            AddLog(log);
        }


        public static string GetReportForEmail()
        {
            string data = "";
            foreach (Log line in text)
            {
                data+= line.ToString() + Environment.NewLine;
            }
            return data;
        }

    }
}
