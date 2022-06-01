using Microsoft.VisualStudio.TestTools.UnitTesting;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace ServicePlusTest
{
    [TestClass]
    public class UnitTest1
    {
        LineControlCuttingServicePlus lineControlCuttingServicePlus = new LineControlCuttingServicePlus();

        [TestMethod]
        public void TestGetCuttingSawFileRelationPlusByBatchStackName()
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.GetCuttingSawFileRelationPlusByBatchStackName("1");
            int count = listCuttingSawFileRelationPlus.Count;
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void Test()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
