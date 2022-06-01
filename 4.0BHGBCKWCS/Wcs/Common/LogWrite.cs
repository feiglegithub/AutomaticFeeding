using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
namespace WCS
{
    public class LogWrite
    {
        //public void WriteLog(string filePath, string content)
        //{
        //    try
        //    {
        //        string FilePath = "D:\\Log\\" + DateTime.Now.ToString("yyyyMMdd") + filePath;

        //        if (File.Exists(FilePath))
        //        {
        //            FileStream aFile = new FileStream(FilePath, FileMode.Append);
        //            StreamWriter sw = new StreamWriter(aFile, System.Text.Encoding.UTF8);
        //            sw.WriteLine(DateTime.Now.ToLongTimeString()+": " +content);
        //            sw.Close();
        //        }
        //        else//文件不存在
        //        {
        //            FileStream aFile = new FileStream(FilePath, FileMode.Create);
        //            StreamWriter sw = new StreamWriter(aFile, System.Text.Encoding.UTF8);
        //            sw.WriteLine(DateTime.Now.ToLongTimeString() + ": " + content);
        //            sw.Close();
        //        }
        //    }
        //    catch { }
        //}

        //public delegate void ErrorLogDelegate(string log);
        //public static ErrorLogDelegate ErrorLog;
        //public static bool WriteError(string content)
        //{
        //    if (ErrorLog != null)
        //        ErrorLog(content);
        //    return true;
        //}


        //public delegate void WriteLogDelegate(string log);  //定义一种方法的类型  void F(string)
        //public static WriteLogDelegate WriteLogBack;      //使用这种类型申明一个静态对象，就是一个方法
        //public static bool WriteLogToMain(string content)
        //{
        //    if (WriteLogBack != null)
        //        WriteLogBack(content);  //执行方法， 但是没有方法体，留给调用者具体实现方法
        //    return true;
        //}

    }
}
