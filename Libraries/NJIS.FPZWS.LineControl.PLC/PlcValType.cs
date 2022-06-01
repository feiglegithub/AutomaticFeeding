//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：PlcValType.cs
//   创建时间：2018-11-20 14:41
//   作    者：
//   说    明：
//   修改时间：2018-11-20 14:41
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.PLC
{
    public enum PlcValType
    {
        Bit = 1,
        Int = 2,
        Short = 3,
        Real = 4,
        Lint = 5,
        Double = 6,
        Byte = 7,
        String = 10,
        Binary = 100
    }
}