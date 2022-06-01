//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：SplashScreenForm.cs
//   创建时间：2018-11-30 10:46
//   作    者：
//   说    明：
//   修改时间：2018-11-30 10:46
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Windows.Forms;

namespace NJIS.FPZWS.LineControl.Drilling.Client
{
    public partial class SplashScreenForm : Form, ISplashForm
    {
        public SplashScreenForm()
        {
            InitializeComponent();
        }

        //实现接口方法，主要用于接口的反射调用

        #region ISplashForm

        void ISplashForm.SetStatusInfo(string newStatusInfo)
        {
            lbStatusInfo.Text = newStatusInfo;
        }

        #endregion
    }
}