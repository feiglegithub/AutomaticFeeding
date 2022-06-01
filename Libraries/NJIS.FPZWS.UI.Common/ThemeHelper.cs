// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform.DataManager
//  项目名称：NJIS.FPZWS.UI.Common
//  文 件 名：ThemeHelper.cs
//  创建时间：2018-07-23 17:59
//  作    者：
//  说    明：
//  修改时间：2018-03-04 17:12
//  修 改 人：11001891
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NJIS.FPZWS.Common;
using Telerik.WinControls;

#endregion

namespace NJIS.FPZWS.UI.Common
{
    public class ThemeHelper
    {
        private static readonly string ThemePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "NJIS.FPZWS");

        private static readonly string ThemeFilePath = Path.Combine(ThemePath, "THEME.txt");
        private static readonly string ThemeFilesPath = Path.Combine(ThemePath, "THEMES.txt");

        public static AppTheme GetDefaultTheme()
        {
            var lst = GetAppThemes();
            if (lst == null || lst.Count == 0) return null;
            if (!File.Exists(ThemeFilePath)) return null;
            var tm = File.ReadAllText(ThemeFilePath);

            var at = lst.FirstOrDefault(m => m.Name.ToLower() == tm.ToLower());
            if (at == null) return null;
            return at;
        }

        public static void SetTheme(string theme)
        {
            File.WriteAllText(ThemeFilePath, theme + "Theme");
        }

        public static string GetCureentTheme()
        {
            if (!File.Exists(ThemeFilePath)) return null;
            var cs = File.ReadAllLines(ThemeFilePath);
            return cs[0];
        }

        public static List<AppTheme> GetAppThemes()
        {
            if (!File.Exists(ThemeFilesPath)) return null;
            return SerializeHelper.FromXmlFile<List<AppTheme>>(ThemeFilesPath);
        }

        public static void InitializeThemesMenuItems()
        {
            if (!Directory.Exists(ThemePath)) Directory.CreateDirectory(ThemePath);
            var files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "themes"),
                "Telerik.WinControls.Themes.*.dll");
            var themes = new List<AppTheme>();
            foreach (var f in files)
                try
                {
                    var types = Assembly.LoadFile(f).GetTypes();
                    foreach (var t in types)
                        if (t.Name.EndsWith("Theme"))
                            themes.Add(new AppTheme
                            {
                                Name = t.Name,
                                ThemeType = t.FullName,
                                ThemeDll = f
                            });
                }
                catch (Exception)
                {
                }

            var tt = GetDefaultTheme();
            SetTheme(tt);

            SerializeHelper.ToXmlFile(themes, ThemeFilesPath);
        }


        public static void SetTheme(AppTheme t)
        {
            if (t == null) return;
            try
            {
                var tn = t.Name.Substring(0, t.Name.Length - 5);
                Assembly.LoadFrom(t.ThemeDll).CreateInstance(t.ThemeType);
                ThemeResolutionService.ApplicationThemeName = tn;
                SetTheme(tn);
            }
            catch (Exception)
            {
            }
        }
    }

    [Serializable]
    public class AppTheme
    {
        public string Name { get; set; }

        public string ThemeDll { get; set; }
        public string ThemeType { get; set; }
    }
}