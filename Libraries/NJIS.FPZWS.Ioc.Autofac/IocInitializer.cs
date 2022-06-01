//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：IocInitializer.cs
//   创建时间：2018-11-29 8:26
//   作    者：
//   说    明：
//   修改时间：2018-11-29 8:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.Common.Dependency;
using NJIS.FPZWS.Common.Initialize;

namespace NJIS.FPZWS.Ioc.Autofac
{
    public class IocInitializer : IInitializer
    {
        public InitializeLevel Level { get; } = InitializeLevel.High;

        public void Initializer(IConfig dgConfig)
        {
            #region IOC 初始化

            IServicesBuilder builder = new ServicesBuilder();
            var services = builder.Build();

            #endregion

            IIocBuilder iocBuilder = new AutofacIocBuilder(services);
            iocBuilder.Build();
        }
    }
}