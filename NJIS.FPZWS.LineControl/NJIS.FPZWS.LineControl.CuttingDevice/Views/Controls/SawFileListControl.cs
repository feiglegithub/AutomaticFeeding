using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.UI.Common.Controls;
using System.Threading;

namespace NJIS.FPZWS.LineControl.CuttingDevice.Views.Controls
{
    public partial class SawFileListControl : UserControl
    {
        LineControlCuttingServicePlus lineControlCuttingServicePlus;

        public SawFileListControl()
        {
            InitializeComponent();

            lineControlCuttingServicePlus = new LineControlCuttingServicePlus();
            ViewInit();
        }

        private void ViewInit()
        {
            try
            {
                List<DeviceInfos> listDeviceInfos = lineControlCuttingServicePlus.GetEnableCuttingDeviceInfos();
                List<string> listDeviceName = new List<string>();
                foreach (var item in listDeviceInfos)
                {
                    listDeviceName.Add(item.DeviceName);
                }

                CuttingSawFileRelationPlus sawFile;
                gridViewBase1.AddColumns(new List<ColumnInfo>(){
                new ColumnInfo(50){ HeaderText="Id", FieldName=nameof(sawFile.Id), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="计划日期", FieldName=nameof(sawFile.PlanDate), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="批次号", FieldName=nameof(sawFile.BatchName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="垛号", FieldName=nameof(sawFile.StackName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="SAW文件序号", FieldName=nameof(sawFile.StackIndex), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="叠板数量", FieldName=nameof(sawFile.BoardCount), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="Saw文件名", FieldName=nameof(sawFile.SawFileName), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="Saw文件类型", FieldName=nameof(sawFile.SawType), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="创建时间", FieldName=nameof(sawFile.CreatedTime), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(50){ HeaderText="更新时间", FieldName=nameof(sawFile.UpdatedTime), DataType=typeof(string), ReadOnly=true },
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="状态", FieldName=nameof(sawFile.Status), DataType=typeof(int), ReadOnly=true
                    ,DataSource = CuttingSawFileRelationPlusStatus.Unassigned.GetAllFinishStatusDescription()
                    ,DisplayMember = "Item2",ValueMember = "Item1"},
                new ColumnInfo(EColumnType.ComboBox)
                { HeaderText="开料设备", FieldName=nameof(sawFile.DeviceName), DataType=typeof(string), ReadOnly=true
                    ,DataSource = listDeviceName}
            });
            }
            catch (Exception)
            {
                //预防服务器重启或者关机无法访问数据库而卡死
            }
            
        }

        private void fillData()
        {
            try
            {
                List<CuttingSawFileRelationPlus> listCuttingSawFileRelation = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlusByDeviceNameAndCreatedTime(CuttingSetting.Current.CurrDeviceName, DateTime.Now);

                gridViewBase1.Invoke(new Action(delegate () {
                    gridViewBase1.DataSource = listCuttingSawFileRelation;
                }));

                //Thread.Sleep(int.Parse(ConfigurationSettings.AppSettings["sleepTime"]));
                Thread.Sleep(CuttingSetting.Current.RefreshTime);

                fillData();
            }
            catch (Exception)
            {

                //预防服务器重启或者关机无法访问数据库而卡死
            }
        }

        private void SawFileListControl_Enter(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(fillData)).Start();
        }
    }
}
