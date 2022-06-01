//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//   文 件 名：ChainBufferUserControl.cs
//   创建时间：2018-12-04 14:05
//   作    者：
//   说    明：
//   修改时间：2018-12-04 14:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Collections.Generic;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.UI.Common;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    public partial class ChainBufferUserControl : UserControl
    {
        public ChainBufferUserControl():this(new ChainBufferArgs())
        {
            InitializeComponent();
        }


        public ChainBufferUserControl(ChainBufferArgs data)
        {
            InitializeComponent();
            rgvMain.ShowRowNumber();
            Data = data;
            rtxtCode.DataBindings.Add("Text", Data, "Code");
            rtxtRemark.DataBindings.Add("Text", Data, "Remark");
            rtxtSize.DataBindings.Add("Text", Data, "Size");
            rtxtStatus.DataBindings.Add("Text", this, "StatusText");
            UpdateSetPartInfo(Data.Parts);
        }

        public void UpdateSetPartInfo(List<PartInfoArgs> datas)
        {
            rgvMain.Rows.Clear();
            foreach (var partInfoArgse in datas)
            {
                var dgr = new GridViewDataRowInfo(rgvMain.MasterView);
                dgr.Cells["PartId"].Value = partInfoArgse.PartId;
                dgr.Cells["BatchName"].Value = partInfoArgse.BatchName;
                dgr.Cells["OrderNumber"].Value = partInfoArgse.OrderNumber;
                dgr.Cells["Length"].Value = partInfoArgse.Length;
                dgr.Cells["Width"].Value = partInfoArgse.Width;
                rgvMain.Rows.Add(dgr);
            }
        }

        public string StatusText => Data.Status + "";

        public ChainBufferArgs Data { get; set; }
    }
}