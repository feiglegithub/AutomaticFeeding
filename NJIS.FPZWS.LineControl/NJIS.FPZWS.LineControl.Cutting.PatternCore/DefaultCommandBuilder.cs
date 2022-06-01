using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command.Args;

namespace NJIS.FPZWS.LineControl.Cutting.PatternCore
{
    public class DefaultCommandBuilder:CommandBuilder
    {
        public override List<ICommand> CreatePatternCommands()
        {
            return new List<ICommand>();
        }

        public override List<ICommand> CreateStackCommands()
        {
            return new List<ICommand>();
        }

        public override List<ICommand> CreateFeedBackCommands()
        {
            return new List<ICommand>();
        }
    }
}
