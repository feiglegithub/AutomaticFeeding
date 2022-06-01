using System;

namespace WCS.Commands.Args
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventBaseArg<T>:EventArgs
    {
        public EventBaseArg(T requestData)
        {
            RequestData = requestData;
            CreatedTime = DateTime.Now;
        }
        /// <summary>
        /// 请求数据
        /// </summary>
        public T RequestData { get; set; }

        public DateTime CreatedTime { get; private set; }
    }

    public class CancelEventArg<T> : EventBaseArg<T>
    {
        public CancelEventArg(T data) : base(data) { }
        /// <summary>
        /// 是否取消回应请求
        /// </summary>
        public bool Cancel { get; set; } = false;
    }

    //public delegate void OnRequestingHandler<T>(object sender, EventBaseArg<T> arg);

    //public delegate void CancelRequestHandler<T>(object sender, CancelEventArg<T> args);

    //public delegate void OnRespondHandler<T>(object sender, EventBaseArg<T> arg);


    
    /// <summary>
    /// 命令接口
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 命令
        /// </summary>
        void Execute();
    }

    //internal interface ICommandBase<TRequest>
    //{
    //    / <summary>
    //    / 加载请求数据
    //    / </summary>
    //    / <param name = "baseArg" > 基础参数 </ param >
    //    / < returns ></ returns >
    //    TRequest LoadRequest(object baseArg);

    //    / <summary>
    //    / 校验请求是否合法
    //    / </summary>
    //    / <param name = "request" ></ param >
    //    / < returns ></ returns >
    //    bool Validating(TRequest request);

    //    / <summary>
    //    / 执行内容
    //    / </summary>
    //    void ExecuteContent();
    //}
}
