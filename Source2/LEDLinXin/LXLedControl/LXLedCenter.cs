using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXLedControl
{
    public class LXLedCenter
    {
        public static bool sendTextToLED(string ip, string text, int size ,out string msg)
        {
            try
            {
                int nResult;
                LedDll.COMMUNICATIONINFO CommunicationInfo = new LedDll.COMMUNICATIONINFO();//定义一通讯参数结构体变量用于对设定的LED通讯，具体对此结构体元素赋值说明见COMMUNICATIONINFO结构体定义部份注示
                CommunicationInfo.LEDType = 0;//LED类型	0.为所有6代单色、双色、七彩卡
                CommunicationInfo.SendType = 0;//设为固定IP通讯模式，即TCP通讯
                CommunicationInfo.IpStr = ip;//给IpStr赋值LED控制卡的IP

                int hProgram;//节目句柄
                hProgram = LedDll.LV_CreateProgram(192, 128, 2);//根据传的参数创建节目句柄，64是屏宽点数，32是屏高点数，2是屏的颜色，注意此处屏宽高及颜色参数必需与设置屏参的屏宽高及颜色一致，否则发送时会提示错误
                                                                //此处可自行判断有未创建成功，hProgram返回NULL失败，非NULL成功,一般不会失败
                nResult = LedDll.LV_AddProgram(hProgram, 1, 0, 1);//添加一个节目，参数说明见函数声明注示
                if (nResult != 0)
                {
                    msg = LedDll.LS_GetError(nResult);
                    return false;
                }

                #region 添加表头

                LedDll.AREARECT AreaRect = new LedDll.AREARECT();//区域坐标属性结构体变量
                AreaRect.left = 0;
                AreaRect.top = 0;
                AreaRect.width = 192;
                AreaRect.height = 128;
                LedDll.FONTPROP FontProp = new LedDll.FONTPROP();//文字属性
                FontProp.FontName = "宋体";
                FontProp.FontSize = size;
                FontProp.FontColor = LedDll.COLOR_RED;
                FontProp.FontBold = 0;
                //int nsize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(LedDll.FONTPROP));
                LedDll.LV_AddImageTextArea(hProgram, 1, 1, ref AreaRect, 0);
                //可以添加多个子项到图文区，如下添加可以选一个或多个添加
                //nResult = LedDll.LV_AddStaticTextToImageTextArea(hProgram, 1, 1, LedDll.ADDTYPE_STRING, text, ref FontProp, 3, 2, 1);
                LedDll.PLAYPROP PlayProp = new LedDll.PLAYPROP();
                PlayProp.InStyle = 0;
                PlayProp.DelayTime = 65535;
                PlayProp.Speed = 200;

                nResult = LedDll.LV_AddMultiLineTextToImageTextArea(hProgram, 1, 1, LedDll.ADDTYPE_STRING, text, ref FontProp, ref PlayProp, 0, 0);
                #endregion

                #region 添加实时显示内容

                //AreaRect.left = 0;
                //AreaRect.top = 0;
                //AreaRect.width = 128;
                //AreaRect.height = 192;
                //FontProp.FontName = "宋体";
                //FontProp.FontSize = 9;
                //FontProp.FontColor = LedDll.COLOR_RED;
                //FontProp.FontBold = 0;
                ////int nsize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(LedDll.FONTPROP));
                //LedDll.LV_AddImageTextArea(hProgram, 1, 2, ref AreaRect, 0);
                ////可以添加多个子项到图文区，如下添加可以选一个或多个添加
                //LedDll.PLAYPROP PlayProp = new LedDll.PLAYPROP();
                //PlayProp.InStyle = 6;
                //PlayProp.DelayTime = 3;
                //PlayProp.Speed = 4;

                //nResult = LedDll.LV_AddSingleLineTextToImageTextArea(hProgram, 1, 2, LedDll.ADDTYPE_STRING, text, ref FontProp, ref PlayProp);

                #endregion

                #region 添加仓库使用率

                //AreaRect.left = 0;
                //AreaRect.top = 50;
                //AreaRect.width = 80;
                //AreaRect.height = 14;
                //FontProp.FontName = "宋体";
                //FontProp.FontSize = 8;
                //FontProp.FontColor = LedDll.COLOR_RED;
                //FontProp.FontBold = 0;
                ////int nsize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(LedDll.FONTPROP));
                //LedDll.LV_AddImageTextArea(hProgram, 1, 3, ref AreaRect, 0);
                ////可以添加多个子项到图文区，如下添加可以选一个或多个添加
                //nResult = LedDll.LV_AddStaticTextToImageTextArea(hProgram, 1, 3, LedDll.ADDTYPE_STRING, string.Format("使用率:{0}/281", stockingRate), ref FontProp, 3, 0, 1);
                #endregion

                #region 时间

                //AreaRect.left = 80;
                //AreaRect.top = 52;
                //AreaRect.width = 48;
                //AreaRect.height = 12;

                //LedDll.DIGITALCLOCKAREAINFO DigitalClockAreaInfo = new LedDll.DIGITALCLOCKAREAINFO();
                //DigitalClockAreaInfo.TimeColor = LedDll.COLOR_RED;
                //DigitalClockAreaInfo.ShowStrFont.FontName = "宋体";
                //DigitalClockAreaInfo.ShowStrFont.FontSize = 8;
                //DigitalClockAreaInfo.IsShowHour = 1;
                //DigitalClockAreaInfo.IsShowMinute = 1;
                //DigitalClockAreaInfo.IsShowSecond = 1;
                //DigitalClockAreaInfo.TimeFormat = 2;

                //nResult = LedDll.LV_AddDigitalClockArea(hProgram, 1, 4, ref AreaRect, ref DigitalClockAreaInfo);//注意区域号不能一样，详见函数声明注示

                #endregion

                nResult = LedDll.LV_Send(ref CommunicationInfo, hProgram);//发送，见函数声明注示
                LedDll.LV_DeleteProgram(hProgram);//删除节目内存对象，详见函数声明注示
                if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
                {
                    msg = LedDll.LS_GetError(nResult);
                    return false;
                }
                else
                {
                    msg = string.Empty;
                    return true;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
    }
}
