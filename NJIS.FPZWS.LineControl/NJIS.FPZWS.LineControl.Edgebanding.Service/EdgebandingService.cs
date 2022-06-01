//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Service
//   文 件 名：EdgebandingService.cs
//   创建时间：2018-12-13 14:35
//   作    者：
//   说    明：
//   修改时间：2018-12-13 14:35
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using NJIS.FPZWS.LineControl.Edgebanding.Contract;
using NJIS.FPZWS.LineControl.Edgebanding.Model;
using NJIS.FPZWS.LineControl.Edgebanding.Repository;

namespace NJIS.FPZWS.LineControl.Edgebanding.Service
{
    public class EdgebandingService : IEdgebandingContract
    {
        public Model.Edgebanding FindEdgebanding(Expression<Func<Model.Edgebanding, bool>> predicate)
        {
            var repository = new EdgebandingRepository(EdgebandingDbSetting.Current.DbConnect);
            return repository.Find(predicate);
        }

        public IEnumerable<Model.Edgebanding> FindEdgebandings()
        {
            return FindEdgebandings(null);
        }

        public IEnumerable<Model.Edgebanding> FindEdgebandings(Expression<Func<Model.Edgebanding, bool>> predicate)
        {
            var repository = new EdgebandingRepository(EdgebandingDbSetting.Current.DbConnect);
            return repository.FindAll(predicate);
        }

        public bool InsertEdgebanding(Model.Edgebanding entity)
        {
            var repository = new EdgebandingRepository(EdgebandingDbSetting.Current.DbConnect);
            return repository.Insert(entity);
        }

        public DataTable PcsProc(string procName, object args)
        {
            var repository = new EdgebandingRepository(EdgebandingDbSetting.Current.DbConnect);

            var table = repository.ExecuteProcToDataTable(procName, args);
            return table;
        }

        public bool InsertEdgebandQueue(PcsEdgebandQueue entity)
        {
            var repository = new PcsEdgebandQueueRepository(EdgebandingDbSetting.Current.DbConnect);
            return repository.Insert(entity);
        }
        public List<string> FindPartIds(DateTime dt)
        {
            var repository = new PcsEdgebandQueueRepository(EdgebandingDbSetting.Current.DbConnect);
            return repository.Connection.Query<string>($"select BarCode from [Edgebanding] where CreatedTime>'{dt.ToString("yyyy-MM-dd HH:mm:ss")}'").ToList();
        }
    }
}