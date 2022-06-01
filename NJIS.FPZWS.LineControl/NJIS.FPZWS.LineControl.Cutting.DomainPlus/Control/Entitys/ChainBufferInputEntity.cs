using NJIS.FPZWS.LineControl.PLC;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Control.Entitys
{
    public class ChainBufferInputEntity : EntityBase
    {
        /// <summary>
        ///     链式缓存编号
        /// </summary>
        public string Code { get; set; }

        public byte[] Buffer { get; set; }
    }
}
