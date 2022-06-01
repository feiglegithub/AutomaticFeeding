using System.Text;
using System.Data;
using WCS.model;
using System.Threading;
using WCS.DataBase;

namespace WCS
{
    class OpcHss
    {
        #region 二楼站台
        //2楼出库站台是否空闲
        public static bool IsStationFree_2(int stationNo)
        {
            try
            {
                int cpuNo;
                int itemNo;
                if (stationNo < 5)
                {
                    cpuNo = 3;
                    itemNo = 2 + (stationNo - 1) * 5;
                }
                else
                {
                    cpuNo = 2;
                    itemNo = 2 + (stationNo - 5) * 5;
                }

                Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemNo);
                return item.ReadBool();
            }
            catch
            {
                return false;
            }
        }

        //给堆垛机出库站台写任务
        public static void WriteStationOutTask(int ddjNo, int pilerNo, int target)
        {
            int itemNo = 0;
            if (ddjNo == 1)
            {
                itemNo = 159;
            }
            else if (ddjNo == 2)
            {
                itemNo = 150;
            }
            else if (ddjNo == 3)
            {
                itemNo = 141;
            }
            else if (ddjNo == 4)
            {
                itemNo = 132;
            }

            OpcBaseManage.opcBase.GetItem(0, itemNo).Write(pilerNo);
            OpcBaseManage.opcBase.GetItem(0, itemNo + 2).Write(target);
        }

        //入库站台是否有入库请求
        public static bool IsStationInRequest(int ddj)
        {
            try
            {
                var itemNo = 0;

                if (ddj == 1)
                {
                    itemNo = 261;
                }
                else if (ddj == 2)
                {
                    itemNo = 260;
                }
                else if (ddj == 3)
                {
                    itemNo = 259;
                }
                else if (ddj == 4)
                {
                    itemNo = 258;
                }

                Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
                return item.ReadBool();
            }
            catch
            {
                return false;
            }
        }

        //入库站台托盘号
        public static string ReadStationPiler(int ddj)
        {
            var itemNo = 0;
            if (ddj == 1)
            {
                itemNo = 225;
            }
            else if (ddj == 2)
            {
                itemNo = 216;
            }
            else if (ddj == 3)
            {
                itemNo = 207;
            }
            else if (ddj == 4)
            {
                itemNo = 198;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadString();
        }

        //2楼入库站台写入处理完成
        public static string WriteStationFinish_2(int stationNo)
        {
            int cpuNo;
            int itemNo;
            if (stationNo < 5)
            {
                cpuNo = 3;
                itemNo = 23 + (stationNo - 1) * 4;
            }
            else
            {
                cpuNo = 2;
                itemNo = 28 + (stationNo - 5) * 4;
            }

            Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemNo);
            return item.Write(1);
        }
        #endregion

        #region 一楼站台

        //堆垛机出库站台是否空闲
        public static bool IsOutStationFree(int ddj)
        {
            try
            {
                var itemNo = 0;
                if (ddj == 1)
                {
                    itemNo = 160;
                }
                else if (ddj == 2)
                {
                    itemNo = 151;
                }
                else if (ddj == 3)
                {
                    itemNo = 142;
                }
                else if (ddj == 4)
                {
                    itemNo = 133;
                }
                Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
                return item.ReadBool();
            }
            catch
            {
                return false;
            }
        }

        //1楼出库站台写入任务
        public static void WriteStationOutTask_1(int stationNo, TaskInfo taskInfo)
        {
            int cpuNo;
            int itemNo;
            if (stationNo < 5)
            {
                cpuNo = 0;
                itemNo = (stationNo - 1) * 5;
            }
            else
            {
                cpuNo = 1;
                itemNo = (stationNo - 5) * 5;
            }
            Item item1 = OpcBaseManage.opcBase.GetItem(cpuNo, itemNo);
            //item1.Write(taskInfo.PallteNo);
            Item item2 = OpcBaseManage.opcBase.GetItem(cpuNo, itemNo + 1);
            item2.Write(taskInfo.Port);
        }
        #endregion

