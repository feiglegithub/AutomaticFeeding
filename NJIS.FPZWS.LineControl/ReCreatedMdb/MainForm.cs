using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.DomainPlus.Mdb;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ReCreatedMdb
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;
        }

        private void btnUpdated_Click(object sender, EventArgs e)
        {
            ILineControlCuttingContractPlus contract = new LineControlCuttingServicePlus();

            var startPlanDate = dtpStart.Value.Date;
            var endPlanDate = dtpEnd.Value.Date;
            if(startPlanDate>endPlanDate) return;
            int i = 0;
            while (true)
            {
                var date = startPlanDate.AddDays(i);
                if(date>endPlanDate) return;
                var patterns = contract.GetPatternsByPlanDate(date);
                var groups = patterns.FindAll(item => item.Status == PatternStatus.Undistributed.GetHashCode()).GroupBy(item=>item.BatchName);
                foreach (var group in groups)
                {
                    var batchName = group.Key;
                    var list = group.ToList();
                    var batchUdis = contract.GetMdbPartsUdisByBatchName(batchName);
                    foreach (var pattern in list)
                    {
                        var mdbName = pattern.FileFullPath;
                        using (AccessDb accessDb = new AccessDb(mdbName))
                        {
                            var data = accessDb.GetDatas();
                            var duis = data.Tables["PARTS_UDI"];
                            foreach (DataRow row in duis.Rows)
                            {
                                var partId = row["INFO30"].ToString().Trim();
                                var udi = batchUdis.FirstOrDefault(item => item.INFO30 == partId);
                                row[nameof(udi.INFO39)] = udi?.INFO39;
                                row[nameof(udi.INFO40)] = udi?.INFO40;
                                row[nameof(udi.INFO41)] = udi?.INFO41;
                            }
                            accessDb.Update(data);
                        }
                    }
                }

                i ++ ;
            }
            
            
        }
    }
}
