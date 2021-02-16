using System;
using System.Configuration;
using System.IO;

namespace Akgul_Yemek.Code
{
    public class MLog
    {
        private MLog()
        {
        }

        private static string _serverMappath;

        public static void SetLogPath(string path)
        {
            _serverMappath = path;
        }

        public static string GetLogDirectory()
        {
            var logPath = Convert.ToString(ConfigurationManager.AppSettings["relative_loging_path"]);
            return _serverMappath + "/" + logPath;
        }

        private static int WriteLog(string type, string caption, string message)
        {
            try
            {
                var logPath = Convert.ToString(ConfigurationManager.AppSettings["relative_loging_path"]);
                var loggingIsActive = Convert.ToBoolean(ConfigurationManager.AppSettings["logging_is_active"]);

                if (!loggingIsActive || logPath == "")
                    return 0;

                message = message.Replace(Environment.NewLine, " <br /> ");

                string logFilename = _serverMappath + "/" + logPath + "/" + DateTime.Now.ToShortDateString() + ".log";
                string log = DateTime.Now + " # " + type + " # " + caption + " # " + message;

                using (StreamWriter writer = new StreamWriter(logFilename, true))
                {
                    writer.WriteLine(log);
                    writer.Close();
                }

                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public static int Error(string caption, string message)
        {
            const string logType = "ERROR  ";
            return WriteLog(logType, caption, message);
        }
        public static int Debug(string caption, string message)
        {
            const string logType = "Debug  ";
            return WriteLog(logType, caption, message);
        }
        public static int Info(string caption, string message)
        {
            const string logType = "Info   ";
            return WriteLog(logType, caption, message);
        }
        public static int Fatal(string caption, string message)
        {
            const string logType = "FATAL  ";
            return WriteLog(logType, caption, message);
        }
        public static int Warning(string caption, string message)
        {
            const string logType = "Warning";
            return WriteLog(logType, caption, message);
        }
        public static int Success(string caption, string message)
        {
            const string logType = "Success";
            return WriteLog(logType, caption, message);
        }

    }
}