//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Contract
//   文 件 名：IEdgebandingContract.cs
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
using System.Linq.Expressions;
using NJIS.FPZWS.Common.Dependency;

namespace NJIS.FPZWS.LineControl.Edgebanding.Contract
{
    public interface IEdgebandingContract : IScopeDependency
    {
        Model.Edgebanding FindEdgebanding(Expression<Func<Model.Edgebanding, bool>> predicate);
        IEnumerable<Model.Edgebanding> FindEdgebandings();
        IEnumerable<Model.Edgebanding> FindEdgebandings(Expression<Func<Model.Edgebanding, bool>> predicate);
        bool InsertEdgebanding(Model.Edgebanding entity);
        DataTable PcsProc(string procName, object args);
        bool InsertEdgebandQueue(Model.PcsEdgebandQueue entity);
    }
}