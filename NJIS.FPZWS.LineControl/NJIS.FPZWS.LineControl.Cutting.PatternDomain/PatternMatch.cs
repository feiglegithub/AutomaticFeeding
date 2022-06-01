using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain
{
    internal class PatternMatch
    {
        public Pattern MaxPattern { get; set; }
        public Pattern MinPattern { get; set; }
        public double PP { get; set; }
    }
}