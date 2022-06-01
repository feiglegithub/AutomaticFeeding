// ************************************************************************************
//  解决方案：NJIS.FPZWS.Sorting.Client
//  项目名称：NJIS.Tools.Client
//  文 件 名：Config.cs
//  创建时间：2017-11-02 16:39
//  作    者：
//  说    明：
//  修改时间：2017-11-03 8:40
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using NJIS.AppUtility.Config;

#endregion

namespace NJIS.Tools.Client
{
    public class Config : ConfigurationBase<Config>
    {
        public Config() : base("App.Settings")
        {
        }

        public string LanguageCode
        {
            get { return Get("LanguageCode", "zh-cn"); }
        }

        //public string BundleFolder { get { return Get("BundleFolder", "bundle"); } }

        public string Theme
        {
            get { return Get("Theme", "Aqua"); }
            set { Set("Theme", value); }
        }

        public string PluginsXml
        {
            get { return Get("Plugins", "Plugins.xml"); }
        }


        public string MachineCode
        {
            get { return Get("MachineCode", "NJIS.SORTING.WORKSTATION"); }
            set { Set("MachineCode", value); }
        }
    }
}