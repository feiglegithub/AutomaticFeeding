using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contract;
using WcsModel;
using WcsService;
using WCS.Communications;

namespace WCS.Commands
{
    /// <summary>
    /// 拣选出托完成
    /// </summary>
    public class SortingOutStackCommand:CommandBase<bool,ESortingStation>
    {
        private LineCommunication _communication = null;
        private LineCommunication Line => _communication ?? (_communication = LineCommunication.GetInstance());

        private IWcsLogContract _wcsLogContract = null;
        private IWcsLogContract LogContract => _wcsLogContract ?? (_wcsLogContract =  WcsLogSevice.GetInstance());

        private ISortingStationInfoContract _sortingStationInfoContract = null;

        private ISortingStationInfoContract SortingStationInfoContract =>
            _sortingStationInfoContract ?? (_sortingStationInfoContract =  SortingStationInfoService.GetInstance());

        public SortingOutStackCommand(ESortingStation baseArg) : base(baseArg)
        {
            this.Validating += SortingOutStackCommand_Validating;
        }

        private void SortingOutStackCommand_Validating(object arg1, Args.CancelEventArg<bool> arg2)
        {
            arg2.Cancel = !RequestData;
        }

        protected override bool LoadRequest(ESortingStation baseArg)
        {
            return Line.StackIsOutingFinished(baseArg);
        }

        protected override void ExecuteContent()
        {
            try
            {
                LogContract.InsertWcsLog($"收到站台【{Convert.ToInt32(BaseArg)}】入库时，出垛完成信号");
                var stationInfos = SortingStationInfoContract.GetSortingStationInfos(Convert.ToInt32(BaseArg));
                if (stationInfos.Count == 0)
                {
                    string msg = $"找不到站台【{Convert.ToInt32(BaseArg)}】信息";
                    LogContract.InsertWcsErrorLog(msg);
                    return;
                }
                var stationInfo = stationInfos[0];

                if (stationInfo.IsOuting == false)
                {
                    Line.ClearStackOuting(BaseArg);
                    return;
                }

                stationInfo.IsOuting = false;
                stationInfo.Color = "";
                stationInfo.BookCount = 0;
                stationInfo.PilerNo = 0;
                stationInfo.HasUpBoard = false;

                //if (BaseArg == ESortingStation.SortingStation2003)//拣选工位的出托完成，其他工位会可能拣选料所以不能做判断
                //{
                //    var bookCount = Line.ReadStationBoardCount(BaseArg);
                //    if (bookCount == 0) //机械手的数量清空则认为已经出垛完成
                //    {
                //        stationInfo.IsOuting = false;
                //        stationInfo.Color = "";
                //        stationInfo.BookCount = 0;
                //        stationInfo.PilerNo = 0;
                //        stationInfo.HasUpBoard = false;
                //    }
                //    else
                //    {
                //        LogContract.InsertWcsErrorLog($"机械手板件数量未清空!");
                //    }
                //}
                //else
                //{
                //    stationInfo.IsOuting = false;
                //    stationInfo.Color = "";
                //    stationInfo.BookCount = 0;
                //    stationInfo.PilerNo = 0;
                //    stationInfo.HasUpBoard = false;
                //}
                var writeSuccess = Line.WriteBoardCountToStation(BaseArg, 0);
                if (!writeSuccess)
                {
                    LogContract.InsertWcsErrorLog($"清空工位{BaseArg}板材数量失败");
                    return;
                }

                var result = SortingStationInfoContract.UpdatedSortingStationInfo(stationInfo);
                if (!result)
                {
                    Thread.Sleep(20);
                    result = SortingStationInfoContract.UpdatedSortingStationInfo(stationInfo);
                }
                LogContract.InsertWcsLog($"站台【{Convert.ToInt32(BaseArg)}】入库时，出垛完成");
                if (!Line.ClearStackOuting(BaseArg))
                {
                    Thread.Sleep(20);
                    Line.ClearStackOuting(BaseArg);
                }
                LogContract.InsertWcsLog($"清除Plc出垛完成信号");
                
            }
            catch (Exception e)
            {
                LogContract.InsertWcsErrorLog(GetType() + e.Message);
            }
            
        }
    }
}
