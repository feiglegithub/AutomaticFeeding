using NJIS.Ini;

namespace NJIS.FPZWS.LineControl.Cutting.PatternCore
{
    [IniFile]
    public class PatternAppSetting : SettingBase<PatternAppSetting>
    {
        [Property("core")]
        public string PatternBuilder { get; set; } = "NJIS.FPZWS.LineControl.Cutting.PatternDomain.RxCommandBuilder,NJIS.FPZWS.LineControl.Cutting.PatternDomain";

        [Property("OnePartByteLength")]
        public int OnePartByteLength { get; set; } = 22;
        [Property("SwapTriggerMinTime")]
        public int SwapTriggerMinTime { get; set; } = 600;
    }
}
