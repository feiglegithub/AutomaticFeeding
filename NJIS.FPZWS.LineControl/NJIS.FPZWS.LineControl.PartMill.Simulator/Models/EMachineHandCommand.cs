using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Models
{
    public enum EMachineHandCommand
    {
        //无任务
        None=0,
        /// <summary>
        /// 取上保护板
        /// </summary>
        GrabFirst=1,
        /// <summary>
        /// 放上保护板
        /// </summary>
        PutFirst=2,
        /// <summary>
        /// 上料1号位置
        /// </summary>
        LoadMaterial1=3,
        /// <summary>
        /// 上料3号位置
        /// </summary>
        LoadMaterial3=5,
        /// <summary>
        /// 上料4号位置
        /// </summary>
        LoadMaterial4=6,
        /// <summary>
        /// 上料5号位置
        /// </summary>
        LoadMaterial5=7,
        /// <summary>
        /// 取板
        /// </summary>
        GrabBoard=8,
        /// <summary>
        /// 放板
        /// </summary>
        PutBoard=9,
        /// <summary>
        /// 放底板
        /// </summary>
        PutLast=10,
        /// <summary>
        /// 取底板
        /// </summary>
        GrabLast=11,
        /// <summary>
        /// 去1号位置
        /// </summary>
        ToPosition1=12,
        /// <summary>
        /// 去2号位置
        /// </summary>
        ToPosition2 = 13,
        /// <summary>
        /// 去3号位置
        /// </summary>
        ToPosition3 = 14,
        /// <summary>
        /// 去4号位置
        /// </summary>
        ToPosition4 = 15,
        /// <summary>
        /// 去5号位置
        /// </summary>
        ToPosition5 = 16,
        /// <summary>
        /// 清空底板
        /// </summary>
        ClearBaseBoard = 17


    }
}
