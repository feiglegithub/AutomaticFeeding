using System;

namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    public class CommandMsg
    {
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime StartTime { get; set; }
        public DateTime FinishedTime { get; set; }
        public TimeSpan TimeSpan => FinishedTime - StartTime;
        public string CommandName { get; set; }
        public string Msg { get; set; }

    }
}
