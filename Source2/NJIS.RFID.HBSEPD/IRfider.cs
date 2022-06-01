//  ************************************************************************************
//   解决方案：NJIS.RFID.HBSEPD
//   项目名称：NJIS.RFID.HBSEPD
//   文 件 名：IRfider.cs
//   创建时间：2019-05-02 9:23
//   作    者：
//   说    明：
//   修改时间：2019-05-02 9:23
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.RFID.HBSEPD
{
    public interface IRfider
    {
        bool Connect();
        bool DisConnect();

        string ReadString();

        string[] ReadStrings();

        string Id { get; set; }

        bool IsConnected { get; }
    }
}