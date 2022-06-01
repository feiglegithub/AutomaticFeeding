using System;
using WCS.DataBase;
using WCS.model;
using WCS.OPC;

namespace WCS
{
    public  static class EmptyReq
    {
        public static void ReqAll()
        {
            EmptyOutReq();
        }

        static void EmptyOutReq()
        {
            try
            {

                if (!OPCExecute.IsConn) { return; }

                var pilerscount = OpcHsc.ReadEmptyBuffersCount();  //获取缓存的空垫板垛数

                if (pilerscount >= 1)
                {
                    return;
                }

                var wmscount = WCSSql.GetEmptyPadTask();  //获取空垫板补料有效任务数
                if (pilerscount + wmscount < 1)
                {
                    var req = new RequestInfo()
                    {
                        ReqType = 2,
                        ProductCode = "空垫板",
                        Amount = 5,
                        ToPosition = "2005"
                    };

                    //向WMS发出要料请求
                    var wms_rlt = WCSSql.RequestTask(req);

                    if (wms_rlt.Status == 200)
                    {
                        WCSSql.InsertLog($"申请空垫板(补料)成功！请求编号：{wms_rlt.ReqId}，目标位：2005", "LOG");
                    }
                    else
                    {
                        WCSSql.InsertLog($"申请空垫板(补料)失败！错误信息：{wms_rlt.Message}", "ERROR");
                    }
                }
            }
            catch (Exception ex)
            {
                WCSSql.InsertLog($"申请空垫板(补料)失败！错误信息：{ex.Message}", "ERROR");
            }
        }
    }
}
