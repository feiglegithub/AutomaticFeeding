//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Core
//   文 件 名：SploterBase.cs
//   创建时间：2018-11-26 8:39
//   作    者：
//   说    明：
//   修改时间：2018-11-26 8:39
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.FPZWS.LineControl.Drilling.Core
{
    public class SploterBase : ISploter
    {
        public SploterBase()
        {
            Name = "默认抽检器";
        }

        public string Name { get; set; }

        public bool IsSplot(string partId)
        {
            if (!string.IsNullOrEmpty(partId))
            {
                return true;
            }

            return false;
        }
    }
}                                                                                                                               