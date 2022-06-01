//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：AlarmConfig.cs
//   创建时间：2018-12-26 8:26
//   作    者：
//   说    明：
//   修改时间：2018-12-26 8:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

#endregion

namespace NJIS.FPZWS.LineControl.Sorting.Model
{
    public class AlarmConfig
    {
        public int LineId { get; set; }
        public string Roller { get; set; }
        public string MachineType { get; set; }
        public string ParamName { get; set; }
        public int Tag { get; set; }
        public string Message { get; set; }
        public int ProductionLine { get; set; }
    }
}