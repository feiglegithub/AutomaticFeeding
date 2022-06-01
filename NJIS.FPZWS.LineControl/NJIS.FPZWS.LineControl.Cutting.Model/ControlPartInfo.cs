namespace NJIS.FPZWS.LineControl.Cutting.Model
{
    /// <summary>
    ///     线控板件信息
    /// </summary>
    public class ControlPartInfo
    {
        public string PartId { get; set; }
        public string BatchName { get; set; }
        //public string OrderNumber { get; set; }
        public string PcsMessage { get; set; }
        public string DeviceName { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }

    }
}
