using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command.Args;
using NJIS.FPZWS.LineControl.Cutting.PatternDomain.Commands;
using NJIS.FPZWS.UI.Common.Message;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain.Services
{
    public class LocalService : ILocalService
    {
        private List<ICommand> PatternCommands { get; set; } = new List<ICommand>();
        private List<ICommand> StackCommands { get; set; } = new List<ICommand>();
        private List<ICommand> FeedBackCommands { get; set; } = new List<ICommand>();

        private List<Thread> Threads { get; set; } = new List<Thread>();
        private static readonly object ObjLock = new object();
        private static LocalService _localService = null;

        public static LocalService GetInstance()
        {
            if (_localService == null)
            {
                lock (ObjLock)
                {
                    if (_localService == null)
                    {
                        _localService = new LocalService();
                    }
                }
            }

            return _localService;
        }

        private object SwapLock = new object();

        //public void BeginSwap(int times=50)
        //{
        //    lock (SwapLock)
        //    {
        //        var command = PatternCommands.FirstOrDefault(item => item.GetType() == typeof(SwapPatternCommand));
        //        if (command == null)
        //            PatternCommands.Add(new SwapPatternCommand(times));
        //        else
        //        {
        //            PatternCommands.Remove(command);
        //            PatternCommands.Add(new SwapPatternCommand(times));
        //        }
        //    }
        //}

        //public void StopSwap()
        //{
        //    lock (SwapLock)
        //    {
        //        var command = PatternCommands.FirstOrDefault(item => item.GetType() == typeof(SwapPatternCommand));
        //        if (command != null)
        //        {
        //            PatternCommands.Remove(command);
        //        }
        //    }
        //}

        private LocalService()
        {
            List<string> devices = new List<string>() {"1#", "2#", "3#", "4#", "5#"};
            devices.ForEach(device=>
            {
                
                PatternCommands.Add(new GetPatternCommand(device));
                StackCommands.Add(new GetStackCommand(device));
            });
            PatternCommands.Add(new SwapPatternCommand(1));
            FeedBackCommands.Add(new BatchGroupCommand());
            FeedBackCommands.Add(new PatternDetailCommand());
            FeedBackCommands.Add(new StackFeedBackCommand());

            Threads.Add(new Thread(() =>
            {
                while (true)
                {
                    lock (SwapLock)
                    {
                        PatternCommands.ForEach(command =>
                        {
                            ExecuteCommand(command);
                            Thread.Sleep(20);
                        });
                    }
                    Thread.Sleep(300);
                }
            }){ IsBackground = true });
            Threads.Add(new Thread(() =>
                {
                    while (true)
                    {
                        StackCommands.ForEach(command =>
                        {
                            ExecuteCommand(command);
                            Thread.Sleep(20);
                        });
                        Thread.Sleep(300);
                    }
                }){ IsBackground = true });
            Threads.Add(new Thread(() =>
                {
                    while (true)
                    {
                        FeedBackCommands.ForEach(command =>
                        {
                            ExecuteCommand(command);
                            Thread.Sleep(20);

                        });Thread.Sleep(300);
                    }
                }){ IsBackground = true });

        }

        private void ExecuteCommand(ICommand command)
        {
            try
            {
                command.Execute();
            }
            catch (Exception e)
            {
                ErrorMsg em = new ErrorMsg() { Command = command.GetType().ToString(), Msg = e.Message };
                BroadcastMessage.Send(nameof(ErrorMsg), em);
            }
        }

        public void Start()
        {
            Threads.ForEach(thread=>thread.Start());
        }

        public void Stop()
        {
            Threads.ForEach(thread => thread.Abort());
        }
    }
}
