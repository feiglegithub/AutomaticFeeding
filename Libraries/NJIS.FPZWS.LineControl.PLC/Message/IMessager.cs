//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：IMessager.cs
//   创建时间：2018-11-23 16:40
//   作    者：
//   说    明：
//   修改时间：2018-11-23 16:40
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.PLC.Message
{
    /// <summary>
    ///     消息接口
    /// </summary>
    public interface IMessager
    {
        /// <summary>
        ///     消息发布类
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="msg"></param>
        void Publish<T>(string topic, T msg);
    }
}