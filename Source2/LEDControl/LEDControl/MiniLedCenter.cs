using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LEDControl
{
    public class MiniLedCenter
    {
        /// <summary>
        /// 屏的编号(任意值)
        /// </summary>
        public short m_ledId = 1;
        /// <summary>
        /// 登录密码(默认空)
        /// </summary>
        public string m_ledPassWord = string.Empty;
        /// <summary>
        /// led屏幕IP地址
        /// </summary>
        public string m_ledRemoteIP = string.Empty;
        /// <summary>
        /// led屏幕端口
        /// </summary>
        public short m_ledUDPPort;
        /// <summary>
        /// 超时时间
        /// </summary>
        public int m_ledTimeOut = 2;
        /// <summary>
        /// 重连次数
        /// </summary>
        public int m_ledRetries = 2;

        public short m_ledPicFIndex = 5;
        public short m_ledPicWidth = 192;
        public short m_ledPicHeight = 96;
        public short m_ledColor = 1;
        public Byte m_ledEncode = 2;
        public Byte m_ledMode = 2;

        /// <summary>
        /// 初始化连接
        /// </summary>
        /// <param name="ledRemoteIP"></param>
        /// <param name="ledPort"></param>
        /// <returns></returns>
        public bool LED_MC_NetInitial(string ledRemoteIP, short ledPort)
        {
            m_ledRemoteIP = ledRemoteIP;
            m_ledUDPPort = ledPort;
            try
            {
                int result = MiniLED.MC_NetInitial(m_ledId, m_ledPassWord, m_ledRemoteIP, m_ledTimeOut, m_ledRetries, m_ledUDPPort);
                if (result != 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// 发送给LED屏的信息
        /// </summary>
        /// <param name="str">发送的字段</param>
        /// <returns>成功为true</returns>
        public bool LED_MC_TxtToXMPXFile(string str)
        {
            try
            {
                var result = MiniLED.MC_TxtToXMPXFile(m_ledId, m_ledPicFIndex, m_ledPicWidth, m_ledPicHeight, m_ledColor, str, m_ledEncode, m_ledMode);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送给LED屏的信息
        /// </summary>
        /// <param name="str">发送的字段</param>
        /// <returns>成功为true</returns>
        public bool LED_MC_ShowString(string str)
        {
            try
            {
                var result = MiniLED.MC_ShowString(m_ledId, 0, 0, m_ledPicWidth, m_ledPicHeight, 30, 50, m_ledColor, str, m_ledMode);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送给LED屏的信息
        /// </summary>
        /// <param name="str">发送的字段,分三行显示</param>
        /// <returns>成功为true</returns>
        public bool LED_MC_ShowString(string str1, string str2, string str3)
        {
            try
            {
                var result = MiniLED.MC_ShowString(m_ledId, 0, 0, m_ledPicWidth, m_ledPicHeight, 30, 30, m_ledColor, str1, m_ledMode);
                if (result)
                    result = MiniLED.MC_ShowString(m_ledId, 0, 0, m_ledPicWidth, m_ledPicHeight, 30, 50, m_ledColor, str2, m_ledMode);
                if (result)
                    result = MiniLED.MC_ShowString(m_ledId, 0, 0, m_ledPicWidth, m_ledPicHeight, 30, 70, m_ledColor, str3, m_ledMode);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="str">发送的字段</param>
        /// <returns>成功为true</returns>
        public bool LED_MC_GetClock(string str)
        {
            try
            {
                string aa = string.Empty;
                var result = MiniLED.MC_GetClock(m_ledId, ref aa);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
