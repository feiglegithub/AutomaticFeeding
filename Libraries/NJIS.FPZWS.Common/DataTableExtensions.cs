// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：DataTableExtensions.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Reflection;
using System.Linq;

#endregion

namespace NJIS.FPZWS.Common
{
    public static class DataTableExtensions
    {
        public static List<TResult> ToList<TResult>(this DataTable dt) where TResult : class, new()
        {
            if (dt == null) throw new ArgumentNullException(nameof(dt));
            var list = new List<TResult>();
            return dt.Rows.Count == 0 ? list : dt.Convert<TResult>();
        }
        public static List<dynamic> ToDynamic(this DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }
    }

    #region emit方法

    ///// <summary>
    /////     实体转换
    ///// </summary>
    //public class EntityConverter
    //{


    //    //把datareader转换为实体的方法的委托定义
    //    public delegate T LoadDataRecord<T>(IDataRecord dr);

    //    //把datarow转换为实体的方法的委托定义
    //    public delegate T LoadDataRow<T>(DataRow dr);

    //    //数据类型和对应的强制转换方法的methodinfo，供实体属性赋值时调用
    //    private static readonly Dictionary<Type, MethodInfo> ConvertMethods = new Dictionary<Type, MethodInfo>
    //    {
    //        { typeof(int), typeof(Convert).GetMethod("ToInt32", new[] { typeof(object) }) },

    //        { typeof(short), typeof(Convert).GetMethod("ToInt16", new[] { typeof(object) }) },
    //        { typeof(long), typeof(Convert).GetMethod("ToInt64", new[] { typeof(object) }) },
    //        { typeof(DateTime), typeof(Convert).GetMethod("ToDateTime", new[] { typeof(object) }) },
    //        { typeof(float), typeof(Convert).GetMethod("ToSingle", new[] { typeof(object) }) },
    //        { typeof(decimal), typeof(Convert).GetMethod("ToDecimal", new[] { typeof(object) }) },
    //        { typeof(double), typeof(Convert).GetMethod("ToDouble", new[] { typeof(object) }) },
    //        { typeof(bool), typeof(Convert).GetMethod("ToBoolean", new[] { typeof(object) }) },
    //        { typeof(string), typeof(Convert).GetMethod("ToString", new[] { typeof(object) }) },

    //         { typeof(int?), typeof(AssembleInfo).GetMethod("CastNullable", new[] { typeof(object) }).MakeGenericMethod(typeof(int)) },

    //        { typeof(short?), typeof(AssembleInfo).GetMethod("CastNullable", new[] { typeof(object) }).MakeGenericMethod(typeof(short)) },
    //        { typeof(long?), typeof(AssembleInfo).GetMethod("CastNullable", new[] { typeof(object) }).MakeGenericMethod(typeof(long))  },
    //        { typeof(DateTime?),  typeof(AssembleInfo).GetMethod("CastNullable", new[] { typeof(object) }).MakeGenericMethod(typeof(DateTime))   },
    //        { typeof(float?),  typeof(AssembleInfo).GetMethod("CastNullable", new[] { typeof(object) }).MakeGenericMethod(typeof(float)) },
    //        { typeof(decimal?), typeof(AssembleInfo).GetMethod("CastNullable", new[] { typeof(object) }).MakeGenericMethod(typeof(decimal))  },
    //        { typeof(double?),  typeof(AssembleInfo).GetMethod("CastNullable", new[] { typeof(object) }).MakeGenericMethod(typeof(double)) },
    //        { typeof(bool?),  typeof(AssembleInfo).GetMethod("CastNullable", new[] { typeof(object) }).MakeGenericMethod(typeof(bool))  }

    //    };

    //    //emit里面用到的针对datarow的元数据信息
    //    private static readonly AssembleInfo DataRowAssembly = new AssembleInfo(typeof(DataRow));

