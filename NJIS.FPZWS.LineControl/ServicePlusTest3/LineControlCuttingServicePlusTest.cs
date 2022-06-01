using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Manager.Service.Utils;

namespace ServicePlusTest3
{
    [TestClass]
    public class UnitTest1
    {
        LineControlCuttingServicePlus lineControlCuttingServicePlus = new LineControlCuttingServicePlus();

        [TestMethod]
        public void TestGetPartFeedBacksByPartId()
        {
            List<PartFeedBack> list = lineControlCuttingServicePlus.GetPartFeedBacksByPartId("1179954631002");
            Console.WriteLine(list.Count);
        }

        [TestMethod]
        public void TestGetStackBordCount()
        {
            string batchName = "HB_RX210927A1-6(1)";

            //已经下发给1-6号锯的垛
            string stackNames = PublicUtils.getStackName(lineControlCuttingServicePlus, batchName);

            List<CuttingStackList> listCuttingStackList = lineControlCuttingServicePlus.
                            GetMinStackProductIndexCuttingStackList(batchName, stackNames, CuttingStackListBatchType.AUTO);
            int requestCount = 0;
            CuttingStackList cuttingStackList = listCuttingStackList[0];
            string stackName = cuttingStackList.StackName;

            List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationByStackName(stackName, SawType.TYPE1);
            foreach (var item in listCuttingSawFileRelation)
            {
                requestCount += item.BoardCount;
            }
            Console.WriteLine(requestCount);
        }
    }
}
