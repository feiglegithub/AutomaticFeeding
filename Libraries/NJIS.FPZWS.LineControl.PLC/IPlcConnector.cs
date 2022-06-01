//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：IPlcConnector.cs
//   创建时间：2018-11-20 14:39
//   作    者：
//   说    明：
//   修改时间：2018-11-20 14:39
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

#endregion

using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.PLC
{
    public interface IPlcConnector
    {
        /// <summary>
        ///     PLC 连接器名称
        /// </summary>
        string Name { get; set; }


        /// <summary>
        ///     命令执行间隔
        /// </summary>
        int CommandExecutInterval { get; set; }

        /// <summary>
        ///     命令集合
        /// </summary>
        List<CommandBase> Commands { get; }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="plcType"></param>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <param name="timeOut"></param>
        /// <param name="receiveTimeOut"></param>
        /// <returns></returns>
        bool Init(string plcType, string address, int port = 102, int timeOut = 5000, int receiveTimeOut = 5000);

        /// <summary>
        ///     关闭PLC连接
        /// </summary>
        bool DisConnect();

        /// <summary>
        ///     打开PLC连接
        /// </summary>
        bool Connect();

        PlcConnectState GetConnectState();


        #region 写操作

        /// <summary>
        ///     读取PLC数据库块
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        bool WriteBytes(string address, byte[] bytes);

        bool WriteBool(string address, bool value);
        bool WriteShort(string address, short value);
        bool WriteInt(string address, int value);
        bool WriteInt16(string address, short value);
        bool WriteInt64(string address, long value);
        bool WriteReal(string address, float value);
        bool WriteByte(string address, byte value);
        bool WriteLong(string address, long value);
        bool WriteDouble(string address, double value);
        bool WriteString(string address, string value);

        #endregion

        #region 读操作

        /// <summary>
        ///     读取PLC数据库块
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        byte[] ReadBytes(string address, ushort length);

        bool ReadBool(string address);
        short ReadShort(string address);
        long ReadInt64(string address);
        int ReadInt(string address);
        short ReadInt16(string address);
        float ReadReal(string address);
        byte ReadByte(string address);
        long ReadLong(string address);
        double ReadDouble(string address);
        string ReadString(string address);
        string ReadString(string address, int length);

        #endregion
    }
}