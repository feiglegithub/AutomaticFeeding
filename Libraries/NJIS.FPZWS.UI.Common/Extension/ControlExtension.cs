// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.DataManager
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：ControlExtension.cs
//  创建时间：2018-07-23 17:59
//  作    者：
//  说    明：
//  修改时间：2018-04-30 7:37
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Windows.Forms;

namespace NJIS.FPZWS.UI.Common.Extension
{
    public static class ControlExtension
    {
        public static WaitExecutor Run(this Control parent, Action action, Action<object> callBack = null,
            string message = "加载中..")
        {
            if (callBack != null)
            {
                var waitExecutorCallBack = new WaitExecutor(action, callBack);
                parent.Invoke(new Action(() => { WaitForm.ShowWait(parent, action, callBack, message); }));
                return waitExecutorCallBack;
            }

            var waitExecutor = new WaitExecutor(action);
            parent.Invoke(new Action(() => { WaitForm.ShowWait(parent, waitExecutor, message); }));
            return waitExecutor;
        }
    }
}