using NJIS.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.Platform.Repository
{
    public class DbConnectSetting : SettingBase<DbConnectSetting>
    {
        internal string DbConnect
        {
            get
            {
                return $"Password={Pwd};Persist Security Info=True;" +
                       $"User ID={UserName};" +
                       $"Initial Catalog={Database};" +
                       $"Data Source={Server}";
            }
        }

        [Property("common")]
        public string Server { get; set; }

        [Property("common")]
        public string Database { get; set; }

        [Property("common")]
        public string UserName { get; set; }

        [Property("common")]
        public string Pwd { get; set; }
    }
}
