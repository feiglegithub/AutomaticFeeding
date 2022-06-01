// ************************************************************************************
//  解决方案：NJIS.FPZWS.Collection
//  项目名称：NJIS.FPZWS.Libraries.MqttClient
//  文 件 名：RequestHandler.cs
//  创建时间：2018-08-11 12:40
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:35
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.Libraries.MqttClient
{
    public class RequestHandler<T> : IRequestHandler where T : class
    {
        public string Topic { get; set; }

        public string RequestId { get; set; }

        public DateTime ReTryTime { get; set; }

        public DateTime OverTime { get; set; }

        public byte[] Data { get; set; }

        /// <summary>
        ///     请求携带的异步回调（解决场景：在自己的请求拿到结果后，再完成对某个请求的响应）
        /// </summary>
        public IResponseAsyncHandler ResponseAsync { get; set; }


        /// <summary>
        ///     使用 internal 关键字保护此方法不会在别的类库中的派生类中看到，别的类库中只能 override 此方法的泛形版本
        /// </summary>
        /// <param name="input"></param>
        public void Excute(MqttMessageBase input)
        {
            var t = input.DeserializeData<T>();
            Receive?.Invoke(this, t);
        }

        public void Aborted()
        {
            TimedOut?.Invoke(this);
        }

        /// <summary>
        ///     接收到消息
        /// </summary>
        public event EventHandler<T> Receive;

        public event Action<IRequestHandler> TimedOut;
    }
}