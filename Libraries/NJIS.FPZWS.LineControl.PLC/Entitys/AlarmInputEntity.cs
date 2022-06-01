//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：AlarmInputEntity.cs
//   创建时间：2018-12-14 14:39
//   作    者：
//   说    明：
//   修改时间：2018-12-14 14:39
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.PLC.Entitys
{
    public class AlarmInputEntity<T> : EntityBase
    {
        public string ParamName { get; set; }
        public bool ParamValue { get; set; }

        public bool Value { get; set; }
        public string AlarmDescribe { get; set; }

        public virtual bool AlarmTrigger()
        {
            return Value.Equals(ParamValue);
        }
    }
}