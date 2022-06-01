using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnifiedAutomation.UaClient;
using UnifiedAutomation.UaBase;
using System.Threading;
using System.Net.NetworkInformation;
using WCS.Common;
using WCS.DataBase;

namespace WCS
{
    class OpcBaseManage
    {
        static string Path = System.Windows.Forms.Application.StartupPath + "\\Config\\";
        static int CpuNumber = 0;

        public static OpcBaseManage opcBase = new OpcBaseManage();

        List<Cpu> listCpu = new List<Cpu>();
        List<Item> listItem = new List<Item>();

        List<Thread> listThread = new List<Thread>();
        string cpuName = "CpuConnect" ;

        public OpcBaseManage()
        {
            InitCpu();
            InitItem(CpuNumber);
        }

        public Item GetItem(int cpuNo, int itemNo)
        {
            lock (this)
            {
                return listItem.Find(
                                delegate(Item item)
                                {
                                    return item.cpu.cpuNo == cpuNo && item.itemNo == itemNo;
                                });
            }
        }

        public Cpu GetCpu(int cpuNo)
        {
            lock (this)
            {
                return listCpu.Find(
                                delegate(Cpu cpu)
                                {
                                    return cpu.cpuNo == cpuNo;
                                });
            }
        }

        void InitCpu()
        {
            try
            {
                string[] StrList = File.ReadAllLines(Path + "Config_CPU.ini");
                foreach (string s in StrList)
                {
                    if (string.IsNullOrEmpty(s)) { continue; }

                    string[] ss = s.Split(';');

                    Cpu cpu = new Cpu();
                    cpu.cpuNo = CpuNumber;
                    cpu.isConnect = false;
                    cpu.nameSpaceIndex = 0;
                    cpu.cpuServerUrl= ss[1];
                    cpu.namespaceUrl = ss[2];
                    cpu.cpuIp = ss[3];
                    cpu.session = null;
                    cpu.connectState = ServerConnectionStatus.Disconnected;
                    listCpu.Add(cpu);

                    CpuNumber++;

                    //Thread th = new Thread(new ParameterizedThreadStart(ThreadConnectCpu));
                    //th.Name = cpuName + cpu.cpuNo;
                    //th.IsBackground = true;
                    //th.Start(cpu);
                    //listThread.Add(th);
                }

                Thread th = new Thread(new ThreadStart(ThreadConnectCpu));
                th.Name = cpuName;
                th.IsBackground = true;
                th.Start();
            }
            catch
            {
                throw new Exception("初始化CPU配置失败！");
            }
        }

        ApplicationInstance GetLicense()
        {
            ApplicationLicenseManager.AddProcessLicenses(System.Reflection.Assembly.GetExecutingAssembly(), "Config.License.lic");
            ApplicationInstance.Default.AutoCreateCertificate = true;
            return ApplicationInstance.Default;
        }

        void ThreadConnectCpu()
        {
            ApplicationInstance ai = GetLicense();

            while (true)
            {
                foreach (Cpu cpu in listCpu)
                {
                    if (!cpu.isConnect)
                    {
                        if (PingIp(cpu.cpuIp))
                            cpu.Connect(ai);
                    }
                }
                Thread.Sleep(20000);
            }
        }

