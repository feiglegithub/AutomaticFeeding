using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum FinishedStatus
    {
        /// <summary>
        /// mdb未生成
        /// </summary>
        [Description("mdb未生成")]
        MdbUnCreated =0,
        /// <summary>
        /// mdb生成中
        /// </summary>
        [Description("mdb生成中")]
        MdbCreating = 1,
        /// <summary>
        /// mdb已生成
        /// </summary>
        [Description("mdb已生成")]
        MdbCreated = 2,
        /// <summary>
        /// 未分配
        /// </summary>
        [Description("未分配")]
        Undistrbuted =3,
        /// <summary>
        /// 未备料
        /// </summary>
        [Description("未备料")]
        UnStock = 4,
        /// <summary>
        /// 备料中
        /// </summary>
        [Description("备料中")]
        Stocking = 5,
        /// <summary>
        /// 备料完成
        /// </summary>
        [Description("备料完成")]
        Stocked = 6,
        /// <summary>
        /// MDB未下载
        /// </summary>
        [Description("未下载")]
        MdbUnloaded=9,
        /// <summary>
        /// MDB下载中
        /// </summary>
        [Description("下载中")]
        MdbLoading =10,

        /// <summary>
        /// 无法在服务器找到MDB文件 -- 需要重新生成
        /// </summary>
        [Description("服务器丢失文件")]
        MdbLost =11,

        /// <summary>
        /// 下载完成
        /// </summary>
        [Description("已下载")]
        MdbLoaded =16,

        /// <summary>
        /// Saw待转换
        /// </summary>
        [Description("Saw待转换")]
        NeedToSaw =17,

        /// <summary>
        /// 正在转换为Saw
        /// </summary>
        [Description("Saw转换中")]
        CreatingSaw =18,

        /// <summary>
        /// Saw转换完成
        /// </summary>
        [Description("Saw已转换")]
        CreatedSaw = 19,

        /// <summary>
        /// 等待物料（请求上料）
        /// </summary>
        [Description("请求上料")]
        WaitMaterial =20,

        /// <summary>
        /// 上料中
        /// </summary>
        [Description("上料中")]
        LoadingMaterial =21,

        /// <summary>
        /// 上料结束
        /// </summary>
        [Description("上料结束")]
        LoadedMaterial =29,

        /// <summary>
        /// 开始开料
        /// </summary>
        [Description("开料中")]
        Cutting =30,

        /// <summary>
        /// 开料完成
        /// </summary>
        [Description("开料完成")]
        Cut =39,
    }

    public static class Extension
    {
        /// <summary>
        /// 获取当前枚举的描述
        /// </summary>
        /// <param name="finishedStatus"></param>
        /// <returns></returns>
        public static Tuple<int, string> GetFinishStatusDescription(this Enum finishedStatus)
        {

            Type t = finishedStatus.GetType();
            var fieldInfo = t.GetField(finishedStatus.ToString());
            if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) is DescriptionAttribute descAttribute)
            {
                var desc = descAttribute.Description;
                return new Tuple<int, string>(Convert.ToInt32(finishedStatus),desc);
            }
            else
            {
                return  new Tuple<int, string>(Convert.ToInt32(finishedStatus),finishedStatus.ToString());
            }
        }
        /// <summary>
        /// 获取所有枚举的描述
        /// </summary>
        /// <param name="finishedStatus"></param>
        /// <returns></returns>
        public static List<Tuple<int, string>> GetAllFinishStatusDescription(this Enum finishedStatus)
        {
            List<Tuple<int, string>> descList = new List<Tuple<int, string>>();
            Type t = finishedStatus.GetType();
            var fieldInfos = t.GetFields();
            foreach (var fieldInfo in fieldInfos)
            {
                if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) is DescriptionAttribute descAttribute)
                {
                    var desc = descAttribute.Description;
                    descList.Add( new Tuple<int, string>(Convert.ToInt32(fieldInfo.GetValue(finishedStatus)), desc));
                }
                else
                {
                    descList.Add(new Tuple<int, string>(Convert.ToInt32(fieldInfo.GetValue(finishedStatus)), fieldInfo.Name));
                }
            }

            return descList;
        }
    }
}
