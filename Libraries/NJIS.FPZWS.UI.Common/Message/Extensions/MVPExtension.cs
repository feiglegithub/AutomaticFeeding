//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Cutting
//   项目名称：NJIS.FPZWS.UI.Common
//   文 件 名：MVPExtension.cs
//   创建时间：2018-11-19 16:06
//   作    者：
//   说    明：
//   修改时间：2018-11-19 16:06
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Windows.Forms;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;

namespace NJIS.FPZWS.UI.Common.Message.Extensions
{
    public static class MVPExtension
    {
        private static readonly IManager Manager = MessageManager.GetInstance();
        private const string receiveTipsMessage = nameof(receiveTipsMessage);
        /// <summary>
        ///     注册（订阅）消息
        /// </summary>
        /// <typeparam name="TMessage">接收的消息类型</typeparam>
        /// <param name="messageTab">接收的消息标识</param>
        /// <param name="action">收到消息时需要执行的委托</param>
        /// <param name="eExecutionMode">收到消息后的执行方式</param>
        public static void Register<TMessage>(this IMVP view, string messageTab, Action<TMessage> action,
            EExecutionMode eExecutionMode = EExecutionMode.Asynchronization)
        {
            Manager.Register(messageTab, view, action, eExecutionMode);
        }
        /// <summary>
        ///     注册（订阅）消息
        /// </summary>
        /// <typeparam name="TMessage">接收的消息类型</typeparam>
        /// <param name="messageTab">接收的消息标识</param>
        /// <param name="actionWithOperated">收到消息时需要执行的委托</param>
        /// <param name="operatedArgs">委托执行时需要操作的对象（一些指定的参数）</param>
        /// <param name="eExecutionMode">收到消息后的执行方式</param>
        public static void Register<TMessage>(this IMVP view, string messageTab, Action<TMessage,object[]> actionWithOperated, object[] operatedArgs,
            EExecutionMode eExecutionMode = EExecutionMode.Asynchronization)
        {
            Manager.Register(messageTab, view, actionWithOperated, operatedArgs, eExecutionMode);
        }

        /// <summary>
        /// 向绑定对象发送消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="view"></param>
        /// <param name="messageTab"></param>
        /// <param name="message"></param>
        public static void Send<TMessage>(this IMVP view, string messageTab, TMessage message)
        {
            Manager.Send(view, messageTab, message);
        }
        /// <summary>
        /// 注册接收提示信息
        /// </summary>
        /// <param name="view"></param>
        public static void RegisterTipsMessage(this IView view)
        {
            Register<string>(view, receiveTipsMessage, ShowTipsMessage,new object[] { view});
        }

        private static void ShowTipsMessage(string errorMessage,object[] views)
        {
            var control = views[0] as Control;
            if (control != null)
            {
                //control.BeginInvoke((Action)(() => RadMessageBox.Show(control, errorMessage)));
            }
        }

        /// <summary>
        ///     绑定Presenter
        /// </summary>
        /// <param name="view"></param>
        /// <param name="presenter"></param>
        public static void BindingPresenter(this IView view, object presenter)
        {
            
            Manager.BindingPresenter(view, presenter);
        }

        /// <summary>
        ///     解除绑定
        /// </summary>
        /// <param name="view"></param>
        public static void UnBindingPresenter(this IView view)
        {
            Manager.UnBindPresenter(view);
        }

        /// <summary>
        ///     向自身发送消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="presenter"></param>
        /// <param name="messageTab"></param>
        /// <param name="message"></param>
        public static void SendSelf<TMessage>(this IPresenter presenter, string messageTab, TMessage message)
        {
            Manager.Send(messageTab, presenter, message);
        }

        /// <summary>
        /// 消息转发（绑定相同的presenter，CurrentView转发至OtherView）
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="view"></param>
        /// <param name="messageTab">消息标签</param>
        /// <param name="message"></param>
        public static void MessageTranSpond<TMessage>(this IView view, string messageTab, TMessage message)
        {
            WeakReference presenterWeakReference = ViewPresenterManager.FindPreenterByView(view);
            if (presenterWeakReference != null)
            {
                if (presenterWeakReference.IsAlive)
                {
                    object presenter = presenterWeakReference.Target;
                    Manager.Send(presenter, messageTab, message);

                }
            }
        }
    }
}