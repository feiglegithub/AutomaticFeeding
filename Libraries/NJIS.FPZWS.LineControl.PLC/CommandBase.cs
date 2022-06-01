//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC
//   文 件 名：CommandBase.cs
//   创建时间：2018-11-20 14:37
//   作    者：
//   说    明：
//   修改时间：2018-11-20 14:37
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Linq;
using NJIS.FPZWS.Log;

#endregion

namespace NJIS.FPZWS.LineControl.PLC
{
    public abstract class CommandBase : ICommand<EntityBase, EntityBase>
    {
        protected readonly ILogger _logger = LogManager.GetLogger<CommandBase>();

        protected CommandBase()
        {
            EntityPlcMaps = new List<EntityPlcMap>();
        }

        public bool IsClearData { get; set; }

        public List<EntityPlcMap> EntityPlcMaps { get; protected set; }

        public int CommandExecutInterval { get; set; } = 100;

        public string CommandCode { get; set; }
        public bool IsSync { get; set; } = true;


        public abstract EntityBase GetInput();

        public abstract EntityBase GetOutput();

        public virtual EntityBase ExecuteCommand(IPlcConnector plc)
        {
            OnCommandExecuting(this, new CommandEventArgs<EntityBase, EntityBase>(GetInput(), GetOutput()));
            Execute(plc);
            OnCommandExecuted(this, new CommandEventArgs<EntityBase, EntityBase>(GetInput(), GetOutput()));

            return GetOutput();
        }


