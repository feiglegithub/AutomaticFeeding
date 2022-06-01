// ************************************************************************************
//  解决方案：NJIS.FPZWS.Sorting.Client
//  项目名称：NJIS.Tools.Client
//  文 件 名：UILocalizedHelper.cs
//  创建时间：2017-11-02 16:39
//  作    者：
//  说    明：
//  修改时间：2017-11-03 8:40
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using NJIS.AppUtility.DbFramework;
using NJIS.AppUtility.Helper;
using NJIS.AppUtility.Localization;
using Telerik.WinControls.UI;

#endregion

namespace NJIS.Tools.Client.Helper
{
    public static class UILocalizedHelper
    {
        public static string GetFieldLocalizedTex<ObjectType>(string fieldName) where ObjectType : new()
        {
            var text = EntityBase<ObjectType>.GetPropertyText(fieldName);
            if (string.IsNullOrEmpty(text))
                text = UserObject.Instance.GetString(string.Format("{0}.{1}", typeof(ObjectType), fieldName));
            return text;
        }

        public static void SetLocalizedUI<ObjectType>(this RadGridView gv) where ObjectType : new()
        {
            gv.AutoGenerateColumns = false;
            gv.AllowDragToGroup = false;
            gv.ShowGroupPanel = false;
            gv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            var columns = TypeProperty<ObjectType>.PopulateGridColumns();

            foreach (var column in columns)
            {
                var un = column["DataMember"].ToString();
                var header = column["Header"].ToString();
                var headerWidth = (int) column["Width"];
                var foundColumns = gv.MasterTemplate.Columns.GetColumnByFieldName(un);

                if (foundColumns.Length > 0)
                {
                    GridViewColumn found = foundColumns[0];
                    found.HeaderText = header;
                    if (headerWidth > 0) found.Width = headerWidth;
                }
                else
                {
                    var col = new GridViewTextBoxColumn(column["DataMember"].ToString());
                    if (headerWidth > 0)
                        col.Width = headerWidth;

                    col.HeaderText = header;
                    col.DataType = (Type) column["Type"];

                    //col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    gv.MasterTemplate.Columns.Add(col);
                }
            }
        }
    }
}