        //一楼出库月台是否空闲 add by liyuan 2017 6-28
        public static bool IsExportStationFree(int station)
        {
            try
            {
                int cpuNo;
                int itemNo;
                if (station <= 106)
                {
                    cpuNo = 0;
                    itemNo = station - 100 + 65;
                }
                else
                {
                    cpuNo = 1;
                    itemNo = station + 13;
                }

                Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemNo);
                return item.ReadBool();
            }
            catch
            {
                return false;
            }
        }

        #region 一楼发货口

        //1楼发货口是否完成
        public static bool IsExportFinish_1(int export)
        {
            try
            {
                int cpuNo;
                int itemNo;
                if (export < 7)
                {
                    cpuNo = 0;
                    itemNo = 22 + (export - 1) * 4;
                }
                else
                {
                    cpuNo = 1;
                    itemNo = 32 + (export - 7) * 4;
                }

                Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemNo);
                return item.ReadBool();
            }
            catch
            {
                return false;
            }
        }

        public static bool IsRequestPalletWay() 
        {
            try 
            {
                Item item = OpcBaseManage.opcBase.GetItem(0, 5);
                return item.ReadBool();
            }
            catch 
            {
                return false;
            }
        }

        public static void WritePalletWay(int way) 
        {
            OpcBaseManage.opcBase.GetItem(0, 7).Write(way);
        }

        //1楼发货口读取托盘号
        public static string ReadExportPallet_1(int export)
        {
            int cpuNo;
            int itemNo;
            if (export < 7)
            {
                cpuNo = 0;
                itemNo = 20 + (export - 1) * 4;
            }
            else
            {
                cpuNo = 1;
                itemNo = 30 + (export - 7) * 4;
            }

            Item item = OpcBaseManage.opcBase.GetItem(cpuNo, itemNo);
            return item.ReadString();
        }

        //1楼发货口写入处理完成
        public static void WriteExportFinish_1(int export)
        {
            int cpuNo;
            int itemNo;
            if (export < 7)
            {
                cpuNo = 0;
                itemNo = 23 + (export - 1) * 4;
            }
            else
            {
                cpuNo = 1;
                itemNo = 33 + (export - 7) * 4;
            }

            OpcBaseManage.opcBase.GetItem(cpuNo, itemNo).Write(1);
        }

        #endregion

        #region RGV
        public static bool RGVRequest(int stationNo)
        {
            var itemNo = 0;
            if (stationNo == 3011)
            {
                itemNo = 387;
            }
            else if (stationNo == 3012)
            {
                itemNo = 388;
            }
            //else if (stationNo == 3014)
            //{
            //    itemNo = 389;
            //}

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadBool();
        }

        public static void ClearRGVRequest(int stationNo)
        {
            var itemNo = 0;
            if (stationNo == 3011)
            {
                itemNo = 387;
            }
            else if (stationNo == 3012)
            {
                itemNo = 388;
            }
            else if (stationNo == 105)
            {
                itemNo = 256;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            item.Write(0);
        }

        //RGV是否自动
        public static bool IsRGVAuto()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 394);
            return item.ReadBool();
        }

        //RGV是否激活
        public static bool IsRGVActication()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 393);
            return item.ReadBool();
        }

        //RGV是否空闲
        public static bool IsRGVFree()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 396);
            return item.ReadBool();
        }

        //RGV是否完成任务
        public static bool IsRGVFinished()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 395);
            return item.ReadBool();
        }

        //读取RGV垛号
        public static int RRGVPilerNo()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 397);
            return item.ReadInt();
        }

        //读取RGV起始位置
        public static int RRGVFromStation()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 399);
            return item.ReadInt();
        }

        //读取RGV目标位置
        public static int RRGVToStation()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 400);
            return item.ReadInt();
        }

        //读取RGV当前工位
        public static int RRGVCurrentStaion()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 398);
            return item.ReadInt();
        }

        //在RGV自动，空闲，激活的状态下，才能接受任务
        public static bool RGVCanDo()
        {
            return IsRGVActication() && IsRGVAuto() && IsRGVFree();
        }

        //给RGV写任务
        public static bool WriteRGVTask(int from, int to, int pilerNo)
        {
            Item item1 = OpcBaseManage.opcBase.GetItem(0, 401);
            item1.Write(pilerNo);
            Item item2 = OpcBaseManage.opcBase.GetItem(0, 402);
            item2.Write(from);
            Item item3 = OpcBaseManage.opcBase.GetItem(0, 403);
            item3.Write(to);

            WaitRGVBusy();

            Item item4 = OpcBaseManage.opcBase.GetItem(0, 392);
            if (item4.ReadBool())
            {
                Item item5 = OpcBaseManage.opcBase.GetItem(0, 404);
                item5.Write(1);
                item2.Write(0);
                item3.Write(0);

                return true;
            }
            else
            {
                return false;
            }
        }

        //等待RGV忙碌,10s
        static void WaitRGVBusy()
        {
            int ct = 0;
            while (IsRGVFree())
            {
                Thread.Sleep(100);
                ct++;
                if (ct >= 100)
                    break;
            }
        }
        
        //接收RGV完成信号时，清除掉状态
        public static void ClearRGVTask()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 404);
            item.Write(0);

            Item item2 = OpcBaseManage.opcBase.GetItem(0, 402);
            item2.Write(0);

            Item item3 = OpcBaseManage.opcBase.GetItem(0, 403);
            item3.Write(0);

            //从RGV接收信号有延迟，所以等待100毫秒
            //Thread.Sleep(100);
        }

        //获取RGV故障代码
        public static int GetRGVErrorMsg()
        {
            //Item item = OpcBaseManage.opcBase.GetItem(0, 404);
            //return item.ReadInt();
            return 0;
        }

        public static bool RRGVTaskStatus()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 404);
            return item.ReadBool();
        }

        public static int RRGVMsg()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 391);
            return item.ReadInt();
        }
        #endregion

        #region 拣选      
        //给拣选工位写板件数量
        public static void WriteBoradsCountToStaion(int stationNo, int Amount)
        {
            var itemNo = 0;
            if (stationNo == 2001)
            {
                itemNo = 285;
            }
            else if (stationNo == 2002)
            {
                itemNo = 286;
            }
            else if (stationNo == 2003)
            {
                itemNo = 287;
            }
            else if (stationNo == 2004)
            {
                itemNo = 288;
            }
            else if (stationNo == 2005)
            {
                itemNo = 289;
            }
            else if(stationNo == 2006)
            {
                itemNo = 242;
            }

            var item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            item.Write(Amount);
            var item2 = OpcBaseManage.opcBase.GetItem(0, 407);
            item2.Write(1);

            //最多等待3s机械手反馈信号
            int ct = 0;
            while (!RRinitial())
            {
                Thread.Sleep(100);

                if (ct >= 30)
                    break;

                ct++;
            }

            if (RRinitial())
            {
                item2.Write(0);
                item.Write(0);
            }
        }

        public static bool RRinitial()
        {
            var item = OpcBaseManage.opcBase.GetItem(0, 408);
            return item.ReadBool();
        }

        //出库到位完成信号
        public static bool IsTaskDone(int stationNo)
        {
            var itemNo = 0;
            if (stationNo == 2001)
            {
                itemNo = 229;
            }
            else if (stationNo == 2002)
            {
                itemNo = 233;
            }
            else if (stationNo == 2004)
            {
                itemNo = 239;
            }
            else if (stationNo == 2005)
            {
                itemNo = 244;
            }
            else if (stationNo == 100)
            {
                itemNo = 253;
            }
            else if (stationNo == 1001)
            {
                itemNo = 390;
            }
            else if (stationNo == 3001)
            {
                itemNo = 368;
            }
            else if (stationNo == 3002)
            {
                itemNo = 370;
            }
            else if (stationNo == 3003)
            {
                itemNo = 372;
            }
            else if (stationNo == 3004)
            {
                itemNo = 374;
            }
            else if (stationNo == 3005)
            {
                itemNo = 376;
            }
            else if (stationNo == 3006)
            {
                itemNo = 378;
            }
            else if (stationNo == 3007)
            {
                itemNo = 380;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadBool();
        }

        //读取垛号
        public static int ReadPilerNo(int stationNo)
        {
            var itemNo = 0;
            if (stationNo == 2001)
            {
                itemNo = 30;
            }
            else if (stationNo == 2002)
            {
                itemNo = 33;
            }
            else if (stationNo == 2004)
            {
                itemNo = 39;
            }
            else if (stationNo == 2005)
            {
                itemNo = 51;
            }
            else if (stationNo == 100)
            {
                itemNo = 123;
            }
            else if (stationNo == 1001)
            {
                itemNo = 90;
            }
            else if (stationNo == 3014)
            {
                itemNo = 87;
            }
            else if (stationNo == 3001)
            {
                itemNo = 298;
            }
            else if (stationNo == 3002)
            {
                itemNo = 304;
            }
            else if (stationNo == 3003)
            {
                itemNo = 310;
            }
            else if (stationNo == 3004)
            {
                itemNo = 322;
            }
            else if (stationNo == 3005)
            {
                itemNo = 328;
            }
            else if (stationNo == 3006)
            {
                itemNo = 334;
            }
            else if (stationNo == 3007)
            {
                itemNo = 340;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadInt();
        }

        //给拣选工位写花色
        public static void WriteProductCodeToStaion(int stationNo, string ProductCode)
        {
            var itemNo = 0;
            if (stationNo == 2001)
            {
                itemNo = 231;
            }
            else if (stationNo == 2002)
            {
                itemNo = 235;
            }
            else if (stationNo == 2004)
            {
                itemNo = 241;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            item.Write(ProductCode);
        }

        //2001,2002,2003,2004,2005拣选工位入库前进完成信号
        public static bool SortStationGoHeadDone(int stationNo)
        {
            int itemNo = 0;
            if (stationNo == 2001)
            {
                itemNo = 228;
            }
            else if (stationNo == 2002)
            {
                itemNo = 232;
            }
            else if (stationNo == 2003)
            {
                itemNo = 236;
            }
            else if (stationNo == 2004)
            {
                itemNo = 238;
            }
            else if (stationNo == 2005)
            {
                itemNo = 249;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadBool();
        }

        //给拣选工位写入库指令
        public static void WriteTaskToSortStation(int stationNo, int pilerNo, int target)
        {
            Item item1, item2, item3;
            if (stationNo == 2001)
            {
                item1 = OpcBaseManage.opcBase.GetItem(0, 230);
                item1.Write(1);
                item2 = OpcBaseManage.opcBase.GetItem(0, 30);
                item2.Write(pilerNo);
                item3 = OpcBaseManage.opcBase.GetItem(0, 32);
                item3.Write(target);
            }
            else if (stationNo == 2002)
            {
                item1 = OpcBaseManage.opcBase.GetItem(0, 234);
                item1.Write(1);
                item2 = OpcBaseManage.opcBase.GetItem(0, 33);
                item2.Write(pilerNo);
                item3 = OpcBaseManage.opcBase.GetItem(0, 35);
                item3.Write(target);
            }
            else if (stationNo == 2003)
            {
                item1 = OpcBaseManage.opcBase.GetItem(0, 237);
                item1.Write(1);
                item2 = OpcBaseManage.opcBase.GetItem(0, 36);
                item2.Write(pilerNo);
                item3 = OpcBaseManage.opcBase.GetItem(0, 38);
                item3.Write(target);
            }
            else if (stationNo == 2004)
            {
                item1 = OpcBaseManage.opcBase.GetItem(0, 240);
                item1.Write(1);
                item2 = OpcBaseManage.opcBase.GetItem(0, 39);
                item2.Write(pilerNo);
                item3 = OpcBaseManage.opcBase.GetItem(0, 41);
                item3.Write(target);
            }
            else if (stationNo == 2005)
            {
                item1 = OpcBaseManage.opcBase.GetItem(0, 250);
                item1.Write(1);
                item2 = OpcBaseManage.opcBase.GetItem(0, 51);
                item2.Write(pilerNo);
                item3 = OpcBaseManage.opcBase.GetItem(0, 53);
                item3.Write(target);
            }
        }

        //读取入库指令
        public static bool ReadInWareCmd(int station)
        {
            var itemNo = 0;          
            switch (station)
            {
                case 2001: itemNo = 230;break;
                case 2002: itemNo = 234;break;
                case 2003: itemNo = 237;break;
                case 2004: itemNo = 240; break;
                case 2005: itemNo = 250; break;
                default:
                    break;
            }

            var item1 = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item1.ReadBool();
        }

        //给机械手清除指令
        public static void ClearSortStation(int stationNo)
        {
            var itemNo = 0;
            if (stationNo == 2001)
            {
                itemNo = 280;
            }
            else if (stationNo == 2002)
            {
                itemNo = 281;
            }
            else if (stationNo == 2003)
            {
                itemNo = 282;
            }
            else if (stationNo == 2004)
            {
                itemNo = 283;
            }
            else if (stationNo == 2005)
            {
                itemNo = 284;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            item.Write(1);

            //等待机械手清除完成反馈信号
            int ct = 0;
            while (!RClearFeed())
            {
                Thread.Sleep(100);

                if (ct >= 30)
                    break;

                ct++; 
            }

            //机械手有反馈后，清掉指令
            if (RClearFeed())
            {
                item.Write(0);
            }
        }

        //读取机械手清除完成信号
        public static bool RClearFeed()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 409);
            return item.ReadBool();
        }

        //获取拣选工位的板件数量
        public static int GetBoardsCount(int station)
        {
            var itemNo = 0;
            if (station == 2001)
            {
                itemNo = 272;
            }
            else if (station == 2002)
            {
                itemNo = 273;
            }
            else if (station == 2003)
            {
                itemNo = 274;
            }
            else if (station == 2004)
            {
                itemNo = 275;
            }
            else if (station == 2005)
            {
                itemNo = 276;
            }
            else if (station == 2006)
            {
                itemNo = 242;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadInt();
        }
        
        //获取工位是否有上保护板
        public static bool IsHaveUpProduct(int station)
        {
            var itemNo = 0;
            if (station == 2001)
            {
                itemNo = 292;
            }
            else if (station == 2002)
            {
                itemNo = 293;
            }
            else if (station == 2004)
            {
                itemNo = 294;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadBool();
        }

        public static void SetHaveUpProduct(int station, int IsHave)
        {
            var itemNo = 0;
            if (station == 2001)
            {
                itemNo = 292;
            }
            else if (station == 2002)
            {
                itemNo = 293;
            }
            else if (station == 2004)
            {
                itemNo = 294;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            item.Write(IsHave);
        }

        //获取拣选工位的花色
        public static string GetProductCode(int station)
        {
            var itemNo = 0;
            if (station == 2001)
            {
                itemNo = 231;
            }
            else if (station == 2002)
            {
                itemNo = 235;
            }
            else if (station == 2004)
            {
                itemNo = 241;
            }
            else
            {
                return "";
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadString();
        }

        //获取设备GT216，GT217上共缓存的垛数
        public static int GetEmptyBuffersCount()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 246);
            return item.ReadInt();
        }

        //给机械手写任务
        public static bool WriteToMainpulator(int no, int fromStation, int toStation)
        {
            Item item1 = null;
            Item item2 = null;
            if (no == 1)
            {
                //1号机械手
                item1 = OpcBaseManage.opcBase.GetItem(0, 290);
                item1.Write(fromStation);
                item2 = OpcBaseManage.opcBase.GetItem(0, 291);
                item2.Write(toStation);
            }

            WaitMBusy(no);

            var rlt = ReadMAcceptTask(no);

            if (rlt)
            {
                //同时计算WCS存储的数量
                WCSSql.AddSortStationCount(fromStation, toStation);
            }

            item1.Write(0);
            item2.Write(0);

            return rlt;
        }

        public static bool ReadMAcceptTask(int no)
        {
            var itemNo = 0;
            if (no == 1)
            {
                itemNo = 405;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadBool();
        }

        public static void WaitMBusy(int no)
        {
            int ct = 0;
            while (RMFree(no))
            {
                Thread.Sleep(100);

                if (ct >= 100)
                    break;

                ct++;
            }
        }

        //读取机械手是否空闲
        public static bool RMFree(int no)
        {
            var itemNo = 0;
            if (no == 1)
            {
                itemNo = 271;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadInt() == 1;
        }

        //读取机械手是否激活
        public static bool RMActivity(int no)
        {
            var itemNo = 0;
            if (no == 1)
            {
                itemNo = 270;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadBool();
        }

        //读取机械手是否自动
        public static bool RMAuto(int no)
        {
            var itemNo = 0;
            if (no == 1)
            {
                itemNo = 269;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadInt() == 1;
        }

        //读取RGV抓板工位
        public static int RMFrom(int no)
        {
            var itemNo = 0;
            if (no == 1)
            {
                itemNo = 277;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadInt();
        }

        //读取RGV放板工位
        public static int RMTo(int no)
        {
            var itemNo = 0;
            if (no == 1)
            {
                itemNo = 278;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadInt();
        }

        public static bool RMCanDo(int no)
        {
            bool a, b, c = false;
            a = RMActivity(no);
            b = RMAuto(no);
            c= RMFree(no); 
            return RMActivity(no) && RMAuto(no) && RMFree(no);
        }

        //机械手错误信息
        public static int JXSMsg(int i)
        {
            //Item item = OpcBaseManage.opcBase.GetItem(0, 279);
            //switch (i)
            //{
            //    case 1:  break;
            //    case 2:  item = OpcBaseManage.opcBase.GetItem(0, 279); break;
            //}

            //return item.ReadInt();
            return 0;
        }

        //前进，后退指令 GT216,GT217
        public static void GoCmd(int type)
        {
            var itemNo = 0;
            if (type == 1)
            {
                //GT217退回指令
                itemNo = 248;
            }
            else if (type == 2)
            {
                //GT216前进指令
                itemNo = 245;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            item.Write(1);
        }

        public static bool GetGoCmd(int type)
        {
            var itemNo = 0;
            if (type == 1)
            {
                //GT217退回指令
                itemNo = 248;
            }
            else if (type == 2)
            {
                //GT216前进指令
                itemNo = 245;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadBool();
        }

        //获取GT216上的板件数量
        public static int Get2006BoradsCount()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 242);
            return item.ReadInt();
        }

        //开料前，告诉线体是否有上保护板
        public static void WriteUpToCutStation(int stationNo, int flg)
        {
            var itemNo = 0;

            if (stationNo == 3001)
            {
                itemNo = 367;
            }
            else if (stationNo == 3002)
            {
                itemNo = 369;
            }
            else if (stationNo == 3003)
            {
                itemNo = 371;
            }
            else if (stationNo == 3004)
            {
                itemNo = 373;
            }
            else if (stationNo == 3005)
            {
                itemNo = 375;
            }
            else if (stationNo == 3006)
            {
                itemNo = 377;
            }
            else if (stationNo == 3007)
            {
                itemNo = 379;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            item.Write(flg);
        }

        //WCS重置完成信号
        public static void ReSetFeedBack(int stationNo)
        {
            var itemNo = 0;
            if (stationNo == 2006)
            {
                itemNo = 247;
            }
            else if (stationNo == 2005)
            {
                itemNo = 244;
            }
            else if (stationNo == 2004)
            {
                itemNo = 239;
            }
            else if (stationNo == 2002)
            {
                itemNo = 233;
            }
            else if (stationNo == 2001)
            {
                itemNo = 229;
            }
            else if (stationNo == 1001)
            {
                itemNo = 390;
            }
            else if (stationNo == 100)
            {
                itemNo = 253;
            }
            else if (stationNo == 3001)
            {
                itemNo = 368;
            }
            else if (stationNo == 3002)
            {
                itemNo = 370;
            }
            else if (stationNo == 3003)
            {
                itemNo = 372;
            }
            else if (stationNo == 3004)
            {
                itemNo = 374;
            }
            else if (stationNo == 3005)
            {
                itemNo = 376;
            }
            else if (stationNo == 3006)
            {
                itemNo = 378;
            }
            else if (stationNo == 3007)
            {
                itemNo = 380;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            item.Write(0);
        }

        //前进，后退指令 完成反馈 GT216,GT217
        public static bool GoCmdFeedBack(int type)
        {
            var itemNo = 0;
            if (type == 1)
            {
                //退回完成信号 GT217-->GT216
                itemNo = 247;
            }
            else if (type == 2)
            {
                //前进完成反馈 GT216-->GT217
                itemNo = 243;
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            return item.ReadBool();
        }
        #endregion

        #region 地面线入库
        public static bool IsInWareRequest(int stationNo)
        {
            var itemNo = 0;
            if (stationNo == 105)
            {
                itemNo = 256;
            }
            else if (stationNo == 3011)
            {

            }
            else if (stationNo == 3012)
            {

            }
            else if (stationNo == 3014)
            {
                //GT207
                //itemNo = 
            }

            Item item = OpcBaseManage.opcBase.GetItem(0, 256);
            return item.ReadBool();
        }

        //获取入库检测信息
        public static int GetInWareMsg()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 257);
            return item.ReadInt();
        }

        //写任务数据给入库站台
        public static void WriteInWareData(int stationNo, int pilerNo, int target)
        {
            var itemNo = 0;
            var itemNo1 = 0;
            if (stationNo == 105)
            {
                itemNo = 189;
            }
            else if (stationNo == 2001)
            {
                itemNo = 30;
                itemNo1 = 230;
            }
            else if (stationNo == 2002)
            {
                itemNo = 33;
                itemNo1 = 234;
            }
            else if (stationNo == 2003)
            {
                itemNo = 36;
                itemNo1 = 237;
            }
            else if (stationNo == 2004)
            {
                itemNo = 39;
                itemNo1 = 240;
            }
            else if (stationNo == 2005)
            {
                itemNo = 51;
                itemNo1 = 250;
            }

            Item item1 = OpcBaseManage.opcBase.GetItem(0, itemNo);
            item1.Write(pilerNo);
            Item item2 = OpcBaseManage.opcBase.GetItem(0, itemNo + 2);
            item2.Write(target);
            if (stationNo > 105)
            {
                Item item3 = OpcBaseManage.opcBase.GetItem(0, itemNo1);
                item3.Write(1);
            }
        }

        //给GT218写任务数据
        public static void WriteDateTo3013(int pilerNo, int target)
        {
            Item item1 = OpcBaseManage.opcBase.GetItem(0, 266);
            item1.Write(pilerNo);
            Item item2 = OpcBaseManage.opcBase.GetItem(0, 268);
            item2.Write(target);
        }

        //105入库退回
        public static void InWareWriteBack()
        {
            Item item = OpcBaseManage.opcBase.GetItem(0, 191);
            item.Write(1);
        }
        #endregion

        #region 报警信息

        #endregion

        //更新设备数据
        public static void MonitorDevice()
        {
            var dt = WCSSql.GetDeviceData();
            int itemNo_PilerNo = 0;
            int itemNo_Target = 0;
            int itemNo_Staus = 0;
            int item_PilerNo=0;
            int item_Target=0;
            bool item_Staus = false;

            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                itemNo_PilerNo = int.Parse(dr["ItemNo"].ToString());
                itemNo_Target = itemNo_PilerNo + 2;
                itemNo_Staus = itemNo_PilerNo + 1;

                item_PilerNo = OpcBaseManage.opcBase.GetItem(0, itemNo_PilerNo).ReadInt();
                item_Target = OpcBaseManage.opcBase.GetItem(0, itemNo_Target).ReadInt();
                item_Staus = OpcBaseManage.opcBase.GetItem(0, itemNo_Staus).ReadBool();

                sb.Append($"update HGWCSBC.[dbo].[WCS_DeviceMonitor] set PilerNo={item_PilerNo},[Target]={item_Target},Staus='{item_Staus}' where ItemNo={itemNo_PilerNo};");
            }

            Sql.ExecSQL(sb.ToString());
        }

        //获取地面柜的状态
        public static int GetHsStatus(int hsNo)
        {
            return OpcBaseManage.opcBase.GetItem(0, 410 + hsNo).ReadInt();
        }
    }
}
