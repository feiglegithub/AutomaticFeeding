//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：DbProcCommand.cs
//   创建时间：2018-11-26 8:56
//   作    者：
//   说    明：
//   修改时间：2018-11-26 8:56
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.LineControl.Drilling.Contract;
using NJIS.FPZWS.LineControl.Drilling.Domain.Control.Entitys;
using NJIS.FPZWS.LineControl.PLC;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Control.Commands
{
    public abstract class DbProcCommand<TI, TO> : DrillingCommandBase<TI, TO>
        where TI : DbProcInputEntity, new()
        where TO : DbProcOutputEntity, new()
    {
        private readonly IDrillingContract _drillingContract = ServiceLocator.Current.GetInstance<IDrillingContract>();

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
                MapperTOOutput(rst);
            }

            return Output;
        }

        private IList<PropertyInfo> properties = typeof(TO).GetProperties();
        public void MapperTOOutput(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            foreach (var item in properties)
            {
                if (dt.Columns.Contains(item.Name))
                {
                    item.SetValue(Output, System.Convert.ChangeType(dt.Rows[0][item.Name], item.PropertyType));
                }
            }
        }
    }
}