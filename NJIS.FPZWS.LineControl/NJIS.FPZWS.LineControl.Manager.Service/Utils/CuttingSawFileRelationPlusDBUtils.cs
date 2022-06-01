using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Manager.Service.Utils
{
    public class CuttingSawFileRelationPlusDBUtils
    {
        /// <summary>
        /// 新增一条已分配数据
        /// </summary>
        /// <returns></returns>
        public static bool AddAssignedByCuttingSawFileRelation(LineControlCuttingServicePlus 
            lineControlCuttingServicePlus, CuttingSawFileRelation cuttingSawFileRelation)
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = new List<CuttingSawFileRelationPlus>();
            
            CuttingSawFileRelationPlus cuttingSawFileRelationPlus = new CuttingSawFileRelationPlus();
            cuttingSawFileRelationPlus.BatchName = cuttingSawFileRelation.BatchName;
            cuttingSawFileRelationPlus.StackName = cuttingSawFileRelation.StackName;
            cuttingSawFileRelationPlus.BoardCount = cuttingSawFileRelation.BoardCount;
            //cuttingSawFileRelationPlus.CreatedTime = cuttingSawFileRelation.CreatedTime;
            cuttingSawFileRelationPlus.CreatedTime = DateTime.Now;
            //cuttingSawFileRelationPlus.Id = cuttingSawFileRelation.Id;
            cuttingSawFileRelationPlus.PlanDate = cuttingSawFileRelation.PlanDate;
            cuttingSawFileRelationPlus.SawFile = cuttingSawFileRelation.SawFile;
            cuttingSawFileRelationPlus.SawFileName = cuttingSawFileRelation.SawFileName;
            cuttingSawFileRelationPlus.StackIndex = cuttingSawFileRelation.StackIndex;
            cuttingSawFileRelationPlus.SawType = cuttingSawFileRelation.SawType;
            cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Assigned;
            cuttingSawFileRelationPlus.TaskDistributeId = cuttingSawFileRelation.TaskDistributeId;
            cuttingSawFileRelationPlus.TaskId = cuttingSawFileRelation.TaskId;
            cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;

            listCuttingSawFileRelationPlus.Add(cuttingSawFileRelationPlus);
            return lineControlCuttingServicePlus.BulkInsertCuttingSawFileRelationPlus(listCuttingSawFileRelationPlus);
        }
    }
}
