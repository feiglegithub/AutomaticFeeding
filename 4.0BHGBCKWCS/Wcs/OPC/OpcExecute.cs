using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using OPCSiemensDAAutomation;
using WCS.DataBase;

namespace WCS
{
    internal class OPCExecute
    {
        private static OPCServer opcServer;

        private static string PcName = Dns.GetHostName();

        private static List<OPCGroupEntity> opcGroup = new List<OPCGroupEntity>();

        public static bool IsConn { get; private set; } = false;

        public static void OPCServerAdd()
        {
            try
            {
                if (opcServer != null)
                    OPCServerRemove();

                opcServer = new OPCServer();
                opcServer.Connect("OPC.SimaticNet", PcName);
                
                for (int i = 1; i <=7; i++)
                {
                    OPCGroupEntity Groupitem = new OPCGroupEntity();
                    string OPCGroupName = "Cn" + i.ToString();
                    Groupitem.OpcGroupObj = opcServer.OPCGroups.Add(OPCGroupName);
                    Groupitem.OpcGroupObj.IsActive = true;
                    Groupitem.OpcGroupObj.IsSubscribed = true;//是否异步，在采用
                    Groupitem.OpcGroupObj.DeadBand = 0;
                    Groupitem.OpcGroupObj.UpdateRate = 1000;

                    string INIPathstr = AppCommon.INIPath + "\\" + OPCGroupName + ".ini";
                    string[] StrList = File.ReadAllLines(INIPathstr);
                    if (StrList == null || StrList.Length == 0)
                    {
                        throw new Exception("无法读取到配置文件");
                    }
                    else
                    {
                        Groupitem.OpcItemList = new List<OPCItem>();
                        foreach (string s in StrList)
                        {
                            if (string.IsNullOrEmpty(s))
                                continue;

                            string c = s.Split(';')[1];
                            Groupitem.OpcItemList.Add(Groupitem.OpcGroupObj.OPCItems.AddItem(c, Groupitem.OpcItemList.Count));

                            //if (s.IndexOf("Status") > 0 && i == 5)
                            //{
                            //    var sql = $"update WCS_DeviceMonitor set ItemNo={s.Split(';')[0].Replace("Item", "")} where DeviceNo='{s.Split(';')[2].Trim().Substring(0, 5)}'";
                            //    WCSSql.F(sql);


                            //if (i == 7 && s.IndexOf("进板超时") > 0)
                            //{
                            //    //var sql = $"update WCS_DeviceAlerm set ItemNo={s.Split(';')[0].Replace("Item", "")} where DeviceNo='{s.Split(';')[2].Trim().Substring(0, 5)}'";
                            //    //var sql = $"insert into WCS_DeviceAlerm(DeviceNo,ItemNo) values('{s.Split(';')[2].Trim().Substring(0, 5)}',{s.Split(';')[0].Replace("Item", "")})";
                            //    WCSSql.F(sql);
                            //}
                        }
                    }
                    opcGroup.Add(Groupitem);
                }
                IsConn = true;
            }
            catch(Exception ex)
            {
                WCSSql.InsertLog($"初始化OPC失败：{ex.Message}", "ERROR");
            }
        }

        static void OPCServerRemove()
        {
            IsConn = false;

            if (opcServer != null)
            {
                opcServer.OPCGroups.RemoveAll();
                opcServer.Disconnect();
                opcGroup.Clear();
            }
        }


        public static object AsyncRead(int OPCGroupNo, int OPCItemNo)
        {
            object ItemValues = null;
            object Qualities = null;
            object TimeStamps = null;
            OPCItem _OPCItem = opcGroup[OPCGroupNo].OpcItemList[OPCItemNo];
            _OPCItem.Read(1, out ItemValues, out Qualities, out TimeStamps);
            return ItemValues;
        }


        public static bool AsyncWrite(int OPCGroupNo, int OPCItemNo, object objValue)
        {
            opcGroup[OPCGroupNo].OpcItemList[OPCItemNo].Write(objValue);
            object obj = null;
            object quality = null;
            object timeStamps = null;
            opcGroup[OPCGroupNo].OpcItemList[OPCItemNo]
                .Read((short) 1, out obj, out quality, out timeStamps);
            return objValue.Equals(obj);
        }

        public static void AsyncWrite(int OPCGroupNo, int OPCItemNo, byte[] objValue)
        {
            opcGroup[OPCGroupNo].OpcItemList[OPCItemNo].Write(objValue);
        }
    }
}
