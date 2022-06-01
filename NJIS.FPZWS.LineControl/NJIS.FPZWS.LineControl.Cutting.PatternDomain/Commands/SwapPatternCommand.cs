using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain.Commands
{
    /// <summary>
    /// 交换锯切图
    /// </summary>
    public class SwapPatternCommand : CommandBase<List<PartFeedBack>, int>
    {
        private ILogger _log = LogManager.GetLogger(typeof(SwapPatternCommand).Name);
        //private ILineControlCuttingContractPlus _contract = null;

        //private ILineControlCuttingContractPlus Contract =>
        //    _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContractPlus>());
        private IPatternDistribute _distribute = null;
        private IPatternDistribute Distribute => _distribute ?? (_distribute = PatternManage.GetInstance());

        protected override List<PartFeedBack> LoadRequest(int baseArg)
        {
            return  new List<PartFeedBack>();
        }

        protected override void ExecuteContent()
        {
            var batchName = Distribute.CurrentBatchName;
            if(string.IsNullOrWhiteSpace(batchName)) return;
            var msg = Distribute.SwapPattern(batchName,BaseArg);
            _log.Info($"执行命令：{this.GetType().Name},{msg}");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseArg">倍速</param>
        public SwapPatternCommand(int baseArg= 40) : base(baseArg)
        {
        }
    }
}
