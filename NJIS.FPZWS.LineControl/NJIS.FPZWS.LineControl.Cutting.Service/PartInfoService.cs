using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Repository;

namespace NJIS.FPZWS.LineControl.Cutting.Service
{
    public class PartInfoService
    {
        public List<PartInfo> GetInfos(string upi)
        {
            PartInfoRepository rep = new PartInfoRepository();
            return rep.FindAll(item => item.PartId == upi).ToList();
        }

        public List<PartInfo> GetInfos(DateTime planDate)
        {
            PartInfoRepository rep = new PartInfoRepository();
            return rep.FindAll(item => item.ProductionDate == planDate && !string.IsNullOrWhiteSpace(item.BatchCode)).ToList();
        }

        public List<PartInfo> GetInfosByBatchName(string batchCode)
        {
            PartInfoRepository rep = new PartInfoRepository();
            return rep.FindAll(item => item.BatchCode == batchCode).ToList();
        }

        public List<TaskDistribute> GeTaskDistributes(string upi)
        {
            TaskDistributeRepository rep = new TaskDistributeRepository();
            string selectSql = " SELECT * FROM [MesDataCenter].[dbo].[TaskDistribute] WHERE [TaskDistributeId] IN " +
                               "(SELECT [TaskDistributeId] FROM [DC_ProductionData].[dbo].[Mdb_Offcuts] " +
                               $"WHERE[CODE] = '{upi}' GROUP BY [TaskDistributeId])";
            return rep.QueryList(selectSql, null).ToList();
        }
    }
}
