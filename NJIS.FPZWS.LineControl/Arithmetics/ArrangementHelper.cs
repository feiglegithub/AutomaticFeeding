using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetics
{
    public class ArrangementHelper
    {
        private readonly static object objLock = new object();
        private static Dictionary<int,List<int[]>> Dictionary { get; set; } =new Dictionary<int, List<int[]>>();

        private static List<int[]> Arrangement(int n)
        {
            lock (objLock)
            {
                if (Dictionary.ContainsKey(n))
                    return Dictionary[n];

                int[] ints = new int[n];
                for (int i = 0; i < n; i++)
                {
                    ints[i] = i;
                }

                var list = Arrangement(ints);
                Dictionary.Add(n, list);
                return list;
            }
            
        }

        public static List<T[]> StaticArrangement<T>(T[] ts)
        {
            var length = ts.Length;
            var staticList = Arrangement(length);

            List<T[]> list = new List<T[]>();
            foreach (var item in staticList)
            {
                T[] t = new T[length];
                for (int i = 0; i < length; i++)
                {
                    t[i] = ts[item[i]];
                }
                list.Add(t);
            }

            return list;
        }


        /// <summary>
        /// 全排列
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static List<T[]> Arrangement<T>(T[] ts)
        {
            int count = ts.Length;

            List<T[]> list = new List<T[]>();
            if (count == 1)
            {
                list.Add(ts);
                return list;
            }
            else if (count == 2)
            {
                list.Add(ts);
                list.Add(Swap(ts, 0, 1));
                return list;
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    T tFirst = ts[i];
                    T[] copyT = new T[count - 1];
                    for (int j = 0; j < count; j++)
                    {
                        if (j == i) continue;
                        if (j > i)
                        {
                            copyT[j - 1] = ts[j];
                        }
                        else
                        {
                            copyT[j] = ts[j];
                        }
                    }

                    var tmpList = Arrangement<T>(copyT);
                    foreach (var item in tmpList)
                    {
                        T[] tmpT =new T[count];
                        tmpT[0] = tFirst;
                        item.CopyTo(tmpT,1);
                        list.Add(tmpT);
                    }
                }

                return list;
            }


        }

        private static T[] Swap<T>(T[] ts, int m, int n)
        {
            var tm = ts[m];
            var tn = ts[n];
            T[] copyInts = new T[ts.Length];
            ts.CopyTo(copyInts,0);
            copyInts[m] = tn;
            copyInts[n] = tm;
            return copyInts;
        }
    }
}