    //    /// <summary>
    //    ///     构造转换动态方法（核心代码），根据assembly可处理datarow和datareader两种转换
    //    /// </summary>
    //    /// <typeparam name="T">返回的实体类型</typeparam>
    //    /// <param name="assembly">待转换数据的元数据信息</param>
    //    /// <returns>实体对象</returns>
    //    private static DynamicMethod BuildMethod<T>(AssembleInfo assembly)
    //    {
    //        var method = new DynamicMethod(assembly.MethodName + typeof(T).Name,
    //            typeof(T),
    //            new[] { assembly.SourceType });

    //        var generator = method.GetILGenerator();
    //        var result = generator.DeclareLocal(typeof(T));
    //        generator.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
    //        generator.Emit(OpCodes.Stloc, result);


    //        foreach (var property in typeof(T).GetProperties())
    //        {
    //            var endIfLabel = generator.DefineLabel();

    //            generator.Emit(OpCodes.Ldstr, property.Name);
    //            generator.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine",new []{typeof(string)}));

    //            generator.Emit(OpCodes.Ldarg_0);  
    //            generator.Emit(OpCodes.Ldstr, property.Name);  
    //            generator.Emit(OpCodes.Callvirt, assembly.CanSettedMethod);
    //            generator.Emit(OpCodes.Brfalse, endIfLabel);
    //            generator.Emit(OpCodes.Ldloc, result);
    //            generator.Emit(OpCodes.Ldarg_0);
    //            generator.Emit(OpCodes.Ldstr, property.Name);
    //            generator.Emit(OpCodes.Callvirt, assembly.GetValueMethod);
    //            if (property.PropertyType.IsGenericType&&property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
    //            {
    //                generator.Emit(OpCodes.Call, ConvertMethods[property.PropertyType]);
    //            }
    //            else if (property.PropertyType == typeof(Guid))
    //            {
    //                generator.Emit(OpCodes.Call, typeof(Guid).GetConstructor(new [] { typeof(string)}));
    //            }
    //            else if (property.PropertyType.IsValueType || (property.PropertyType == typeof(string)))
    //            {
    //                generator.Emit(OpCodes.Call, ConvertMethods[property.PropertyType]);
    //            }
    //            else
    //            {
    //                generator.Emit(OpCodes.Castclass, property.PropertyType);
    //            }
    //            generator.Emit(OpCodes.Callvirt, property.GetSetMethod());
    //            generator.MarkLabel(endIfLabel);
    //        }
    //        generator.Emit(OpCodes.Ldloc, result);
    //        generator.Emit(OpCodes.Ret);
    //        return method;
    //    }

    //    private static readonly Dictionary<string, Delegate> Cache = new Dictionary<string, Delegate>();


    //    /// <summary>
    //    ///     从Cache获取委托的方法实例，没有则调用BuildMethod构造一个。
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <returns></returns>
    //    private static LoadDataRow<T> GetDataRowMethod<T>()
    //    {
    //        if (!Cache.ContainsKey(DataRowAssembly.MethodName + typeof(T).Name))
    //        {
    //            Cache.Add(DataRowAssembly.MethodName + typeof(T).Name, BuildMethod<T>(DataRowAssembly).CreateDelegate(typeof(LoadDataRow<T>)));
    //        }

    //        return (LoadDataRow<T>)Cache[DataRowAssembly.MethodName + typeof(T).Name];
    //    }


    //    public static T ToItem<T>(DataRow dr)
    //    {
    //        var load = GetDataRowMethod<T>();
    //        return load(dr);
    //    }

    //    public static List<T> ToList<T>(DataTable dt)
    //    {
    //        var list = new List<T>();
    //        if (dt.Rows.Count == 0)
    //            return list;
    //        var load = GetDataRowMethod<T>();
    //        foreach (DataRow dr in dt.Rows)
    //            list.Add(load(dr));
    //        return list;
    //    }
    //}

