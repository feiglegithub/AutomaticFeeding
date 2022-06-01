//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Sorting
//   项目名称：NJIS.FPZWS.LineControl.Sorting.Model
//   文 件 名：UserInfo.cs
//   创建时间：2018-12-26 8:26
//   作    者：
//   说    明：
//   修改时间：2018-12-26 8:26
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NJIS.Model.Attributes;

#endregion

namespace NJIS.FPZWS.LineControl.Sorting.Model
{
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key] [Identity] public long LineId { get; set; }

        public string UserNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SecretOrder { get; set; }

        [CreatedAt] public DateTime? CreatedTime { get; set; }
    }
}