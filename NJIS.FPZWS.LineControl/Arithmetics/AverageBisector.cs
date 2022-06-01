using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetics
{
    /// <summary>
    /// 均值二等分器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AverageBisector<T> : IAverageBisector<T>
    {
        private Func<IEnumerable<T>, bool> canBisectFunc = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doubleSelector"></param>
        /// <param name="canBisectFunc"></param>
        public AverageBisector(Func<T, double> doubleSelector,Func<IEnumerable<T>,bool> canBisectFunc)
        {
            this.DoubleSelector = doubleSelector;
            this.canBisectFunc = canBisectFunc;
        }

        public Func<T, double> DoubleSelector { get; } = null;

        public BisectResult<T> Bisector(IEnumerable<T> tSource)
        {
            BisectResult<T> bisectorResult = new BisectResult<T>();
            bisectorResult.Source = tSource;

            if (CanBisect(tSource))
            {
                bisectorResult.IsBisect = true;
                var avg = tSource.Average(DoubleSelector);
                bisectorResult.FirstEnumerable = tSource.ToList().FindAll(item => DoubleSelector(item) < avg);

                bisectorResult.SecondEnumerable = tSource.ToList().FindAll(item => DoubleSelector(item) >= avg);
                //bisectorResult.FirstEnumerable = tSource.Where(item => DoubleSelector(item) < avg);

                //bisectorResult.SecondEnumerable = tSource.Where(item => (DoubleSelector(item) >=avg));
            }

            return bisectorResult;
        }

        public bool CanBisect(IEnumerable<T> tSource)
        {
            return canBisectFunc(tSource);
        }
    }
}
