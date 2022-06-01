using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using OPCSiemensDAAutomation;
using WCS.DataBase;
using System.Data;

namespace WCS
{
    internal class OPCExecute
    {
        private static OPCServer opcServer;

        private static string PcName = Dns.GetHostName();

        private static List<OPCGroupEntity> opcGroup = new List<OPCGroupEntity>();

        private static bool isConnect = false;

        public static bool IsConn
        {
            get
            {
                return isConnect;
            }
        }

        public static void OPCServerAdd()
        {
            try
            {
                if (opcServer != null)
                    OPCServerRemove();

                opcServer = new OPCServer();
                opcServer.Connect("OPC.SimaticNet", PcName);
                
                //加载堆垛机信号地址
                for (int i = 1; i <=17; i++)
                {
                    try
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

                                //if (s.IndexOf(" 任务号") > 0 && i <= 6)
                                //{
                                //    var sql = $"update Wcs_Device set ItemNo={s.Split(';')[0].Replace("Item", "")} where DId={s.Split(';')[2].Replace("任务号", "").Trim()}";
                                //    WcsSqlB.F(sql);
                                //}
                            }
                        }
                        opcGroup.Add(Groupitem);
                    }
                    catch (Exception)
                    {

                        
                    }
                }
                isConnect = true;
            }
            catch(Exception ex)
            {
                WcsSqlB.InsertLog($"初始化OPC失败：{ex.Message}", 2);
            }
        }

        static void OPCServerRemove()
        {
            isConnect = false;

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


        public static void AsyncWrite(int OPCGroupNo, int OPCItemNo, object objValue)
        {
            opcGroup[OPCGroupNo].OpcItemList[OPCItemNo].Write(objValue);
        }

        public static void AsyncWrite(int OPCGroupNo, int OPCItemNo, byte[] objValue)
        {
            opcGroup[OPCGroupNo].OpcItemList[OPCItemNo].Write(objValue);
        }
    }
}
