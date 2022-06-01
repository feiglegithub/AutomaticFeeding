//  ************************************************************************************
//   解决方案：NJIS.RFID.HBSEPD
//   项目名称：NJIS.RFID.HBSEPD
//   文 件 名：RfidManager.cs
//   创建时间：2019-05-02 9:22
//   作    者：
//   说    明：
//   修改时间：2019-05-02 9:22
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace NJIS.RFID.HBSEPD
{
    public class RfidManager
    {
        public static List<IRfider> Rfiders { get; private set; }

        static RfidManager()
        {
            Rfiders = new List<IRfider>();
        }

        public void RegisterRfider(IRfider rfider)
        {
            Rfiders.Add(rfider);
        }

        /// <summary>
        /// 断开所有连接
        /// </summary>
        /// <returns></returns>
        public bool DisConnect()
        {
            foreach (var rfider in Rfiders)
            {
                if (rfider.IsConnected)
                {
                    rfider.DisConnect();
                }
            }

            return true;
        }

        /// <summary>
        /// 断开指定RFID连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DisConnect(string id)
        {
            var rfider = Rfiders.FirstOrDefault(m => m.Id == id);
            if (rfider == null) return false;
            if (rfider.IsConnected)
            {
                rfider.DisConnect();
            }
            return true;
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ReadString(string id)
        {
            var rfider = Rfiders.FirstOrDefault(m => m.Id == id);
            if (rfider == null) return "";
            if (!rfider.IsConnected)
            {
                rfider.Connect();
            }

            if (rfider.IsConnected)
            {
                return rfider.ReadString();
            }

            return "";
        }

        /// <summary>
        /// 读取字符串数组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string[] ReadStrings(string id)
        {
            var rfider = Rfiders.FirstOrDefault(m => m.Id == id);
            if (rfider == null) return null;
            if (!rfider.IsConnected)
            {
                rfider.Connect();
            }
            if (rfider.IsConnected)
            {
                return rfider.ReadStrings();
            }
            return null;
        }

        #region Singleton  单例模式

        public static RfidManager Current => Nested.Instance;

        private sealed class Nested
        {
            internal static readonly RfidManager Instance = (RfidManager)Activator.CreateInstance(typeof(RfidManager), true);

            static Nested()
            {
            }
        }

        #endregion
    }
}