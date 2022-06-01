namespace NJIS.Common.Data
{
    /// <summary>
    ///     Framework操作结果
    /// </summary>
    /// <typeparam name="TResultType"></typeparam>
    public interface IFrameworkResult<TResultType> : IFrameworkResult<TResultType, object>
    {
    }


    /// <summary>
    ///     Framework操作结果
    /// </summary>
    public interface IFrameworkResult<TResultType, TData>
    {
        /// <summary>
        ///     获取或设置 结果类型
        /// </summary>
        TResultType ResultType { get; set; }

        /// <summary>
        ///     获取或设置 返回消息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        ///     获取或设置 结果数据
        /// </summary>
        TData Data { get; set; }
    }
}