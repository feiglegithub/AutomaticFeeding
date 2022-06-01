using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.model
{
    public class Wcs_Task
    {
        ///<summary>
        /// 属性说明：唯一任务号
        /// 长度：10	 默认值： 	 允许空：
        /// </summary>
        public int SeqID { get; set; }

        ///<summary>
        /// 属性说明：任务类型：1:"整盘入库";3:"整盘出库";5:"空盘入库";6:"空盘出库";
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int NordID { get; set; }

        ///<summary>
        /// 属性说明：任务状态：WMS下发为1。99完成；0待定；1已分配；2执行中；7已取消；8执行错误；5已通过会合口；
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int NwkStatus { get; set; }

        ///<summary>
        /// 属性说明：电气号：范围1-32000
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int NWorkID { get; set; }

        ///<summary>
        /// 属性说明：托盘号：例如（NSP10000001）
        /// 长度：50	 默认值： 	 允许空：√
        /// </summary>
        public string NPalletID { get; set; }

        ///<summary>
        /// 属性说明：来源仓位号：例如（A01.001.001）
        /// 长度：50	 默认值： 	 允许空：√
        /// </summary>
        public string CPosidFrom { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：50	 默认值： 	 允许空：√
        /// </summary>
        public string CPosidTo { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值：((0)) 	 允许空：
        /// </summary>
        public int Roadway { get; set; }

        ///<summary>
        /// 属性说明：站台类型（废弃）
        /// 长度：50	 默认值： 	 允许空：√
        /// </summary>
        public string NoptType { get; set; }

        ///<summary>
        /// 属性说明：站台号
        /// 长度：10	 默认值：((0)) 	 允许空：
        /// </summary>
        public int NoptStation { get; set; }

        ///<summary>
        /// 属性说明：波次
        /// 长度：10	 默认值：((0)) 	 允许空：
        /// </summary>
        public int NlotID { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值：((0)) 	 允许空：
        /// </summary>
        public int NpackOrder { get; set; }

        ///<summary>
        /// 属性说明：优先级
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int Npri { get; set; }

        ///<summary>
        /// 属性说明：装箱类别
        /// 长度：50	 默认值： 	 允许空：√
        /// </summary>
        public string CPackType { get; set; }

        ///<summary>
        /// 属性说明：完成模式
        /// 长度：50	 默认值： 	 允许空：√
        /// </summary>
        public string CFinishMode { get; set; }

        ///<summary>
        /// 属性说明：下发任务用户
        /// 长度：50	 默认值：((0)) 	 允许空：√
        /// </summary>
        public string Cuser { get; set; }

        ///<summary>
        /// 属性说明：货物数量
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int Quantitity { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值： 	 允许空：√
        /// </summary>
        public int Height { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：500	 默认值： 	 允许空：√
        /// </summary>
        public string LedMsg { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：10	 默认值：((0)) 	 允许空：√
        /// </summary>
        public int ErrorNum { get; set; }

        ///<summary>
        /// 属性说明：WMS的任务创建时间,出库(1),入库(1)
        /// 长度：23	 默认值： 	 允许空：
        /// </summary>
        public DateTime DoptDate { get; set; }

        ///<summary>
        /// 属性说明：WCS的任务开始执行时间,出库(30),入库(20);
        /// 长度：23	 默认值： 	 允许空：√
        /// </summary>
        public DateTime DStartDate { get; set; }

        ///<summary>
        /// 属性说明：WMS任务完成时间,对应状态(99);
        /// 长度：23	 默认值： 	 允许空：√
        /// </summary>
        public DateTime DFinishDate { get; set; }

        ///<summary>
        /// 属性说明：
        /// 长度：20	 默认值： 	 允许空：√
        /// </summary>
        public string CustomerName { get; set; }

        ///<summary>
        /// 属性说明：出库时记录的是堆垛机把托盘放到线体上的时间,对应状态(31);入库时记录的是堆垛机准备从入库站台取托盘的时间,对应状态(21);
        /// 长度：23	 默认值： 	 允许空：√
        /// </summary>
        public DateTime Date_ext1 { get; set; }

        ///<summary>
        /// 属性说明：记录的是托盘过汇流口的时间,对应状态(32)
        /// 长度：23	 默认值： 	 允许空：√
        /// </summary>
        public DateTime Date_ext2 { get; set; }

        ///<summary>
        /// 属性说明：记录的是WCS的任务完成时间,对应状态98;
        /// 长度：23	 默认值： 	 允许空：√
        /// </summary>
        public DateTime Date_ext3 { get; set; }

        ///<summary>
        /// 属性说明：WMS取消任务的时间
        /// 长度：23	 默认值： 	 允许空：√
        /// </summary>
        public DateTime Date_ext4 { get; set; }

        #region 对象需求扩展
        /// <summary>
        /// 写给线体PLC的逻辑目标值
        /// </summary>
        public int Target { get; set; }
        /// <summary>
        /// 出发行，堆垛机PLC只能识别1和2列(1 巷道左边 ;  2 巷道右边)   
        /// </summary>
        public int FromRow { get; set; }
        /// <summary>
        /// 出发列
        /// </summary>
        public int FromColumn { get; set; }
        /// <summary>
        /// 出发层
        /// </summary>
        public int FromLayer { get; set; }
        /// <summary>
        /// 目标行，堆垛机PLC只能识别1和2列(1 巷道左边   2 巷道右边)   
        /// </summary>
        public int ToRow { get; set; }
        /// <summary>
        /// 目标列
        /// </summary>
        public int ToColumn { get; set; }
        /// <summary>
        /// 目标层
        /// </summary>
        public int ToLayer { get; set; }
        #endregion
    }
}
