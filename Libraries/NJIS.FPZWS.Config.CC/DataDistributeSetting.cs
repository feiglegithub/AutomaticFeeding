//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：DataDistributeSetting.cs
//   创建时间：2018-09-25 17:05
//   作    者：
//   说    明：
//   修改时间：2018-09-25 17:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Collections.Generic;

namespace NJIS.FPZWS.Config.CC
{
    public class DataDistributeSetting : ConfigWapper<DataDistributeSetting>
    {
        private readonly string ss =
            "InspectionPart,CuttingStackList,CuttingSawFile,Drilling,Edgebanding,Buffer2,Buffer1,SortingPart,PackagingPart,PartInfo,MixPackingRule,PackingRule,PackageDetail,Package,PackageOrder,Mdb_Boards,Mdb_Cuts,Mdb_Header,Mdb_Jobs,Mdb_Materials,Mdb_Notes,Mdb_Offcuts,Mdb_Parts_Dst,Mdb_Parts_Req,Mdb_Parts_Udi,Mdb_Patterns,DrillingMpr";

        public DataDistributeSetting()
        {
            DepCode = "51005";
            Datas = ss;
        }

        public string DepCode { get; set; } = "51005";

        public string DataFile { get; set; }

        public string HttpServerIpAddress { get; set; }

        public int HttpServerPort { get; set; }

        public string Datas { get; set; }

        public string ResultMsgTopic { get; set; } = "/sfy/pdd/tc/distribution/ResultMsgTopic";


        public override string Path { get; protected set; } = "生产数据/DataDistributeSetting";
    }
}