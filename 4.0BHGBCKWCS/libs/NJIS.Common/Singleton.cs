using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.Common
{
    /// <summary>
    /// 单例
    /// </summary>
    /// <typeparam name="T">必须有私有无参构造函数</typeparam>
    public class Singleton<T>
        where T : class

    {
        private static T _T = null;
        private static object _LockObj = new object();

        public static T GetInstance()
        {
            if (_T == null)
            {
                lock (_LockObj)
                {
                    if (_T == null)
                    {
                        var constructorInfos = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                        //_T = (T) constructorInfos[0].Invoke(null);
                        var constructorInfo =
                            constructorInfos.FirstOrDefault(item => item.GetParameters().Length == 0);
                        if (constructorInfo == null)
                        {
                            throw new Exception("类型:" + typeof(T).FullName + "没有私有无参构造函数，不可作为单例.");
                        }

                        _T = (T)constructorInfo.Invoke(null);

                    }

                }
            }

            return _T;
        }

    }

}
