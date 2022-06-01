using NJIS.PLC.Communication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.LocalServices;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Helpers
{
    public class PlcOperatorHelper:Singleton<PlcOperatorHelper>
    {
        private SiemensS7Net _siemensS7Net = null;//new SiemensS7Net(SiemensPLCS.S1500);
        private IPAddress _address = null;

        private bool _isConnect = false;
        public SiemensS7Net Siemens => _siemensS7Net ?? (_siemensS7Net = new SiemensS7Net(SiemensPLCS.S1500));
        //private static PlcOperatorHelper _plcOperator = null;
        private static object objLocck = new object();
        private PlcOperatorHelper() { }

        public bool CheckConnect()
        {
            return (_address != null && _isConnect);

        }

        public bool Connect(string ip)
        {
            if (!IPAddress.TryParse(ip, out _address))
            {
                return false;
            }

            Siemens.IpAddress = ip;
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

        public PLC.Communication.Core.Types.OperateResult Write(string addr, string content)
        {
            return Siemens.Write(addr, content);


        }

        public PLC.Communication.Core.Types.OperateResult Write(string addr, int content)
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
        /// 读取int 失败返回-1
        /// </summary>
        /// <param name="addr">plc地址</param>
        /// <returns></returns>
        public int ReadLong(string addr)
        {
            var result = Siemens.ReadInt32(addr);
            if (result.IsSuccess)
            {
                return result.Content;
            }

            return -1;
        }
    }
}
