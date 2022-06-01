//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：PlcVariant.cs
//   创建时间：2018-11-20 14:41
//   作    者：
//   说    明：
//   修改时间：2018-11-20 14:41
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.PLC
{
    public class PlcVariant
    {
        public string Name { get; set; }
        public PlcValType Type { get; set; }

        public string Desc { get; set; }

        public object Value { get; set; }

        public override bool Equals(object obj)
        {
            return obj != null && string.Equals(Name, obj.ToString());
        }

        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }

        protected bool Equals(PlcVariant other)
        {
            return string.Equals(Name, other.Name);
        }
    }

    public class PlcVariant<T> : PlcVariant
    {
        public PlcVariant(string name)
        {
            Name = name;
        }


        public PlcVariant(string name, T val, PlcValType type)
        {
            Name = name;
            Value = val;
            Type = type;
        }

        public PlcVariant(string name, T val)
        {
            Name = name;
            Value = val;
            Type = PlcValType.Bit;
        }

        public new T Value { get; set; }
    }
}