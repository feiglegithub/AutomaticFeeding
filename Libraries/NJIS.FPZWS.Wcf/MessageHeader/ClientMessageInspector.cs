// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.Cutting
//  项目名称：NJIS.FPZWS.Wcf
//  文 件 名：ClientMessageInspector.cs
//  创建时间：2017-12-29 14:06
//  作    者：
//  说    明：
//  修改时间：2017-12-29 14:33
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

#endregion

#region

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

#endregion

namespace NJIS.FPZWS.Wcf.MessageHeader
{
    public class ClientMessageInspector : IClientMessageInspector, IEndpointBehavior
    {
        #region Implementation for IClientMessageInspector

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            //先删除消息头
            HeaderOperater.RemoveHeader(request.Headers);
            HeaderOperater.RemoveAppNameHeader(request.Headers);
            //设定传递的上下文信息
            var hContext = HeaderOperater.GetClientWcfHeader();
            if (hContext != null)
            {
                HeaderOperater.AddHeader(request.Headers, hContext);
                //Console.WriteLine("取到上下文:"+hContext.ToString());
            }
            var appnamehContext = HeaderOperater.GetClientWcfAppNameHeader();
            if (appnamehContext != null)
            {
                HeaderOperater.AddHeader(request.Headers, appnamehContext);
            }
            return null;
        }

        #endregion

        #region Implementation for IEndpointBehavior

        //==================================  
        public void AddBindingParameters(ServiceEndpoint serviceEndpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint serviceEndpoint, ClientRuntime behavior)
        {
            //此处为Extension附加到ClientRuntime。  
            behavior.MessageInspectors.Add(this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint serviceEndpoint, EndpointDispatcher endpointDispatcher)
        {
            //如果是扩展服务器端的MessageInspector，则要附加到EndpointDispacther上了。  
            //endpointDispatcher.DispatchRuntime.MessageInspectors.Add(this);  
        }

        public void Validate(ServiceEndpoint serviceEndpoint)
        {
        }

        #endregion
    }
}