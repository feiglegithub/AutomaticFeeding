using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters
{
    public class CheckManageControlPresenter:PresenterBase
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public const string GetData = nameof(GetData);
        /// <summary>
        /// 保存数据
        /// </summary>
        public const string Save = nameof(Save);

        private List<CuttingCheckRule> cuttingCheckRules = null;
        private ILineControlCuttingContract _contract = null;

        private ILineControlCuttingContract Contract => _contract ?? (_contract = CuttingServerSettings.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>() : new LineControlCuttingService());

        public CheckManageControlPresenter()
        {
            Register<string>(GetData, ExecuteGetData);
            Register<List<CuttingCheckRule>>(Save, ExecuteSave);
        }

        private void ExecuteGetData(object sender, string str="")
        {
            List<CuttingCheckRule> data = null;
            try
            {
                data = Contract.GetCuttingCheckRules();
                cuttingCheckRules = data;
            }
            catch (Exception e)
            {
                SendTipsMessage(e.Message, sender);
            }
            finally
            {
                Send(GetData, sender, data);
            }
            
        }

        private void ExecuteSave(object sender, List<CuttingCheckRule> checkRules)
        {
            if (checkRules == null || checkRules.Count == 0)
            {
                Send(Save,sender,true);
                ExecuteGetData(sender, "");
                return;
            }

            List<CuttingCheckRule> newCheckRules =new List<CuttingCheckRule>();
            List<CuttingCheckRule> changeCheckRules = new List<CuttingCheckRule>();
            checkRules.ForEach(item =>
            {
                if (item.LineId == 0)
                {
                    newCheckRules.Add(item);
                }
                else
                {
                    changeCheckRules.Add(item);
                }
            });
            var deleteCheckRules =
                cuttingCheckRules.FindAll(item => !changeCheckRules.Exists(item1 => item1.LineId == item.LineId));

            //Contract.
        }
    }
}
