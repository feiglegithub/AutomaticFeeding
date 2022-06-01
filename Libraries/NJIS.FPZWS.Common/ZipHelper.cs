// ************************************************************************************
//  解决方案：NJIS.FPZWS.PDD.DataGenerate
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：ZipHelper.cs
//  创建时间：2018-08-11 13:51
//  作    者：
//  说    明：
//  修改时间：2017-07-28 14:33
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

#endregion

namespace NJIS.FPZWS.Common
{
    public class ZipHelper
    {
        /// <summary>
        ///     压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件全名</param>
        /// <param name="compressionLevel">压缩程度，范围0-9，数值越大，压缩程序越高</param>
        /// <param name="blockSize">分块大小</param>
        public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
        {
            if (!File.Exists(fileToZip)) //如果文件没有找到，则报错  
            {
                throw new FileNotFoundException("The specified file " + fileToZip +
                                                " could not be found. Zipping aborderd");
            }

            var streamToZip = new FileStream(fileToZip, FileMode.Open, FileAccess.Read);
            var zipFile = File.Create(zipedFile);
            var zipStream = new ZipOutputStream(zipFile);
            var zipEntry = new ZipEntry(fileToZip);
            zipStream.PutNextEntry(zipEntry);
            zipStream.SetLevel(compressionLevel);
            var buffer = new byte[blockSize];
            var size = streamToZip.Read(buffer, 0, buffer.Length);
            zipStream.Write(buffer, 0, size);

            try
            {
                while (size < streamToZip.Length)
                {
                    var sizeRead = streamToZip.Read(buffer, 0, buffer.Length);
                    zipStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            catch (Exception ex)
            {
                GC.Collect();
                throw ex;
            }

            zipStream.Finish();
            zipStream.Close();
            streamToZip.Close();
            GC.Collect();
        }

        /// <summary>
        ///     压缩目录下所有文件
        /// </summary>
        /// <param name="rootPath">要压缩的根目录</param>
        /// <param name="destinationPath">保存路径</param>
        /// <param name="compressLevel">压缩程度，范围0-9，数值越大，压缩程序越高</param>
        /// <param name="isDeleteFile">删除压缩后的文件</param>
        public static void ZipFileFromDirectory(string rootPath, string destinationPath, int compressLevel = 9,
            bool isDeleteFile = false)
        {
            var files = Directory.GetFiles(rootPath);
            var rootMark = rootPath + "\\"; //得到当前路径的位置，以备压缩时将所压缩内容转变成相对路径。  
            var crc = new Crc32();
            using (var outPutStream = new ZipOutputStream(File.Create(destinationPath)))
            {
                outPutStream.SetLevel(compressLevel); // 0 - store only to 9 - means best compression  
                foreach (var file in files)
                {
                    var fileStream = File.OpenRead(file); //打开压缩文件  
                    var buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    var entry = new ZipEntry(file.Replace(rootMark, string.Empty))
                    {
                        DateTime = DateTime.Now,
                        Size = fileStream.Length
                    };
                    fileStream.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    outPutStream.PutNextEntry(entry);
                    outPutStream.Write(buffer, 0, buffer.Length);
                }

                if (isDeleteFile)
                {
                    foreach (var file in files)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }

                outPutStream.Finish();
                outPutStream.Close();
            }
        }

        /// <summary>
        ///     功能：解压zip格式的文件。
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>
        /// <returns>解压是否成功</returns>
        public static void UnZipMoreFile(string zipFilePath, string unZipDir)
        {
            if (string.IsNullOrWhiteSpace(zipFilePath))
            {
                throw new Exception("压缩文件不能为空！");
            }

            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("压缩文件不存在！");
            }

            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
            if (string.IsNullOrWhiteSpace(unZipDir))
            {
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath),
                    Path.GetFileNameWithoutExtension(zipFilePath));
            }

            UnZipMoreFile(File.OpenRead(zipFilePath), unZipDir);
        }

        public static void UnZipMoreFile(byte[] zipBytes, string unZipDir)
        {
            UnZipMoreFile(new MemoryStream(zipBytes), unZipDir);
        }

        /// <summary>
        ///     解压多文件zip
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="unZipDir"></param>
        public static void UnZipMoreFile(Stream zipStream, string unZipDir)
        {
            if (string.IsNullOrWhiteSpace(unZipDir))
            {
                throw new Exception("解压路径不能为空！");
            }

            if (!(unZipDir.EndsWith("/") || unZipDir.EndsWith(@"\")))
            {
                unZipDir += "/";
            }

            if (!Directory.Exists(unZipDir))
            {
                Directory.CreateDirectory(unZipDir);
            }

            using (var s = new ZipInputStream(zipStream))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    var directoryName = Path.GetDirectoryName(theEntry.Name);
                    var fileName = Path.GetFileName(theEntry.Name);

                    if (directoryName.Length > 0)
                    {
                        if (!(directoryName.EndsWith("/") || directoryName.EndsWith(@"\")))
                        {
                            directoryName += "/";
                        }

                        Directory.CreateDirectory(unZipDir + directoryName);
                    }

                    if (fileName != string.Empty)
                    {
                        using (var streamWriter = File.Create(unZipDir + theEntry.Name))
                        {
                            var size = 2048;
                            var data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     解压zip文件到目录
        /// </summary>
        /// <param name="zipBytes"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Dictionary<string, string> UnZipToDictionary(byte[] zipBytes, Encoding e)
        {
            var dic = new Dictionary<string, string>();

            using (var s = new ZipInputStream(new MemoryStream(zipBytes)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    var streamWriter = new MemoryStream();
                    var size = 2048;
                    var data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                    dic.Add(theEntry.Name, StreamToString(streamWriter, e));
                }
            }

            return dic;
        }

        public static string StreamToString(Stream s, Encoding encoding)
        {
            if (s == null) return null;
            var e = Encoding.Default;
            if (encoding != null)
            {
                e = encoding;
            }

            s.Position = 0;
            var sr = new StreamReader(s, e);
            return sr.ReadToEnd();
        }
    }
}