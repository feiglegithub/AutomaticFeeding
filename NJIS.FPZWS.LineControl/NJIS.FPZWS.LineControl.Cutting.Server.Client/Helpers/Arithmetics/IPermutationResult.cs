using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Helpers.Arithmetics
{
    public interface IPermutationResult<T>
    {
        List<T> PermutationCollection { get; set; }
    }
}
