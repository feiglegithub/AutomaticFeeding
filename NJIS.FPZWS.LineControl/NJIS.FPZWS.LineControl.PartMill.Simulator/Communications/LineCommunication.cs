using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NJIS.FPZWS.LineControl.PartMill.Model;
using NJIS.FPZWS.LineControl.PartMill.Simulator.CommunicationBase.Plc;
using NJIS.FPZWS.LineControl.PartMill.Simulator.Models;

namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Communications
{
    public class LineCommunication
    {
        public LineCommunication(List<line_config> lineConfigs=null)
        {
            LineConfigs = lineConfigs;
        }

        public List<line_config> LineConfigs { get; set; }=new List<line_config>()
        {
            new line_config(){line_name = ELineName.Gt5001.ToString(),has_board_address = "DB4.0",piler_no_address = "DB4.2",target_address = "DB4.6",is_finished_address = "DB4.8",amount_address = "DB4.10",run_signal_address = "DB4.12",accept_task_address = "DB4.14",backup_string_address = "DB4.16"},
            new line_config(){line_name = ELineName.Gt5002.ToString(),has_board_address = "DB4.32",piler_no_address = "DB4.34",target_address = "DB4.38",is_finished_address = "DB4.40",amount_address = "DB4.42",run_signal_address = "DB4.44",accept_task_address = "DB4.46",backup_string_address = "DB4.48"},

            new line_config(){line_name = ELineName.Xz5003.ToString(),has_board_address = "DB4.76",piler_no_address = "DB4.78",target_address = "DB4.82",is_finished_address = "DB4.84",amount_address = "DB4.86",run_signal_address = "DB4.88",accept_task_address = "DB4.90",backup_string_address = "DB4.92"},
            new line_config(){line_name = ELineName.Gt5004.ToString(),has_board_address = "DB4.108",piler_no_address = "DB4.110",target_address = "DB4.114",is_finished_address = "DB4.116",amount_address = "DB4.118",run_signal_address = "DB4.120",accept_task_address = "DB4.122",backup_string_address = "DB4.124"},

            new line_config(){line_name = ELineName.Gt5005.ToString(),has_board_address = "DB4.140",piler_no_address = "DB4.142",target_address = "DB4.146",is_finished_address = "DB4.148",amount_address = "DB4.150",run_signal_address = "DB4.152",accept_task_address = "DB4.154",backup_string_address = "DB4.156"},

            new line_config(){line_name = ELineName.Gt5006.ToString(),has_board_address = "DB4.172",piler_no_address = "DB4.174",target_address = "DB4.178",is_finished_address = "DB4.180",amount_address = "DB4.182",run_signal_address = "DB4.184",accept_task_address = "DB4.186",backup_string_address = "DB4.188"},

        };

        private const string ip = "192.168.100.65";

        public PlcOperator Plc = new PlcOperator("192.168.100.65");

        public bool IsConnect { get; set; } = false;

        public bool Connect()
        {
            var ret = Plc.Connect(ip);
            IsConnect = ret;
            return IsConnect;
        }

        public bool Close()
        {
            if (!IsConnect) return true;
            var ret = Plc.Close();
            if (ret.IsSuccess)
            {
                IsConnect = false;
            }
            return ret.IsSuccess;
        }

        public LineModel ReadLineModel(ELineName lineName)
        {
            if (!IsConnect)
            {
                var ret = Connect();
                if (!ret) return null;
            }

            var config = LineConfigs.FirstOrDefault(item => item.line_name == lineName.ToString());
            if (config == null) return null;
            LineModel lineModel = new LineModel()
            {
                LineName = lineName.ToString(),
                Amount = Plc.ReadShort(config.amount_address),
                NeedRun = Plc.ReadBoolean(config.run_signal_address),
                BackupShort = Plc.ReadShort(config.accept_task_address),
                BackupString = Plc.ReadString(config.backup_string_address),
                HasBoard = Plc.ReadBoolean(config.has_board_address),
                IsFinished = Plc.ReadBoolean(config.is_finished_address),
                PilerNo = Plc.ReadInt(config.piler_no_address),
                Target = Plc.ReadShort(config.target_address)
                
            };

            return lineModel;
        }

        /// <summary>
        /// 写入目标地址
        /// </summary>
        /// <param name="fromLineName"></param>
        /// <param name="targetLineName"></param>
        /// <returns></returns>
        public OperationResult WriteTarget(ELineName fromLineName, ELineName targetLineName)
        {
            if (!IsConnect)
            {
                var ret = Connect();
                if (!ret) return new OperationResult(){Result = false,Msg = "连接Plc失败"};
            }

            var fromConfig = LineConfigs.FirstOrDefault(item => item.line_name == fromLineName.ToString());
            if (fromConfig == null) return new OperationResult() { Result = false, Msg = $"找不到线体{fromLineName.ToString()}的配置信息" };
            var targetConfig = LineConfigs.FirstOrDefault(item => item.line_name == targetLineName.ToString());
            if (targetConfig == null) return new OperationResult() { Result = false, Msg = $"找不到线体{targetLineName.ToString()}的配置信息" };

            var result = Plc.Write(fromConfig.target_address, targetConfig.position);
            if (result.IsSuccess)
            {
                // 写入启动信号
                result = Plc.Write(fromConfig.run_signal_address, true);
                
                if (result.IsSuccess)
                {
                    //var isAccept = Plc.ReadShort(fromConfig.accept_task_address)==1;
                    //int i = 20;
                    //while (i>0)
                    //{
                    //    isAccept = Plc.ReadShort(fromConfig.accept_task_address)==1;
                    //    if (isAccept)
                    //    {
                    //        break;
                    //    }
                    //    Thread.Sleep(50);
                    //    i--;
                    //}
                    
                    //result.IsSuccess &= isAccept;
                    //if (!isAccept)
                    //{
                    //    return  new OperationResult(){Msg = "线体接收任务超时!",Result = false};
                    //}

                    //清除接收信号
                    var ret = Plc.Write(fromConfig.accept_task_address, (short) 0);
                    if (!ret.IsSuccess)
                    {
                        Thread.Sleep(20);
                        ret = Plc.Write(fromConfig.accept_task_address, (short)0);
                        return new OperationResult(){Msg = ret.IsSuccess? "写入成功" : "清除线体接收信号失败!"+ret.Message,Result = ret.IsSuccess};
                    }

                    return new OperationResult() { Msg = "写入成功",Result = true};

                }
                else
                {
                    return new OperationResult(){Result = false,Msg = "写入启动信号失败！"+result.Message};
                }
            }
            else
            {
                return new OperationResult(){Result = false,Msg = result.Message};
            }
           

        }

        /// <summary>
        /// 写入垛号
        /// </summary>
        /// <param name="lineName"></param>
        /// <param name="pilerNo"></param>
        /// <returns></returns>
        public bool WritePilerNo(ELineName lineName, int pilerNo)
        {
            if (!IsConnect)
            {
                var ret = Connect();
                if (!ret) return false;
            }

            var config = LineConfigs.FirstOrDefault(item => item.line_name == lineName.ToString());
            if (config == null) return false;
            var result = Plc.Write(config.piler_no_address, pilerNo);
            return result.IsSuccess;
        }

        /// <summary>
        /// 写入板材数
        /// </summary>
        /// <param name="lineName"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool WriteAmount(ELineName lineName, short amount)
        {
            if (!IsConnect)
            {
                var ret = Connect();
                if (!ret) return false;
            }

            var config = LineConfigs.FirstOrDefault(item => item.line_name == lineName.ToString());
            if (config == null) return false;
            var result = Plc.Write(config.amount_address, amount);
            return result.IsSuccess;
        }

        /// <summary>
        /// 清除完成信号
        /// </summary>
        /// <param name="lineName"></param>
        /// <returns></returns>
        public bool ClearFinished(ELineName lineName)
        {
            if (!IsConnect)
            {
                var ret = Connect();
                if (!ret) return false;
            }

            var config = LineConfigs.FirstOrDefault(item => item.line_name == lineName.ToString());
            if (config == null) return false;
            var result = Plc.Write(config.is_finished_address, false);
            return result.IsSuccess;
        }

        /// <summary>
        /// 读取所有的线体数据
        /// </summary>
        /// <returns></returns>
        public List<LineModel> ReadAlLineModels()
        {
            if (!IsConnect)
            {
                var ret = Connect();
                if (!ret) return null;
            }
            List<LineModel> lineModels = new List<LineModel>();
            foreach (var config in LineConfigs)
            {
                LineModel lineModel = new LineModel()
                {
                    LineName = config.line_name,
                    Amount = Plc.ReadShort(config.amount_address),
                    NeedRun = Plc.ReadBoolean(config.run_signal_address),
                    BackupShort = Plc.ReadShort(config.accept_task_address),
                    BackupString = Plc.ReadString(config.backup_string_address),
                    HasBoard = Plc.ReadBoolean(config.has_board_address),
                    IsFinished = Plc.ReadBoolean(config.is_finished_address),
                    PilerNo = Plc.ReadInt(config.piler_no_address),
                    Target = Plc.ReadShort(config.target_address),
                    Position = config.position
                    
                };
                lineModels.Add(lineModel);
            }

            return lineModels;
        }

        public class OperationResult
        {
            public bool Result { get; set; }
            public string Msg { get; set; }
        }
    }
}
