using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WCS.Common
{
    public class LogHelper
    {
        private static string sort_folder = System.Environment.CurrentDirectory + @"\SortLog";
        private static string system_folder = System.Environment.CurrentDirectory + @"\SystemLog";
        private static string error_foler = System.Environment.CurrentDirectory + @"ErrorLog";
        private static string file_name = "";

        public static void LogSortInfo(string content)
        {
            content = $"【{DateTime.Now.ToString()}】{content}";
            if (!Directory.Exists(sort_folder))
            {
                Directory.CreateDirectory(sort_folder);
            }
            file_name = sort_folder + $@"\{DateTime.Now.ToString("yyyyMMdd")}.txt";
            if (!File.Exists(file_name))
            {
                File.Create(file_name).Dispose();
            }
            Write(file_name, content);
        }

        public static void LogErrorInfo(string content)
        {
            content = $"【{DateTime.Now.ToString()}】{content}";
            if (!Directory.Exists(error_foler))
            {
                Directory.CreateDirectory(error_foler);
            }
            file_name = error_foler + $@"\{DateTime.Now.ToString("yyyyMMdd")}.txt";
            if (!File.Exists(file_name))
            {
                File.Create(file_name).Dispose();
            }
            Write(file_name, content);
        }

        public static void LogSystemInfo(string content)
        {
            content = $"【{DateTime.Now.ToString()}】{content}";
            if (!Directory.Exists(system_folder))
            {
                Directory.CreateDirectory(system_folder);
            }
            file_name = system_folder + $@"\{DateTime.Now.ToString("yyyyMMdd")}.txt";
            if (!File.Exists(file_name))
            {
                File.Create(file_name).Dispose();
            }
            Write(file_name, content);
        }

        static void Write(string File, string content)
        {
            FileStream fs = new FileStream(File, FileMode.Append);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(content);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
            fs.Dispose();
        }
    }
}
