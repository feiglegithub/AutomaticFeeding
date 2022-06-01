using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.Wcf.Service;

namespace NJIS.FPZWS.LineControl.Cutting.ContractPlus
{
    [ServiceContract]
    public interface IFileContract : IWcfServiceContract
    {
        [OperationContract]
        MemoryStream DownLoadMdb(Pattern pattern);
    }
}
