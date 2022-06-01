using NJIS.FPZWS.LineControl.Cutting.PatternCore;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command.Args;
using NJIS.FPZWS.LineControl.Cutting.PatternDomain.Commands;
using System.Collections.Generic;

namespace NJIS.FPZWS.LineControl.Cutting.PatternDomain
{
    public class RxCommandBuilder: DefaultCommandBuilder
    {
        private List<string> Devices { get; }=new List<string>() { "0-240-07-8014", "0-240-07-8015", "0-240-07-8016", "0-240-07-8017", "0-240-07-8018" };

        public override List<ICommand> CreatePatternCommands()
        {

            List<ICommand> commands = new List<ICommand>();
            Devices.ForEach(device =>
            {
                commands.Add(new GetPatternCommand(device));
                
            });
            commands.Add(new SwapPatternCommand(1));

            return commands;
        }

        public override List<ICommand> CreateStackCommands()
        {
            List<ICommand> commands = new List<ICommand>();
            Devices.ForEach(device =>
            {
                commands.Add(new GetStackCommand(device));
                commands.Add(new PatternCommand(device));
                commands.Add(new MdbConvertCommand(device));
            });
            
            return commands;
        }

        public override List<ICommand> CreateFeedBackCommands()
        {
            List<ICommand> commands = new List<ICommand>();
            commands.Add(new BatchGroupCommand());
            commands.Add(new PatternDetailCommand());
            commands.Add(new StackDetailCommand());
            commands.Add(new StackFeedBackCommand());
            return commands;
        }
    }
}
