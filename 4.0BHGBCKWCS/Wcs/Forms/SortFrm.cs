using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Contract;
using WcsModel;
using WcsService;
using WCS.Communications;
using WCS.DataBase;
using WCS.Helpers;
using WCS.Interfaces;
using WCS.Mod;
using WCS.model;
using WCS.OPC;

namespace WCS.Forms
{
    public partial class SortFrm : Form
    {
        private ISortingStationInfoContract _sortingStationInfoContract = null;

        private ISortingStationInfoContract SortingStationInfoContract =>
            _sortingStationInfoContract ?? (_sortingStationInfoContract =  SortingStationInfoService.GetInstance());

        private LineCommunication _communication = null;
        private LineCommunication Line => _communication ?? (_communication = LineCommunication.GetInstance());

        private MachineHandCommunication _handCommunication = null;

        private MachineHandCommunication Hand =>
            _handCommunication ?? (_handCommunication = MachineHandCommunication.GetInstance());

        private IWms wms = WmsServiceHelper.GetInstance();

        ESortingStation[] sortingStations = new ESortingStation[]
        {
            ESortingStation .SortingStation2001,
            ESortingStation .SortingStation2002,
            ESortingStation .SortingStation2003,
            ESortingStation .SortingStation2004,
            ESortingStation .SortingStation2005
        };
        int[] array = { 2001, 2002, 2003, 2004, 2005 };
        //CancellationTokenSource cts = new CancellationTokenSource();
        public SortFrm()
        {
            InitializeComponent();
            //this.Load += SortFrm_Load;
            this.Activated += SortFrm_Activated;
            this.Deactivate += SortFrm_Deactivate;
            AutoScales(this);
        }

        private void SortFrm_Deactivate(object sender, EventArgs e)
        {
            //cts.Cancel();
            this.timer1.Enabled = false;           
        }

        private void SortFrm_Activated(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }


        #region 控件大小随像素分辨率自适应
        //这里是让所有的控件随窗体的变化而变化
        public void AutoScales(Form frm)
        {
            frm.Tag = frm.Width.ToString() + "," + frm.Height.ToString();
            frm.SizeChanged += new EventHandler(MainFrm_SizeChanged);
        }

