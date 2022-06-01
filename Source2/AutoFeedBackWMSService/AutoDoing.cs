using AutoFeedBackWMSService.Common;
using System;
using System.Data;

namespace AutoFeedBackWMSService
{
    public class AutoDoing
    {
        public void Go()
        {
            try
            {
                //LogWrite.WriteLog("Hello!");
                //更改任务状态
                DoingUpdateTask();
            }
            catch (Exception ex)
            {
                LogWrite.WriteError(ex.Message);
            }
        }

        //过账任务
        void DoingUpdateTask()
        {
            string sql = "select * from  wcs_task where nwkstatus = 98 and errornum < 50";
            DataSet ds = Sql.GetSQL(sql);
            if (ds == null || ds.Tables[0].Rows.Count == 0)
                return;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string seqid = dr["Seqid"].ToString();
                string result = UpdateWMSTask(seqid);
                if (!string.IsNullOrEmpty(result))
                {
                    LogWrite.WriteError(seqid + ":" + result);
                }
                else
                {
                    LogWrite.WriteLog(" 更新任务状态成功：" + seqid);
                }
            }
        }

        //更改堆垛机状态
        void DoingUpdateSc()
        {
            string sql = "select * from  Wcs_DdjStatue where flag = 1";
            DataSet ds = Interface.Sql.GetSQL(sql);
            if (ds == null || ds.Tables[0].Rows.Count == 0)
                return;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string ddj = dr["No"].ToString();
                int status = int.Parse(dr["Status"].ToString());
                string result = UpdateScStatus(ddj, status);
                if (!string.IsNullOrEmpty(result))
                {
                    //LogWrite.WriteErrorMain(ddj + "更新失败:" + result);
                }
                else
                {
                    //LogWrite.WriteLogToMain(DateTime.Now + " 更新堆垛机状态成功：" + ddj);
                }
            }
        }

        #region 调用接口
        string UpdateWMSTask(string seqid)
        {
            WebService.interface2wcsServiceClient client = new WebService.interface2wcsServiceClient();
            WebService.updateTaskStatus uts = new WebService.updateTaskStatus();
            uts.SEQID = seqid;
            uts.NWKSTATUS = 99;
            uts.NWKSTATUSSpecified = true;
            uts.NWKMESSAGE = "WCS更新成功";
            WebService.updateTaskStatusResponse result = client.updateTaskStatus(uts);
            if (result.STATUS == 0)
            {
                string sql = string.Format(" UPDATE Wcs_Task SET NwkStatus = 99,DFinishDate=GETDATE() WHERE seqid = '{0}' ", seqid);

                Sql.ExecSQL(sql);
                return "";
            }
            else
            {
                string sql = string.Format("  UPDATE Wcs_Task SET ErrorNum = ErrorNum + 1 WHERE seqid = '{0}' ", seqid);
                Sql.ExecSQL(sql);
                return result.STATUS + result.MESSAGE;
            }
        }

        string UpdateScStatus(string ddjNo, int status)
        {
            WebService.interface2wcsServiceClient client = new WebService.interface2wcsServiceClient();
            WebService.updateScStatus uss = new WebService.updateScStatus();
            uss.SEQID = ddjNo;
            uss.NWKSTATUS = status;
            uss.NWKSTATUSSpecified = true;
            uss.NWKMESSAGE = "WCS更新成功";
            WebService.updateScStatusResponse result = client.updateScStatus(uss);
            if (result.STATUS == 0)
            {
                string sql = string.Format(" update Wcs_DdjStatue set flag = 0,UpdateDate = GETDATE() where no = '{0}' ", ddjNo);
                Sql.ExecSQL(sql);
                return "";
            }
            else
                return result.STATUS + result.MESSAGE;
        }
        #endregion

    }
}
