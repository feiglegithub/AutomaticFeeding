using System;
using System.Collections.Generic;
using System.IO;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.Wcf.Service;

namespace NJIS.FPZWS.LineControl.Cutting.ServicePlus
{
    public class FileService : WcfServer<FileService>, IFileContract
    {
        //private readonly string _mdbDownLoadPath = Path.Combine(Directory.GetCurrentDirectory(), "DownLoadMDBs");
        private readonly ILineControlCuttingContractPlus _lineControlCuttingContract =new LineControlCuttingServicePlus();



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


        //public bool UpLoadFile(Dictionary<string, MemoryStream> memoryStreams)
        //{
        //    foreach(var item in memoryStreams)
        //    {
        //        string ItemName = item.Key;
        //        MemoryStream memoryStream = item.Value;
        //        byte[] sizeBytes = new byte[memoryStream.Length];
        //        if (!Directory.Exists(_mdbDownLoadPath))
        //        {
        //            Directory.CreateDirectory(_mdbDownLoadPath);
        //        }
        //        string mdbFullName = Path.Combine(_mdbDownLoadPath, ItemName + ".mdb");
        //        using (FileStream fs = new FileStream(mdbFullName, FileMode.Create, FileAccess.Write))
        //        {
        //            int ret = memoryStream.Read(sizeBytes, 0, sizeBytes.Length);
        //            if (ret > 0)
        //            {
        //                fs.Write(sizeBytes, 0, ret);
        //            }
        //            fs.Flush();
        //            fs.Close();
        //        }
                    
        //    }
            

        //    return true;
        //}


    }
}
