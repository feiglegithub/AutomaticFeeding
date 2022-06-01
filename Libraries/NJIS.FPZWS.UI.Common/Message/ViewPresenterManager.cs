//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Cutting
//   项目名称：NJIS.FPZWS.UI.Common
//   文 件 名：ViewPresenterManager.cs
//   创建时间：2018-10-18 7:54
//   作    者：
//   说    明：
//   修改时间：2018-10-18 7:54
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;

namespace NJIS.FPZWS.UI.Common.Message
{
    internal class ViewPresenterManager
    {
        /// <summary>
        ///     view绑定presenter int为view的HashCode Tuple：Item1为view的弱引用，item2为presenter弱引用
        ///     不允许一个view绑定多个presenter，如果有这种需求则表示该view需要拆分成多个view进行设计
        /// </summary>
        private static readonly Dictionary<int, Tuple<WeakReference, WeakReference>> ViewLinkPresenter =
            new Dictionary<int, Tuple<WeakReference, WeakReference>>();


        /// <summary>
        ///     presenter绑定view int为presenter的HashCode Tuple：Item1为presenter的弱引用，item2为view弱引
        ///     --- 目的是支持多个view绑定一个presenter，由view自行注销view与presenter（ViewFirst的MVP模式）
        /// </summary>
        private static readonly Dictionary<int, List<Tuple<WeakReference, WeakReference>>> PresenterLinkViews =
            new Dictionary<int, List<Tuple<WeakReference, WeakReference>>>();

        /// <summary>
        ///     消息注册者是否为 View
        /// </summary>
        /// <param name="recipent">消息注册者</param>
        /// <returns></returns>
        public static bool IsView(object recipent)
        {
            var hashCode = recipent.GetHashCode();
            return ViewLinkPresenter.ContainsKey(hashCode);
        }

        /// <summary>
        ///     消息注册者是否为 Presenter
        /// </summary>
        /// <param name="recipent">消息注册者</param>
        /// <returns></returns>
        public static bool IsPresenter(object recipent)
        {
            var hashCode = recipent.GetHashCode();
            return PresenterLinkViews.ContainsKey(hashCode);
        }


        public static WeakReference FindPreenterByView(object view)
        {
            var hashCode = view.GetHashCode();
            if (ViewLinkPresenter.ContainsKey(hashCode))
            {
                return ViewLinkPresenter[hashCode].Item2;
            }

            return null;
        }

        /// <summary>
        ///     获取presenter绑定的所有view
        /// </summary>
        /// <param name="presenter">presenter</param>
        /// <returns></returns>
        public static List<WeakReference> FindViewsByPresenter(object presenter)
        {
            var presenterHashCode = presenter.GetHashCode();
            if (PresenterLinkViews.ContainsKey(presenterHashCode))
            {
                var views = new List<WeakReference>();
                foreach (var tuple in PresenterLinkViews[presenterHashCode])
                {
                    views.Add(tuple.Item2);
                }

                return views;
            }

            return default(List<WeakReference>);
        }

        /// <summary>
        ///     删除指定与presneter绑定的view
        /// </summary>
        /// <param name="presenter">presenter</param>
        /// <param name="view">指定的view</param>
        private static void RemoveViewByPresenter(object presenter, object view)
        {
            var presenterHashCode = presenter.GetHashCode();
            if (PresenterLinkViews.ContainsKey(presenterHashCode))
            {
                PresenterLinkViews[presenterHashCode].RemoveAll(tuple => tuple.Item2.Target.Equals(view));
                if (PresenterLinkViews[presenterHashCode].Count == 0)
                {
                    PresenterLinkViews.Remove(presenterHashCode);
                }
            }
        }

        /// <summary>
        ///     view-presenter绑定
        /// </summary>
        /// <param name="view">view</param>
        /// <param name="presenter">presenter</param>
        public static void BindingPresenter(object view, object presenter)
        {
            var viewHashCode = view.GetHashCode();
            var presenterHashCode = presenter.GetHashCode();
            ////检查presenter是否有注册消息，如果有则允许绑定，如果没有，则报异常
            //if (!MessageRegisterManager.ContainsRecipient(presenterHashCode)&& !MessageRegisterManager.ContainsRecipient(viewHashCode))
            //{
            //    throw new Exception("Error：View与Presenter至少有一个注册消息,请先注册后再绑定");
            //}


            #region Old Code

            //检查presenter是否有注册消息，如果有则允许绑定，如果没有，则报异常
            if (!MessageRegisterManager.ContainsRecipient(presenterHashCode))
            {
                throw new Exception("Error：无法绑定该presenter,无法找到该Presenter注册的消息,请先注册后再绑定");
            }

            if (!MessageRegisterManager.ContainsRecipient(viewHashCode))
            {
                throw new Exception("Error：无法绑定该view,无法找到该view注册的消息,请先注册后再绑定");
            }

            #endregion

            var viewWeakReference = MessageRegisterManager.RecipientGetWeakReference(view);
            var presenterWeakReference = MessageRegisterManager.RecipientGetWeakReference(presenter);
            if (!ViewLinkPresenter.ContainsKey(viewHashCode))
            {
                ViewLinkPresenter.Add(viewHashCode,
                    new Tuple<WeakReference, WeakReference>(viewWeakReference, presenterWeakReference));
                var tuple = new Tuple<WeakReference, WeakReference>(presenterWeakReference, viewWeakReference);
                if (PresenterLinkViews.ContainsKey(presenterHashCode))
                {
                    PresenterLinkViews[presenterHashCode].Add(tuple);
                }
                else
                {
                    PresenterLinkViews.Add(presenterHashCode, new List<Tuple<WeakReference, WeakReference>> {tuple});
                }
            }
        }


        /// <summary>
        ///     view-presenter解绑
        /// </summary>
        /// <param name="view">view</param>
        public static void UnBindPresenter(object view)
        {
            var hashCode = view.GetHashCode();
            if (IsView(view))
            {
                var presenterWeakReference = ViewLinkPresenter[hashCode].Item2;

                //解除view与presenter的绑定
                ViewLinkPresenter.Remove(hashCode);
                //移除view的所有消息
                MessageRegisterManager.RecipientRemoveMessage(view);

                if (!presenterWeakReference.IsAlive)
                {
                    //将presenter加入到需要注销的列表
                    MessageRegisterManager.RecipientRemoveMessage(presenterWeakReference);
                    return;
                }

                var presenterHashCode = presenterWeakReference.Target.GetHashCode();

                if (PresenterLinkViews.ContainsKey(presenterHashCode))
                {
                    //解除presenter与该view的绑定
                    PresenterLinkViews[presenterHashCode].RemoveAll(tuple => tuple.Item2.Target.Equals(view));
                }
            }
        }
    }
}