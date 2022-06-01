//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Contract
//   文 件 名：IDrillingContrace.cs
//   创建时间：2018-11-07 11:02
//   作    者：
//   说    明：
//   修改时间：2018-11-07 11:02
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using NJIS.FPZWS.Common.Dependency;
using NJIS.FPZWS.LineControl.Drilling.Model;

namespace NJIS.FPZWS.LineControl.Drilling.Contract
{
    public interface IDrillingContract : IScopeDependency
    {
        #region Drilling
        /// <summary>
        /// 根据PartId 获取板件信息
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        Model.Drilling FindDrilling(string partId);

        /// <summary>
        /// 根据批次获取加工数据
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        List<Model.Drilling> FindDrillings(string batchName);

        /// <summary>
        /// 根据批次获取加工数据
        /// </summary>
        /// <param name="productionDate">日期</param>
        /// <returns></returns>
        List<Model.Drilling> FindDrillings(DateTime productionDate);
        #endregion

        #region PcsPartInfoQueue

        /// <summary>
        /// 查找板件队列
        /// </summary>
        /// <param name="top">最新多少条</param>
        /// <returns></returns>
        List<PcsPartInfoQueue> FindPartInfoQueues(int top);

        /// <summary>
        /// 插入数据到板件队列
        /// </summary>
        /// <param name="drilling"></param>
        /// <param name="position">板件位置</param>
        /// <returns></returns>
        PcsPartInfoQueue InsertPartInfoQueues(Model.Drilling drilling, int position);

        /// <summary>
        /// 更新板件队列位置
        /// </summary>
        /// <param name="partId"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        bool UpdatePartInfoQueuesPlace(string partId, string place);

        string DeletePartInfoQueues(string partId);
        #endregion

        #region PcsMachine

        /// <summary>
        /// 获取所有设备
        /// </summary>
        /// <returns></returns>
        List<PcsMachine> FindAllMachine();

        /// <summary>
        /// 获取所有设备
        /// </summary>
        /// <returns></returns>
        PcsMachine FindMachine(string code);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool UpdateMachine(PcsMachine entitys);

        #endregion 

        #region

        /// <summary>
        /// 获取所有链式缓存
        /// </summary>
        /// <returns></returns>
        List<ChainBuffer> FindChainBuffers();

        /// <summary>
        /// 更新链式缓存状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool UpdateChainBufferStatus(string code, int status);

        #endregion

        #region 板件位置


        /// <summary>
        /// 获取所有链式缓存
        /// </summary>
        /// <returns></returns>
        List<PcsPartPosition> FindPartPositions(string partId);
        #endregion

        DataTable PcsProc(string procName, object args);

        List<string> GetTestPart();

        #region 数据导入

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void SaveDrillingImport(DrillingImport entity);
        bool CheckDrillingImport(string batch,int way,string machine);
        List<string> GetNotImportBatchs(DateTime dt, string machine);
        #endregion

        #region PcsPartInfoQueue

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        bool ExistsNg(string partId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        PcsNg FindNg(string partId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="top">最新多少条</param>
        /// <returns></returns>
        List<PcsNg> FindNgs(int top);

        /// <summary>
        /// 
        /// </summary>                                
        /// <returns></returns>
        bool InsertNg(PcsNg entity);

        bool DeleteNg(string partId);
        #endregion

    }
}