    ///// <summary>
    /////     emit所需要的元数据信息
    ///// </summary>
    //public class AssembleInfo
    //{
    //    public readonly MethodInfo CanSettedMethod;
    //    public readonly MethodInfo GetValueMethod;
    //    public readonly string MethodName;
    //    public readonly Type SourceType;

    //    public AssembleInfo(Type type)
    //    {
    //        SourceType = type;
    //        MethodName = "Convert" + type.Name + "To";
    //        CanSettedMethod = GetType().GetMethod("CanSetted", new[] { type, typeof(string) });
    //        GetValueMethod = type.GetMethod("get_Item", new[] { typeof(string) });
    //    }

    //    public static Nullable<T> CastNullable<T>(object obj) where T : struct
    //    {
    //        if (obj == null)
    //            return null;
    //        Console.WriteLine(obj);
    //        return (Nullable<T>)Convert.ChangeType(obj, typeof(T));
    //    }

    //    /// <summary>
    //    ///     判断datareader是否存在某字段并且值不为空
    //    /// </summary>
    //    /// <param name="dr">当前的datareader</param>
    //    /// <param name="name">字段名</param>
    //    /// <returns></returns>
    //    public static bool CanSetted(IDataRecord dr, string name)
    //    {
    //        var result = false;
    //        for (var i = 0; i < dr.FieldCount; i++)
    //        {
    //            if (dr.GetName(i).Equals(name, StringComparison.CurrentCultureIgnoreCase) && !dr[i].Equals(DBNull.Value))
    //            {
    //                result = true;
    //                break;
    //            }
    //        }
    //        return result;
    //    }

    //    /// <summary>
    //    ///     判断datarow所在的datatable是否存在某列并且值不为空
    //    /// </summary>
    //    /// <param name="dr">当前datarow</param>
    //    /// <param name="name">字段名</param>
    //    /// <returns></returns>
    //    public static bool CanSetted(DataRow dr, string name)
    //    {
    //        return dr.Table.Columns.Contains(name) && !dr.IsNull(name);
    //    }
    //}

    #endregion

    /// <summary>
    ///     DataTable to List converter generic class.
    ///     Convert DataTable to a specific class List<>.
    ///     The Class Property Name must be same as the Column Name of the DataTable.
    ///     The mapping is directly upon "Class Property Name" and "Column Name of the DataTable".
    /// </summary>
    public static class DataTableToList

    {
        public static List<T> Convert<T>(this DataTable table)
            where T : class, new()
        {
            var map =
                new List<Tuple<DataColumn, PropertyInfo>>();
            foreach (var pi in typeof(T).GetProperties())
            {
                foreach (DataColumn tableColumn in table.Columns)
                {
                    if (!tableColumn.ColumnName.Equals(pi.Name, StringComparison.CurrentCultureIgnoreCase)) continue;
                    map.Add(new Tuple<DataColumn, PropertyInfo>(
                        tableColumn, pi));
                    break;
                }
            }


            var list = new List<T>(table.Rows.Count);

            foreach (DataRow row in table.Rows)

            {
                if (row == null)

                {
                    list.Add(null);

                    continue;
                }

                var item = new T();

                foreach (var pair in map)

                {
                    var value = row[pair.Value1];

                    if (value is DBNull) value = null;


                    try
                    {
                        if (pair.Value2.PropertyType == typeof(Guid) && value != null)
                            pair.Value2.SetValue(item, new Guid(value.ToString()), null);
                        else
                        {
                            pair.Value2.SetValue(item,
                                value != null
                                    ? System.Convert.ChangeType(value,
                                        pair.Value2.PropertyType.IsGenericType &&
                                        pair.Value2.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                                            ? pair.Value2.PropertyType.GetGenericArguments()[0]
                                            : pair.Value2.PropertyType)
                                    : null, null);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                list.Add(item);
            }

            return list;
        }
    }


    internal sealed class Tuple<T1, T2>
    {
        public Tuple()
        {
        }

        public Tuple(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
    }
}