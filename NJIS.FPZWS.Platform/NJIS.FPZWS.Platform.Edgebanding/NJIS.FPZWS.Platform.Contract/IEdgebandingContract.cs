using NJIS.FPZWS.Common.Dependency;
using NJIS.FPZWS.Platform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.Platform.Contract
{
    public interface IEdgebandingContract : IScopeDependency
    {
        Edgebanding Find(Expression<Func<Edgebanding, bool>> predicate);
        IEnumerable<Edgebanding> FindAll();

        IEnumerable<Edgebanding> FindAll(Expression<Func<Edgebanding, bool>> predicate);

        bool Insert(Edgebanding entity);

        bool Insert(string adress, StringBuilder sql);

        IEnumerable<Edgebanding> GetTaskDistributeId(DateTime starttime, DateTime endtime);

        bool DelectOldData(string address, string TaskDistributeId);
    }
}
