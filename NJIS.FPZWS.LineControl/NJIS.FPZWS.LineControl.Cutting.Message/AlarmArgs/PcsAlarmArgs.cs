namespace NJIS.FPZWS.LineControl.Cutting.Message.AlarmArgs
{
    public class PcsAlarmArgs:AlarmArgsBase
    {
        private const string _Category = "PCS";
        public PcsAlarmArgs(string paramName, string msg)
        {
            Category = _Category;
            ParamName = paramName;
            Value = msg;
        }
    }
}