        void MainFrm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                string[] tmp = new string[2];
                tmp = ((Form)sender).Tag.ToString().Split(',');
                if ((int)((Form)sender).Width > 200)
                {
                    float width = (float)((Form)sender).Width / (float)Convert.ToInt16(tmp[0]);
                    float heigth = (float)((Form)sender).Height / (float)Convert.ToInt16(tmp[1]);

                    ((Form)sender).Tag = ((Form)sender).Width.ToString() + "," + ((Form)sender).Height;

                    foreach (Control control in ((Form)sender).Controls)
                    {
                        control.Scale(new SizeF(width, heigth));
                    }
                }
            }
            catch
            {
            }

        }
        #endregion

        public void ShowSortInfo()
        {
            try
            {
                foreach (var station in sortingStations)
                {
                    var count = Line.ReadStationBoardCount(station);
                    var isOutingFinished = Line.StackIsOutingFinished(station);
                    var str = isOutingFinished ? "是" : "否";
                    if (station == ESortingStation.SortingStation2001)
                    {
                        if (count == 0)
                        {
                            ClearStation(station.GetHashCode());
                        }
                        else
                        {
                            Paints(pl2001, count);
                            this.lbC2001.Text = count.ToString();
                            this.lbPC2001.Text = OpcHsc.ReadStaionProductCode(2001);
                            
                        }
                        lbPlc2001Out.Text = str;
                    }
                    else if (station == ESortingStation.SortingStation2002)
                    {
                        if (count == 0)
                        {
                            ClearStation(station.GetHashCode());
                        }
                        else
                        {
                            Paints(pl2002, count);
                            this.lbC2002.Text = count.ToString();
                            this.lbPC2002.Text = OpcHsc.ReadStaionProductCode(2002);

                        }
                        lbPlc2002Out.Text = str;
                    }
                    else if (station == ESortingStation.SortingStation2003)
                    {
                        if (count == 0)
                        {
                            ClearStation(station.GetHashCode());
                        }
                        else
                        {
                            Paints(pl2003, count);
                            this.lbC2003.Text = count.ToString();
                        }
                        lbPlc2003Out.Text = str;
                    }
                    else if (station == ESortingStation.SortingStation2004)
                    {
                        if (count == 0)
                        {
                            ClearStation(station.GetHashCode());
                        }
                        else
                        {
                            Paints(pl2004, count);
                            this.lbC2004.Text = count.ToString();
                            this.lbPC2004.Text = OpcHsc.ReadStaionProductCode(2004);
                        }
                        lbPlc2004Out.Text = str;
                    }
                    else if (station == ESortingStation.SortingStation2005)
                    {
                        if (count == 0)
                        {
                            ClearStation(station.GetHashCode());
                        }
                        else
                        {
                            Paints(pl2005, count);
                            this.lbC2005.Text = count.ToString();
                        }
                        lbPlc2005Out.Text = str;

                    }
                }

                #region old code
                //foreach (int no in array)
                //{
                //    var c = OpcHsc.ReadBoradsCount(no);
                //    if (c == 0)
                //    {
                //        ClearStation(no);
                //    }
                //    else
                //    {
                //        if (no == 2001)
                //        {
                //            Paints(pl2001, c);
                //            this.lbC2001.Text = c.ToString();
                //            this.lbPC2001.Text = OpcHsc.ReadStaionProductCode(2001);
                //        }
                //        else if (no == 2002)
                //        {
                //            Paints(pl2002, c);
                //            this.lbC2002.Text = c.ToString();
                //            this.lbPC2002.Text = OpcHsc.ReadStaionProductCode(2002);
                //        }
                //        else if (no == 2003)
                //        {
                //            Paints(pl2003, c);
                //            this.lbC2003.Text = c.ToString();
                //        }
                //        else if (no == 2004)
                //        {
                //            Paints(pl2004, c);
                //            this.lbC2004.Text = c.ToString();
                //            this.lbPC2004.Text = OpcHsc.ReadStaionProductCode(2004);
                //        }
                //        else if (no == 2005)
                //        {
                //            Paints(pl2005, c);
                //            this.lbC2005.Text = c.ToString();
                //        }
                //    }
                //}
                #endregion

            }
            catch { }
        }

        private void ShowWCSCount()
        {
            var stationInfos = SortingStationInfoContract.GetSortingStationInfos();
            var item1 = stationInfos.First(item => item.StationNo == 2001);
            lb2001.Text = (item1.HasUpBoard? item1.BookCount+1: item1.BookCount).ToString();
            lb2001OutStackStatus.Text = item1.IsOuting ? "是" : "否";

            item1 = stationInfos.First(item => item.StationNo == 2002);
            lb2002.Text = (item1.HasUpBoard ? item1.BookCount + 1 : item1.BookCount).ToString();
            lb2002OutStackStatus.Text = item1.IsOuting ? "是" : "否";

            item1 = stationInfos.First(item => item.StationNo == 2003);
            lb2003.Text = (item1.HasUpBoard ? item1.BookCount + 1 : item1.BookCount).ToString();
            lb2003OutStackStatus.Text = item1.IsOuting ? "是" : "否";

            item1 = stationInfos.First(item => item.StationNo == 2004);
            lb2004.Text = (item1.HasUpBoard ? item1.BookCount + 1 : item1.BookCount).ToString();
            lb2004OutStackStatus.Text = item1.IsOuting ? "是" : "否";

            item1 = stationInfos.First(item => item.StationNo == 2005);
            lb2005OutStackStatus.Text = item1.IsOuting ? "是" : "否";
        }

        private void ClearStation(int no)
        {
            if (no == 2001)
            {
                Paints(this.pl2001, 0);
                this.lbC2001.Text = "0";
                this.lbPC2001.Text = "";
            }
            else if(no == 2002)
            {
                Paints(this.pl2002, 0);
                this.lbC2002.Text = "0";
                this.lbPC2002.Text = "";
            }
            else if (no == 2003)
            {
                Paints(this.pl2003, 0);
                this.lbC2003.Text = "0";
                //this.lbPC2003.Text = "";
            }
            else if (no == 2004)
            {
                Paints(this.pl2004, 0);
                this.lbC2004.Text = "0";
                this.lbPC2004.Text = "";
            }
            else if (no == 2005)
            {
                Paints(this.pl2005, 0);
                this.lbC2005.Text = "0";
                this.lbPC2005.Text = "空垫板";
            }
        }

        private void Paints(Panel pl, int count)
        {
            foreach (Control c in pl.Controls)
            {
                if (c is Button)
                {
                    if (int.Parse(c.Tag.ToString()) > count)
                    {
                        c.Visible = false;
                    }
                    else
                    {
                        c.Visible = true;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ShowSortInfo();
            ShowWCSCount();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            int num1;
            if (!string.IsNullOrEmpty(this.cboStation.Text.Trim()))
            {
                if (int.TryParse(txtInAmout.Text, out num1))
                {
                    if (num1 < 0)
                    {
                        return;
                    }
                    else if (num1 == 0)
                    {
                        OpcHsc.ClearSortStation(int.Parse(this.cboStation.Text));
                    }
                    else
                    {
                        OpcHsc.WriteBoradsCountToStaion(int.Parse(this.cboStation.Text), num1);
                    }
                }
            }
        }

        
        private void button6_Click(object sender, EventArgs e)
        {
            var machineTestForm = new MachineHandTestForm();
            machineTestForm.Owner = this;
            machineTestForm.Show();
        }

        private void btnOutFinished_Click(object sender, EventArgs e)
        {
            var station = int.Parse(cboStation.Text.Trim());
            ESortingStation eSortingStation;
            var ret = Enum.TryParse(station.ToString(), out eSortingStation);
            if (ret)
            {
                Line.SetSortingStationOut(eSortingStation);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Hand.ClearMhTask();
        }

        private void btnRequestMaterial_Click(object sender, EventArgs e)
        {
            int boardCount = 0;
            var ret = int.TryParse(txtNeedCount.Text.Trim(), out boardCount);
            if(!ret || boardCount<1) return;
            var color = txtColor.Text.Trim();
            if(string.IsNullOrWhiteSpace(color)) return;

            var station = int.Parse(cmbTarget.Text.Trim());

            var result =  wms.ApplySortingMaterial(boardCount, station, color);
            if (result.Status == 200)
            {
                MessageBox.Show($"请求号：{result.ReqId},要料成功!");
            }
            else
            {
                MessageBox.Show("要料失败!");
            }
        }
    }
}
