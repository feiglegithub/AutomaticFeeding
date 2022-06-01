// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.PDD
//  文 件 名：AppSetting.cs
//  创建时间：2017-06-29 10:00
//  作    者：
//  说    明：
//  修改时间：2017-07-10 12:07
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region


#endregion

using System;
using System.IO;
using System.Text;
using NJIS.ConfigurationCenter.Client;
using System.Runtime.Serialization.Json;

namespace NJIS.FPZWS.App.Settings
{
    [ConfigCenter("AppSetting.json", true)]
    public class AppSetting : ConfigurationCenter.Client.ConfigBase<AppSetting>
    {
        protected AppSetting()
        {
        }

        public string App { get; set; }

        public string Service { get; set; }

        public string ServiceName { get; set; }

        public string InstanceName { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public int StartTimeout { get; set; }

        public int StopTimeout { get; set; }

        public string HelpTextPrefix { get; set; }


        public bool Save()
        {
            try
            {
                var json = JsonHelper.ObjectToJson(this);
                File.WriteAllText(this.GetType().Name + ".json", json);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }

    public static class JsonHelper
    {/// <summary>
        /// Json转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string jsonText)
        {
            // Framework 2.0 不支持
            DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonText));
            T obj = (T)s.ReadObject(ms);
            ms.Dispose();
            return obj;


        }


        /// <summary>
        /// 对象转换成JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson<T>(T obj)
        {
            // Framework 2.0 不支持
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            string result = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                ms.Position = 0;

                using (StreamReader read = new StreamReader(ms))
                {
                    result = read.ReadToEnd();
                }
            }
            return result;
        }
    }
}