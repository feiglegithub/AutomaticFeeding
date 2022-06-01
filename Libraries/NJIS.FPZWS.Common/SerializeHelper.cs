// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：SerializeHelper.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace NJIS.FPZWS.Common
{
    /// <summary>
    ///     序列化辅助操作类
    /// </summary>
    public static class SerializeHelper
    {
        #region 二进制序列化

        /// <summary>
        ///     将数据序列化为二进制数组
        /// </summary>
        public static byte[] ToBinary(object data)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, data);
                ms.Seek(0, 0);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     将二进制数组反序列化为强类型数据
        /// </summary>
        public static T FromBinary<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                return (T) formatter.Deserialize(ms);
            }
        }

        /// <summary>
        ///     将数据序列化为二进制数组并写入文件中
        /// </summary>
        public static void ToBinaryFile(object data, string fileName)
        {
            //fileName.CheckFileExists("fileName");
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, data);
            }
        }

        /// <summary>
        ///     将指定二进制数据文件还原为强类型数据
        /// </summary>
        public static T FromBinaryFile<T>(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var formatter = new BinaryFormatter();
                return (T) formatter.Deserialize(fs);
            }
        }

        #endregion

        #region XML序列化

        /// <summary>
        ///     将数据序列化为XML形式
        /// </summary>
        public static string ToXml(object data)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(ms, data);
                ms.Seek(0, 0);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        /// <summary>
        ///     将XML数据反序列化为强类型
        /// </summary>
        public static T FromXml<T>(string xml)
        {
            var bytes = Encoding.Default.GetBytes(xml);
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T) serializer.Deserialize(ms);
            }
        }

        /// <summary>
        ///     将数据序列化为XML并写入文件
        /// </summary>
        public static void ToXmlFile(object data, string fileName)
        {
            //fileName.CheckNotNullOrEmpty("fileName");
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                var serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(fs, data);
            }
        }

        /// <summary>
        ///     将指定XML数据文件还原为强类型数据
        /// </summary>
        public static T FromXmlFile<T>(string fileName)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T) serializer.Deserialize(fs);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return default(T);
        }

        #endregion
    }
}