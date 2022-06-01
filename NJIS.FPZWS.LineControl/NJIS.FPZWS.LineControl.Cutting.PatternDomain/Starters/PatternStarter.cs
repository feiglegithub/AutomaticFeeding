using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.Cutting.PatternCore;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command.Args;
using NJIS.FPZWS.Log;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain.Starters
{
    /// <summary>
    /// 
    /// </summary>
    public class PatternStarter : IModularStarter
    {
        private ILogger _log = LogManager.GetLogger(typeof(PatternStarter).Name);
        public StarterLevel Level  {get;}

        public List<Thread> Threads { get; } = new List<Thread>();


        public PatternStarter()
        {
            Threads.Add(new Thread(() =>
                {
                    while (true)
                    {

                        CommandContext.Instance.PatternCommands.ForEach(command =>
                        {
                            ExecuteCommand(command);
                            Thread.Sleep(20);
                        });
                        
                        Thread.Sleep(300);
                    }
                })
                { IsBackground = true });

            Threads.Add(new Thread(() =>
                {
                    while (true)
                    {
                        CommandContext.Instance.StackCommands.ForEach(command =>
                        {
                            ExecuteCommand(command);
                            Thread.Sleep(20);
                        });
                        Thread.Sleep(300);
                    }
                })
                { IsBackground = true });
            Threads.Add(new Thread(() =>
                {
                    while (true)
                    {
                        CommandContext.Instance.FeedBackCommands.ForEach(command =>
                        {
                            ExecuteCommand(command);
                            Thread.Sleep(20);

                        }); Thread.Sleep(300);
                    }
                })
                { IsBackground = true });
        }

        private void ExecuteCommand(ICommand command)
        {
            try
            {
                command.Execute();
            }
            catch (Exception e)
            {
                _log.Error(e,e);
                //ErrorMsg em = new ErrorMsg() { Command = command.GetType().ToString(), Msg = e.Message };

                //BroadcastMessage.Send(nameof(ErrorMsg), em);
            }
        }

        public void Start()
        {

            LogManager.AddLoggerAdapter(new Log.Implement.Log4Net.Log4NetLoggerAdapter());
            _log = LogManager.GetLogger(typeof(PatternStarter).Name);

            Threads.ForEach(thread => thread.Start());
            _log.Debug("锯切图服务启动成功");
        }

        public void Stop()
        {
            Threads.ForEach(thread => thread.Abort());
        }
    }
}
