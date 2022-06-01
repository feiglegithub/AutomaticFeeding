//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：OutputElement.cs
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
    public class OutputElement : ConfigurationElement
    {
        private const string EntityKey = "entity";
        private const string PropertysKey = "propertys";

        [ConfigurationProperty(EntityKey, IsRequired = true)]
        public virtual string Entity
        {
            get {return this[EntityKey].ToString(); }
            set { this[EntityKey] = value; }
        }

        [ConfigurationProperty(PropertysKey, IsRequired = true)]
        public virtual PropertyCollection Propertys
        {
            get { return (PropertyCollection)this[PropertysKey]; }
            set { this[PropertysKey] = value; }
        }
    }
}