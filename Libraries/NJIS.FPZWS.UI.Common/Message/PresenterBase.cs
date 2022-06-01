//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Cutting
//   项目名称：NJIS.FPZWS.UI.Common
//   文 件 名：PresenterBase.cs
//   创建时间：2018-11-19 16:06
//   作    者：
//   说    明：
//   修改时间：2018-11-19 16:06
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;

namespace NJIS.FPZWS.UI.Common.Message
{
    public class PresenterBase : MVPBase
    {
        private const string receiveTipsMessage = nameof(receiveTipsMessage);

        public void SendTipsMessage(string errorMessage)
        {
            Send(receiveTipsMessage, errorMessage);
        }

        public void SendTipsMessage(string errorMessage,object recipient)
        {
            Send(receiveTipsMessage,recipient, errorMessage);
        }

        /// <summary>
        ///     向向绑定对象发送消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageTab"></param>
        /// <param name="recipient">消息接收者</param>
        /// <param name="message"></param>
        public void Send<TMessage>(string messageTab, object recipient, TMessage message)
        {
            Manager.Send(this, messageTab, recipient, message);
        }
        /// <summary>
        ///     向自身发送消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageTab"></param>
        /// <param name="message"></param>
        public void SendSelf<TMessage>(string messageTab, TMessage message)
        {
            Manager.Send(messageTab, this, message);
        }
        /// <summary>
        /// 获取所有绑定的view
        /// </summary>
        /// <returns></returns>
        public List<object> GetBindingViews()
        {
            List<WeakReference> views = ViewPresenterManager.FindViewsByPresenter(this);
            List<object> list = new List<object>();
            views.ForEach(weak =>
            {
                if (weak.IsAlive)
                {
                    list.Add(weak.Target);
                }
            });
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIn1">执行者类型（Presenter）</typeparam>
        /// <typeparam name="TIn2">发送者类型（View）</typeparam>
        /// <typeparam name="TIn3">输入参数类型（View发送的参数类型）</typeparam>
        /// <param name="executor">执行者</param>
        /// <param name="sender">发送者</param>
        /// <param name="inArg">参数</param>
        /// <param name="executeFunc">执行的内容(无返回值)</param>
        /// <param name="returnAction">返回</param>
        protected void ExecuteBase<TIn1, TIn2, TIn3>(TIn1 executor, TIn2 sender, TIn3 inArg, Action<TIn2,TIn3> executeFunc, Action returnAction = null)
            where TIn1 : PresenterBase
        {
            try
            {
                executeFunc.Invoke(sender,inArg);
                returnAction?.Invoke();

            }
            catch (Exception e)
            {
                executor.SendTipsMessage("执行失败:" + e.Message, sender);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIn1">执行者类型（Presenter）</typeparam>
        /// <typeparam name="TIn2">发送者类型（View）</typeparam>
        /// <typeparam name="TIn3">输入参数类型（View发送的参数类型）</typeparam>
        /// <param name="executor">执行者</param>
        /// <param name="sender">发送者</param>
        /// <param name="inArg">参数</param>
        /// <param name="executeFunc">执行的内容(无返回值)</param>
        /// <param name="returnAction">返回</param>
        protected void ExecuteBase<TIn1, TIn2, TIn3>(TIn1 executor, TIn2 sender, TIn3 inArg, Action executeFunc, Action returnAction = null)
            where TIn1 : PresenterBase
        {
            try
            {
                executeFunc.Invoke();
                returnAction?.Invoke();

            }
            catch (Exception e)
            {
                executor.SendTipsMessage("执行失败:" + e.Message, sender);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIn1">执行者类型（Presenter）</typeparam>
        /// <typeparam name="TIn2">发送者类型（View）</typeparam>
        /// <typeparam name="TIn3">输入参数类型（View发送的参数类型）</typeparam>
        /// <param name="executor">执行者</param>
        /// <param name="sender">发送者</param>
        /// <param name="inArg">参数</param>
        /// <param name="executeFunc">执行的内容(无返回值)</param>
        /// <param name="returnAction">返回</param>
        protected void ExecuteBase<TIn1, TIn2, TIn3>(TIn1 executor, TIn2 sender, TIn3 inArg, Action<TIn3> executeFunc, Action returnAction = null)
            where TIn1 : PresenterBase
        {
            try
            {
                executeFunc.Invoke(inArg);
                returnAction?.Invoke();

            }
            catch (Exception e)
            {
                executor.SendTipsMessage("执行失败:" + e.Message, sender);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIn1">执行者类型（Presenter）</typeparam>
        /// <typeparam name="TIn2">发送者类型（View）</typeparam>
        /// <typeparam name="TIn3">输入参数类型（View发送的参数类型）</typeparam>
        /// <typeparam name="TOut">执行结果返回参数类型（返回View）</typeparam>
        /// <param name="executor">执行者</param>
        /// <param name="sender">发送者</param>
        /// <param name="inArg">参数</param>
        /// <param name="executeFunc">执行的内容</param>
        /// <param name="returnAction">返回</param>
        protected void ExecuteBase<TIn1, TIn2, TIn3, TOut>(TIn1 executor, TIn2 sender, TIn3 inArg, Func<TIn2,TIn3, TOut> executeFunc, Action<TOut> returnAction)
            where TIn1 : PresenterBase
        {
            try
            {
                returnAction.Invoke(executeFunc.Invoke(sender,inArg));

            }
            catch (Exception e)
            {
                executor.SendTipsMessage("执行失败:" + e.Message, sender);

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIn1">执行者类型（Presenter）</typeparam>
        /// <typeparam name="TIn2">发送者类型（View）</typeparam>
        /// <typeparam name="TIn3">输入参数类型（View发送的参数类型）</typeparam>
        /// <typeparam name="TOut">执行结果返回参数类型（返回View）</typeparam>
        /// <param name="executor">执行者</param>
        /// <param name="sender">发送者</param>
        /// <param name="inArg">参数</param>
        /// <param name="executeFunc">执行的内容</param>
        /// <param name="returnAction">返回</param>
        protected void ExecuteBase<TIn1, TIn2, TIn3, TOut>(TIn1 executor, TIn2 sender, TIn3 inArg, Func<TIn3, TOut> executeFunc, Action<TOut> returnAction)
            where TIn1 : PresenterBase
        {
            try
            {
                returnAction.Invoke(executeFunc.Invoke(inArg));

            }
            catch (Exception e)
            {
                executor.SendTipsMessage("执行失败:" + e.Message, sender);

            }
        }
        
        
        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="TIn1">执行者类型（Presenter）</typeparam>
        /// <typeparam name="TIn2">发送者类型（View）</typeparam>
        /// <typeparam name="TIn3">输入参数类型（View发送的参数类型）</typeparam>
        /// <param name="executor">执行者（Presenter）</param>
        /// <param name="sender">发送者（View）</param>
        /// <param name="inArg">输入参数（View）</param>
        /// <param name="executeFunc">执行的委托</param>
        protected void Execute<TIn1, TIn2, TIn3>(TIn1 executor, TIn2 sender, TIn3 inArg, Func<TIn3, bool> executeFunc)
            where TIn1 : PresenterBase
        {
            ExecuteBase(executor, sender, inArg, executeFunc, ret => executor.Send(ret ? "执行成功" : "执行失败", sender));

        }


    }

    public class PresenterBase<T>: PresenterBase
    where T: class ,new()
    {
        private static T _instance = null;
        private static object objLock = new object();

        public static T GetInstance()
        {
            if (_instance == null)
            {
                lock (objLock)
                {
                    if (_instance == null)
                    {
                        #region Old Code 由于反射创建对象只能在运行时生效，所以在自定义控件时，无法正常拖拽使用

                        //var constructors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance|BindingFlags.Public);
                        //foreach (ConstructorInfo constructorInfo in constructors)
                        //{
                        //    if (constructorInfo.GetParameters().Length == 0)
                        //    {
                        //        _instance = (T)constructorInfo.Invoke(null);
                        //        return _instance;
                        //    }
                        //}
                        //throw new Exception("该类不存在无参构造函数");

                        #endregion
                        _instance = new T();
                    }
                }
            }

            return _instance;
        }
    }
}