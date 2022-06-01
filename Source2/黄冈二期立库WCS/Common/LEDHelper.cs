using LYLedControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.Common
{
    public class LEDHelper
    {
        static string initText= "                                                                                                                                                                            ";
        public static bool Send(short deviceId, string ip, string content, short port)
        {
            var rlt1 = MiniLedCenter.sendTextToLED(deviceId, ip, initText, port);  //先清空旧的文本
            var rlt2 = MiniLedCenter.sendTextToLED(deviceId, ip, content, port);  //再发送新的文本
            return rlt1 && rlt2;
        }
    }
}
