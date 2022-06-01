using System;
using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command.Args;

namespace NJIS.FPZWS.LineControl.Cutting.PatternCore
{
    public class CommandContext
    {
        private static CommandContext _instance = null;

        public static CommandContext Instance
        {
            get => _instance ?? (_instance = new CommandContext());
            set => _instance = value;
        }

        internal bool Build(CommandBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            FeedBackCommands = builder.CreateFeedBackCommands();
            PatternCommands = builder.CreatePatternCommands();
            StackCommands = builder.CreateStackCommands();
            return true;
        }
       
        public List<ICommand> FeedBackCommands { get; protected set; }

        public List<ICommand> PatternCommands { get; protected set; }

        public List<ICommand> StackCommands { get; protected set; }

    }
}
