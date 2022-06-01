using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain.Commands
{
    public class PatternCommand:CommandBase<List<Pattern>,string>
    {
        private ILineControlCuttingContractPlus _contract = null;

        private ILineControlCuttingContractPlus Contract =>
            _contract ?? (_contract = WcfClient.GetProxy<ILineControlCuttingContractPlus>());
        private IPatternDistribute _distribute = null;
        private IPatternDistribute Distribute => _distribute ?? (_distribute = PatternManage.GetInstance());

        public PatternCommand(string baseArg) : base(baseArg)
        {
            this.Validating += PatternCommand_Validating;
        }

        private void PatternCommand_Validating(object arg1, PatternCore.Command.Args.CancelEventArg<List<Pattern>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<Pattern> LoadRequest(string baseArg)
        {
            var patterns = Distribute.GetPatternsByDeviceName(baseArg);
            return patterns.FindAll(item => item.Status >= Convert.ToInt32(PatternStatus.Converted) 
                                            && item.FinishedTime==null
                /*&& item.Status< Convert.ToInt32(PatternStatus.Cut)*/);
        }

        protected override void ExecuteContent()
        {
            var patterns = RequestData;
            List<Pattern> updatedPatterns = new List<Pattern>();
            foreach (var pattern in patterns)
            {
                var feedBacks = Contract.GetPatternFeedBacksByMdbName(pattern.MdbName);

                

                if (feedBacks.Count > 0)
                {
                    

                    var feedBack = feedBacks.First();
                    if (feedBack.FinishedTime != null)
                    {
                        if (pattern.Status == Convert.ToInt32(PatternStatus.Converted))
                        {
                            pattern.StartTime = feedBack.StartTime;
                        }

                        pattern.FinishedTime = feedBack.FinishedTime;
                        //pattern.Status = Convert.ToInt32(PatternStatus.Cut);
                        //var pain = feedBack.FinishedTime - pattern.StartTime;
                        pattern.ActuallyTotalTime = feedBack.TotallyTime;// pain == null ? 0 : Convert.ToInt32(pain.Value.TotalSeconds);
                        pattern.UpdatedTime = DateTime.Now;
                        updatedPatterns.Add(pattern);
                    }
                    else
                    {
                        if (pattern.Status == Convert.ToInt32(PatternStatus.Converted))
                        {
                            pattern.StartTime = feedBack.StartTime;
                            pattern.Status = Convert.ToInt32(PatternStatus.Cutting);
                            pattern.UpdatedTime = DateTime.Now;
                            updatedPatterns.Add(pattern);
                        }
                    }
                }
            }

            if (updatedPatterns.Count > 0)
            {
                Distribute.SavePatterns(updatedPatterns);
            }
        }
    }
}
