// ************************************************************************************
//  解决方案：NJIS.FPZWS.Sorting.Client
//  项目名称：NJIS.FPZWS.Tools.Sorting
//  文 件 名：UiHelper.cs
//  创建时间：2017-11-02 16:39
//  作    者：
//  说    明：
//  修改时间：2017-11-03 8:19
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Telerik.WinControls.UI;

#endregion

namespace NJIS.Windows.TemplateBase
{

    public static class UiHelper
    {
        public static Color DefColor = Color.FromArgb(0, 112, 240);
        public static RadButtonElement CreateRadBtnEle(string btnText, Bitmap btnImage, bool changeColor, Color? color = null)
        {
            var currbtn = new RadButtonElement(btnText);
            currbtn.Image = GetThumbnail(btnImage, 38, 38, changeColor, color);
            currbtn.TextAlignment = ContentAlignment.BottomCenter;
            currbtn.ImageAlignment = ContentAlignment.MiddleCenter;
            currbtn.ShowBorder = false;
            return currbtn;
        }

        public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth, bool changeColor = false, Color? color = null)
        {
            if (changeColor)
            {

                for (int i = 0; i < b.Width; i++)
                {
                    for (int j = 0; j < b.Height; j++)
                    {
                        var currCol = b.GetPixel(i, j);
                        if (currCol.A != 0)
                        {
                            b.SetPixel(i, j, color == null ? Color.FromArgb(currCol.A, DefColor.R, DefColor.G, DefColor.B) : (Color)color);
                        }
                    }
                }
            }

            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放           
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量     
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }
        public static void DataBind(this DataGridView dgv, object obj)
        {
            if (dgv.InvokeRequired)
            {
                dgv.Invoke(new Action<DataGridView, object>(DataBind), dgv, obj);
            }
            else
            {
                dgv.DataSource = null;
                dgv.DataSource = obj;
            }
        }

        public static void TextBind(this Control ctl, string text)
        {
            if (ctl.InvokeRequired)
            {
                ctl.Invoke(new Action<Control, string>(TextBind), ctl, text);
            }
            else
            {
                ctl.Text = text;
            }
        }

        public static void ValueBind(this ProgressBar ctl, int value)
        {
            if (ctl.InvokeRequired)
            {
                ctl.Invoke(new Action<ProgressBar, int>(ValueBind), ctl, value);
            }
            else
            {
                ctl.Value = value;
                ctl.Text = value + "%";
            }
        }



        public static RadButtonElement CreateRadBtnEle(string btnText, Bitmap btnImage)
        {
            var currbtn = new RadButtonElement(btnText);
            currbtn.Image = GetThumbnail(btnImage, 38, 38);
            currbtn.TextAlignment = ContentAlignment.BottomCenter;
            currbtn.ImageAlignment = ContentAlignment.MiddleCenter;
            currbtn.ShowBorder = false;
            return currbtn;
        }

        public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放           
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量     
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }
    }
}