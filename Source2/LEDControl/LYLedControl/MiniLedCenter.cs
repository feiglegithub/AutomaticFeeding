using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LYLedControl
{
    public class MiniLedCenter
    {
        static MiniLedCenter()
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = AppDomain.CurrentDomain.BaseDirectory + @"\";
        }

        static string fileName = string.Empty;
        static bool init = false;
        const int LedHeight = 0x60;
        const int LedWidth = 0xb8;
        static bool proListCreated = false;
        static Random udpRnd = new Random();

        // Methods
        private static int BMPToBuff(ref Bitmap bmp, out byte[] buff, short Off, short Width, short Height, bool bDblColor)
        {
            byte vR = 0;
            byte vG = 0;
            int offg = 0;
            int length = ((Height + 7) / 8) * Width;
            if (bDblColor)
            {
                length *= 2;
                offg = (Height / 8) * Width;
            }
            buff = new byte[Off + length];
            int len = 0;
            for (short x = 0; x < Width; x = (short)(x + 1))
            {
                short y = 0;
                while (y < Height)
                {
                    vR = 0;
                    if (bDblColor)
                    {
                        vG = 0;
                    }
                    short yy = 1;
                    for (short z = 0; z <= 7; z = (short)(z + 1))
                    {
                        Color vC = bmp.GetPixel(x, y);
                        if (vC.R > 0x20)
                        {
                            vR = Convert.ToByte((int)(vR + yy));
                        }
                        if (bDblColor && (vC.G > 0x20))
                        {
                            vG = Convert.ToByte((int)(vG + yy));
                        }
                        y = (short)(y + 1);
                        yy = (short)(yy << 1);
                    }
                    buff[Off + len] = vR;
                    if (bDblColor)
                    {
                        buff[(Off + offg) + len] = vG;
                    }
                    len++;
                }
            }
            if (bDblColor)
            {
                len *= 2;
            }
            return (len + Off);
        }

        private static Bitmap ConvertToBitmap(string str, int bmpWidth, int bmpHeight)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = " ";
            }
            Bitmap bmp = new Bitmap(bmpWidth, bmpHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);
            string[] lines = str.Split(new char[] { '\n' });
            float emSize = 36f;
            while (true)
            {
                Font font = new Font("宋体", emSize);
                SizeF sz = g.MeasureString(lines[0], font);
                if ((bmp.Width <= sz.Width) && (emSize > 8f))
                {
                    emSize--;
                }
                else
                {
                    float y = 0f;
                    emSize += 3f;
                    if (bmp.Height >= (emSize * lines.Length))
                    {
                        y = (bmp.Height - (emSize * lines.Length)) / 2f;
                    }
                    g.DrawString(str, font, Brushes.Red, (bmp.Width - sz.Width) / 2f, y);
                    return bmp;
                }
            }
        }

        private static byte[] convertToBuff1(string text)
        {
            byte[] buff = Encoding.GetEncoding("GB2312").GetBytes(text);
            Array.Resize<byte>(ref buff, buff.Length + 1);
            buff[buff.Length - 1] = 0;
            return buff;
        }

        private static string ConvertToLedFormatText(string text, short width, short height)
        {
            Font font;
            SizeF sz;
            if (string.IsNullOrEmpty(text))
            {
                text = " ";
            }
            string[] lines = text.Split(new char[] { '\n' });
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            if (lines.Length == 1)
            {
                font = new Font("宋体", 24f);
                sz = g.MeasureString(text, font);
                return string.Format("^F2^X+{0:0000}^Y+{1:0000}{2}", Convert.ToInt32((float)((bmp.Width - sz.Width) / 2f)), 12, text);
            }
            font = new Font("宋体", 19f);
            sz = g.MeasureString(lines[0], font);
            int x1 = Convert.ToInt32((float)((bmp.Width - sz.Width) / 2f));
            sz = g.MeasureString(lines[1], font);
            int x2 = Convert.ToInt32((float)((bmp.Width - sz.Width) / 2f));
            return string.Format("^F1^X{0:0000}^Y{1:0000}{2}^F1^X{3:0000}^Y{4:0000}{5}", new object[] { x1, 12, lines[0], x2, 13, lines[1] });
        }

        private static ProgItemType[] CreateProgList()
        {
            ProgItemType[] result = new ProgItemType[1];
            result[0].Flag = 0;
            result[0].PicFIndex = 0;
            result[0].Effect = 2;
            result[0].SpeedStay = 0xfffff;
            result[0].Schedule = -1;
            return result;
        }

        private static SizeF getXPos(Graphics g, Font font, string s)
        {
            return g.MeasureString(s, font);
        }

        private static bool SendPicture(short deviceId, short picIndex, Bitmap[] bmp, bool bDblColor)
        {
            int i;
            MemoryStream m = new MemoryStream();
            for (i = 0; i < bmp.Length; i++)
            {
                bmp[i].Save(m, ImageFormat.Bmp);
            }
            PicFileHdr hdr = new PicFileHdr();
            ArrayList tar = new ArrayList();
            int len = 0;
            for (i = 0; i < bmp.Length; i++)
            {
                short off;
                byte[] tbuff;
                if (i == 0)
                {
                    off = (short)Marshal.SizeOf(hdr);
                }
                else
                {
                    off = 0;
                }
                int tlen = BMPToBuff(ref bmp[i], out tbuff, off, (short)bmp[i].Width, (short)bmp[i].Height, bDblColor);
                for (int j = 0; j < tlen; j++)
                {
                    tar.Add(tbuff[j]);
                }
            }
            len = tar.Count;
            byte[] buff = new byte[len];
            buff = (byte[])tar.ToArray(typeof(byte));
            if (bDblColor)
            {
                hdr.Type = 1;
            }
            else
            {
                hdr.Type = 0;
            }
            hdr.PicCount = (byte)bmp.Length;
            hdr.PicHeight = (short)bmp[0].Height;
            hdr.PicWidth = (short)bmp[0].Width;
            hdr.PicOffset = (short)Marshal.SizeOf(hdr);
            hdr.LastPicH = (short)bmp[0].Height;
            hdr.LastPicW = (short)bmp[0].Width;
            IntPtr pData = Marshal.AllocHGlobal(Marshal.SizeOf(hdr));
            Marshal.StructureToPtr(hdr, pData, false);
            Marshal.Copy(pData, buff, 0, Marshal.SizeOf(hdr));
            Marshal.FreeHGlobal(pData);
            FileStream fs = new FileStream(fileName + picIndex.ToString() + ".xmpx", FileMode.Create, FileAccess.Write);
            fs.Write(buff, 0, buff.Length);
            fs.Close();
            return MiniLED.MC_SendXMPXPic(deviceId, picIndex, ref buff[0], (long)(len + Marshal.SizeOf(hdr)));
        }

        private static bool SendProgList(short deviceId, ProgItemType[] progList)
        {
            IntPtr pSndData = Marshal.AllocHGlobal((int)(Marshal.SizeOf(progList[0]) * progList.Length));
            IntPtr buff = new IntPtr(pSndData.ToInt32());
            for (int i = 0; i < progList.Length; i++)
            {
                Marshal.StructureToPtr(progList[i], buff, false);
                buff = new IntPtr(buff.ToInt32() + Marshal.SizeOf(progList[0]));
            }
            bool result = MiniLED.MC_SendProgList(deviceId, pSndData, (short)progList.Length);
            Marshal.FreeHGlobal(pSndData);
            return result;
        }

        public static bool SendText(short deviceId, string formatText, short width, short height)
        {
            Func<string, byte[]> convertToBuff = delegate (string text)
            {
                byte[] buff = Encoding.GetEncoding("GB2312").GetBytes(formatText);
                Array.Resize<byte>(ref buff, buff.Length + 1);
                buff[buff.Length - 1] = 0;
                return buff;
            };
            byte[] data = convertToBuff("".PadLeft(1, 'Ǵ'));
            if (MiniLED.MC_ShowString(deviceId, 0, 0, width, height, 0, 0, 1, ref data[0], 2))
            {
                Thread.Sleep(100);
                data = convertToBuff(formatText);
                return MiniLED.MC_ShowString(deviceId, 0, 0, width, height, 0, 0, 1, ref data[0], 2);
            }
            return false;
        }

        public static bool sendTextToLED(short deviceId, string ip, string text, short port)
        {
            bool sendSuccess = false;
            try
            {
                init = false;
                if (!init)
                {
                    init = MiniLED.MC_NetInitial(deviceId, "", ip, 2, 3, port) == 0;
                    if (!init)
                    {
                        return false;
                    }
                }
                sendSuccess = SendText(deviceId, text, 0xc0, 0x80);
                //if (sendSuccess && !proListCreated)
                //{
                //    ProgItemType[] ProgList = new ProgItemType[] { new ProgItemType() };
                //    ProgList[0].Flag = 0;
                //    ProgList[0].PicFIndex = 4;
                //    ProgList[0].Effect = 6;
                //    ProgList[0].SpeedStay = 0x3b9ac9ff;
                //    ProgList[0].Schedule = -1;
                //    proListCreated = SendProgList(deviceId, ProgList);
                //    FileStream fs = new FileStream(fileName + "ProgList.ly", FileMode.Create, FileAccess.Write);
                //    byte[] fileContent = StructToBytes(ProgList[0]);
                //    fs.Write(fileContent, 0, fileContent.Length);
                //    fileContent = StructToBytes(ProgList[1]);
                //    fs.Write(fileContent, 0, fileContent.Length);
                //    fs.Close();
                //    if (!proListCreated)
                //    {
                //        return false;
                //    }
                //}
                MiniLED.MC_Close(deviceId);
                return sendSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MiniLED.MC_Close(deviceId);
                return false;
            }
        }

        private static bool ShowPicture(short deviceId, Bitmap bmp, bool bDblColor)
        {
            byte[] buff;
            MemoryStream m = new MemoryStream();
            bmp.Save(m, ImageFormat.Bmp);
            PicFileHdr hdr = new PicFileHdr();
            int len = BMPToBuff(ref bmp, out buff, 0, (short)bmp.Width, (short)bmp.Height, bDblColor);
            if (bDblColor)
            {
                hdr.Type = 1;
            }
            else
            {
                hdr.Type = 0;
            }
            hdr.PicCount = 1;
            hdr.PicHeight = (short)bmp.Height;
            hdr.PicWidth = (short)bmp.Width;
            hdr.PicOffset = (short)Marshal.SizeOf(hdr);
            hdr.LastPicH = (short)bmp.Height;
            hdr.LastPicW = (short)bmp.Width;
            return MiniLED.MC_ShowXMPXPic(deviceId, 0, 0, (short)bmp.Width, (short)bmp.Height, ref buff[0], (long)len);
        }

        private static Bitmap SquareImage(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);
            Pen pen = new Pen(Brushes.Red, 1f);
            g.DrawLine(pen, new Point(0, 0), new Point(width - 1, 0));
            g.DrawLine(pen, new Point(0, 1), new Point(0, height - 1));
            g.DrawLine(pen, new Point(width - 1, 1), new Point(width - 1, height - 1));
            g.DrawLine(pen, new Point(1, height - 1), new Point(width - 2, height - 1));
            bmp.Save(fileName + "00.bmp");
            return bmp;
        }

        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }
    }
}

