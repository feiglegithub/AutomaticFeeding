// ************************************************************************************
//  解决方案：NJIS.FPZWS.Platform
//  项目名称：NJIS.FPZWS.Common
//  文 件 名：IniFile.cs
//  创建时间：2017-07-28 14:33
//  作    者：
//  说    明：
//  修改时间：2017-10-08 10:31
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace NJIS.Ini
{
    public sealed class IniFile : KeyedCollection<string, IniFile.Section>
    {
        public const string PropertyFormat = PropertyKeyFormat + @"\s*=" + PropertyValueFormat;
        public const string PropertyKeyFormat = @"(\w[\w\s]+\w)";
        public const string PropertyValueFormat = @"(.*)";

        private static readonly Regex SectionPattern = new Regex(@"^\[\s*(\w[\w\s]*)\s*\]$");
        private static readonly Regex PropertyPattern = new Regex(@"^(\s*[a-zA-Z][\w]*)\s*=(.*)$");
        private readonly StringComparison _comparison;

        public void SaveTo(string filePath)
        {
            using (var writer = File.CreateText(filePath))
                SaveTo(writer);
        }

        public void SaveTo(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
                SaveTo(writer);
        }

        public void SaveTo(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            InternalSave(writer.WriteLine, writer.WriteLine);
            writer.Flush();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            InternalSave((str, args) => sb.AppendFormat(str, args), () => sb.AppendLine());
            return sb.ToString();
        }


        private void InternalSave(Action<string, object[]> writeAction, Action writeBlankLine)
        {
            foreach (var section in this)
            {
                writeAction("[{0}]", new object[] {section.Name});
                //writeBlankLine();
                foreach (var property in section)
                {
                    writeAction("{0}={1}", new object[] {property.Key, property.Value});
                    //writeBlankLine();
                }
                writeBlankLine();
            }
        }

        protected override string GetKeyForItem(Section item)
        {
            return item.Name;
        }

        #region Section

        public sealed class Section : Collection<Property>
        {
            public Section(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public string this[string key]
            {
                get
                {
                    var matchingProperty = this.FirstOrDefault(p => p.Key.ToLower() == key.ToLower());
                    return matchingProperty != null ? matchingProperty.Value : null;
                }
                set
                {
                    var p = this.FirstOrDefault(m => m.Key == key);
                    if (p != null)
                    {
                        p.Value = value;
                    }
                    else
                    {
                        Add(key, value);
                    }
                }
            }

            public void Add(string key, string value)
            {
                Add(new Property
                {
                    Key = key,
                    Value = value
                });
            }

            public bool Remove(string key)
            {
                for (var i = 0; i < Count; i++)
                {
                    var property = this[i];
                    if (key == property.Key)
                    {
                        RemoveAt(i);
                        return true;
                    }
                }
                return false;
            }


            private bool Equals(Section other)
            {
                return string.Equals(Name, other.Name);
            }
        }

        #endregion

        #region Property

        public sealed class Property
        {
            public Property()
            {
            }

            public Property(string key, string value)
            {
                Key = key;
                Value = value;
            }

            public string Key { get; set; }
            public string Value { get; set; }

            public override bool Equals(object obj)
            {
                if (!(obj is Property)) return false;
                var t = obj as Property;
                return string.Equals(Key, t.Key);
            }

            private bool Equals(Property other)
            {
                return string.Equals(Key, other.Key);
            }
        }

        #endregion

        #region Construction

        public IniFile(IniLoadSettings settings = null)
        {
            settings = settings ?? IniLoadSettings.Default;
            _comparison = settings.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        }

        public IniFile(string iniFilePath, IniLoadSettings settings = null)
        {
            settings = settings ?? IniLoadSettings.Default;

            if (iniFilePath == null)
                throw new ArgumentNullException("iniFilePath");
            if (!File.Exists(iniFilePath))
            {
                if (settings.AutoCreate)
                {
                    using (var f = File.Create(iniFilePath))
                    {
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format("INI file '{0}' does not exist", iniFilePath),
                        "iniFilePath");
                }
            }

            _comparison = settings.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            using (
                var reader = new StreamReader(iniFilePath, settings.Encoding ?? Encoding.UTF8, settings.DetectEncoding))
            {
                ParseIniFile(reader);
            }
        }

        public IniFile(Stream stream, IniLoadSettings settings = null)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Cannot read from specified stream", "stream");

            settings = settings ?? IniLoadSettings.Default;
            _comparison = settings.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            using (var reader = new StreamReader(stream, settings.Encoding ?? Encoding.UTF8, settings.DetectEncoding))
                ParseIniFile(reader);
        }

        /// <summary>
        ///     Load <see cref="IniFile" /> from <see cref="string" />
        /// </summary>
        /// <param name="content"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IniFile Load(string content, IniLoadSettings settings = null)
        {
            settings = settings ?? IniLoadSettings.Default;
            var encoding = settings.Encoding ?? Encoding.UTF8;

            var contentBytes = encoding.GetBytes(content);
            var stream = new MemoryStream(contentBytes.Length);
            stream.Write(contentBytes, 0, contentBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return new IniFile(stream);
        }

        /// <summary>
        ///     Parse ini File
        /// </summary>
        /// <param name="reader"></param>
        private void ParseIniFile(TextReader reader)
        {
            Section currentSection = null;
            for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                line = line.Trim();

                //Comment
                if (line.StartsWith(";"))
                    continue;

                //Section
                var sectionMatch = SectionPattern.Match(line);
                if (sectionMatch.Success)
                {
                    var sectionName = sectionMatch.Groups[1].Value;
                    if (this.Any(section => section.Name.Equals(sectionName, _comparison)))
                        throw new NotSupportedException(string.Format("Duplicate section found - '{0}'", sectionName));
                    currentSection = new Section(sectionName);
                    Add(currentSection);
                    continue;
                }

                //Property
                var propertyMatch = PropertyPattern.Match(line);
                if (propertyMatch.Success)
                {
                    var propertyName = propertyMatch.Groups[1].Value.TrimStart(' ').TrimEnd(' ');
                    var propertyValue = propertyMatch.Groups[2].Value.TrimStart(' ').TrimEnd(' ');

                    if (currentSection == null)
                        throw new NotSupportedException(string.Format("Property '{0}' is not part of any section",
                            propertyName));
                    if (currentSection.Any(property => property.Key.Equals(propertyName, _comparison)))
                        throw new NotSupportedException(string.Format("Key '{0}' already exists in section '{1}'",
                            propertyName, currentSection.Name));

                    currentSection.Add(propertyName, propertyValue);
                    continue;
                }

                throw new NotSupportedException(string.Format("Unrecognized line '{0}'", line));
            }
        }

        #endregion
    }

    /// <summary>
    ///     Load INI file settings
    /// </summary>
    public sealed class IniLoadSettings
    {
        public static readonly IniLoadSettings Default = new IniLoadSettings();

        public IniLoadSettings()
        {
            Encoding = Encoding.UTF8;
            AutoCreate = false;
        }

        public bool AutoCreate { get; set; }

        public Encoding Encoding { get; set; }

        public bool DetectEncoding { get; set; }

        public bool CaseSensitive { get; set; }
    }
}