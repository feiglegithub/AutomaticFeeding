using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.UI.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using NJIS.PLC.Communication.Profinet.Siemens;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator
{
    public class MainFormPresenter:PresenterBase
    {

        private SiemensS7Net _siemensS7Net = null;
        private  SiemensS7Net siemensS7Net =>_siemensS7Net??(_siemensS7Net= PlcOperator.GetInstance().Siemens) ;

        private static MainFormPresenter _mainFormPresenter = null;
        private static object objLock = new object();
        private const int PartIdLength = 20;
        public static MainFormPresenter GetInstance()
        {
            if (_mainFormPresenter == null)
            {
                lock (objLock)
                {
                    if(_mainFormPresenter==null)
                    {
                        _mainFormPresenter = new MainFormPresenter();
                    };
                }
            }

            return _mainFormPresenter;
        }

        public const string WritePartId1 = nameof(WritePartId1);
        public const string WritePartId = nameof(WritePartId);
        public const string ReadPartId1 = nameof(ReadPartId1);
        public const string ReadPartId = nameof(ReadPartId);
        public const string Connect = nameof(Connect);
        public const string Close = nameof(Close);

        public const string BeginReadPlc = nameof(BeginReadPlc);

        private IPAddress _address = null;
        private Dictionary<int, PartInfoPositionArgs> _dicPartPositions = null;
        private readonly Dictionary<int, string> _dicDbAddress = new Dictionary<int, string>();
        private readonly List<PartInfoPositionArgs> _partInfoPositionArgses = new List<PartInfoPositionArgs>()
        {
            new PartInfoPositionArgs(){Position = 10},
            new PartInfoPositionArgs(){Position = 11},
            new PartInfoPositionArgs(){Position = 12},
            new PartInfoPositionArgs(){Position = 13},
            new PartInfoPositionArgs(){Position = 14},
            new PartInfoPositionArgs(){Position = 15},
            new PartInfoPositionArgs(){Position = 16},
            new PartInfoPositionArgs(){Position = 17},
            new PartInfoPositionArgs(){Position = 18},
            new PartInfoPositionArgs(){Position = 19},

            new PartInfoPositionArgs(){Position = 20},
            new PartInfoPositionArgs(){Position = 21},
            new PartInfoPositionArgs(){Position = 22},
            new PartInfoPositionArgs(){Position = 23},
            new PartInfoPositionArgs(){Position = 24},
            new PartInfoPositionArgs(){Position = 25},

            new PartInfoPositionArgs(){Position = 30},
            new PartInfoPositionArgs(){Position = 31},
            new PartInfoPositionArgs(){Position = 32},
            new PartInfoPositionArgs(){Position = 33},
            new PartInfoPositionArgs(){Position = 34},
            new PartInfoPositionArgs(){Position = 35},
            new PartInfoPositionArgs(){Position = 36},
            new PartInfoPositionArgs(){Position = 37},
            new PartInfoPositionArgs(){Position = 38},
            new PartInfoPositionArgs(){Position = 39},

            new PartInfoPositionArgs(){Position = 40},
            new PartInfoPositionArgs(){Position = 41},
            
        };

        private Thread _th = null;


        private bool _isConnect = false;
        private MainFormPresenter()
        {
            Register();
            
        }

        private void Register()
        {
            
            Register<string>(Connect,ExecuteConnectPlc);
            Register<string>(Close, ExecuteCloseConnect);
            Register<string>(BeginReadPlc, ExecuteBeginReadPlc);
            Register<Tuple<string, int>>(WritePartId, ExecuteWritePartId);
            Register<int>(ReadPartId, ExecuteReadPartId);


            _dicPartPositions = _partInfoPositionArgses.ToDictionary(item => item.Position);
            int startAddr = 10000;
            foreach (var item in _dicPartPositions)
            {
                _dicDbAddress.Add(item.Key,string.Format("DB1.{0}", startAddr+=20));
            }
        }

        private void ReadPositions()
        {
            while (true)
            {
                foreach (var item in _dicDbAddress)
                {
                    var result = siemensS7Net.ReadString(item.Value, PartIdLength);
                    _dicPartPositions[item.Key].PartId = result.Content;
                    _dicPartPositions[item.Key].Time = DateTime.Now;
                }
                MqttManager.Current.Publish(EmqttSettings.Current.PcsPartInfoPositionRep, _dicPartPositions.Values.ToList());
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void ExecuteBeginReadPlc(string str = "")
        {
            _th = new Thread(ReadPositions);
            _th.IsBackground = true;
            _th.Start();
        }

        private void ExecuteWritePartId(object sender, Tuple<string, int> tuple)
        {
            var recipient = sender;
            if (CheckConnect())
            {
                string addr = _dicDbAddress[tuple.Item2];
                string partId = tuple.Item1;
                var result = siemensS7Net.Write(addr, "", PartIdLength);
                result = siemensS7Net.Write(addr, partId);
                if (result.IsSuccess)
                {
                    SendTipsMessage($"条码:{partId} 写入地址：{addr} 成功", recipient);
                }
            }
        }

        private void ExecuteReadPartId(object sender,int position)
        {
            object recipient = sender;
            if (CheckConnect())
            {
                var result = siemensS7Net.ReadString("DB"+position+".2", PartIdLength);
                SendTipsMessage(result.Content, recipient);
                Send(ReadPlcControl.ReceivePartId, recipient, result.Content);
            }
            //if (CheckConnect())
            //{
            //    string addr = _dicDbAddress[position];
            //    var result = _siemensS7Net.ReadString(addr, PartIdLength);
            //    SendTipsMessage(result.Content,recipient);
            //    Send(ReadPlcControl.ReceivePartId, recipient, result.Content);
            //}
        }

        private bool CheckConnect()
        {
            return (_address != null && _isConnect);

        }

        private void ExecuteConnectPlc(object sender, string ip)
        {
            var recipient = sender;
            if (!IPAddress.TryParse(ip, out _address))
            {
                SendTipsMessage("Ip地址输入失败！", recipient);
                return;
            }

            siemensS7Net.IpAddress = ip;
            try
            {
                var connectResult = siemensS7Net.ConnectServer();
                if (connectResult.IsSuccess)
                {
                    SendTipsMessage("连接成功！", recipient);
                    _isConnect = true;
                    return;
                }

                SendTipsMessage("连接失败！", recipient);
                _isConnect = false;
            }
            catch (Exception e)
            {
                SendTipsMessage("连接失败！", recipient);
                _isConnect = false;
            }
            finally
            {
                Send(MainForm.ConnectResult, recipient, _isConnect);
            }

        }

        private void ExecuteCloseConnect(object sender,string str = "")
        {
            object recipient = sender;
            var operatedResult = siemensS7Net.ConnectClose();
            if (operatedResult.IsSuccess)
            {
                SendTipsMessage("断开成功！", recipient);
            }
            else
            {
                SendTipsMessage("Error:断开失败！"+operatedResult.Message, recipient);
            }
            Send(MainForm.CloseResult,operatedResult.IsSuccess);

        }
    }
}
