using System;
using System.Collections.Generic;
using System.Linq;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.DomainPlus.Cache;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus
{
    public class RxSpotter : ISpotter
    {
        private static object objLock = new object();
        private readonly ILineControlCuttingContractPlus contract = new LineControlCuttingServicePlus();
        private static readonly Dictionary<string,Dictionary<string,int>> PartCountDictionary = new Dictionary<string, Dictionary<string, int>>();
        private static readonly Dictionary<string, List<CuttingCheckPart>> CheckPartsDictionary = new Dictionary<string, List<CuttingCheckPart>>();
        private bool CheckIsSpot(ControlPartInfo pi)
        {
            lock (objLock)
            {
                //检查是否是指定板件的列表
                if (!CheckPartsDictionary.ContainsKey(pi.BatchName))
                {
                    var checkParts = contract.GetCuttingCheckPartsByBatchName(pi.BatchName);
                    CheckPartsDictionary.Add(pi.BatchName,checkParts);
                }
                if(CheckPartsDictionary.ContainsKey(pi.BatchName))
                {
                    if (CheckPartsDictionary[pi.BatchName].Exists(item => item.PartId == pi.PartId))
                        return true;
                }
                //获取有效规则
                var checkRules = contract.GetCuttingCheckRulesByEnable(true);
                //是否为任务抽检
                var ret = checkRules.FirstOrDefault(item => item.CheckObject == Convert.ToInt32(CheckObject.Task));
                if (ret!=null)
                {
                    if (ret.CheckWay == Convert.ToInt32(CheckWay.Quantify))
                    {

                    }
                    else if (ret.CheckWay == Convert.ToInt32(CheckWay.Top))
                    {
                        
                    }
                    else if (ret.CheckWay == Convert.ToInt32(CheckWay.Timing))
                    {
                        
                    }
                }
                if (PartCountDictionary.ContainsKey(pi.DeviceName))
                {
                    if (PartCountDictionary[pi.DeviceName].ContainsKey(pi.BatchName))
                    {
                        var count = PartCountDictionary[pi.DeviceName][pi.BatchName] += 1;
                        if (count == 20)
                        {
                            PartCountDictionary[pi.DeviceName][pi.BatchName] = 0;
                            return true;
                        }

                        return false;
                    }
                    PartCountDictionary[pi.DeviceName].Clear();
                    PartCountDictionary[pi.DeviceName].Add(pi.BatchName, 1);
                    return false;
                }
                PartCountDictionary.Add(pi.DeviceName,new Dictionary<string, int>());
                PartCountDictionary[pi.DeviceName].Add(pi.BatchName,1);
                return false;
            }
        }

        public RxSpotter()
        {
            Name = "按比例抽检";
        }
        public string Name { get; set; }

        public bool IsSpot(string partId)
        {
            var pi = PartInfoCache.GetScanPartInfo(partId);
            if (pi == null)
            {
                return true;
            }
            ControlPartInfo cpi = new ControlPartInfo()
            {
                BatchName = pi.BatchName,
                DeviceName = pi.DeviceName,
                Length = pi.Length,
                PartId = pi.PartId,
                Width = pi.Width
            };
            return CheckIsSpot(cpi);
            
        }
    }
}
