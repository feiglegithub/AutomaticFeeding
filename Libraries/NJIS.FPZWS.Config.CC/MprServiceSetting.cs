//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：MprServiceSetting.cs
//   创建时间：2018-09-25 17:05
//   作    者：
//   说    明：
//   修改时间：2018-09-25 17:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.Config.CC
{
    public class MprServiceSetting : ConfigWapper<MprServiceSetting>
    {
        public MprServer Server1 { get; set; }
        public MprServer Server2 { get; set; }
        public override string Path { get; protected set; } = "工具/MprServiceSetting";
    }

    public class MprServer
    {
        public string Name { get; set; }
        public string ServicePath { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
    }
}