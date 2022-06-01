namespace NJIS.FPZWS.LineControl.Cutting.ModelPlus
{
    /// <summary>
    /// 上料状态
    /// </summary>
    public enum LoadMaterialStatus
    {
        /// <summary>
        /// 等待上料
        /// </summary>
        UnLoadMaterial=10,
        /// <summary>
        /// 上料中
        /// 
        /// </summary>
        LoadingMaterial=20,
        /// <summary>
        /// 上料结束
        /// </summary>
        LoadedMaterial=30
    }
}
