//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Service
//   文 件 名：DrillingService.cs
//   创建时间：2018-11-07 11:03
//   作    者：
//   说    明：
//   修改时间：2018-11-07 11:03
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.LineControl.Drilling.Contract;
using NJIS.FPZWS.LineControl.Drilling.Model;
using NJIS.FPZWS.LineControl.Drilling.Repository;

namespace NJIS.FPZWS.LineControl.Drilling.Service
{
    public class DrillingService : IDrillingContract
    {
        public Model.Drilling FindDrilling(string partId)
        {
            var repository = new DrillingRepository(DrillingDbSetting.Current.DbConnect);
            return repository.Find(m => m.PartID == partId);
        }

        public List<Model.Drilling> FindDrillings(string batchName)
        {
            var repository = new DrillingRepository(DrillingDbSetting.Current.DbConnect);
            return repository.FindAll(m => m.BatchName == batchName).ToList();
        }

        public List<Model.Drilling> FindDrillings(DateTime productionDate)
        {
            var repository = new DrillingRepository(DrillingDbSetting.Current.DbConnect);
            Expression<Func<Model.Drilling, bool>> express = m => true;
            express = express.And(m =>
                m.ProductionDate >= DateTime.Parse(productionDate.ToString("yyyy-MM-dd 00:00:00")));
            express = express.And(m =>
                m.ProductionDate <= DateTime.Parse(productionDate.ToString("yyyy-MM-dd 23:59:59")));

            return repository.FindAll(express).ToList();
        }

        public List<PcsPartInfoQueue> FindPartInfoQueues(int top)
        {
            var repository = new PcsPartInfoQueueRepository(DrillingDbSetting.Current.DbConnect);
            return repository.Connection.Query<PcsPartInfoQueue>($"select top {top} * from PcsPartInfoQueue order by LineId desc ").ToList();
        }

        public PcsPartInfoQueue InsertPartInfoQueues(Model.Drilling part, int position)
        {
            var repository = new PcsPartInfoQueueRepository(DrillingDbSetting.Current.DbConnect);

            var ppiq = repository.Connection.QueryFirst<PcsPartInfoQueue>("PcsInParter", new
            {
                PartId = part.PartID,
                Position = position,
                BatchName = part.BatchName,
                OrderNumber = part.OrderNumber,
                DrillingRouting = part.DrillingRouting,
                FinishLength = part.FinishLength,
                FinishWidth = part.FinishWidth,
                FinishThickness = part.FinishThickness

            }, commandType: CommandType.StoredProcedure, commandTimeout: 10);
            return ppiq;
        }

        public bool UpdatePartInfoQueuesPlace(string partId, string place)
        {
            var repository = new PcsPartInfoQueueRepository(DrillingDbSetting.Current.DbConnect);
            return repository.Connection.Execute($"update PcsPartInfoQueue set place='{place}' where partId='{partId}' ") > 0;

        }

        public string DeletePartInfoQueues(string partId)
        {
            var repository = new PcsPartInfoQueueRepository(DrillingDbSetting.Current.DbConnect);
            return repository.Connection.QueryFirst<string>($"PcsDeletePartInfoQueue", new { PartId = partId }, null, null, CommandType.StoredProcedure);
        }

        public List<PcsMachine> FindAllMachine()
        {
            var repository = new PcsMachineRepository(DrillingDbSetting.Current.DbConnect);
            return repository.FindAll(m => m.Code != "").ToList();
        }

        PcsMachine IDrillingContract.FindMachine(string code)
        {
            var repository = new PcsMachineRepository(DrillingDbSetting.Current.DbConnect);
            return repository.Find(m => m.Code == code);
        }

        public bool UpdateMachine(PcsMachine entity)
        {
            var repository = new PcsMachineRepository(DrillingDbSetting.Current.DbConnect);
            return repository.Connection.Execute($"UPDATE PCSMACHINE SET STATUS={entity.Status},IsProcessDouble={entity.IsProcessDouble},IsProcessSingle={entity.IsProcessSingle} WHERE CODE='{entity.Code}'") > 0;
        }

        public List<ChainBuffer> FindChainBuffers()
        {
            var repository = new ChainBufferRepository(DrillingDbSetting.Current.DbConnect);
            return repository.FindAll(m => m.LineId > 0).ToList();
        }

        public bool UpdateChainBufferStatus(string code, int status)
        {
            var repository = new ChainBufferRepository(DrillingDbSetting.Current.DbConnect);
            return repository.Connection.Execute($"Update ChainBuffer set Status={status} where Code='{code}'") > 0;
        }

        public List<PcsPartPosition> FindPartPositions(string partId)
        {
            var repository = new PcsPartPositionRepository(DrillingDbSetting.Current.DbConnect);
            return repository.FindAll(m => m.PartId == partId).ToList();
        }

        public DataTable PcsProc(string procName, object args)
        {
            var repository = new DrillingRepository(DrillingDbSetting.Current.DbConnect);
            var table = repository.ExecuteProcToDataTable(procName, args);
            return table;
        }

        public List<string> GetTestPart()
        {
            var repository = new DrillingRepository(DrillingDbSetting.Current.DbConnect);
            var jdds = repository.Connection.Query<string>("select top 10000 PartID from Drilling order by LineID");
            return jdds.ToList();
        }

        public void SaveDrillingImport(DrillingImport entity)
        {
            var repository = new DrillingImportRepository(DrillingDbSetting.Current.DbConnect);
            repository.Insert(entity);
        }

        public bool CheckDrillingImport(string batch,int way,string machine)
        {
            var repository = new DrillingImportRepository(DrillingDbSetting.Current.DbConnect);
            var jdds = repository.Connection.Execute($"select count(*) from DrillingImport where BatchName='{batch}' and way={way} and Machine='{machine}'");
            return jdds > 0;
        }

        public List<string> GetNotImportBatchs(DateTime dt, string machine)
        {
            var repository = new DrillingImportRepository(DrillingDbSetting.Current.DbConnect);
            var jdds = repository.Connection.Query<string>("select distinct a.BatchName from Drilling a " +
                                                     $"left join DrillingImport b on a.BatchName = b.BatchName  and Machine='{machine}' " +
                                                     $"where a.CreatedTime > '{dt.ToString("yyyy-MM-dd HH:mm:ss")}' and b.batchName is null");
            return jdds.ToList();
        }

        public bool ExistsNg(string partId)
        {
            var repository = new PcsNgRepository(DrillingDbSetting.Current.DbConnect);

            return repository.Connection.Execute($"select count(*) from  PcsNg where PartId='{partId}'") > 0;
        }

        public PcsNg FindNg(string partId)
        {
            var repository = new PcsNgRepository(DrillingDbSetting.Current.DbConnect);

            return repository.Find(m => m.PartId == partId);
        }

        public List<PcsNg> FindNgs(int top)
        {
            var repository = new PcsNgRepository(DrillingDbSetting.Current.DbConnect);

            return repository.Connection.Query<PcsNg>($"select top {top} * from PcsNg order by LineId ").ToList();
        }

        public bool InsertNg(PcsNg entity)
        {
            var repository = new PcsNgRepository(DrillingDbSetting.Current.DbConnect);

            return repository.Insert(entity);
        }

        public bool DeleteNg(string partId)
        {
            var repository = new PcsNgRepository(DrillingDbSetting.Current.DbConnect);
            return repository.Connection.Execute($"Delete PcsNg where PartId='{partId}'") > 0;
        }
    }
}