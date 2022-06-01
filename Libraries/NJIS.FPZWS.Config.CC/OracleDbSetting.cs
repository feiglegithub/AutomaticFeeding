﻿//  ************************************************************************************
//   解决方案：NJIS.FPZWS.PDD.DataGenerate
//   项目名称：NJIS.FPZWS.Config.CC
//   文 件 名：OracleDbSetting.cs
//   创建时间：2018-09-25 17:05
//   作    者：
//   说    明：
//   修改时间：2018-09-25 17:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.Config.CC
{
    public class OracleDbSetting : ConfigWapper<OracleDbSetting>
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Pwd { get; set; }

        public int Timeout { get; set; } = 0;

        public int Port { get; set; }
        public override string Path { get; protected set; } = "数据库连接/ORACLE/OracleDbSetting";

        public string OracleConnectString =>
            $"DATA SOURCE = {Server}:{Port}/{Database};PASSWORD={Pwd};PERSIST SECURITY INFO=True;USER ID = {UserId};";
    }
}