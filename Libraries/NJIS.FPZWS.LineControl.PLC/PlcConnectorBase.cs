//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：PlcConnectorBase.cs
//   创建时间：2018-11-20 14:51
//   作    者：
//   说    明：
//   修改时间：2018-11-20 14:51
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.PLC
{
    public abstract class PlcConnectorBase : IPlcConnector
    {
        public PlcConnectorBase()
        {
            Commands = new List<CommandBase>();
        }

        public string Name { get; set; }
        public int CommandExecutInterval { get; set; } = 10;
        public List<CommandBase> Commands { get; protected set; }

        public abstract bool Init(string plcType, string address, int port = 102, int timeOut = 5000,
            int receiveTimeOut = 5000);

        public abstract bool DisConnect();

        public abstract bool Connect();

        public abstract PlcConnectState GetConnectState();
        public abstract bool WriteBytes(string address, byte[] bytes);

        public abstract bool WriteBool(string address, bool value);

        public abstract bool WriteShort(string address, short value);

        public abstract bool WriteInt(string address, int value);
        public abstract bool WriteInt16(string address, short value);

        public abstract bool WriteInt64(string address, long value);

        public abstract bool WriteReal(string address, float value);
        public abstract bool WriteByte(string address, byte value);

        public abstract bool WriteLong(string address, long value);

        public abstract bool WriteDouble(string address, double value);
        public abstract bool WriteString(string address, string value);

        public abstract bool ReadBool(string address);

        public abstract short ReadShort(string address);
        public abstract long ReadInt64(string address);

        public abstract int ReadInt(string address);
        public abstract short ReadInt16(string address);

        public abstract float ReadReal(string address);
        public abstract byte ReadByte(string address);

        public abstract long ReadLong(string address);

        public abstract double ReadDouble(string address);

        public abstract string ReadString(string address);
        public abstract string ReadString(string address, int length);

        public abstract byte[] ReadBytes(string address, ushort length);
    }
}