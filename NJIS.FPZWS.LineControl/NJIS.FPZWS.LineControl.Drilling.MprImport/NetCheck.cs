//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.MprImport
//   文 件 名：NetCheck.cs
//   创建时间：2019-07-25 9:26
//   作    者：
//   说    明：
//   修改时间：2019-07-25 9:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace NJIS.FPZWS.LineControl.Drilling.MprImport
{
    public class NetCheck
    {
        #region 网络检测

        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(int description, int reservedValue);

        #region 方法一

        /// <summary>
        ///     用于检查网络是否可以连接互联网,true表示连接成功,false表示连接失败
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectInternet()
        {
            var Description = 0;
            return InternetGetConnectedState(Description, 0);
        }

        #endregion 方法一

        #region 方法二

        /// <summary>
        ///     用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令),true表示Ping成功,false表示Ping失败
        /// </summary>
        /// <param name="strIpOrDName">输入参数,表示IP地址或域名,多个可用','分隔</param>
        /// <param name="timeOut"></param>
        /// <param name="cnt">尝试次数</param>
        /// <returns></returns>
        public static bool PingIpOrDomainName(string strIpOrDName, int timeOut = 120, int cnt = 1)
        {
            var rst = true;
            if (cnt <= 0) return false;
            while (cnt-- > 0)
            {
                var ips = strIpOrDName.Split(',');
                foreach (var ip in ips)
                {
                    try
                    {
                        var objPingSender = new Ping();
                        var objPinOptions = new PingOptions();
                        objPinOptions.DontFragment = true;
                        var data = "test";
                        var buffer = Encoding.UTF8.GetBytes(data);
                        var objPinReply = objPingSender.Send(ip, timeOut, buffer, objPinOptions);
                        var strInfo = objPinReply.Status.ToString();
                        if (strInfo != "Success")
                        {
                            rst = false;
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }

            return rst;
        }

        #endregion 方法二

        #endregion
    }
}