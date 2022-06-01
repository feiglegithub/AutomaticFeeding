//  ************************************************************************************
//   解决方案：NJIS.RFID.HBSEPD
//   项目名称：NJIS.RFID.HBSEPD
//   文 件 名：Program.cs
//   创建时间：2019-05-02 9:59
//   作    者：
//   说    明：
//   修改时间：2019-05-02 9:59
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NJIS.RFID.HBSEPD
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("RFID扫码服务程序。。。");

            var rfids = SqlHelper.GetRFIDConfigs(); 

            //注册
            foreach(DataRow dr in rfids.Rows)
            {
                RfidManager.Current.RegisterRfider(new UdpRfider(dr["ReaderIp"].ToString(), Convert.ToUInt16(dr["ReaderPort"].ToString()), dr["HostIp"].ToString(), Convert.ToUInt16(dr["HostPort"].ToString())));
            }

            while (true)
            {
                foreach (DataRow item in rfids.Rows)
                {
                    try
                    {
                        var rst = RfidManager.Current.ReadString(item["ReaderIp"].ToString());
                        if (!string.IsNullOrEmpty(rst) && rst.Length == 11)
                        {
                            //item.RFIDCore = rst;
                            //item.Status = 0;
                            //service.UpdateAPD_RFIDConfig(item);
                            SqlHelper.UpdateRFID(item["ReaderIp"].ToString(), rst);                       
                        }
                        //Console.WriteLine($"读取RFID[{item["ReaderIp"].ToString()}:{item.ReaderPort}]数据{rst}");
                    }
                    catch
                    {
                        Console.WriteLine("读取RFID数据失败!");
                    }
                }

                System.Threading.Thread.Sleep(50);
            }



            //if (args.Length == 0)
            //{
            //    Console.WriteLine("======================使用方法====================");
            //    Console.WriteLine("==UDP 方式 代码调用");
            //    Console.WriteLine("==var udp = new UdpRfider(\"(RFID IP 地址)\", (RFID 端口), \"(接收端IP地址-如工控机)\", (接收端端口);");
            //    Console.WriteLine("==RfidManager.Current.RegisterRfider(udp);");
            //    Console.WriteLine("==var rst = RfidManager.Current.ReadString(\"RFID ID[默认RFID IP 地址]\");");
            //    Console.WriteLine("=================================================");
            //}
            //else
            //{
            //    if (args[0].ToLower() == "udp")
            //    {
            //        var readIp = args[1].Trim();
            //        var readPort = Convert.ToUInt32(args[2].Trim());
            //        var hostIp = args[3].Trim();
            //        var hostPort = Convert.ToUInt32(args[4].Trim());
            //        var isAuto = args.Length <= 5 ? "" : args[5].Trim();

            //        var udp = new UdpRfider(readIp, readPort, hostIp, hostPort);
            //        RfidManager.Current.RegisterRfider(udp);

            //        if (!string.IsNullOrEmpty(isAuto))
            //        {
            //            while (true)
            //            {
            //                var rst = RfidManager.Current.ReadString(readIp);
            //                Console.WriteLine($"读取到RFID值{rst}");
            //                System.Threading.Thread.Sleep(1000);
            //            }
            //        }
            //        else
            //        {
            //            var rst = RfidManager.Current.ReadString(readIp);
            //            Console.WriteLine($"读取到RFID值{rst}");
            //        }
            //    }
            //    else
            //    {
            //        var service = new CMSService.WebServiceSoapClient();
            //        var rfids = service.GetRFIDConfigs();

            //        foreach (var item in rfids)
            //        {
            //            var udp = new UdpRfider(item.ReaderIP, item.ReaderPort, item.HostIP, item.HostPort);
            //            RfidManager.Current.RegisterRfider(udp);

            //            System.Threading.Tasks.Task.Factory.StartNew(() =>
            //            {
            //                while (true)
            //                {
            //                    try
            //                    {
            //                        var rst = RfidManager.Current.ReadString(item.ReaderIP);
            //                        if (!string.IsNullOrEmpty(rst) && rst.Length >= 11)
            //                        {

            //                            service.UpdateAPD_RFIDConfig(new CMSService.APD_RFIDConfig()
            //                            {
            //                                ReaderIP = item.ReaderIP,
            //                                RFIDCore = rst,
            //                                Status = 0
            //                            });
            //                        }
            //                        Console.WriteLine($"读取RFID[{item.ReaderIP}:{item.ReaderPort}]数据{rst}");
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Console.WriteLine("读取RFID数据失败!");
            //                    }
            //                    System.Threading.Thread.Sleep(100);
            //                }
            //            });
            //        }

            //    }
            //}
            //Console.ReadKey();
        }
    }
}