using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.LocalServices;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Interfaces
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface IService
    {
        bool IsStart { get; set; }
        void Start();
        void Stop();
    }

    /// <summary>
    /// 服务基类
    /// </summary>
    public class ServiceBase : IService
    {
        public virtual bool IsStart { get; set; } = false;

        public virtual void Start()
        {
            IsStart = true;
        }

        public virtual void Stop()
        {
            IsStart = false;
        }
    }

    /// <summary>
    /// 单例服务基类
    /// </summary>
    public class ServiceBase<T>: Singleton<T> ,IService
    where T :class
    {
        public virtual bool IsStart { get; set; } = false;

        public virtual void Start()
        {
            IsStart = true;
        }

        public virtual void Stop()
        {
            IsStart = false;
        }
    }
}
