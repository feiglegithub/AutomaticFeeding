namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    public class SlotterBase : ISpotter
    {
        public SlotterBase()
        {
            Name = "默认抽检器";
        }

        public string Name { get; set; }

        public virtual bool IsSpot(string partId)
        {
            if (!string.IsNullOrEmpty(partId))
            {
                return true;
            }

            return false;
        }
    }
}
