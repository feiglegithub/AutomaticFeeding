using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetics
{
    public interface IAverageBisector<T>: ICollectionBisector<T>
    {
        Func<T, double> DoubleSelector { get; }
        //Func<T, int> IntSelector { get; }
        //Func<T, float> FloatSelector { get; }
        //Func<T, decimal> DecimalSelector { get; }
    }
}
