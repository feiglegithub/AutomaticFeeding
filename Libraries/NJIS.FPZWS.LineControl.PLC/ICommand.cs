//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：ICommand.cs
//   创建时间：2018-11-20 14:37
//   作    者：
//   说    明：
//   修改时间：2018-11-20 14:37
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.PLC
{
    /// <summary>
    ///     PLC 命令
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        ///     命令代码
        /// </summary>
        string CommandCode { get; }

        /// <summary>
        ///     是否为同步命令
        /// </summary>
        bool IsSync { get; set; }
    }

    /// <summary>
    ///     PLC 命令
    /// </summary>
    /// <typeparam name="TInput">输入值</typeparam>
    /// <typeparam name="TOutput">输出值</typeparam>
    public interface ICommand<TInput, TOutput> : ICommand
        where TInput : EntityBase
        where TOutput : EntityBase
    {
        /// <summary>
        ///     命令输入参数
        /// </summary>
        TInput GetInput();

        /// <summary>
        ///     命令输出参数
        /// </summary>
        TOutput GetOutput();

        /// <summary>
        ///     执行命令
        /// </summary>
        /// <returns></returns>
        TOutput ExecuteCommand(IPlcConnector plc);
    }

    /// <summary>
    ///     输入命令
    ///     输出数据到 PLC
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public interface IInputCommand<TInput> : ICommand<TInput, EmptyEntityBase>
        where TInput : EntityBase
    {
    }

    /// <summary>
    ///     输出命令
    ///     从 PLC 获取数据
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    public interface IOutPutCommand<TOutput> : ICommand<EmptyEntityBase, TOutput>
        where TOutput : EntityBase
    {
    }
}