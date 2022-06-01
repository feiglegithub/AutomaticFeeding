using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NJIS.Ini;
using NJIS.PLC.Communication.Profinet.Siemens;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator
{
    public class PlcOperator//:SettingBase<PlcOperator>
    {

        
        private SiemensS7Net _siemensS7Net = null;//new SiemensS7Net(SiemensPLCS.S1500);
        private IPAddress _address = null;

        private bool _isConnect = false;
        public SiemensS7Net Siemens => _siemensS7Net??(_siemensS7Net= new SiemensS7Net(SiemensPLCS.S1500));
        private static PlcOperator _plcOperator = null;
        private static object objLocck = new object();
        private PlcOperator() { }
        public static PlcOperator GetInstance()
        {
            if (_plcOperator == null)
            {
                lock (objLocck)
                {
                    if (_plcOperator == null)
                    {
                        _plcOperator = new PlcOperator();
                    }
                }
            }

            return _plcOperator;
        }
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

            _siemensS7Net.IpAddress = ip;
            try
            {
                var connectResult = _siemensS7Net.ConnectServer();
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
            PLC.Communication.Core.Types.OperateResult operatedResult = _siemensS7Net.ConnectClose();
            return operatedResult;
        }

        public PLC.Communication.Core.Types.OperateResult Write(string addr, string content, ushort length)
        {
            return _siemensS7Net.Write(addr, content, length);

        }

        public PLC.Communication.Core.Types.OperateResult Write(string addr, int content)
        {
            return _siemensS7Net.Write(addr, content);
        }

        /// <summary>
        /// 读取string 失败返回null
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string ReadString(string addr, ushort length)
        {
            var result = _siemensS7Net.ReadString(addr, length);
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
            var result = _siemensS7Net.ReadInt32(addr);
            if (result.IsSuccess)
            {
                return result.Content;
            }

            return -1;
        }
    }
}
