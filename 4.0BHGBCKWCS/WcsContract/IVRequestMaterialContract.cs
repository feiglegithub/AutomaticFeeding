using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcsModel;

namespace Contract
{
    public interface IVRequestMaterialContract
    {
        /// <summary>
        /// 获取要料请求
        /// </summary>
        /// <returns></returns>
        List<V_RequestMaterial> GetRequestMaterials();
        /// <summary>
        /// 插入要料请求
        /// </summary>
        /// <param name="groupId">组id</param>
        /// <param name="reqId">要料请求id</param>
        /// <returns></returns>
        bool InsertGroupRequestId(long groupId, long reqId);
    }
}
