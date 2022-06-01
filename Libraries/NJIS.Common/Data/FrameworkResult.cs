namespace NJIS.Common.Data
{
    /// <summary>
    ///     Framework结果基类
    /// </summary>
    /// <typeparam name="TResultType"></typeparam>
    public abstract class FrameworkResult<TResultType> : FrameworkResult<TResultType, object>,
        IFrameworkResult<TResultType>
    {
        /// <summary>
        ///     初始化一个<see cref="FrameworkResult{TResultType}" />类型的新实例
        /// </summary>
        protected FrameworkResult()
            : this(default(TResultType))
        {
        }

        /// <summary>
        ///     初始化一个<see cref="FrameworkResult{TResultType}" />类型的新实例
        /// </summary>
        protected FrameworkResult(TResultType type)
            : this(type, null, null)
        {
        }

        /// <summary>
        ///     初始化一个<see cref="FrameworkResult{TResultType}" />类型的新实例
        /// </summary>
        protected FrameworkResult(TResultType type, string message)
            : this(type, message, null)
        {
        }

        /// <summary>
        ///     初始化一个<see cref="FrameworkResult{TResultType}" />类型的新实例
        /// </summary>
        protected FrameworkResult(TResultType type, string message, object data)
            : base(type, message, data)
        {
        }
    }


    /// <summary>
    ///     Framework结果基类
    /// </summary>
    /// <typeparam name="TResultType">结果类型</typeparam>
    /// <typeparam name="TData">结果数据类型</typeparam>
    public abstract class FrameworkResult<TResultType, TData> : IFrameworkResult<TResultType, TData>
    {
        /// <summary>
        ///     初始化一个<see cref="FrameworkResult{TResultType,TData}" />类型的新实例
        /// </summary>
        protected FrameworkResult()
            : this(default(TResultType))
        {
        }

        /// <summary>
        ///     初始化一个<see cref="FrameworkResult{TResultType,TData}" />类型的新实例
        /// </summary>
        protected FrameworkResult(TResultType type)
            : this(type, null, default(TData))
        {
        }

        /// <summary>
        ///     初始化一个<see cref="FrameworkResult{TResultType,TData}" />类型的新实例
        /// </summary>
        protected FrameworkResult(TResultType type, string message)
            : this(type, message, default(TData))
        {
        }

        /// <summary>
        ///     初始化一个<see cref="FrameworkResult{TResultType,TData}" />类型的新实例
        /// </summary>
        protected FrameworkResult(TResultType type, string message, TData data)
        {
            ResultType = type;
            Message = message;
            Data = data;
        }

        /// <summary>
        ///     获取或设置 结果类型
        /// </summary>
        public TResultType ResultType { get; set; }

        /// <summary>
        ///     获取或设置 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     获取或设置 结果数据
        /// </summary>
        public TData Data { get; set; }
    }
}