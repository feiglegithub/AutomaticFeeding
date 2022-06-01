using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.Wcf.Client;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArithmeticsTest.Commands
{
    public class DownLoadCommand:CommandBase<List<Pattern>,string>
    {
        private string _mdbDownLoadPath = Path.Combine(Directory.GetCurrentDirectory(), "DownLoadMDBs");
        private ILineControlCuttingContract _contract = null;
        private ILineControlCuttingContract Contract =>
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContract>());
        private IFileContract _fileContract = null;
        private IFileContract FileContract => _fileContract ?? (_fileContract = WcfClient.GetProxy<IFileContract>());
        public DownLoadCommand(string deviceName) : base(deviceName)
        {
            this.Validating += DownLoadCommand_Validating;
        }

        private void DownLoadCommand_Validating(object arg1, Args.CancelEventArg<List<Pattern>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<Pattern> LoadRequest(string baseArg)
        {
            var patterns = Contract.GetPatternsByDevice(baseArg,PatternStatus.UndistributedButUnLoad);
            return patterns;
        }

        protected override void ExecuteContent()
        {
            foreach (var pattern in RequestData)
            {
                var ms = FileContract.DownLoadMdb(pattern);

                byte[] sizeBytes = new byte[ms.Length];

                string mdbFullName = Path.Combine(_mdbDownLoadPath, pattern.PlanDate.ToString("yyyy-MM-dd"),
                    pattern.BatchName, pattern.MdbName + ".mdb");
                string path = Path.GetDirectoryName(mdbFullName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                FileStream fs = new FileStream(mdbFullName, FileMode.Create, FileAccess.Write);

                int ret = ms.Read(sizeBytes, 0, sizeBytes.Length);
                if (ret > 0)
                {
                    fs.Write(sizeBytes, 0, ret);
                }
                fs.Flush();
                fs.Close();
                pattern.Status = Convert.ToInt32(PatternStatus.Loaded);
                //pattern.UpdatedTime = DateTime.Now;
            }

            Contract.BulkUpdatePatterns(RequestData);
        }
    }
}
