//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：CommandEventArgs.cs
//   创建时间：2018-11-20 14:38
//   作    者：
//   说    明：
//   修改时间：2018-11-20 14:38
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.PLC
{
    public class CommandEventArgs<TInput, TOutput>
        where TInput : EntityBase
        where TOutput : EntityBase
    {
        public CommandEventArgs(TInput input, TOutput output) : this(null, input, output)
        {
        }

        public CommandEventArgs(object sender, TInput input, TOutput output)
        {
            Input = input;
            Output = output;
            Sender = sender;
        }

        public object Sender { get; }
        public TInput Input { get; }
        public TOutput Output { get; }
    }
}