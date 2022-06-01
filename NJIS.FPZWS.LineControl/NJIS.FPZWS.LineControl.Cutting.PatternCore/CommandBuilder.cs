using System.Collections.Generic;
using NJIS.FPZWS.LineControl.Cutting.PatternCore.Command.Args;

namespace NJIS.FPZWS.LineControl.Cutting.PatternCore
{
    public abstract class CommandBuilder
    {
        /// <summary>
        ///     创建链式缓存
        /// </summary>
        /// <returns></returns>
        public abstract List<ICommand> CreatePatternCommands();

        /// <summary>
        ///     创建入板器
        /// </summary>
        /// <returns></returns>
        public abstract List<ICommand> CreateStackCommands();

        /// <summary>
        ///     创建抽检器
        /// </summary>
        /// <returns></returns>
        public abstract List<ICommand> CreateFeedBackCommands();

    }
}
