//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Plc
//   文 件 名：DbProcCommand.cs
//   创建时间：2018-12-13 16:56
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:56
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Edgebanding.Contract;
using NJIS.FPZWS.LineControl.Edgebanding.Plc.Control.Entitys;
using NJIS.FPZWS.LineControl.PLC;

namespace NJIS.FPZWS.LineControl.Edgebanding.Plc.Control.Commands
{
    public abstract class DbProcCommand<TI, TO> : EdgebandingCommandBase<TI, TO>
        where TI : DbProcInputEntity, new()
        where TO : DbProcOutputEntity, new()
    {
        private readonly IEdgebandingContract _drillingContract =
            ServiceLocator.Current.GetInstance<IEdgebandingContract>();

        public DbProcCommand() : base("DbProcCommand")
        {
        }

        public DbProcCommand(string commandName) : base(commandName)
        {
        }


        protected override EntityBase Execute(IPlcConnector plc)
        {
            // 获取输入参数
            var args = new Dictionary<string, object>();
            foreach (var entityPlcMap in EntityPlcMaps.FindAll(m => m.Direction == PlcVariableDirection.Input))
            {
                var obj = entityPlcMap.PropertyInfo.GetValue(Input);
                args.Add(entityPlcMap.PropertyInfo.Name, obj);
            }

            // 执行动作
            var rst = _drillingContract.PcsProc(Input.ProcName, args);

            //赋值给Output
            if (rst != null && rst.Rows.Count > 0)
            {
                // 根据返回值的列名称
                foreach (var entityPlcMap in EntityPlcMaps.FindAll(m => m.Direction == PlcVariableDirection.Output))
                {
                    if (!rst.Columns.Contains(entityPlcMap.PropertyInfo.Name)) continue;
                    var obj = rst.Rows[0][entityPlcMap.PropertyInfo.Name];
                    entityPlcMap.PropertyInfo.SetValue(Output, obj);
                }

                Output.Trigger = true;
                Output.TriggerOut = Input.TriggerOut + 1;
            }

            return Output;
        }
    }
}