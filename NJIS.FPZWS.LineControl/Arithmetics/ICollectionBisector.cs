using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetics
{
    /// <summary>
    /// 集合二等分接口
    /// </summary>
    public interface ICollectionBisector<T>
    {
        bool CanBisect(IEnumerable<T> tSource);

        BisectResult<T> Bisector(IEnumerable<T> tSource);
    }

    /// <summary>
    /// 二等分结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BisectResult<T>
    {
        /// <summary>
        /// 是否为二等分
        /// </summary>
        public bool IsBisect { get; set; } = false;
        /// <summary>
        /// 第一个序列
        /// </summary>
        public IEnumerable<T> FirstEnumerable { get; set; } = null;
        /// <summary>
        /// 第二个序列
        /// </summary>
        public IEnumerable<T> SecondEnumerable { get; set; } = null;
        /// <summary>
        /// 原序列
        /// </summary>
        public IEnumerable<T> Source { get; set; }
    }
}
