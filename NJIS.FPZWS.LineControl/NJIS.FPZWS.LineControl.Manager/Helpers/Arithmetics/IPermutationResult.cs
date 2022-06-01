using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Manager.Helpers.Arithmetics
{
    public interface IPermutationResult<T>
    {
        List<T> PermutationCollection { get; set; }
    }
}
