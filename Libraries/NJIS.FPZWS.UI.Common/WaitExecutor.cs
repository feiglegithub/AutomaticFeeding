// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.DataManager
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：WaitExecutor.cs
//  创建时间：2018-07-23 18:00
//  作    者：
//  说    明：
//  修改时间：2018-05-02 10:53
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;

#endregion

namespace NJIS.FPZWS.UI.Common
{
    /// <summary>
    ///     等待执行器
    /// </summary>
    public class WaitExecutor
    {
        private readonly Action _doWork;

        protected Action<object> CallBack;

        public WaitExecutor(Action doWork, Action<object> callBack = null)
        {
            _doWork = doWork;
            CallBack = callBack;
            Args = null;
        }

        protected WaitExecutor(params object[] args)
        {
            Args = args;
        }

        public object Result { get; set; }
        public object[] Args { get; set; }

        public virtual void Execute()
        {
            _doWork.Invoke();
        }

        public virtual void OnCallBack()
        {
            if (CallBack != null) CallBack.Invoke(Result);
        }

        protected virtual void CheckArgumentLength(int length)
        {
            if (Args.Length < length)
                throw new ArgumentOutOfRangeException($"too many parameters or too few parameters");
        }

        protected virtual T CheckArgumentType<T>(object arg)
        {
            if (!(arg is T)) throw new ArgumentException("parameter type error");
            return (T) arg;
        }
    }

    public class WaitExecutor<T1> : WaitExecutor
        where T1 : class
    {
        private readonly Action<T1> _doWork;
        private readonly Func<T1> _doWorkResult;

        public WaitExecutor(Action<T1> doWork, Action<object> callBack = null, params object[] args) : base(args)
        {
            _doWork = doWork;
            CallBack = callBack;
        }

        public WaitExecutor(Func<T1> doWork, Action<object> callBack = null, params object[] args) : base(args)
        {
            _doWorkResult = doWork;
            CallBack = callBack;
        }

        public override void Execute()
        {
            if (_doWork != null)
            {
                CheckArgumentLength(1);

                _doWork.Invoke(CheckArgumentType<T1>(Args[0]));
            }
            else
            {
                Result = _doWorkResult.Invoke();
            }
        }
    }

    public class WaitExecutor<T1, T2> : WaitExecutor
    {
        private readonly Action<T1, T2> _doWork;
        private readonly Func<T1, T2> _doWorkResult;

        public WaitExecutor(Action<T1, T2> doWork, Action<object> callBack = null,
            params object[] args) : base(args)
        {
            _doWork = doWork;
            CallBack = callBack;
        }

        public WaitExecutor(Func<T1, T2> doWork, Action<object> callBack = null,
            params object[] args) : base(args)
        {
            _doWorkResult = doWork;
            CallBack = callBack;
        }


        public override void Execute()
        {
            if (_doWork != null)
            {
                CheckArgumentLength(2);
                _doWork.Invoke(CheckArgumentType<T1>(Args[0]), CheckArgumentType<T2>(Args[1]));
            }
            else
            {
                CheckArgumentLength(1);
                Result = _doWorkResult.Invoke(CheckArgumentType<T1>(Args[0]));
            }
        }
    }

    public class WaitExecutor<T1, T2, T3> : WaitExecutor
    {
        private readonly Action<T1, T2, T3> _doWork;

        private readonly Func<T1, T2, T3> _doWorkResult;

        public WaitExecutor(Action<T1, T2, T3> doWork, Action<object> callBack = null,
            params object[] args) : base(args)
        {
            _doWork = doWork;
            CallBack = callBack;
        }

        public WaitExecutor(Func<T1, T2, T3> doWork, Action<object> callBack = null, params object[] args) : base(args)
        {
            _doWorkResult = doWork;
            CallBack = callBack;
        }

        public override void Execute()
        {
            if (_doWork != null)
            {
                CheckArgumentLength(3);

                _doWork.Invoke(
                    CheckArgumentType<T1>(Args[0]),
                    CheckArgumentType<T2>(Args[1]),
                    CheckArgumentType<T3>(Args[2]));
            }
            else
            {
                CheckArgumentLength(2);
                Result = _doWorkResult.Invoke(CheckArgumentType<T1>(Args[0]), CheckArgumentType<T2>(Args[1]));
            }
        }
    }

    public class WaitExecutor<T1, T2, T3, T4> : WaitExecutor
    {
        private readonly Action<T1, T2, T3, T4> _doWork;

        private readonly Func<T1, T2, T3, T4> _doWorkResult;

        public WaitExecutor(Action<T1, T2, T3, T4> doWork, Action<object> callBack = null,
            params object[] args) : base(args)
        {
            _doWork = doWork;
            CallBack = callBack;
        }

        public WaitExecutor(Func<T1, T2, T3, T4> doWork, Action<object> callBack = null,
            params object[] args) : base(args)
        {
            _doWorkResult = doWork;
            CallBack = callBack;
        }

        public override void Execute()
        {
            if (_doWork != null)
            {
                CheckArgumentLength(4);

                _doWork.Invoke(
                    CheckArgumentType<T1>(Args[0]),
                    CheckArgumentType<T2>(Args[1]),
                    CheckArgumentType<T3>(Args[2]),
                    CheckArgumentType<T4>(Args[3]));
            }
            else
            {
                CheckArgumentLength(3);
                Result = _doWorkResult.Invoke(CheckArgumentType<T1>(Args[0]),
                    CheckArgumentType<T2>(Args[1]), CheckArgumentType<T3>(Args[2]));
            }
        }
    }


    public class WaitExecutor<T1, T2, T3, T4, T5> : WaitExecutor
    {
        private readonly Action<T1, T2, T3, T4, T5> _doWork;

        private readonly Func<T1, T2, T3, T4, T5> _doWorkResult;

        public WaitExecutor(Action<T1, T2, T3, T4, T5> doWork, Action<object> callBack = null,
            params object[] args) : base(args)
        {
            _doWork = doWork;
            CallBack = callBack;
        }

        public WaitExecutor(Func<T1, T2, T3, T4, T5> doWork, Action<object> callBack = null,
            params object[] args) : base(args)
        {
            _doWorkResult = doWork;
            CallBack = callBack;
        }

        public override void Execute()
        {
            if (_doWork != null)
            {
                CheckArgumentLength(5);

                _doWork.Invoke(
                    CheckArgumentType<T1>(Args[0]),
                    CheckArgumentType<T2>(Args[1]),
                    CheckArgumentType<T3>(Args[2]),
                    CheckArgumentType<T4>(Args[3]),
                    CheckArgumentType<T5>(Args[4]));
            }
            else
            {
                CheckArgumentLength(4);
                Result = _doWorkResult.Invoke(CheckArgumentType<T1>(Args[0]),
                    CheckArgumentType<T2>(Args[1]),
                    CheckArgumentType<T3>(Args[2]),
                    CheckArgumentType<T4>(Args[3]));
            }
        }
    }
}