using NJIS.PLC.Communication.Profinet.Siemens;
using System;
using System.Net;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.CommunicationBase.Plc
{
    public class PlcOperator
    {

        
        private SiemensS7Net _siemensS7Net = null;
        private IPAddress _address = null;
        private string ip = null;
        private bool _isConnect = false;
        public SiemensS7Net Siemens { get; private set; } 
        public PlcOperator(string ip, SiemensPLCS siemensPlc =SiemensPLCS.S1200)
        {
            Siemens = new SiemensS7Net(siemensPlc);
            this.ip = ip;
        }
        public bool CheckConnect()
        {
            return (_address != null && _isConnect);

        }

        public bool Connect(string ip=null)
        {
            if (!IPAddress.TryParse(ip==null?this.ip:ip, out _address))
            {
                return false;
            }

            Siemens.IpAddress = this.ip;
            try
            {
                var connectResult = Siemens.ConnectServer();
                if (connectResult.IsSuccess)
                {
                    
                    _isConnect = true;
                    return true;
                }
                _isConnect = false;
            }
            catch (Exception e)
            {
                _isConnect = false;
                return false;
            }

            return _isConnect;
        }

        public PLC.Communication.Core.Types.OperateResult Close()
        {
            PLC.Communication.Core.Types.OperateResult operatedResult = Siemens.ConnectClose();
            return operatedResult;
        }

        public PLC.Communication.Core.Types.OperateResult Write(string addr, string content, ushort length)
        {
            return Siemens.Write(addr, content, length);

        }

        public PLC.Communication.Core.Types.OperateResult Write(string addr, int content)
        {
            return Siemens.Write(addr, content);
        }
        public PLC.Communication.Core.Types.OperateResult Write(string addr, short content)
        {
            return Siemens.Write(addr, content);
        }

        /// <summary>
        /// 读取string 失败返回null
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string ReadString(string addr, ushort length)
        {
            var result = Siemens.ReadString(addr, length);
            if (result.IsSuccess)
            {
                return result.Content;
            }

            return null;
        }

        /// <summary>
        /// 读取string 失败返回null
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public string ReadString(string addr)
        {
            var result = Siemens.ReadString(addr);
            if (result.IsSuccess)
            {
                return result.Content;
            }

            return null;
        }

        public bool WriteString(string addr,string value)
        {
            var result = Siemens.Write(addr,value);
            return result.IsSuccess;
        }

        /// <summary>
        /// 读取int 失败返回-1
        /// </summary>
        /// <param name="addr">plc地址</param>
        /// <returns></returns>
        public short ReadShort(string addr)
        {
            var result = Siemens.ReadInt16(addr);
            if (result.IsSuccess)
            {
                return result.Content;
            }
            return -1;
        }

        public int ReadInt(string addr)
        {
            var result = Siemens.ReadInt32(addr);
            if (result.IsSuccess)
            {
                return result.Content;
            }
            return -1;
        }

        public bool ReadBoolean(string addr)
        {
            var result = Siemens.ReadBool(addr);
            if (result.IsSuccess)
            {
                return result.Content;
            }
            return false;
        }

        public PLC.Communication.Core.Types.OperateResult Write(string addr, bool value)
        {
            var result = Siemens.Write(addr,value);
            return result;
        }
    }
}
