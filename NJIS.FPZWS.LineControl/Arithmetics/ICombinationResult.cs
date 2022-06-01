using System.Collections.Generic;

namespace Arithmetics
{
    public interface ICombinationResult<T>
    {
        /// <summary>
        /// 组合的集合
        /// </summary>
        List<T> CombinationCollection { get; set; }
    }
}
