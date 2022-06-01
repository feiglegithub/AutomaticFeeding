using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS.Commands.Args;

namespace WCS.Commands
{
    public abstract class CommandBase<TRequest,TBaseArg> : ICommand
    {
        protected TRequest RequestData { get; set; }

        protected TBaseArg BaseArg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseArg">基础参数</param>
        protected CommandBase(TBaseArg baseArg)
        {
            BaseArg = baseArg;
        }

        //public event Action<object, EventBaseArg<TRequest>> Requesting;

        public event Action<object, CancelEventArg<TRequest>> Validating;

        //public event Action<object, EventBaseArg<TRequest>> Respond;

        /// <summary>
        /// 加载请求数据
        /// </summary>
        /// <param name="baseArg">基础参数</param>
        /// <returns></returns>
        protected abstract TRequest LoadRequest(TBaseArg baseArg);

        /// <summary>
        /// 执行内容
        /// </summary>
        protected abstract void ExecuteContent();

        public void Execute()
        {
            RequestData = LoadRequest(BaseArg);
            var cancelArg = new CancelEventArg<TRequest>(RequestData);
            Validating?.Invoke(this, cancelArg);
            if (cancelArg.Cancel) return;
            //var arg = new EventBaseArg<TRequest>(Data);
            //Requesting?.Invoke(this, arg);
            ExecuteContent();
            //Respond?.Invoke(this, arg);

        }


    }

}
