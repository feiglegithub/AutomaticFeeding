// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.DataManager
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：LocalizationProvider.cs
//  创建时间：2018-07-23 17:59
//  作    者：
//  说    明：
//  修改时间：2018-04-30 7:37
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace NJIS.FPZWS.UI.Common.LocalizationProvider
{
    public static class ControlLocalizationProvider
    {
        public static void InitializeLocalizationProvider(this RadFormControlBase form)
        {
            //var classes = Assembly.Load("Telerik.WinControls.UI").GetTypes().Where(w => w.FullName.IndexOf("LocalizationProvider") >= 0);
            //foreach (var item in classes)
            //{
            //    Console.WriteLine(item.Name);
            //}

            RadGridLocalizationProvider.CurrentProvider = new GermanRadGridLocalizationProvider();
            DataFilterLocalizationProvider.CurrentProvider = new GermanDataFilterLocalizationProvider();

            //PrintDialogsLocalizationProvider.CurrentProvider = new PrintDialogsLocalizationProvider();
            //RadBrowseEditorLocalizationProvider.CurrentProvider = new RadBrowseEditorLocalizationProvider();
            //CalendarLocalizationProvider.CurrentProvider = new CalendarLocalizationProvider();
            //GanttViewLocalizationProvider.CurrentProvider = new GanttViewLocalizationProvider();
            //LayoutControlLocalizationProvider.CurrentProvider = new LayoutControlLocalizationProvider();
            //PropertyGridLocalizationProvider.CurrentProvider = new PropertyGridLocalizationProvider();
            //ColorDialogLocalizationProvider.CurrentProvider = new ColorDialogLocalizationProvider();
            //CommandBarLocalizationProvider.CurrentProvider = new CommandBarLocalizationProvider();
            //RadPageViewLocalizationProvider.CurrentProvider = new RadPageViewLocalizationProvider();
            //TextBoxControlLocalizationProvider.CurrentProvider = new TextBoxControlLocalizationProvider();
            //RadTimePickerLocalizationProvider.CurrentProvider = new RadTimePickerLocalizationProvider();
            //RadWizardLocalizationProvider.CurrentProvider = new RadWizardLocalizationProvider();
            //TreeViewLocalizationProvider.CurrentProvider = new TreeViewLocalizationProvider();
            //RadMessageLocalizationProvider.CurrentProvider = new RadMessageLocalizationProvider();
        }
    }

    public class GermanRadGridLocalizationProvider : RadGridLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadGridStringId.ConditionalFormattingMenuItem: return "格式化条件菜单";
                case RadGridStringId.ConditionalFormattingPleaseSelectValidCellValue: return "请选择有效的值";
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValue: return "请设置有效的值";
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValues: return "请设置有效的值";
                case RadGridStringId.ConditionalFormattingPleaseSetValidExpression: return "请设置有效的表达式";
                //case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitive: return "ConditionalFormattingPropertyGridCaseSensitive";
                //case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitiveDescription: return "ConditionalFormattingPropertyGridCaseSensitiveDescription";
                //case RadGridStringId.ConditionalFormattingPropertyGridCellBackColor: return "ConditionalFormattingPropertyGridCellBackColor";
                //case RadGridStringId.ConditionalFormattingPropertyGridCellBackColorDescription: return "ConditionalFormattingPropertyGridCellBackColorDescription";
                //case RadGridStringId.ConditionalFormattingPropertyGridCellForeColor: return "ConditionalFormattingPropertyGridCellForeColor";
                //case RadGridStringId.ConditionalFormattingPropertyGridCellForeColorDescription: return "ConditionalFormattingPropertyGridCellForeColorDescription";
                //case RadGridStringId.ConditionalFormattingPropertyGridEnabled: return "ConditionalFormattingPropertyGridEnabled";
                //case RadGridStringId.ConditionalFormattingPropertyGridEnabledDescription: return "ConditionalFormattingPropertyGridEnabledDescription";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowBackColor: return "ConditionalFormattingPropertyGridRowBackColor";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowBackColorDescription: return "ConditionalFormattingPropertyGridRowBackColorDescription";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowForeColor: return "ConditionalFormattingPropertyGridRowForeColor";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowForeColorDescription: return "ConditionalFormattingPropertyGridRowForeColorDescription";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignment: return "ConditionalFormattingPropertyGridRowTextAlignment";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignmentDescription: return "ConditionalFormattingPropertyGridRowTextAlignmentDescription";
                //case RadGridStringId.ConditionalFormattingPropertyGridTextAlignment: return "ConditionalFormattingPropertyGridTextAlignment";
                //case RadGridStringId.ConditionalFormattingPropertyGridTextAlignmentDescription: return "ConditionalFormattingPropertyGridTextAlignmentDescription";
                //case RadGridStringId.ConditionalFormattingRuleAppliesOn: return "Rule applies on:";
                //case RadGridStringId.ConditionalFormattingSortAlphabetically: return "ConditionalFormattingSortAlphabetically";
                //case RadGridStringId.ConditionalFormattingStartsWith: return "starts with [Value1]";
                //case RadGridStringId.ConditionalFormattingTextBoxExpression: return "ConditionalFormattingTextBoxExpression";
                //case RadGridStringId.CopyMenuItem: return "CopyMenuItem";
                //case RadGridStringId.CustomFilterDialogBtnCancel: return "CustomFilterDialogBtnCancel";
                //case RadGridStringId.CustomFilterDialogBtnOk: return "CustomFilterDialogBtnOk";
                //case RadGridStringId.CustomFilterDialogCaption: return "CustomFilterDialogCaption";
                //case RadGridStringId.CustomFilterDialogCheckBoxNot: return "CustomFilterDialogCheckBoxNot";
                //case RadGridStringId.CustomFilterDialogFalse: return "CustomFilterDialogFalse";
                //case RadGridStringId.CustomFilterDialogLabel: return "CustomFilterDialogLabel";
                //case RadGridStringId.CustomFilterDialogRbAnd: return "CustomFilterDialogRbAnd";
                //case RadGridStringId.CustomFilterDialogRbOr: return "CustomFilterDialogRbOr";
                //case RadGridStringId.CustomFilterDialogTrue: return "CustomFilterDialogTrue";
                //case RadGridStringId.CustomFilterMenuItem: return "CustomFilterMenuItem";
                //case RadGridStringId.CutMenuItem: return "CutMenuItem";
                //case RadGridStringId.DeleteRowMenuItem: return "DeleteRowMenuItem";
                //case RadGridStringId.EditMenuItem: return "EditMenuItem";
                //case RadGridStringId.ExpressionFormAndButton: return "ExpressionFormAndButton";
                //case RadGridStringId.ExpressionFormCancelButton: return "ExpressionFormCancelButton";
                //case RadGridStringId.ExpressionFormConstants: return "ExpressionFormConstants";
                //case RadGridStringId.ExpressionFormDescription: return "ExpressionFormDescription";
                //case RadGridStringId.ExpressionFormFields: return "ExpressionFormFields";
                //case RadGridStringId.ExpressionFormFunctions: return "ExpressionFormFunctions";
                //case RadGridStringId.ExpressionFormFunctionsAggregate: return "ExpressionFormFunctionsAggregate";
                //case RadGridStringId.ExpressionFormFunctionsDateTime: return "ExpressionFormFunctionsDateTime";
                //case RadGridStringId.ExpressionFormFunctionsLogical: return "ExpressionFormFunctionsLogical";
                //case RadGridStringId.ExpressionFormFunctionsMath: return "ExpressionFormFunctionsMath";
                //case RadGridStringId.ExpressionFormFunctionsOther: return "ExpressionFormFunctionsOther";
                //case RadGridStringId.ExpressionFormFunctionsText: return "ExpressionFormFunctionsText";
                //case RadGridStringId.ExpressionFormNotButton: return "ExpressionFormNotButton";
                //case RadGridStringId.ExpressionFormOKButton: return "ExpressionFormOKButton";
                //case RadGridStringId.ExpressionFormOperators: return "ExpressionFormOperators";
                //case RadGridStringId.ExpressionFormOrButton: return "ExpressionFormOrButton";
                //case RadGridStringId.ExpressionFormResultPreview: return "ExpressionFormResultPreview";
                //case RadGridStringId.ExpressionFormTitle: return "ExpressionFormTitle";
                //case RadGridStringId.ExpressionFormTooltipAnd: return "ExpressionFormTooltipAnd";
                //case RadGridStringId.ExpressionFormTooltipDivide: return "ExpressionFormTooltipDivide";
                //case RadGridStringId.ExpressionFormTooltipEqual: return "ExpressionFormTooltipEqual";
                //case RadGridStringId.ExpressionFormTooltipGreater: return "ExpressionFormTooltipGreater";
                //case RadGridStringId.ExpressionFormTooltipGreaterOrEqual: return "ExpressionFormTooltipGreaterOrEqual";
                //case RadGridStringId.ExpressionFormTooltipLess: return "ExpressionFormTooltipLess";
                //case RadGridStringId.ExpressionFormTooltipLessOrEqual: return "ExpressionFormTooltipLessOrEqual";
                //case RadGridStringId.ExpressionFormTooltipMinus: return "ExpressionFormTooltipMinus";
                //case RadGridStringId.ExpressionFormTooltipModulo: return "ExpressionFormTooltipModulo";
                //case RadGridStringId.ExpressionFormTooltipMultiply: return "ExpressionFormTooltipMultiply";
                //case RadGridStringId.ExpressionFormTooltipNot: return "ExpressionFormTooltipNot";
                //case RadGridStringId.ExpressionFormTooltipNotEqual: return "ExpressionFormTooltipNotEqual";
                //case RadGridStringId.ExpressionFormTooltipOr: return "ExpressionFormTooltipOr";
                //case RadGridStringId.ExpressionFormTooltipPlus: return "ExpressionFormTooltipPlus";
                //case RadGridStringId.ExpressionMenuItem: return "ExpressionMenuItem";
                //case RadGridStringId.FilterCompositeNotOperator: return "FilterCompositeNotOperator";
                //case RadGridStringId.FilterFunctionBetween: return "FilterFunctionsBetween";
                //case RadGridStringId.FilterFunctionContains: return "FilterFunctionContains";
                //case RadGridStringId.FilterFunctionCustom: return "FilterFunctionsCustom";
                //case RadGridStringId.FilterFunctionDoesNotContain: return "FilterFunctionDoesNotContain";
                //case RadGridStringId.FilterFunctionDuringLast7days: return "FilterFunctionDuringLast7days";
                //case RadGridStringId.FilterFunctionEndsWith: return "FilterFunctionEndsWith";
                //case RadGridStringId.FilterFunctionEqualTo: return "FilterFunctionEqualTo";
                //case RadGridStringId.FilterFunctionGreaterThan: return "FilterFunctionGreaterThan";
                //case RadGridStringId.FilterFunctionGreaterThanOrEqualTo: return "FilterFunctionGreaterThanOrEqualTo";
                //case RadGridStringId.FilterFunctionIsEmpty: return "FilterFunctionIsEmpty";
                //case RadGridStringId.FilterFunctionIsNull: return "FilterFunctionIsNull";
                //case RadGridStringId.FilterFunctionLessThan: return "FilterFunctionLessThan";
                //case RadGridStringId.FilterFunctionLessThanOrEqualTo: return "FilterFunctionLessThanOrEqualTo";
                //case RadGridStringId.FilterFunctionNoFilter: return "FilterFunctionNoFilter";
                //case RadGridStringId.FilterFunctionNotBetween: return "FilterFunctionNotBetween";
                //case RadGridStringId.FilterFunctionNotEqualTo: return "FilterFunctionNotEqualTo";
                //case RadGridStringId.FilterFunctionNotIsEmpty: return "FilterFunctionNotIsEmpty";
                //case RadGridStringId.FilterFunctionNotIsNull: return "FilterFunctionNotIsNull";
                //case RadGridStringId.FilterFunctionSelectedDates: return "Filter by specific dates:";
                //case RadGridStringId.FilterFunctionStartsWith: return "FilterFunctionStartsWith";
                //case RadGridStringId.FilterFunctionToday: return "FilterFunctionToday";
                //case RadGridStringId.FilterFunctionYesterday: return "FilterFunctionYesterday";
                //case RadGridStringId.FilterLogicalOperatorAnd: return "FilterLogicalOperatorAnd";
                //case RadGridStringId.FilterLogicalOperatorOr: return "FilterLogicalOperatorOr";
                //case RadGridStringId.FilterMenuAvailableFilters: return "FilterMenuAvailableFilters";
                //case RadGridStringId.FilterMenuBlanks: return "FilterMenuBlanks";
                //case RadGridStringId.FilterMenuButtonCancel: return "FilterMenuButtonCancel";
                //case RadGridStringId.FilterMenuButtonOK: return "FilterMenuButtonOK";
                //case RadGridStringId.FilterMenuClearFilters: return "FilterMenuClearFilters";
                //case RadGridStringId.FilterMenuSearchBoxText: return "FilterMenuSearchBoxText";
                //case RadGridStringId.FilterMenuSelectionAll: return "FilterMenuSelectionAll";
                //case RadGridStringId.FilterMenuSelectionAllSearched: return "FilterMenuSelectionAllSearched";
                //case RadGridStringId.FilterMenuSelectionNotNull: return "FilterMenuSelectionNotNull";
                //case RadGridStringId.FilterMenuSelectionNull: return "FilterMenuSelectionNull";
                //case RadGridStringId.FilterOperatorBetween: return "FilterOperatorBetween";
                //case RadGridStringId.FilterOperatorContains: return "FilterOperatorContains";
                //case RadGridStringId.FilterOperatorCustom: return "FilterOperatorCustom";
                //case RadGridStringId.FilterOperatorDoesNotContain: return "FilterOperatorDoesNotContain";
                //case RadGridStringId.FilterOperatorEndsWith: return "FilterOperatorEndsWith";
                //case RadGridStringId.FilterOperatorEqualTo: return "FilterOperatorEqualTo";
                //case RadGridStringId.FilterOperatorGreaterThan: return "FilterOperatorGreaterThan";
                //case RadGridStringId.FilterOperatorGreaterThanOrEqualTo: return "FilterOperatorGreaterThanOrEqualTo";
                //case RadGridStringId.FilterOperatorIsContainedIn: return "FilterOperatorIsContainedIn";
                //case RadGridStringId.FilterOperatorIsEmpty: return "FilterOperatorIsEmpty";
                //case RadGridStringId.FilterOperatorIsLike: return "FilterOperatorIsLike";
                //case RadGridStringId.FilterOperatorIsNull: return "FilterOperatorIsNull";
                //case RadGridStringId.FilterOperatorLessThan: return "FilterOperatorLessThan";
                //case RadGridStringId.FilterOperatorLessThanOrEqualTo: return "FilterOperatorLessThanOrEqualTo";
                //case RadGridStringId.FilterOperatorNoFilter: return "FilterOperatorNoFilter";
                //case RadGridStringId.FilterOperatorNotBetween: return "FilterOperatorNotBetween";
                //case RadGridStringId.FilterOperatorNotEqualTo: return "FilterOperatorNotEqualTo";
                //case RadGridStringId.FilterOperatorNotIsContainedIn: return "FilterOperatorNotIsContainedIn";
                //case RadGridStringId.FilterOperatorNotIsEmpty: return "FilterOperatorNotIsEmpty";
                //case RadGridStringId.FilterOperatorNotIsLike: return "FilterOperatorNotIsLike";
                //case RadGridStringId.FilterOperatorNotIsNull: return "FilterOperatorNotIsNull";
                //case RadGridStringId.FilterOperatorStartsWith: return "FilterOperatorStartsWith";
                //case RadGridStringId.GroupByThisColumnMenuItem: return "GroupByThisColumnMenuItem";
                //case RadGridStringId.GroupingPanelDefaultMessage: return "GroupingPanelDefaultMessage";
                //case RadGridStringId.GroupingPanelHeader: return "GroupingPanelHeader";
                //case RadGridStringId.HideGroupMenuItem: return "HideGroupMenuItem";
                //case RadGridStringId.HideMenuItem: return "HideMenuItem";
                case RadGridStringId.NoDataText: return "空数据";
                //case RadGridStringId.PagingPanelOfPagesLabel: return "PagingPanelOfPagesLabel";
                //case RadGridStringId.PagingPanelPagesLabel: return "PagingPanelPagesLabel";
                //case RadGridStringId.PasteMenuItem: return "PasteMenuItem";
                //case RadGridStringId.PinAtBottomMenuItem: return "PinAtBottomMenuItem";
                //case RadGridStringId.PinAtLeftMenuItem: return "PinAtLeftMenuItem";
                //case RadGridStringId.PinAtRightMenuItem: return "PinAtRightMenuItem";
                //case RadGridStringId.PinAtTopMenuItem: return "PinAtTopMenuItem";
                //case RadGridStringId.PinMenuItem: return "PinMenuItem";
                //case RadGridStringId.SearchRowChooseColumns: return "SearchRowChooseColumns";
                //case RadGridStringId.SearchRowMatchCase: return "SearchRowMatchCase";
                //case RadGridStringId.SearchRowMenuItemAllColumns: return "SearchRowMenuItemAllColumns";
                //case RadGridStringId.SearchRowMenuItemChildTemplates: return "SearchRowMenuItemChildTemplates";
                //case RadGridStringId.SearchRowMenuItemMasterTemplate: return "SearchRowMenuItemMasterTemplate";
                //case RadGridStringId.SearchRowResultsOfLabel: return "SearchRowResultsOfLabel";
                //case RadGridStringId.SearchRowSearchFromCurrentPosition: return "SearchRowSearchFromCurrentPosition";
                //case RadGridStringId.SearchRowTextBoxNullText: return "SearchRowTextBoxNullText";
                //case RadGridStringId.SortAscendingMenuItem: return "SortAscendingMenuItem";
                //case RadGridStringId.SortDescendingMenuItem: return "SortDescendingMenuItem";
                //case RadGridStringId.UngroupThisColumn: return "UngroupThisColumn";
                //case RadGridStringId.UnpinMenuItem: return "UnpinMenuItem";
                //case RadGridStringId.UnpinRowMenuItem: return "UnpinRowMenuItem";

                default: return base.GetLocalizedString(id);
            }
        }
    }

    public class GermanDataFilterLocalizationProvider : DataFilterLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case DataFilterStringId.AddNewButtonExpression: return "创建条件";
                case DataFilterStringId.AddNewButtonGroup: return "创建分组";
                case DataFilterStringId.AddNewButtonText: return "添加";
                case DataFilterStringId.DialogApplyButton: return "应用";
                case DataFilterStringId.DialogCancelButton: return "取消";
                case DataFilterStringId.DialogOKButton: return "确定";
                case DataFilterStringId.DialogTitle: return "标题";
                case DataFilterStringId.ErrorAddNodeDialogText: return "错误";
                case DataFilterStringId.ErrorAddNodeDialogTitle: return "错误";
                case DataFilterStringId.FieldNullText: return "字段为空";
                case DataFilterStringId.FilterFunctionBetween: return "之间";
                case DataFilterStringId.FilterFunctionContains: return "包含";
                case DataFilterStringId.FilterFunctionCustom: return "自定义";
                case DataFilterStringId.FilterFunctionDoesNotContain: return "不包含";
                case DataFilterStringId.FilterFunctionEndsWith: return "以…结束";
                case DataFilterStringId.FilterFunctionEqualTo: return "等于";
                case DataFilterStringId.FilterFunctionGreaterThan: return "大于";
                case DataFilterStringId.FilterFunctionGreaterThanOrEqualTo: return "大于等于";
                case DataFilterStringId.FilterFunctionIsEmpty: return "为空";
                case DataFilterStringId.FilterFunctionIsNull: return "为空";
                case DataFilterStringId.FilterFunctionLessThan: return "小于";
                case DataFilterStringId.FilterFunctionLessThanOrEqualTo: return "小于等于";
                case DataFilterStringId.FilterFunctionNoFilter: return "请选择";
                case DataFilterStringId.FilterFunctionNotBetween: return "不在范围";
                case DataFilterStringId.FilterFunctionNotEqualTo: return "不等于";
                case DataFilterStringId.FilterFunctionNotIsEmpty: return "不为空";
                case DataFilterStringId.FilterFunctionNotIsNull: return "不为空";
                case DataFilterStringId.FilterFunctionStartsWith: return "以…开始";
                case DataFilterStringId.LogicalOperatorAnd: return "与";
                case DataFilterStringId.LogicalOperatorDescription: return "";
                case DataFilterStringId.LogicalOperatorOr: return "或";
                case DataFilterStringId.ValueNullText: return "空文本";
                default: return base.GetLocalizedString(id);
            }
        }
    }
}