using NJIS.FPZWS.LineControl.PLC;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Control.Entitys
{
    public class InPartInputEntity: EntityBase
    {
        public string PartId { get; set; }
        public int Position { get; set; }
        /// <summary>
        /// 是否为Ng或者抽检板件请求
        /// </summary>
        public bool IsNgRequest { get; set; } = true;
        /// <summary>
        /// 交互点名称
        /// </summary>
        public string InteractionPoints { get; set; }
    }
}
