using System;
using System.IO;
using System.Text;

namespace AutoFeedBackWMSService.Common
{
    public class LogWrite
    {
        static string log = @"LOG";
        static string error = @"Error";
        static string root = AppDomain.CurrentDomain.BaseDirectory;

        public static void WriteLog(string content)
        {
            var now = DateTime.Now;
            var log1 = $@"{root}{log}";

            if (!Directory.Exists(log1)) { Directory.CreateDirectory(log1); }

            var log2 = $@"{log1}\{now.ToString("yyyyMM")}";

            if (!Directory.Exists(log2)) { Directory.CreateDirectory(log2); }

            var log3 = $@"{log2}\{now.ToString("dd")}.txt";

            var sw = new StreamWriter(log3, true);
            sw.WriteLine($"【{now.ToString()}】{content}");
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        public static void WriteError(string content)
        {
            var now = DateTime.Now;
            var error1 = $@"{root}{error}";

            if (!Directory.Exists(error1)) { Directory.CreateDirectory(error1); }

            var error2 = $@"{error1}\{now.ToString("yyyyMM")}";

            if (!Directory.Exists(error2)) { Directory.CreateDirectory(error2); }

            var error3 = $@"{error2}\{now.ToString("dd")}.txt";

            var sw = new StreamWriter(error3, true);
            sw.WriteLine($"【{now.ToString()}】{content}");
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
    }
}
