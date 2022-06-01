using CommomLY1._0;
using NJIS.RFID.HBSEPD;
using System;
using System.Threading;

namespace RFIDCA2015
{
    class Program
    {
        static void Main(string[] args)
        {
            var rip = "192.168.1.216";
            var inport = 2015;
            var connstr = "Persist Security Info =true; Password=!Q@W#E$R5t6y7u8i;User ID = sa ; Initial Catalog = HGWCSB; Data Source =.";
            string pallet = "";
            //string pallet_old = "";

            UdpRfider ur = new UdpRfider(rip, 8010, "192.168.1.253", 8009);
            Console.Title = $"{inport}入口扫码器";
            int rows = 0;

            var conn_rlt = ur.Connect();

            if (conn_rlt)
            {
                Console.WriteLine($"入口{inport}扫码器{rip}：连接成功！");

                try
                {
                    while (true)
                    {
                        pallet = ur.ReadString();

                        if (pallet.Length == 11)
                        {
                            SqlBase.ExecSql($"insert RFID_ScanLog(InStation,NPallet,ScanTime)values({inport},'{pallet}','{DateTime.Now.ToString()}')", connstr);
                            //SqlBase.ExecSql($"update RFID_Config set RPallet='{pallet}',RStatus=0,RTime=GETDATE() where ReaderIp='{rip}'", connstr);
                            Console.WriteLine($"入口{inport}扫码器{rip}：读取条码{pallet}成功！时间：{DateTime.Now.ToString()}");
                            rows++;
                            if (rows >= 50) { Console.Clear(); rows = 0; }
                        }

                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine($"扫码器{rip}：连接失败！");
            }
        }
    }
}
