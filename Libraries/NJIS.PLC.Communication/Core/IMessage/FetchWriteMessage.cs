//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.PLC.Communication
//   文 件 名：FetchWriteMessage.cs
//   创建时间：2018-11-08 16:16
//   作    者：
//   说    明：
//   修改时间：2018-11-08 16:16
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

namespace NJIS.PLC.Communication.Core.IMessage
{
    /// <summary>
    ///     西门子Fetch/Write消息解析协议
    /// </summary>
    public class FetchWriteMessage : INetMessage
    {
        /// <summary>
        ///     消息头的指令长度
        /// </summary>
        public int ProtocolHeadBytesLength => 16;


        /// <summary>
        ///     从当前的头子节文件中提取出接下来需要接收的数据长度
        /// </summary>
        /// <returns>返回接下来的数据内容长度</returns>
        public int GetContentLengthByHeadBytes()
        {
            if (SendBytes != null)
            {
                if (HeadBytes[5] == 0x04)
                {
                    return 0;
                }

                return SendBytes[12] * 256 + SendBytes[13];
            }

            return 16;
        }


        /// <summary>
        ///     检查头子节的合法性
        /// </summary>
        /// <param name="token">特殊的令牌，有些特殊消息的验证</param>
        /// <returns>是否合法</returns>
        public bool CheckHeadBytesLegal(byte[] token)
        {
            if (HeadBytes == null) return false;

            if (HeadBytes[0] == 0x53 && HeadBytes[1] == 0x35)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        ///     获取头子节里的消息标识
        /// </summary>
        /// <returns>消息标识</returns>
        public int GetHeadBytesIdentity()
        {
            return HeadBytes[3];
        }


        /// <summary>
        ///     消息头字节
        /// </summary>
        public byte[] HeadBytes { get; set; }


        /// <summary>
        ///     消息内容字节
        /// </summary>
        public byte[] ContentBytes { get; set; }

        /// <summary>
        ///     发送的字节信息
        /// </summary>
        public byte[] SendBytes { get; set; }
    }
}