//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Config
//   文 件 名：EntityElement.cs
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
    public class EntityElement : ConfigurationElement
    {
        private const string InputKey = "input";
        private const string OutputKey = "output";

        [ConfigurationProperty(InputKey, IsRequired = false)]
        public virtual InputElement Input
        {
            get { return (InputElement)this[InputKey]; }
            set { this[InputKey] = value; }
        }

        [ConfigurationProperty(OutputKey, IsRequired = false)]
        public virtual OutputElement Output
        {
            get { return (OutputElement)this[OutputKey]; }
            set { this[OutputKey] = value; }
        }
    }
}