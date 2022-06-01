using NJIS.FPZWS.Platform.Contract;
using NJIS.FPZWS.Platform.Model;
using NJIS.FPZWS.Platform.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace NJIS.FPZWS.Platform.Service
{
    public class EdgebandingService : IEdgebandingContract
    {
        public Edgebanding Find(Expression<Func<Edgebanding, bool>> predicate)
        {
            var repository = new EdgebandingRepository();
            return repository.Find(predicate);
        }

        public IEnumerable<Edgebanding> FindAll()
        {
            return FindAll(null);
        }

        public IEnumerable<Edgebanding> FindAll(Expression<Func<Edgebanding, bool>> predicate)
        {
            var repository = new EdgebandingRepository();
            return repository.FindAll(predicate);
        }

        public bool Insert(Edgebanding entity)
        {
            var repository = new EdgebandingRepository();
            return repository.Insert(entity);
        }

        public bool Insert(string adress, StringBuilder sql)
        {
            var repository = new EdgebandingRepository();
            var rst = repository.Connection.Execute(
                $"INSERT INTO {adress}.[NJIS.FPZWS.LineControl.Edgebanding].[dbo].[Edgebanding]([Id],[BatchName] ,[OrderNumber],[BarCode],[Description],[Width] ,[Length],[Thickness],[L1_OFFCUT],[L1_FORMAT],[L1_EDGE],[L1_CORNER],[L1_GROOVE],[L1_EDGECODE],[L2_FORMAT] ,[L2_EDGE] ,[L2_CORNER] ,[L2_EDGECODE] ,[C1_OFFCUT],[C1_FORMAT],[C1_EDGE],[C1_CORNER],[C1_EDGECODE],[C1_GROOVE],[C2_FORMAT],[C2_EDGE],[C2_CORNER],[C2_EDGECODE],[CreatedTime],[UpdatedTime],[TaskId],[TaskDistributeId]){sql}");
            return rst > 0;
        }

        public IEnumerable<Edgebanding> GetTaskDistributeId(DateTime starttime, DateTime endtime)
        {
            var repository = new EdgebandingRepository();
            var ret = repository.Connection.Query<Edgebanding>($"select distinct [BatchName],[CreatedTime] from  [NJIS.FPZWS.LineControl.Edgebanding].[dbo].[Edgebanding] where CreatedTime > '{starttime.ToString("yyyy-MM-dd")}' and CreatedTime < '{endtime.AddDays(1).ToString("yyyy-MM-dd")}'");
            return ret;
            //select distinct [TaskDistributeId],[CreatedTime] from[NJIS.FPZWS.LineControl.Edgebanding].[dbo].[Edgebanding] where CreatedTime > '2018-10-21' and CreatedTime < '2018-10-26'[TaskId],[TaskDistributeId],
        }

        public bool DelectOldData(string address, string TaskDistributeId)
        {
            var repository = new EdgebandingRepository();
            //var sql = "DELETE from [" + address + "].[NJIS.FPZWS.LineControl.Edgebanding].[dbo].[Edgebanding] where [TaskDistributeId] = '" + TaskDistributeId +"'";
            var ret = repository.Connection.Execute(
                "DELETE  from [" + address + "].[NJIS.FPZWS.LineControl.Edgebanding].[dbo].[Edgebanding] where [TaskDistributeId] = '" + TaskDistributeId + "'");
            //var ret = repository.Connection.Execute(sql,null);
            return ret > 0;
        }
    }
}
