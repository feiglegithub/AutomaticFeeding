// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：XMLHelper.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2018-05-03 17:53
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace NJIS.FPZWS.Common
{
    public class XMLHelper
    {
        /// <summary>
        ///     将DataTable对象转换成XML字符串
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <returns>XML字符串</returns>
        public static string CDataToXml(DataTable dt)
        {
            if (dt != null)
            {
                MemoryStream ms = null;
                XmlTextWriter xmlWriter = null;
                try
                {
                    ms = new MemoryStream();
                    //根据ms实例化XmlWt 
                    xmlWriter = new XmlTextWriter(ms, Encoding.Unicode);
                    //获取ds中的数据 
                    dt.WriteXml(xmlWriter);
                    var count = (int) ms.Length;
                    var temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);
                    //返回Unicode编码的文本 
                    var ucode = new UnicodeEncoding();
                    var returnValue = ucode.GetString(temp).Trim();
                    return returnValue;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源 
                    if (xmlWriter != null)
                    {
                        xmlWriter.Close();
                        ms.Close();
                        ms.Dispose();
                    }
                }
            }

            return "";
        }

        /// <summary>
        ///     将DataSet对象中指定的Table转换成XML字符串
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <param name="tableIndex">DataSet对象中的Table索引</param>
        /// <returns>XML字符串</returns>
        public static string CDataToXml(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                return "";
            }

            if (tableIndex >= 0 && tableIndex < ds.Tables.Count)
            {
                return CDataToXml(ds.Tables[tableIndex]);
            }

            return CDataToXml(ds.Tables[0]);
        }

        /// <summary>
        ///     将DataSet中对象转换成XML字符串
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <param name="tableName">DataSet对象中的TableName索引</param>
        /// <returns>对应的XML字符串</returns>
        public static string CDataToXml(DataSet ds, string tableName)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                return "";
            }

            if (string.IsNullOrEmpty(tableName) && ds.Tables.Contains(tableName))
            {
                return CDataToXml(ds.Tables[tableName]);
            }

            return CDataToXml(ds.Tables[0]);
        }

        /// <summary>
        ///     将DataSet对象转换成XML字符串
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <returns>XML字符串</returns>
        public static string CDataToXml(DataSet ds)
        {
            //return CDataToXml(ds, -1);

            if (ds == null)
            {
                return "";
            }

            using (var ms = new MemoryStream())
            {
                using (var xmlWriter = new XmlTextWriter(ms, Encoding.Unicode))
                {
                    //ds.WriteXml(xmlWriter, XmlWriteMode.WriteSchema);                        
                    ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                    //ds.GetXml();
                    var count = (int) ms.Length;
                    var temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);
                    //返回Unicode编码的文本 
                    var ucode = new UnicodeEncoding();
                    var xmlString = ucode.GetString(temp).Trim();
                    return xmlString;
                }
            }
        }

        /// <summary>
        ///     将DataView对象转换成XML字符串
        /// </summary>
        /// <param name="dv">DataView对象</param>
        /// <returns>XML字符串</returns>
        public static string CDataToXml(DataView dv)
        {
            return CDataToXml(dv.Table);
        }

        /// <summary>
        ///     将DataSet对象数据保存为XML文件
        /// </summary>
        /// <param name="dt">DataSet</param>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <returns>bool值</returns>
        public static bool CDataToXmlFile(DataTable dt, string xmlFilePath)
        {
            if (dt != null && !string.IsNullOrEmpty(xmlFilePath))
            {
                var path = xmlFilePath; //HttpContext.Current.Server.MapPath(xmlFilePath); 
                MemoryStream ms = null;
                XmlTextWriter XmlWt = null;
                try
                {
                    ms = new MemoryStream();
                    //根据ms实例化XmlWt 
                    XmlWt = new XmlTextWriter(ms, Encoding.Unicode);

                    //获取ds中的数据 
                    dt.WriteXml(XmlWt);
                    var count = (int) ms.Length;
                    var temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);

                    //返回Unicode编码的文本 
                    var ucode = new UnicodeEncoding();

                    //写文件 
                    var sw = new StreamWriter(path);

                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    sw.WriteLine(ucode.GetString(temp).Trim());
                    sw.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源 
                    if (XmlWt != null)
                    {
                        XmlWt.Close();
                        ms.Close();
                        ms.Dispose();
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     将DataSet对象中指定的Table转换成XML文件
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <param name="tableIndex">DataSet对象中的Table索引</param>
        /// <param name="xmlFilePath">xml文件路径</param>
        /// <returns>bool]值</returns>
        public static bool CDataToXmlFile(DataSet ds, int tableIndex, string xmlFilePath)
        {
            if (tableIndex != -1)
            {
                return CDataToXmlFile(ds.Tables[tableIndex], xmlFilePath);
            }

            return CDataToXmlFile(ds.Tables[0], xmlFilePath);
        }

        /// <summary>
        ///     将DataSet对象转换成XML文件
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <param name="xmlFilePath">xml文件路径</param>
        /// <returns>bool]值</returns>
        public static bool CDataToXmlFile(DataSet ds, string xmlFilePath)
        {
            return CDataToXmlFile(ds, -1, xmlFilePath);
        }

        /// <summary>
        ///     将DataView对象转换成XML文件
        /// </summary>
        /// <param name="dv">DataView对象</param>
        /// <param name="xmlFilePath">xml文件路径</param>
        /// <returns>bool]值</returns>
        public static bool CDataToXmlFile(DataView dv, string xmlFilePath)
        {
            return CDataToXmlFile(dv.Table, xmlFilePath);
        }

        /// <summary>
        ///     将Xml内容字符串转换成DataSet对象
        /// </summary>
        /// <param name="xmlStr">Xml内容字符串</param>
        /// <returns>DataSet对象</returns>
        public static DataSet CXmlToDataSet(string xmlStr)
        {
            if (!string.IsNullOrEmpty(xmlStr))
            {
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    var ds = new DataSet();
                    //读取字符串中的信息 
                    StrStream = new StringReader(xmlStr);

                    //获取StrStream中的数据 
                    Xmlrdr = new XmlTextReader(StrStream);

                    //ds获取Xmlrdr中的数据
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源 
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                        StrStream.Dispose();
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///     将Xml字符串转换成DataTable对象
        /// </summary>
        /// <param name="xmlStr">Xml字符串</param>
        /// <param name="tableIndex">Table表索引</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDatatTable(string xmlStr, int tableIndex)
        {
            return CXmlToDataSet(xmlStr).Tables[tableIndex];
        }

        /// <summary>
        ///     将Xml字符串转换成DataTable对象
        /// </summary>
        /// <param name="xmlStr">Xml字符串</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDatatTable(string xmlStr)
        {
            return CXmlToDataSet(xmlStr).Tables[0];
        }

        /// <summary>
        ///     读取Xml文件信息,并转换成DataSet对象
        /// </summary>
        /// <remarks>
        ///     DataSet ds = new DataSet();
        ///     ds = CXmlFileToDataSet("/XML/upload.xml");
        /// </remarks>
        /// <param name="xmlFilePath">Xml文件地址</param>
        /// <returns>DataSet对象</returns>
        public static DataSet CXmlFileToDataSet(string xmlFilePath)
        {
            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                var path = xmlFilePath;
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    var xmldoc = new XmlDocument();
                    //根据地址加载Xml文件 
                    xmldoc.Load(path);
                    var ds = new DataSet();
                    //读取文件中的字符流 
                    StrStream = new StringReader(xmldoc.InnerXml);
                    //获取StrStream中的数据 
                    Xmlrdr = new XmlTextReader(StrStream);
                    //ds获取Xmlrdr中的数据 
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源 
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                        StrStream.Dispose();
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///     读取Xml文件信息,并转换成DataTable对象
        /// </summary>
        /// <param name="xmlFilePath">xml文江路径</param>
        /// <param name="tableIndex">Table索引</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDataTable(string xmlFilePath, int tableIndex)
        {
            return CXmlFileToDataSet(xmlFilePath).Tables[tableIndex];
        }

        /// <summary>
        ///     读取Xml文件信息,并转换成DataTable对象
        /// </summary>
        /// <param name="xmlFilePath">xml文江路径</param>
        /// <returns>DataTable对象</returns>
        public static DataTable CXmlToDataTable(string xmlFilePath)
        {
            return CXmlFileToDataSet(xmlFilePath).Tables[0];
        }

        /// <summary>
        ///     根据元素名称获得其内容
        /// </summary>
        /// <param name="xmlFilePath">xml文件名称</param>
        /// <param name="EleName">元素名称</param>
        /// <returns></returns>
        public static string GetContentByEleName(string xmlFilePath, string EleName)
        {
            var content = "";
            var doc = new XmlDocument();
            doc.Load(xmlFilePath);
            var root = doc.DocumentElement;
            if (root != null)
            {
                var list = root.GetElementsByTagName(EleName);
                if (list != null && list.Count > 0)
                {
                    content = list[0].InnerXml;
                }
            }

            return content;
        }

        /// <summary>
        ///     根据元素名称获得其内容
        /// </summary>
        /// <param name="xmlFilePath">包含xml结构的字符串</param>
        /// <param name="EleName">元素名称</param>
        /// <returns></returns>
        public static string GetContentByEleNameWithXmlString(string xmlString, string EleName)
        {
            var content = "";
            var doc = new XmlDocument();
            doc.LoadXml(xmlString);
            var root = doc.DocumentElement;
            if (root != null)
            {
                var list = root.GetElementsByTagName(EleName);
                if (list != null && list.Count > 0)
                {
                    content = list[0].InnerXml;
                }
            }

            return content;
        }


        //反序列化
        //接收2个参数:xmlFilePath(需要反序列化的XML文件的绝对路径),type(反序列化XML为哪种对象类型)
        public static object DeserializeFromXml(string xmlFilePath, Type type)
        {
            try
            {
                object result = null;
                if (File.Exists(xmlFilePath))
                {
                    using (var reader = new StreamReader(xmlFilePath))
                    {
                        var xs = new XmlSerializer(type);
                        result = xs.Deserialize(reader);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //序列化
        //接收4个参数:srcObject(对象的实例),type(对象类型),xmlFilePath(序列化之后的xml文件的绝对路径),xmlRootName(xml文件中根节点名称)
        //当需要将多个对象实例序列化到同一个XML文件中的时候,xmlRootName就是所有对象共同的根节点名称,如果不指定,.net会默认给一个名称(ArrayOf+实体类名称)
        public static void SerializeToXml(object srcObject, Type type, string xmlFilePath, string xmlRootName)
        {
            try
            {
                if (srcObject != null && !string.IsNullOrEmpty(xmlFilePath))
                {
                    type = type != null ? type : srcObject.GetType();
                    using (var sw = new StreamWriter(xmlFilePath))
                    {
                        var xs = string.IsNullOrEmpty(xmlRootName)
                            ? new XmlSerializer(type)
                            : new XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                        xs.Serialize(sw, srcObject);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}