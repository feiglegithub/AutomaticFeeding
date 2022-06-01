using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Communications;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Models;
using NJIS.FPZWS.UI.Common.Message;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Presenters
{
    public class LineControlPresenter:PresenterBase
    {
        /// <summary>
        /// 写目标地址
        /// </summary>
        public const string WriteTarget = nameof(WriteTarget);
        /// <summary>
        /// 写入板垛好
        /// </summary>
        public const string WritePilerNo = nameof(WritePilerNo);
        /// <summary>
        /// 写入板材数
        /// </summary>
        public const string WriteAmount = nameof(WriteAmount);
        /// <summary>
        /// 清除完成信号
        /// </summary>
        public const string ClearFinished = nameof(ClearFinished);

        public const string Listen = nameof(Listen);

        public const string Stop = nameof(Stop);

        public const string BindingData = nameof(BindingData);

        private LineCommunication Line { get; }=new LineCommunication();

        private bool IsListen { get; set; } = false;

        private Thread ListenThread { get; set; } = null;

        public LineControlPresenter()
        {
            Register();
        }
        private void Register()
        {
            Register<Tuple<ELineName,ELineName,short,int>>(WriteTarget, (sender, arg) =>
                {

                    Execute(this, sender, arg,
                        inArg =>
                        {
                            if (!Line.ReadLineModel(inArg.Item1).HasBoard) return false;
                            return Line.WriteTarget(inArg.Item1, inArg.Item2).Result &&
                                   Line.WriteAmount(inArg.Item1, inArg.Item3) &&
                                   Line.WritePilerNo(inArg.Item1, inArg.Item4);
                        });
                    //try
                    //{
                    //    var ret = Line.WriteTarget(arg.Item1, arg.Item2);

                    //    SendTipsMessage(ret ? "写入成功" : "写入失败", sender);
                    //}
                    //catch (Exception e)
                    //{
                    //    SendTipsMessage("写入失败:"+e.Message, sender);
                    //}
                    
                });

            Register<Tuple<ELineName, short>>(WriteAmount, (sender, arg) =>
            {
                ExecuteWritePlc(this, sender, arg,
                        inArg => Line.WriteAmount(inArg.Item1, inArg.Item2));
               
            });

            Register<ELineName>(ClearFinished, (sender, arg) =>
            {
                ExecuteWritePlc(this, sender, arg,
                    inArg => Line.ClearFinished(inArg));
            });

            Register<string>(Listen, (sender, arg) =>
            {
                ListenThread = new Thread(ExecuteListen) {IsBackground = true};
                ListenThread.Start(new Tuple<object, string>(sender,arg));

            });
            Register<string>(Stop, (sender, arg) =>
            {
                if (ListenThread != null && ListenThread.IsAlive)
                {
                    ListenThread.Abort();
                }
            });

        }

        private void ExecuteListen(object arg)
        {
            while (true)
            {
                Tuple<object, string> obj = (Tuple<object, string>) arg;
                ExecutePlcCommunicationBase(this, obj.Item1, obj.Item2, inArg => Line.ReadAlLineModels(),
                    models => Send(BindingData,  obj.Item1, models));
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIn1">执行者类型</typeparam>
        /// <typeparam name="TIn2">发送类型</typeparam>
        /// <typeparam name="TIn3">参数类型</typeparam>
        /// <typeparam name="TOut">执行结果返回参数类型</typeparam>
        /// <param name="executor">执行者</param>
        /// <param name="sender">发送者</param>
        /// <param name="inArg">参数</param>
        /// <param name="executeFunc">执行的内容</param>
        /// <param name="returnAction">返回</param>
        private void ExecutePlcCommunicationBase<TIn1,TIn2,TIn3,TOut>(TIn1 executor,TIn2 sender,TIn3 inArg,Func<TIn3, TOut> executeFunc,Action<TOut> returnAction)
        where TIn1: PresenterBase
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
        /// 
        /// </summary>
        /// <typeparam name="TIn1"></typeparam>
        /// <typeparam name="TIn2"></typeparam>
        /// <typeparam name="TIn3"></typeparam>
        /// <param name="executor"></param>
        /// <param name="sender"></param>
        /// <param name="inArg"></param>
        /// <param name="executeFunc"></param>
        private void ExecuteWritePlc<TIn1, TIn2, TIn3>(TIn1 executor, TIn2 sender, TIn3 inArg, Func<TIn3, bool> executeFunc)
            where TIn1 : PresenterBase
        {

            ExecutePlcCommunicationBase(executor, sender, inArg, executeFunc, ret => executor.Send(ret ? "执行成功" : "执行失败", sender));
            
        }
    }
}
