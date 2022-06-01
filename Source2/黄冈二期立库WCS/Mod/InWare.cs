using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WCS.Common;
using WCS.DataBase;
using WCS.model;
using System.Threading.Tasks;

namespace WCS
{
    public class InWare
    {
        static InWareStaion iws2001 = new InWareStaion("2001入口", 2001, 5008, 464, "192.168.1.101");
        static InWareStaion iws2002 = new InWareStaion("2002入口", 2002, 4107, 577, "192.168.1.100");
        static InWareStaion iws2007 = new InWareStaion("2007入口", 2007, 1324, 1244, "192.168.1.102");
        static InWareStaion iws2015 = new InWareStaion("2015入口", 2015, 6002, 324, "192.168.1.105");
        static InWareStaion iws2017 = new InWareStaion("2017入口", 2017, 6010, 325, "192.168.1.104");
        static InWareStaion iws2019 = new InWareStaion("2019入口", 2019, 6016, 326, "192.168.1.103");

        static InWareStaion iws2012 = new InWareStaion("2012入口", 2012, 6071, 327, "192.168.1.103");
        static InWareStaion iws2010 = new InWareStaion("2010入口", 2010, 6078, 328, "192.168.1.103");

        public static void AllInWare()
        {
            InW(iws2001);
            InW(iws2002);
            InW(iws2007);
            InW(iws2015);
            InW(iws2017);
            InW(iws2019);
            InW(iws2010);
            InW(iws2012);
        }

        public static void InWare2001()
        {
            InW(iws2001);
        }

        public static void InWare2002()
        {
            InW(iws2002);
        }

        public static void InWare2007()
        {
            InW(iws2007);
        }

        public static void InWare2015()
        {
            InW(iws2015);
        }

        public static void InWare2017()
        {
            InW(iws2017);
        }

        public static void InWare2019()
        {
            InW(iws2019);
        }

        public static void InWare2010()
        {
            InW(iws2010);
        }

        public static void InWare2012()
        {
            InW(iws2012);
        }

        static void InW(InWareStaion iws)
        {
            try
            {
                //Step1:检测入口有没有入库请求
                if (iws.ReadTarget() != 999) { return; }

                var s = WcsSqlB.GetStationByNo(iws.StationNo);
                if (s.State == false)
                {
                    WcsSqlB.InsertLog($"{iws.StationName}：入口被禁用！", 1);
                    iws.WriteBack();
                    WcsSqlB.InsertLED($"湖北索菲亚欢迎您\r\n位置：{iws.StationName}\r\n信息：入口已被禁用！请联系仓储管理员！", iws.StationNo, 0);
                    return;
                }

                //Step2:读取托盘高度
                //var height = iws.ReadHeight();
                var height = 2;
                if (height != 1 && height != 2)
                {
                    WcsSqlB.InsertLog($"{iws.StationName}：高度[{height}]读取错误！", 2);
                    WcsSqlB.InsertLED($"湖北索菲亚欢迎您\r\n位置：{iws.StationName}\r\n信息：高度[{height}]读取错误！\r\nWCS处理失败！", iws.StationNo, 0);
                    iws.WriteBack();
                    return;
                }

                //外型检测
                var errorCode = iws.ReadErrorCode();
                if (errorCode > 0)
                {
                    WcsSqlB.InsertLog($"{iws.StationName}：{AppCommon.GetDeviceErrorMsg(errorCode)}！", 2);
                    WcsSqlB.InsertLED($"湖北索菲亚欢迎您\r\n位置：{iws.StationName}\r\n信息：{AppCommon.GetDeviceErrorMsg(errorCode)}\r\nWCS处理失败！", iws.StationNo, 0);
                    iws.WriteBack();
                    return;
                }

                //Step3:读取托盘号
                var pallet = "";
                int ct = 0;
                while (ct < 5)
                {
                    pallet = WcsSqlB.ReadRFIDPallet(iws.StationNo);
                    //WcsSqlB.InsertLog($"{iws.StationName}：读RFID：{pallet}，{DateTime.Now.ToString()}", 1);
                    if (pallet.Length == 11)
                    {
                        break;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                        ct++;
                    }
                }

                var palletmsg = AppCommon.ValidPallet(pallet);
                if (palletmsg.Length > 0)
                {
                    WcsSqlB.InsertLog($"{iws.StationName}：托盘号[{pallet}]读取错误！", 2);
                    WcsSqlB.InsertLED($"湖北索菲亚欢迎您\r\n位置：{iws.StationName}\r\n信息：{palletmsg}！\r\nWCS处理失败！", iws.StationNo, 0);
                    iws.WriteBack();
                    return;
                }

                //Step4:向WMS申请入库
                var rlt = WcsSqlB.CreateInTask(pallet, iws.StationNo, height);
                if (rlt.Length > 0)
                {
                    WcsSqlB.InsertLog($"{iws.StationName}：托盘[{pallet}]入库失败！{rlt}", 2, pallet);
                    WcsSqlB.InsertLED($"湖北索菲亚欢迎您\r\n位置：{iws.StationName}\r\n托盘号：{pallet}\r\n信息：{rlt}\r\nWCS处理失败！", iws.StationNo, 0);
                    iws.WriteBack();
                }
                else
                {
                    //Step5:写任务给设备
                    var task = WcsSqlB.GetTaskInfoByPallet(pallet);
                    iws.WriteDataToPLC(task.SeqID, task.NPalletID, task.Target);
                    var rlt2 = WcsSqlB.UpdateWMSTask(task.SeqID.ToString(), 10, 1);
                    if (rlt2.Length == 0)
                    {
                        WcsSqlB.InsertLog($"{iws.StationName}：入库开始！托盘号：{pallet}，目标：{task.Target}", 1, pallet);
                        WcsSqlB.InsertLED($"湖北索菲亚欢迎您\r\n位置：{iws.StationName}\r\n托盘号：{pallet}\r\n入库货位：{task.CPosidTo}\r\n装箱类别：{task.CPackType}\r\nWCS处理成功！", iws.StationNo, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                WcsSqlB.InsertLog($"{iws.StationName}：入库异常！{ex.Message}", 2);
            }
        }

        public static void InWareErrorAll()
        {
            InWareError(iws2001);
            InWareError(iws2002);
            InWareError(iws2007);
            InWareError(iws2015);
            InWareError(iws2017);
            InWareError(iws2019);
        }

        static void InWareError(InWareStaion iws)
        {
            //var d = OPCHelper.GetDeviceByNo(iws.DeviceNo);
            //if (d.DErrorNo > 0)
            //{
            //    WcsSqlB.InsertLog($"{iws.StationName}：{AppCommon.GetDeviceErrorMsg(d.DErrorNo)}！", 2);
            //    if (iws.ErrorCode != d.DErrorNo)
            //    {
            //        iws.ErrorCode = d.DErrorNo;
            //        WcsSqlB.InsertLED($"湖北索菲亚欢迎您\r\n位置：{iws.StationName}\r\n信息：{AppCommon.GetDeviceErrorMsg(d.DErrorNo)}\r\nWCS处理失败！", iws.StationNo, 0);
            //    }

            //if (iws.ErrorCode >= 16)
            //{
            //    //外型检测不合格，才写退回
            //    OPCHelper.WriteTarget(iws.DeviceNo, 1);
            //}
            // }
        }
    }
}