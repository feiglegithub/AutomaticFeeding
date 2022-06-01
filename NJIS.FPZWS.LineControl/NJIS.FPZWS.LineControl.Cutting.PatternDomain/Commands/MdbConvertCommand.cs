using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain.Commands
{
    public class MdbConvertCommand:CommandBase<List<Pattern>,string>
    {
        private IPatternDistribute _distribute = null;
        private IPatternDistribute Distribute => _distribute ?? (_distribute = PatternManage.GetInstance());

        public MdbConvertCommand(string baseArg) : base(baseArg)
        {
            this.Validating += MdbConvertCommand_Validating;
        }

        private void MdbConvertCommand_Validating(object arg1, PatternCore.Command.Args.CancelEventArg<List<Pattern>> arg2)
        {
            arg2.Cancel = arg2.RequestData.Count == 0;
        }

        protected override List<Pattern> LoadRequest(string baseArg)
        {
            var patterns = Distribute.GetPatternsByDeviceName(baseArg);
            return patterns.FindAll(item => item.Status < Convert.ToInt32(PatternStatus.Cut));
        }

        protected override void ExecuteContent()
        {
            var patterns = RequestData;
            var downLoadPattern = patterns.FirstOrDefault(item => item.Status == Convert.ToInt32(PatternStatus.Loaded));
            var convertedPattern = patterns.FirstOrDefault(item => item.Status == Convert.ToInt32(PatternStatus.Converted));
            if(downLoadPattern==null) return;
            if (convertedPattern == null)
            {
                downLoadPattern.Status = Convert.ToInt32(PatternStatus.UnConvertSaw);
                downLoadPattern.UpdatedTime = DateTime.Now;
                Distribute.SavePatterns(new List<Pattern>() {downLoadPattern});
            }
        }
    }
}
