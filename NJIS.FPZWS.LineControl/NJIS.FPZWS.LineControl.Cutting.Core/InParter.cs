namespace NJIS.FPZWS.LineControl.Cutting.Core
{
    /// <summary>
    ///     入板器
    /// </summary>
    public class InParter
    {
        /// <summary>
        ///     加入一块板件
        /// </summary>
        /// <returns></returns>
        public virtual ControlPartInfo InPart(string partId, int position)
        {
            return new ControlPartInfo();
        }
    }
}
