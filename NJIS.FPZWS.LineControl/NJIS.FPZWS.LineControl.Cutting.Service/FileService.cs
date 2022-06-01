using NJIS.FPZWS.Wcf.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using System.IO;
using NJIS.FPZWS.LineControl.Cutting.Repository;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Service
{
    public class FileService : WcfServer<FileService>, IFileContract
    {
        private readonly string _mdbDownLoadPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "DownLoadMDBs");
        private readonly ILineControlCuttingContract _lineControlCuttingContract =new LineControlCuttingService();
        public Dictionary<SpiltMDBResult, MemoryStream> DownLoadFile(List<SpiltMDBResult> spiltMdbResults)
        {
            Dictionary<SpiltMDBResult, MemoryStream> memoryStreams = new Dictionary<SpiltMDBResult, MemoryStream>();
            foreach (var item in spiltMdbResults)
            {
                item.MdbStatus = Convert.ToInt32(FinishedStatus.MdbLoading);
            }
            _lineControlCuttingContract.BulkUpdateMdbStatus(spiltMdbResults);
            foreach (SpiltMDBResult item in spiltMdbResults)
            {
                string path = item.MDBFullName;
                MemoryStream ms = new MemoryStream();
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                fs.CopyTo(ms);
                ms.Position = 0;
                memoryStreams.Add(item, ms);
                fs.Close();
            }
            return memoryStreams;
        }

        public bool PushMdbFile(List<string> itemNames)
        {
            //BroadcastMessage.Send("getMDBFile", ItemNames);
            return true;//UpLoadFile(DownLoadFile(ItemNames));
        }

        public MemoryStream DownLoadMdb(Pattern pattern)
        {
            pattern.Status = Convert.ToInt32(PatternStatus.Loading);
            _lineControlCuttingContract.BulkUpdatePatterns(new List<Pattern>() {pattern});
            string path = pattern.FileFullPath;
            MemoryStream ms = new MemoryStream();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.CopyTo(ms);
            ms.Position = 0;
            fs.Close();
            return ms;
        }
        

        public bool UpLoadFile(Dictionary<string, MemoryStream> memoryStreams)
        {
            foreach(var item in memoryStreams)
            {
                string ItemName = item.Key;
                MemoryStream memoryStream = item.Value;
                byte[] sizeBytes = new byte[memoryStream.Length];
                if (!Directory.Exists(_mdbDownLoadPath))
                {
                    Directory.CreateDirectory(_mdbDownLoadPath);
                }
                string mdbFullName = Path.Combine(_mdbDownLoadPath, ItemName + ".mdb");
                using (FileStream fs = new FileStream(mdbFullName, FileMode.Create, FileAccess.Write))
                {
                    int ret = memoryStream.Read(sizeBytes, 0, sizeBytes.Length);
                    if (ret > 0)
                    {
                        fs.Write(sizeBytes, 0, ret);
                    }
                    fs.Flush();
                    fs.Close();
                }
                    
            }
            

            return true;
        }
    }
}
