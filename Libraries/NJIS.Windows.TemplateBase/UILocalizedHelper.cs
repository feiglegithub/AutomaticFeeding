using NJIS.AppUtility.DbFramework;
using NJIS.AppUtility.Helper;
using NJIS.AppUtility.Localization;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace NJIS.Windows.TemplateBase
{
    public static class UILocalizedHelper
    {
        public static string GetFieldLocalizedTex<ObjectType>(string fieldName) where ObjectType : new()
        {
            string text = EntityBase<ObjectType>.GetPropertyText(fieldName);
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
            List<Dictionary<string, object>> columns = TypeProperty<ObjectType>.PopulateGridColumns();
            
            foreach (Dictionary<string, object> column in columns)
            {
                string un = column["DataMember"].ToString();
                string header = column["Header"].ToString();
                int headerWidth = (int)column["Width"];
                var foundColumns = gv.MasterTemplate.Columns.GetColumnByFieldName(un);

                if (foundColumns.Length >0)
                {
                    GridViewColumn found = foundColumns[0];
                    found.HeaderText = header;
                    if (headerWidth > 0) found.Width = headerWidth;
                    continue;
                }
                else
                {
                    GridViewTextBoxColumn col = new GridViewTextBoxColumn(column["DataMember"].ToString());
                    if (headerWidth > 0)
                        col.Width = headerWidth;

                    col.HeaderText = header;
                    col.DataType = (Type)column["Type"];

                    //col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    gv.MasterTemplate.Columns.Add(col);
                }
            }

        }
    }
}
