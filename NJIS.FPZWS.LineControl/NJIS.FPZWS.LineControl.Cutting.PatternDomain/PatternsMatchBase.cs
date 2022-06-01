using Arithmetics;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain
{
    internal class PatternsMatchBase
    {
        public ICombinationResult<Pattern> Combination { get; set; }
        public int PatternCount { get; set; }
        public double PP { get; set; }
    }
}