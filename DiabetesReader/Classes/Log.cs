using DiabetesReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesReader.Classes.Logging
{
    public class Log
    {
        public enum levels
        {
            Info,
            Warning,
            Error
        }

        public string timeStamp = "";
        public string Text = "";
        public levels level = levels.Info;
        public string version = GlobalVars.VERSION;
        public int lineNumber = 0;
        public string fileName = "";
        public string function = "";
        public Log(levels level, string Text, int lineNumber, string function)
        {
            this.timeStamp = DateTime.Now.ToString("H:mm:ss dd-MM-yyyy");
            this.Text = Text;
            this.level = level;
            this.lineNumber = lineNumber;
            this.fileName = fileName;
            this.function = function;
        }
        /// <summary>
        /// Converts the Log Object to a string of log, for email ready
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string type = "";
            switch (level)
            {
                case levels.Info:
                    type = "Info";
                    break;
                case levels.Error:
                    type = "Error";
                    break;
                    case levels.Warning:
                    type = "Warning";
                    break;
            }


            return $"{timeStamp}: <{type}>: {Text}, on line {lineNumber}:{function}";

        }
    }
}