        public virtual bool LoadInput(IPlcConnector plc)
        {
            var inputs = EntityPlcMaps.FindAll(m => m.Direction == PlcVariableDirection.Input);
            //var filters = new string[] { "trigger", "triggerin", "triggerout" };
            var filters = new string[] { };
            var flag = true;
            foreach (var entityPlcMap in inputs)
            {
                if (filters.Contains(entityPlcMap.PropertyInfo.Name.ToLower())) continue;
                object val = null;
                if (entityPlcMap.IsMap)
                {
                    switch (entityPlcMap.ValueType)
                    {
                        case PlcValType.Bit:
                            val = plc.ReadBool(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.Double:
                            val = plc.ReadDouble(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.Int:
                            val = plc.ReadInt(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.Lint:
                            val = plc.ReadLong(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.Real:
                            val = plc.ReadReal(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.Short:
                            val = plc.ReadShort(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.Byte:
                            val = plc.ReadByte(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.String:
                            if (entityPlcMap.Length > 0)
                            {
                                val = plc.ReadString(entityPlcMap.PlcVariable, entityPlcMap.Length);
                            }
                            else
                            {
                                val = plc.ReadString(entityPlcMap.PlcVariable);
                            }

                            break;
                        case PlcValType.Binary:
                            val = plc.ReadBytes(entityPlcMap.PlcVariable, (ushort)entityPlcMap.Length);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (entityPlcMap.ValueType)
                    {
                        case PlcValType.Bit:
                            val = bool.Parse(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.Double:
                            val = double.Parse(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.Int:
                        case PlcValType.Lint:
                        case PlcValType.Short:
                            val = int.Parse(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.Real:
                            val = float.Parse(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.String:
                            val = entityPlcMap.PlcVariable;
                            break;
                        case PlcValType.Byte:
                            val = byte.Parse(entityPlcMap.PlcVariable);
                            break;
                        case PlcValType.Binary:
                            break;
                        default:
                            break;
                    }
                }

                try
                {
                    _logger.Debug($"{entityPlcMap.PropertyInfo.Name}:{entityPlcMap.IsMap}[{entityPlcMap.ValueType}]=>{val}");
                    entityPlcMap.PropertyInfo.SetValue(GetInput(), val);
                }
                catch (Exception e)
                {
                    _logger.Error($"读取PLC变量失败:{e.Message}");
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        public virtual bool LoadOutput(IPlcConnector plc)
        {
            // 不写
            if (GetOutput() == null) return true;

            var outputs = EntityPlcMaps.FindAll(m => m.Direction == PlcVariableDirection.Output);
            //var filters = new string[] { "trigger", "triggerin" };
            var filters = new string[] { };
            var flag = true;
            var canWriteLastValue = true;
            var needWriteOutputs = outputs.FindAll(item => item.IsMap).ToList();
            needWriteOutputs.Sort((x,y)=>x.WriteIndex.CompareTo(y.WriteIndex));
            foreach (var entityPlcMap in needWriteOutputs)
            {
                if (filters.Contains(entityPlcMap.PropertyInfo.Name.ToLower())) continue;

                var val = entityPlcMap.PropertyInfo.GetValue(GetOutput());
                var ops = true;
                if (!canWriteLastValue) break;
                //if ((needWriteOutputs.Count==0||needWriteOutputs.Last().Equals(entityPlcMap)) || !canWriteLastValue) break;
                if (entityPlcMap.IsMap && val != null)
                {
                    try
                    {
                        _logger.Debug($"{entityPlcMap.PropertyInfo.Name}:{entityPlcMap.IsMap}[{entityPlcMap.ValueType}]=>{val}");
                        switch (entityPlcMap.ValueType)
                        {
                            case PlcValType.Bit:
                                ops = plc.WriteBool(entityPlcMap.PlcVariable, val.ToBool());
                                if (entityPlcMap.IsCheck)
                                {
                                    var writedValue = plc.ReadBool(entityPlcMap.PlcVariable);
                                    canWriteLastValue &= (val.ToBool() == writedValue);
                                }
                                break;
                            case PlcValType.Double:
                                ops = plc.WriteDouble(entityPlcMap.PlcVariable, val.ToDouble());
                                if (entityPlcMap.IsCheck)
                                {
                                    var writedValue = plc.ReadDouble(entityPlcMap.PlcVariable);
                                    canWriteLastValue &= (val.ToDouble() == writedValue);
                                }
                                break;
                            case PlcValType.Int:
                                ops = plc.WriteInt(entityPlcMap.PlcVariable, val.ToInt());
                                if (entityPlcMap.IsCheck)
                                {
                                    var writedValue = plc.ReadInt(entityPlcMap.PlcVariable);
                                    canWriteLastValue &= (val.ToInt() == writedValue);
                                }
                                break;
                            case PlcValType.Lint:
                                ops = plc.WriteLong(entityPlcMap.PlcVariable, val.ToLong());
                                if (entityPlcMap.IsCheck)
                                {
                                    var writedValue = plc.ReadLong(entityPlcMap.PlcVariable);
                                    canWriteLastValue &= (val.ToLong() == writedValue);
                                }
                                break;
                            case PlcValType.Real:
                                ops = plc.WriteReal(entityPlcMap.PlcVariable, val.ToFloat());
                                if (entityPlcMap.IsCheck)
                                {
                                    var writedValue = plc.ReadReal(entityPlcMap.PlcVariable);
                                    canWriteLastValue &= (val.ToFloat() == writedValue);
                                }
                                break;
                            case PlcValType.Short:
                                ops = plc.WriteShort(entityPlcMap.PlcVariable, val.ToShort());
                                if (entityPlcMap.IsCheck)
                                {
                                    var writedValue = plc.ReadShort(entityPlcMap.PlcVariable);
                                    canWriteLastValue &= (val.ToShort() == writedValue);
                                }
                                break;
                            case PlcValType.Byte:
                                ops = plc.WriteByte(entityPlcMap.PlcVariable, val.ToByte());
                                if (entityPlcMap.IsCheck)
                                {
                                    var writedValue = plc.ReadByte(entityPlcMap.PlcVariable);
                                    canWriteLastValue &= (val.ToShort() == writedValue);
                                }
                                break;
                            case PlcValType.String:
                                var v = val.ToString();
                                if (v.Length > entityPlcMap.Length)
                                {
                                    v = v.Substring(0, entityPlcMap.Length);
                                }
                                ops = plc.WriteString(entityPlcMap.PlcVariable, v);
                                if (entityPlcMap.IsCheck)
                                {
                                    var writedValue = plc.ReadString(entityPlcMap.PlcVariable);
                                    canWriteLastValue &= (v == writedValue);
                                }
                                break;
                            case PlcValType.Binary:
                                var bts = val as byte[];
                                ops = plc.WriteBytes(entityPlcMap.PlcVariable, bts);
                                if (entityPlcMap.IsCheck)
                                {
                                    var writedValue = plc.ReadBytes(entityPlcMap.PlcVariable,Convert.ToUInt16(bts.Length));
                                    canWriteLastValue &= (BytesEquals(bts, writedValue));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"写入PLC变量失败:{e.Message}");
                        flag = false;
                    }
                }

                if (!ops)
                {
                    flag = false;
                    break;
                }
            }

            return flag && canWriteLastValue;
        }

        private bool BytesEquals(Byte[] left, Byte[] right)
        {
            if (left == null || right == null) return false;
            if (left.Length != right.Length) return false;
            if (left.Length == 0 && right.Length == 0) return true;
            bool compareResult = true;
            for (int i = 0; i < left.Length; i++)
            {
                compareResult &= (left[i].CompareTo(right[i]) == 0);
                if (!compareResult) return false;
            }

            return true;
        }

        /// <summary>
        ///     验证输入参数
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckInput(IPlcConnector plc)
        {
            var inputs = EntityPlcMaps.FindAll(m => m.Direction == PlcVariableDirection.Input);
            var trigger = inputs.Find(m => m.PropertyInfo.Name.ToLower() == "trigger");
            if (trigger != null)
            {
                _logger.Debug($"[{trigger.PropertyInfo.Name}]:[{trigger.PlcVariable}][{trigger.IsMap}]");
                if (trigger.IsMap)
                {
                    _logger.Debug($"[{trigger.PropertyInfo.Name}]:[{trigger.PlcVariable}][{trigger.IsMap}]=>true");
                    return plc.ReadBool(trigger.PlcVariable);
                }

                return trigger.PlcVariable.ToLower() == "true";
            }


            var triggerInt = inputs.Find(m => m.PropertyInfo.Name.ToLower() == "triggerin");
            var triggerOut = inputs.Find(m => m.PropertyInfo.Name.ToLower() == "triggerout");

            if (triggerInt != null && triggerOut != null)
            {
                var ti = triggerInt.IsMap ? plc.ReadInt(triggerInt.PlcVariable) : int.Parse(triggerInt.PlcVariable);
                var to = triggerInt.IsMap ? plc.ReadInt(triggerOut.PlcVariable) : int.Parse(triggerOut.PlcVariable);
                _logger.Debug($"[{triggerInt.PropertyInfo.Name}]:[{triggerInt.PlcVariable}][{triggerInt.IsMap}]=>{ti}");
                _logger.Debug($"[{triggerOut.PropertyInfo.Name}]:[{triggerOut.PlcVariable}][{triggerOut.IsMap}]=>{to}");

                if (ti > 0 && ti == to + 1)
                {
                    return true;
                }

                return false;
            }

            return true;
        }

        public event Action<object, CommandEventArgs<EntityBase, EntityBase>> CommandExecuting;
        public event Action<object, CommandEventArgs<EntityBase, EntityBase>> CommandExecuted;

        protected virtual void OnCommandExecuting(object arg1, CommandEventArgs<EntityBase, EntityBase> arg2)
        {
            CommandExecuting?.Invoke(arg1, arg2);
        }

        protected virtual void OnCommandExecuted(object arg1, CommandEventArgs<EntityBase, EntityBase> arg2)
        {
            CommandExecuted?.Invoke(arg1, arg2);
        }

        protected abstract EntityBase Execute(IPlcConnector plc);
    }

    public abstract class CommandBase<TInput, TOutput> : CommandBase
        where TInput : EntityBase, new()
        where TOutput : EntityBase, new()

    {
        protected CommandBase() : this("unknown")
        {
        }

        protected CommandBase(string commandCode)
        {
            CommandCode = commandCode;

            Input = new TInput();
            Output = new TOutput();
        }


        public TInput Input { get; set; }
        public TOutput Output { get; set; }

        public override EntityBase GetInput()
        {
            return Input;
        }

        public override EntityBase GetOutput()
        {
            return Output;
        }
    }
}