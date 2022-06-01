//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：StringResources.cs
//   创建时间：2018-11-08 16:22
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:22
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Globalization;
using NJIS.PLC.Communication.Language;

namespace NJIS.PLC.Communication
{
    /*******************************************************************************
     * 
     *    用于显示和保存的数据信息，未来支持中英文
     *
     *    Used to the return result class in the synchronize communication and communication for industrial Ethernet
     * 
     *******************************************************************************/


    /// <summary>
    ///     系统的字符串资源及多语言管理中心 ->
    ///     System string resource and multi-language management Center
    /// </summary>
    public static class StringResources
    {
        /// <summary>
        ///     获取或设置系统的语言选项 ->
        ///     Gets or sets the language options for the system
        /// </summary>
        public static DefaultLanguage Language = new DefaultLanguage();

        #region Constractor

        static StringResources()
        {
            if (CultureInfo.CurrentCulture.ToString().StartsWith("zh"))
            {
                SetLanguageChinese();
            }
            else
            {
                SeteLanguageEnglish();
            }
        }

        #endregion

        /// <summary>
        ///     将语言设置为中文 ->
        ///     Set the language to Chinese
        /// </summary>
        public static void SetLanguageChinese()
        {
            Language = new DefaultLanguage();
        }

        /// <summary>
        ///     将语言设置为英文 ->
        ///     Set the language to English
        /// </summary>
        public static void SeteLanguageEnglish()
        {
            Language = new English();
        }
    }
}