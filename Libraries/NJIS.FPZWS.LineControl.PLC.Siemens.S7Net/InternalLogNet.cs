//  ************************************************************************************
//   解决方案：NJIS.FPZWS.Libraries
//   项目名称：NJIS.FPZWS.LineControl.PLC.Siemens.S7Net
//   文 件 名：InternalLogNet.cs
//   创建时间：2018-11-22 10:05
//   作    者：
//   说    明：
//   修改时间：2018-11-22 10:05
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using NJIS.FPZWS.Log;
using NJIS.PLC.Communication.LogNet.Core;


namespace NJIS.FPZWS.LineControl.PLC.Siemens.S7Net
{
    public class InternalLogNet : ILogNet
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(InternalLogNet));

        public void Dispose()
        {
        }

        public int LogSaveMode { get; }
        public event EventHandler<NjisEventArgs> BeforeSaveToFile;

        public void RecordMessage(NjisMessageDegree degree, string keyWord, string text)
        {
        }

        public void WriteDebug(string text)
        {
            //_logger.Debug(text);
        }

        public void WriteDebug(string keyWord, string text)
        {
            //_logger.Debug($"{keyWord}=>{text}");
        }

        public void WriteDescrition(string description)
        {
        }

        public void WriteError(string text)
        {
            _logger.Error(text);
        }

        public void WriteError(string keyWord, string text)
        {
            _logger.Error($"{keyWord}=>{text}");
        }

        public void WriteException(string keyWord, Exception ex)
        {
            _logger.Error($"{keyWord}=>{ex}");
        }

        public void WriteException(string keyWord, string text, Exception ex)
        {
            _logger.Error($"{keyWord}=>{ex}");
        }

        public void WriteFatal(string text)
        {
            _logger.Fatal($"{text}");
        }

        public void WriteFatal(string keyWord, string text)
        {
            _logger.Fatal($"{keyWord}=>{text}");
        }

        public void WriteInfo(string text)
        {
            _logger.Info($"{text}");
        }

        public void WriteInfo(string keyWord, string text)
        {
            _logger.Error($"{keyWord}=>{text}");
        }

        public void WriteNewLine()
        {
        }

        public void WriteWarn(string text)
        {
            _logger.Warn($"{text}");
        }

        public void WriteWarn(string keyWord, string text)
        {
            _logger.Error($"{keyWord}=>{text}");
        }

        public void SetMessageDegree(NjisMessageDegree degree)
        {
        }

        public string[] GetExistLogFileNames()
        {
            return null;
        }

        public void FiltrateKeyword(string keyword)
        {
        }
    }
}