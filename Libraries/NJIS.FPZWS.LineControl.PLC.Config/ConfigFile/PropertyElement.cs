//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：PropertyElement.cs
//   创建时间：2018-11-23 17:14
//   作    者：
//   说    明：
//   修改时间：2018-11-23 17:14
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System.Configuration;

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Config.ConfigFile
{
    public class PropertyElement : ConfigurationElement
    {
        private const string NameKey = "name";
        private const string MapKey = "map";
        private const string IsMapKey = "isMap";
        private const string DescKey = "desc";
        private const string LengthKey = "length";
        private const string ValueTypeKey = "valueType";
        private const string IsCheckKey = "isCheck";
        private const string WriteIndexKey = "writeIndex";

        [ConfigurationProperty(IsCheckKey, IsRequired = false,DefaultValue = false)]
        public virtual bool IsCheck
        {
            get { return (bool)this[IsCheckKey]; }
            set { this[IsCheckKey] = value; }
        }

        [ConfigurationProperty(WriteIndexKey, IsRequired = false,DefaultValue = -1)]
        public virtual int WriteIndex
        {
            get { return (int)this[WriteIndexKey]; }
            set { this[WriteIndexKey] = value; }
        }

        [ConfigurationProperty(NameKey, IsRequired = true)]
        public virtual string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        [ConfigurationProperty(MapKey, IsRequired = true)]
        public virtual string Map
        {
            get { return (string)this[MapKey]; }
            set { this[MapKey] = value; }
        }

        [ConfigurationProperty(ValueTypeKey, IsRequired = false, DefaultValue = "Int")]
        public virtual string ValueType
        {
            get { return (string)this[ValueTypeKey]; }
            set { this[ValueTypeKey] = value; }
        }


        [ConfigurationProperty(DescKey, IsRequired = false)]
        public virtual string Desc
        {
            get { return (string)this[DescKey]; }
            set { this[DescKey] = value; }
        }

        [ConfigurationProperty(IsMapKey, IsRequired = false, DefaultValue = true)]
        public virtual bool IsMap
        {
            get { return bool.Parse(this[IsMapKey].ToString()); }
            set { this[IsMapKey] = value; }
        }

        [ConfigurationProperty(LengthKey, IsRequired = false, DefaultValue = 256)]
        public virtual int Length
        {
            get { return int.Parse(this[LengthKey].ToString()); }
            set { this[LengthKey] = value; }
        }
    }
}