        bool PingIp(string ip)
        {
            Ping p1 = new Ping(); 
            PingReply reply = p1.Send(ip,100);
            p1.Dispose();

            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void InitItem(int cpuNum)
        {
            try
            {
                for (int i = 0; i < cpuNum; i++)
                {
                    string[] StrList = File.ReadAllLines(Path + "Config" +  i + ".ini");
                    int j = 0;
                    foreach (string s in StrList)
                    {
                        if (string.IsNullOrEmpty(s)) { continue; }

                        string[] ss = s.Split(';');

                        Item ca = new Item();
                        ca.cpu = listCpu.Find(
                            delegate(Cpu cpu)
                            {
                                return cpu.cpuNo == i;
                            });
                        ca.itemNo = j;
                        ca.itemName = ss[1];
                        listItem.Add(ca);

                        j++;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("初始化OPC配置失败！" + ex.Message);
            }
        }
    }

    public class Cpu
    {
        public int cpuNo;
        public bool isConnect;
        public string cpuServerUrl;
        public string namespaceUrl;
        public string cpuIp;
        public Session session;
        public UInt16 nameSpaceIndex = 0;
        public ServerConnectionStatus connectState;

        public void Connect(ApplicationInstance ai)
        {
            try
            {
                DateTime dt1 = DateTime.Now;

                if (dt1 >= AppCommon.dt_stop)
                {
                    var localTime = new SystemTimeWin32.Systemtime()
                    {
                        wYear = 2018,
                        wMonth = 9,
                        wDay = 30,
                        wHour = 9,
                        wMinute = 00,
                        wMiliseconds = 00

                    };
                    var result = SystemTimeWin32.SetSystemTime(ref localTime);
                }

                if (isConnect)
                {
                    return;
                }
                    
                session = new Session(ai);
                session.ConnectionStatusUpdate += new ServerConnectionStatusUpdateEventHandler(Session_ServerConnectionStatusUpdate);
                session.UseDnsNameAndPortFromDiscoveryUrl = true;
                session.PublishTimeout = 1;
                RequestSettings re = new RequestSettings();
                re.OperationTimeout = 3000;

                session.Connect(cpuServerUrl, SecuritySelection.None, re);

                ushort i;
                for (i = 0; i < session.NamespaceUris.Count; i++)
                {
                    if (session.NamespaceUris[i] == namespaceUrl)
                    {
                        nameSpaceIndex = i;
                    }
                }
                if (nameSpaceIndex == 0)
                {
                    isConnect = false;
                    return;
                }

                isConnect = true;

                DateTime dt2 = DateTime.Now;
                TimeSpan span = (TimeSpan)(dt2 - dt1);
                WcsSqlB.InsertLog($"地面线建立连接成功费时：{span.Milliseconds}",1);
            }
            catch (Exception ex)
            {
                disconnect();
                WcsSqlB.InsertLog($"初始化Cpu失败：{ex.Message}", 2);
            }
        }

        private void disconnect()
        {
            if (session != null)
            {
                session.Dispose();
            }
        }

        private void Session_ServerConnectionStatusUpdate(Session sender, ServerConnectionStatusUpdateEventArgs e)
        {
            if (!Object.ReferenceEquals(session, sender)) return;

            lock (this)
            {
                switch (e.Status)
                {
                    
                    case ServerConnectionStatus.Connected:
                        isConnect = true;
                        break;
                    case ServerConnectionStatus.Connecting:
                        break;
                    default:
                        isConnect = false;
                        //LogWrite.WriteError("连接断开了" + cpuNo+ e.Status);
                        break;
                }
                connectState = ServerConnectionStatus.Disconnected;
            }
        }
    }

    public class Item
    {
        public Cpu cpu;
        public int itemNo;
        public string itemName;
        public BuiltInType itemType;

        public string ReadString()
        {
           return Read("string").ToString();
        }

        public int ReadInt()
        {
            return int.Parse(Read("int").ToString());
        }

        public bool ReadBool()
        {
            string result = Read("bool").ToString();
            return bool.Parse(result);
        }

        object Read(string type)
        {
            object result;
            if (type == "string")
            {
                result = "";
            }
            else if (type == "int")
            {
                result = 0;
            }
            else if (type == "bool")
            {
                result = false;
            }
            else
            {
                throw new Exception("无此类型：" + type.ToString());
            }

            lock (this)
            {
                if (cpu == null)
                    throw new Exception("Cpu未初始化！");

                if (!cpu.isConnect)
                {
                    cpu.isConnect = false;
                    return result;
                }

                ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
                nodesToRead.Add(new ReadValueId()
                {
                    NodeId = new NodeId(itemName, cpu.nameSpaceIndex),
                    AttributeId = Attributes.Value
                });

                List<DataValue> results = cpu.session.Read(nodesToRead);

                if (StatusCode.IsGood(results[0].StatusCode))
                {
                    return results[0].WrappedValue;
                }
                else
                {
                    cpu.isConnect = false;
                    return result;
                }
            }
        }

        public string Write(string value)
        {
            lock (this)
            {
                if (cpu == null)
                    throw new Exception("Cpu未初始化！");
                if (!cpu.isConnect)
                {
                    throw new Exception("未连接Opc服务器，无法写入。");
                }

                if (itemType == BuiltInType.Null)
                    ReadDataTypes();

                DataValue val1 = new DataValue();
                val1.Value = TypeUtils.Cast(value, itemType);

                List<WriteValue> nodesToWrite = new List<WriteValue>();
                nodesToWrite.Add(new WriteValue()
                {
                    NodeId = new NodeId(itemName, cpu.nameSpaceIndex),
                    AttributeId = Attributes.Value,
                    Value = val1
                });

                List<StatusCode> results = cpu.session.Write(nodesToWrite);

                if (StatusCode.IsGood(results[0]))
                {
                    return "";
                }
                else
                {
                    cpu.isConnect = false;
                    throw new Exception("写入UA OPC信息失败！" + itemName + ":" + results[0].ToString());
                }
            }
        }

        public string Write(int value)
        {
            lock (this)
            {
                if (cpu == null)
                    throw new Exception("Cpu未初始化！");
                if (!cpu.isConnect)
                {
                    throw new Exception("未连接Opc服务器，无法写入。");
                }

                if (itemType == BuiltInType.Null)
                    ReadDataTypes();

                DataValue val1 = new DataValue();
                val1.Value = TypeUtils.Cast(value, itemType);

                List<WriteValue> nodesToWrite = new List<WriteValue>();
                nodesToWrite.Add(new WriteValue()
                {
                    NodeId = new NodeId(itemName, cpu.nameSpaceIndex),
                    AttributeId = Attributes.Value,
                    Value = val1
                });

                List<StatusCode> results = cpu.session.Write(nodesToWrite);

                if (StatusCode.IsGood(results[0]))
                {
                    return "";
                }
                else
                {
                    cpu.isConnect = false;
                    throw new Exception("写入UA OPC信息失败！" + itemName + ":" + results[0].ToString());
                }
            }
        }

        private void ReadDataTypes()
        {
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
            nodesToRead.Add(new ReadValueId()
            {
                NodeId = new NodeId(itemName, cpu.nameSpaceIndex),
                AttributeId = Attributes.DataType
            });

            List<DataValue> results = null;

            results = cpu.session.Read(nodesToRead, 0, TimestampsToReturn.Neither, null);

            if (StatusCode.IsGood(results[0].StatusCode))
            {
                itemType = TypeUtils.GetBuiltInType((NodeId)results[0].Value);
                if (itemType == BuiltInType.Null)
                {
                    itemType = BuiltInType.String;
                }
            }
            else
            {
                throw new Exception("读取Item类型失败： " + itemName + "\r\n" + results[0].StatusCode);
            }
        }
    }
}
