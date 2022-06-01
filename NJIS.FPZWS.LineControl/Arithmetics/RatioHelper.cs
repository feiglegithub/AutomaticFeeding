using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetics
{
    public class RatioHelper
    {
        public static RatioResult<TSource, TRatio> RatioBase<TSource, TRatio>(IEnumerable<TSource> tSources,
            Func<TSource, IEnumerable<TSource>, TRatio> ratioFunc)
        {
            RatioResult<TSource, TRatio> ratioResult = new RatioResult<TSource, TRatio>();
            foreach (var source in tSources)
            {
                ratioResult.CombinationCollection.Add(new RatioResultBase<TSource, TRatio>(){Source = source,Ratio = ratioFunc(source,tSources)});
            }

            return ratioResult;
        }

        public static RatioResult<TSource, TRatio> RatioBase<TSource, TRatio>(IEnumerable<TSource> tSources,
            Func<TSource, TRatio> ratioFunc)
        {
            RatioResult<TSource, TRatio> ratioResult = new RatioResult<TSource, TRatio>();
            foreach (var source in tSources)
            {
                ratioResult.CombinationCollection.Add(new RatioResultBase<TSource, TRatio>() { Source = source, Ratio = ratioFunc(source) });
            }

            return ratioResult;
        }
    }

    public class RatioResultBase<TSource, TRatio>
    {
        public TSource Source { get; set; }
        public TRatio Ratio { get; set; }
    }

    public class RatioResult<TSource, TRatio> : ICombinationResult<RatioResultBase<TSource, TRatio>>
    {
        public List<RatioResultBase<TSource, TRatio>> CombinationCollection { get; set; }=new List<RatioResultBase<TSource, TRatio>>();
    }
}
