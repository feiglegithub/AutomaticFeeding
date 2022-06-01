using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.Domain.Cache;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Cutting.Domain
{
    public class RxSpotter : ISpotter
    {
        private static object objLock = new object();
        private readonly ILineControlCuttingContract contract = new LineControlCuttingService();
        private static readonly Dictionary<string,Dictionary<string,int>> PartCountDictionary = new Dictionary<string, Dictionary<string, int>>();
        private static readonly Dictionary<string, List<CuttingCheckPart>> CheckPartsDictionary = new Dictionary<string, List<CuttingCheckPart>>();
        private readonly ILogger _log = LogManager.GetLogger<RxSpotter>();
        private bool CheckIsSpot(ControlPartInfo pi)
        {

            lock (objLock)
            {
                //检查是否是指定板件的列表
                if (!CheckPartsDictionary.ContainsKey(pi.BatchName))
                {
                    var checkParts = contract.GetCuttingCheckPartsByBatchName(pi.BatchName);
                    if (checkParts.Count > 0)
                    {
                        CheckPartsDictionary.Add(pi.BatchName, checkParts);
                        _log.Debug($"当前批次：{pi.BatchName}抽检的板件：{checkParts.Select(p => p.PartId).ToArray()}");
                    }
                }
                if (CheckPartsDictionary.ContainsKey(pi.BatchName))
                {
                    if (CheckPartsDictionary[pi.BatchName].Exists(item => item.PartId == pi.PartId))
                        return true;
                }
                //获取有效规则
                var checkRules = contract.GetCuttingCheckRulesByEnable(true);
                //是否为批次抽检
                var ret = checkRules.FirstOrDefault(item =>
                    item.CheckObject == Convert.ToInt32(CheckObject.Batch) && item.CheckOperatorArgs == pi.BatchName);
                if (ret != null)
                {
                    if (ret.CheckWay == Convert.ToInt32(CheckWay.Quantify))
                    {
                        int quantify = Convert.ToInt32(ret.Args);
                        ret.CurrentCheckCount += 1;
                        if (quantify == ret.CurrentCheckCount)
                        {
                            //抽检
                            ret.CurrentCheckCount = 0;
                            return true;
                        }
                    }
                    else if (ret.CheckWay == Convert.ToInt32(CheckWay.Top))
                    {
                        int topCount = Convert.ToInt32(ret.Args);
                        ret.CurrentCheckCount += 1;
                        if (topCount >= ret.CurrentCheckCount)
                        {
                            //抽检
                            if (topCount == ret.CurrentCheckCount)
                            {
                                //对此规则失效
                                ret.IsEnable = false;
                                ret.EndCheckTime = DateTime.Now;
                                var flag = contract.BulkUpdatedCuttingCheckRules(checkRules);
                                _log.Debug($"按批次：前N抽检结束，更新抽检规则{flag}");
                            }
                            return true;
                        }
                    }
                    else if (ret.CheckWay == Convert.ToInt32(CheckWay.Timing))
                    {
                        if (DateTime.Now > ret.StartCheckTime && DateTime.Now <
                            ret.StartCheckTime.AddMinutes(Convert.ToInt32(ret.Args)))
                        {
                            return true;
                        }
                    }
                }

                ret = checkRules.FirstOrDefault(item =>
                    item.CheckObject == Convert.ToInt32(CheckObject.Device) && item.CheckOperatorArgs == pi.DeviceName);
                if (ret != null)
                {
                    if (ret.CheckWay == Convert.ToInt32(CheckWay.Quantify))
                    {
                        int quantify = Convert.ToInt32(ret.Args);
                        ret.CurrentCheckCount += 1;
                        if (quantify == ret.CurrentCheckCount)
                        {
                            //抽检
                            ret.CurrentCheckCount = 0;
                            return true;
                        }
                    }
                    else if (ret.CheckWay == Convert.ToInt32(CheckWay.Top))
                    {
                        int topCount = Convert.ToInt32(ret.Args);
                        ret.CurrentCheckCount += 1;
                        if (topCount >= ret.CurrentCheckCount)
                        {
                            //抽检
                            if (topCount == ret.CurrentCheckCount)
                            {
                                //对此规则失效
                                ret.IsEnable = false;
                                ret.EndCheckTime = DateTime.Now;
                                var flag = contract.BulkUpdatedCuttingCheckRules(checkRules);
                                _log.Debug($"按设备：前N抽检结束，更新抽检规则{flag}");
                            }
                            return true;
                        }
                    }
                    else if (ret.CheckWay == Convert.ToInt32(CheckWay.Timing))
                    {
                        if (DateTime.Now > ret.StartCheckTime && DateTime.Now <
                            ret.StartCheckTime.AddMinutes(Convert.ToInt32(ret.Args)))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            //lock (objLock)
            //{
            //    //检查是否是指定板件的列表
            //    if (!CheckPartsDictionary.ContainsKey(pi.BatchName))
            //    {
            //        var checkParts = contract.GetCuttingCheckPartsByBatchName(pi.BatchName);
            //        CheckPartsDictionary.Add(pi.BatchName,checkParts);
            //    }
            //    if(CheckPartsDictionary.ContainsKey(pi.BatchName))
            //    {
            //        if (CheckPartsDictionary[pi.BatchName].Exists(item => item.PartId == pi.PartId))
            //            return true;
            //    }
            //    //获取有效规则
            //    var checkRules = contract.GetCuttingCheckRulesByEnable(true);
            //    //是否为任务抽检
            //    var ret = checkRules.FirstOrDefault(item => item.CheckObject == Convert.ToInt32(CheckObject.Task));
            //    if (ret!=null)
            //    {
            //        if (ret.CheckWay == Convert.ToInt32(CheckWay.Quantify))
            //        {

            //        }
            //        else if (ret.CheckWay == Convert.ToInt32(CheckWay.Top))
            //        {

            //        }
            //        else if (ret.CheckWay == Convert.ToInt32(CheckWay.Timing))
            //        {

            //        }
            //    }
            //    if (PartCountDictionary.ContainsKey(pi.DeviceName))
            //    {
            //        if (PartCountDictionary[pi.DeviceName].ContainsKey(pi.BatchName))
            //        {
            //            var count = PartCountDictionary[pi.DeviceName][pi.BatchName] += 1;
            //            if (count == 20)
            //            {
            //                PartCountDictionary[pi.DeviceName][pi.BatchName] = 0;
            //                return true;
            //            }

            //            return false;
            //        }
            //        PartCountDictionary[pi.DeviceName].Clear();
            //        PartCountDictionary[pi.DeviceName].Add(pi.BatchName, 1);
            //        return false;
            //    }
            //    PartCountDictionary.Add(pi.DeviceName,new Dictionary<string, int>());
            //    PartCountDictionary[pi.DeviceName].Add(pi.BatchName,1);
            //    return false;
        }
    

        public RxSpotter()
        {
            Name = "按比例抽检";
        }
        public string Name { get; set; }

        public bool IsSpot(string partId)
        {
            var pi = PartInfoCache.GetPartInfo(partId);
            if (pi == null)
            {
                return true;
            }

            return CheckIsSpot(pi);
            
        }
    }
}
