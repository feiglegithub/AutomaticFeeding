// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.WinCc.Buffer
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IHandler.cs
//  创建时间：2018-09-06 13:42
//  作    者：
//  说    明：
//  修改时间：2018-09-06 13:41
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

namespace NJIS.FPZWS.Common
{
    /// <summary>
    ///     消息处理程序
    /// </summary>
    public interface IHandler<in T>
    {
        void Handle(T message);
    }
}