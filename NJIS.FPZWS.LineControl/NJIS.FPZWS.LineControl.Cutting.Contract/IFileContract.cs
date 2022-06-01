using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.Wcf.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.Common.Dependency;

namespace NJIS.FPZWS.LineControl.Cutting.Contract
{
    [ServiceContract]
    public interface IFileContract : IWcfServiceContract
    {
        [OperationContract]
        Dictionary<SpiltMDBResult, MemoryStream> DownLoadFile(List<SpiltMDBResult> spiltMdbResults);

        [OperationContract]
        bool UpLoadFile(Dictionary<string, MemoryStream> memoryStreams);

        [OperationContract]
        bool PushMdbFile(List<string> itemNames);

        [OperationContract]
        MemoryStream DownLoadMdb(Pattern pattern);
    }
}
