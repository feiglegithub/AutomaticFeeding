//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：IPlcCommandExecutor.cs
//   创建时间：2018-11-20 17:13
//   作    者：
//   说    明：
//   修改时间：2018-11-20 17:13
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.PLC
{
    public interface IPlcCommandExecutor
    {
        bool Init();
        bool Start();
        bool Stop();
    }
}