using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
//using NJIS.FPZWS.LineControl.CuttingDevice.Helpers;
using NJIS.FPZWS.LineControl.Manager.Helpers;

namespace ServicePlusTest3
{
    /// <summary>
    /// PLCTest 的摘要说明
    /// </summary>
    [TestClass]
    public class PLCTest
    {
        public PLCTest()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestPLCReadLong()
        {
            //string ip = ConfigurationSettings.AppSettings["PlcIp"];
            string ip = "10.30.40.246";
            //string TriggerInDbAddr = ConfigurationSettings.AppSettings["TriggerInDbAddr"];
            string TrrigerOutAddr = "";

            PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();
            if (plc.Connect(ip))
            {
                int value = plc.ReadLong("DB30.162");
                Console.Write(value);
            }
            else
            {
                Console.Write("连接失败");
            }
        }

        [TestMethod]
        public void TestPLCReadStr()
        {
            //string ip = ConfigurationSettings.AppSettings["PlcIp"];
            string ip = "10.30.40.10";

            PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();

            string Addr = "DB450.684";

            if (plc.Connect(ip))
            {
                string value = plc.ReadString(Addr, 28);
                if (string.IsNullOrEmpty(value))
                {
                    value = "异常：读取失败";
                }
                Console.Write(value);
            }
            else
            {
                Console.Write("连接失败");
            }
        }

        [TestMethod]
        public void TestPLCReadShort()
        {
            string ip = "192.168.0.1";

            PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();
            if (plc.Connect(ip))
            {
                short value = plc.ReadShort("DB38.2");
                Console.Write(value);
            }
            else
            {
                Console.Write("连接失败");
            }
        }

        [TestMethod]
        public void TestPLCWriteStr()
        {
            //string ip = ConfigurationSettings.AppSettings["PlcIp"];
            string ip = "10.30.40.10";
            PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();

            string Addr = "DB450.684";
            if (plc.Connect(ip))
            {
                plc.Write(Addr, "HB_RX210320A1-3(1)", 28);
                //plc.Write(TriggerInAddr, 537);
            }
            else
            {
                Console.Write("连接失败");
            }
        }

        [TestMethod]
        public void TestPLCWriteInt()
        {
            string ip = ConfigurationSettings.AppSettings["PlcIp"];
            PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();

            string TriggerOutAddr = "DB30.0";

            string TriggerInAddr = "DB30.12";
            string ResultAddr = "DB30.16";

            string Addr = "DB30.490";

            if (plc.Connect("192.168.0.1"))
            {
                short s = 3;
                plc.Write("DB38.2", s);

                s = 4;
                plc.Write("DB38.4", s);

                s = 6;
                plc.Write("DB38.6", s);
                //plc.Write(TriggerInAddr, 537);
            }
            else
            {
                Console.Write("连接失败");
            }
        }
    }
}
