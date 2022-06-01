// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.DataManager
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：ComboxHelper.cs
//  创建时间：2018-07-23 17:59
//  作    者：
//  说    明：
//  修改时间：2017-12-25 9:07
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Windows.Forms;

#endregion

namespace NJIS.FPZWS.UI.Common
{
    public static class ComboxHelper
    {
        public static void Bind<T>(this ComboBox combox, IList<T> list, string displayMember, string valueMember,
            string displayText)
        {
            AddItem(list, displayMember, displayText);
            combox.DataSource = list;
            combox.DisplayMember = displayMember;
            if (!string.IsNullOrEmpty(valueMember))
                combox.ValueMember = valueMember;
        }

        public static void InvokeExecute(this Control ctl, Action action)
        {
            if (ctl.IsHandleCreated) ctl.Invoke(new Action(action));
        }

        private static void AddItem<T>(IList<T> list, string displayMember, string displayText)
        {
            object obj = Activator.CreateInstance<T>();
            var type = obj.GetType();
            if (!string.IsNullOrEmpty(displayMember) && !string.IsNullOrEmpty(displayText))
            {
                var displayProperty = type.GetProperty(displayMember);
                displayProperty.SetValue(obj, displayText, null);
            }

            list.Insert(0, (T) obj);
        }
    }
}