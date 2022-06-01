//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：PropertyCollection.cs
//   创建时间：2018-11-12 17:01
//   作    者：
//   说    明：
//   修改时间：2018-11-12 17:01
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System.Configuration;

#endregion

namespace NJIS.FPZWS.LineControl.PLC.Config.ConfigFile
{
    public class PropertyCollection : ConfigurationElementCollection
    {
        private const string PropertyKey = "property";

        public PropertyCollection()
        {
            AddElementName = PropertyKey;
        }

        protected override string ElementName => PropertyKey;

        protected override ConfigurationElement CreateNewElement()
        {
            return new PropertyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PropertyElement) element).Name;
        }
    }
}