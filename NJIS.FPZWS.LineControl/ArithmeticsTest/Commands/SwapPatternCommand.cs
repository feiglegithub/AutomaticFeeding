using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.Wcf.Client;
using System.Collections.Generic;

namespace ArithmeticsTest.Commands
{
    /// <summary>
    /// 交换锯切图
    /// </summary>
    public class SwapPatternCommand : CommandBase<List<PartFeedBack>, int>
    {

        private ILineControlCuttingContract _contract = null;

        private ILineControlCuttingContract Contract =>
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContract>());
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
            Distribute.SwapPattern(batchName,BaseArg);
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
