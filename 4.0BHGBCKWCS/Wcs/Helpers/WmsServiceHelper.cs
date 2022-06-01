using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Common;
using WCS.Interfaces;
using WCS.WebServiceDemo;

namespace WCS.Helpers
{
    public class WmsServiceHelper:Singleton<WmsServiceHelper>,IWms
    {
        private WmsServiceHelper() { }

        private ResultMsg WmsRequest(WMSInParams param)
        {
            using (var client = new WMSServiceSoapClient())
            {
                return client.ApplyWMSTask(param);
            }
        }

        public ResultMsg ApplyEmptyBoard(int boardCount,int stationNo=2005)
        {
            return WmsRequest(new WMSInParams()
            {
                Count = boardCount,
                ToStation = stationNo,
                ProductCode = "空垫板",
                ReqType = 2

            });

            
        }

        public ResultMsg ApplySortingInStock(long taskId, int fromStation)
        {
            return WmsRequest(new WMSInParams()
            {
                TaskId = taskId,
                FromStation = fromStation,
                ReqType = 1

            });
        }

        public ResultMsg ApplyEmptyBoardInStock(int boardCount, int fromStation=2005)
        {
            return WmsRequest(new WMSInParams()
            {
                Count = boardCount,
                FromStation = fromStation,
                ReqType = 3
            });
        }

        public ResultMsg ApplyBoardReenterStock(int boardCount, int fromStation, int stackNo)
        {
            return WmsRequest(new WMSInParams()
            {
                Count = boardCount,
                FromStation = fromStation,
                PilerNo = stackNo,
                ReqType = 4

            });
        }

        public ResultMsg ApplySortingMaterial(int boardCount, int toStation, string productCode)
        {
            return WmsRequest(new WMSInParams()
            {
                Count = boardCount,
                ToStation = toStation,
                ProductCode=productCode,
                ReqType = 2
            });
        }
    }
}
