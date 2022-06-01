//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Siemens.S7Net
//   文 件 名：SiemensTcpNetConnector.cs
//   创建时间：2018-11-23 17:14
//   作    者：
//   说    明：
//   修改时间：2018-11-23 17:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using NJIS.PLC.Communication.Profinet.Siemens;

namespace NJIS.FPZWS.LineControl.PLC.Siemens.S7Net
{
    public class SiemensTcpNetConnector : PlcConnectorBase
    {
        private SiemensS7Net _siemensTcpNet;

        public PlcConnectState PlcConnectState { get; set; }

        public override bool Init(string plcType, string address, int port = 102, int timeOut = 5000,
            int receiveTimeOut = 5000)
        {
            SiemensPLCS pt;
            if (Enum.TryParse(plcType, out pt))
            {
                _siemensTcpNet = new SiemensS7Net(pt, address)
                {
                    ConnectTimeOut = timeOut,
                    ReceiveTimeOut = receiveTimeOut,
                    Port = port
                };
                _siemensTcpNet.LogNet = new InternalLogNet();
                PlcConnectState = PlcConnectState.Disconnect;
            }

            return true;
        }

        public override bool DisConnect()
        {
            var ops = _siemensTcpNet.ConnectClose();
            if (ops.IsSuccess)
            {
                PlcConnectState = PlcConnectState.Disconnect;
                _siemensTcpNet.LogNet.WriteInfo($"Disconnect PLC:{_siemensTcpNet.IpAddress} connection");
            }

            return ops.IsSuccess;
        }

        public override bool Connect()
        {
            var ops = _siemensTcpNet.ConnectServer();
            if (ops.IsSuccess)
            {
                PlcConnectState = PlcConnectState.Connected;
                _siemensTcpNet.LogNet.WriteInfo($"Connect PLC:{_siemensTcpNet.IpAddress} connection");
            }

            return ops.IsSuccess;
        }

        public override PlcConnectState GetConnectState()
        {
            return PlcConnectState;
        }

        public override bool WriteBytes(string address, byte[] bytes)
        {
            var ops = _siemensTcpNet.Write(address, bytes);
            return ops.IsSuccess;
        }

        public override bool WriteBool(string address, bool value)
        {
            var ops = _siemensTcpNet.Write(address, value);
            return ops.IsSuccess;
        }

        public override bool WriteShort(string address, short value)
        {
            var ops = _siemensTcpNet.Write(address, value);
            return ops.IsSuccess;
        }

        public override bool WriteInt(string address, int value)
        {
            var ops = _siemensTcpNet.Write(address, value);
            return ops.IsSuccess;
        }

        public override bool WriteInt16(string address, short value)
        {
            var ops = _siemensTcpNet.Write(address, value);
            return ops.IsSuccess;
        }

        public override bool WriteInt64(string address, long value)
        {
            var ops = _siemensTcpNet.Write(address, value);
            return ops.IsSuccess;
        }

        public override bool WriteReal(string address, float value)
        {
            var ops = _siemensTcpNet.Write(address, value);
            return ops.IsSuccess;
        }

        public override bool WriteByte(string address, byte value)
        {
            var ops = _siemensTcpNet.Write(address, value);
            return ops.IsSuccess;
        }

        public override bool WriteLong(string address, long value)
        {
            var ops = _siemensTcpNet.Write(address, value);
            return ops.IsSuccess;
        }

        public override bool WriteDouble(string address, double value)
        {
            var ops = _siemensTcpNet.Write(address, value);
            return ops.IsSuccess;
        }

        public override bool WriteString(string address, string value)
        {
            var ops = _siemensTcpNet.Write(address, value);
            return ops.IsSuccess;
        }

        public override bool ReadBool(string address)
        {
            var ops = _siemensTcpNet.ReadBool(address);
            return ops.Content;
        }

        public override short ReadShort(string address)
        {
            var ops = _siemensTcpNet.ReadInt16(address);
            return ops.Content;
        }

        public override long ReadInt64(string address)
        {
            var ops = _siemensTcpNet.ReadInt64(address);
            return ops.Content;
        }

        public override int ReadInt(string address)
        {
            var ops = _siemensTcpNet.ReadInt32(address);
            return ops.Content;
        }

        public override short ReadInt16(string address)
        {
            var ops = _siemensTcpNet.ReadInt16(address);
            return ops.Content;
        }

        public override float ReadReal(string address)
        {
            var ops = _siemensTcpNet.ReadInt32(address);
            return ops.Content;
        }

        public override byte ReadByte(string address)
        {
            var ops = _siemensTcpNet.ReadByte(address);
            return ops.Content;
        }

        public override long ReadLong(string address)
        {
            var ops = _siemensTcpNet.ReadInt16(address);
            return ops.Content;
        }

        public override double ReadDouble(string address)
        {
            var ops = _siemensTcpNet.ReadDouble(address);
            return ops.Content;
        }


        public override string ReadString(string address)
        {
            var ops = _siemensTcpNet.ReadString(address);
            return ops.Content;
        }

        public override string ReadString(string address, int length)
        {
            var ops = _siemensTcpNet.ReadString(address, (ushort) length);
            return ops.Content;
        }

        public override byte[] ReadBytes(string address, ushort length)
        {
            var ops = _siemensTcpNet.ReadBytes(address, length);
            return ops.Content;
        }
    }
}