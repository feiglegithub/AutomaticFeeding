// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.FPZWS.LineControl.Drilling.Client
//  文 件 名：DomainPasswordValidator.cs
//  创建时间：2019-11-03 17:03
//  作    者：
//  说    明：
//  修改时间：2019-11-03 17:02
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NJIS.FPZWS.LineControl.Drilling.Client
{
    public class DomainPasswordValidator
    {
        public bool Validate(string userName, string password)
        {
            return ValidateUserAccount1("LDAP://sogal.org", userName, password);
            //return ValidateDomainUser(userName, password, "10.10.10.10");
            //return true;
        }

        public static bool ValidateUserAccount1(string astrDomainName, string astrDomainAccount,
            string astrDomainPassword)
        {
            using (var adsEntry = new DirectoryEntry(astrDomainName, astrDomainAccount, astrDomainPassword,
                AuthenticationTypes.Secure))
            {
                var obj = adsEntry.NativeObject;
                using (var adsSearcher = new DirectorySearcher(adsEntry))
                {
                    adsSearcher.Filter = $"(sAMAccountName={astrDomainAccount})";
                    try
                    {
                        adsSearcher.PropertiesToLoad.Add("cn");

                        var adsSearchResult = adsSearcher.FindOne();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    finally
                    {
                        adsEntry.Close();
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="astrDomainName"></param>
        /// <param name="astrDomainAccount"></param>
        /// <param name="astrDomainPassword"></param>
        /// <returns></returns>
        public static bool ValidateUserAccount(string astrDomainName, string astrDomainAccount,
            string astrDomainPassword)
        {
            const int logon32LogonInteractive = 2; //通过网络验证账户合法性
            const int logon32ProviderDefault = 0; //使用默认的Windows 2000/NT NTLM验证方
            var tokenHandle = IntPtr.Zero;

            return DomainPrincipal.LogonUser(astrDomainAccount, astrDomainName, astrDomainPassword,
                logon32LogonInteractive, logon32ProviderDefault, ref tokenHandle);
        }

        public bool ValidateDomainUser(string userName, string password, string Domain)
        {
            var bValid = false;

            using (var context = new PrincipalContext(ContextType.Domain, Domain))
            {
                bValid = context.ValidateCredentials(userName, password);
            }

            return bValid;
        }
    }

    #region advapi32

    public class DomainPrincipal
    {
        public DomainPrincipal(UserPrincipal userPrincipalSearchResult)
        {
            Description = userPrincipalSearchResult.Description;
            SamAccountName = userPrincipalSearchResult.SamAccountName;
            UserPrincipalName = userPrincipalSearchResult.UserPrincipalName;
            DistinguishedName = userPrincipalSearchResult.DistinguishedName;
            Guid = userPrincipalSearchResult.Guid;
            StructuralObjectClass = userPrincipalSearchResult.StructuralObjectClass;
            Name = userPrincipalSearchResult.Name;
            DisplayName = userPrincipalSearchResult.DisplayName;

            GivenName = userPrincipalSearchResult.GivenName;
            MiddleName = userPrincipalSearchResult.MiddleName;
            Surname = userPrincipalSearchResult.Surname;
            EmailAddress = userPrincipalSearchResult.EmailAddress;
            VoiceTelephoneNumber = userPrincipalSearchResult.VoiceTelephoneNumber;
            EmployeeId = userPrincipalSearchResult.EmployeeId;
            Enabled = userPrincipalSearchResult.Enabled;
            AccountLockoutTime = userPrincipalSearchResult.AccountLockoutTime;
            LastLogon = userPrincipalSearchResult.LastLogon;
            PermittedWorkstations = userPrincipalSearchResult.PermittedWorkstations;
            PermittedLogonTimes = userPrincipalSearchResult.PermittedLogonTimes;
            AccountExpirationDate = userPrincipalSearchResult.AccountExpirationDate;
            SmartcardLogonRequired = userPrincipalSearchResult.SmartcardLogonRequired;
            DelegationPermitted = userPrincipalSearchResult.DelegationPermitted;
            BadLogonCount = userPrincipalSearchResult.BadLogonCount;
            HomeDirectory = userPrincipalSearchResult.HomeDirectory;
            HomeDrive = userPrincipalSearchResult.HomeDrive;
            ScriptPath = userPrincipalSearchResult.ScriptPath;
            LastPasswordSet = userPrincipalSearchResult.LastPasswordSet;
            LastBadPasswordAttempt = userPrincipalSearchResult.LastBadPasswordAttempt;
            PasswordNotRequired = userPrincipalSearchResult.PasswordNotRequired;
            PasswordNeverExpires = userPrincipalSearchResult.PasswordNeverExpires;
            UserCannotChangePassword = userPrincipalSearchResult.UserCannotChangePassword;
            AllowReversiblePasswordEncryption = userPrincipalSearchResult.AllowReversiblePasswordEncryption;
        }

        /// <summary>
        ///     获取或设置主体的说明。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     获取或设置此主体的显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     获取或设置此主体的 SAM 帐户名。
        /// </summary>
        public string SamAccountName { get; set; }

        /// <summary>
        ///     获取或设置与此主体关联的用户主要名称 (UPN)。
        /// </summary>
        public string UserPrincipalName { get; set; }

        /// <summary>
        ///     获取与此主体关联的 GUID。
        /// </summary>
        public Guid? Guid { get; set; }

        /// <summary>
        ///     获取此主体的可分辨名称 (DN)。
        /// </summary>
        public string DistinguishedName { get; set; }

        /// <summary>
        ///     获取结构对象类目录特性。
        /// </summary>
        public string StructuralObjectClass { get; set; }

        /// <summary>
        ///     获取或设置此主体的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     获取或设置用户主体的名字。
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        ///     获取或设置用户主体的中间名。
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        ///     获取或设置用户主体的姓氏。
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        ///     获取或设置此帐户的电子邮件地址。
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        ///     获取或设置用户主体的语音电话号码。
        /// </summary>
        public string VoiceTelephoneNumber { get; set; }

        /// <summary>
        ///     获取或设置此用户主体的雇员 ID。
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        ///     获取或设置一个可以为 null 的布尔值，该值指定是否支持此帐户进行身份验证。
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        ///     获取一个可以为 null 的 System.DateTime，用于指定帐户被锁定的日期和时间。
        /// </summary>
        public DateTime? AccountLockoutTime { get; set; }

        /// <summary>
        ///     获取一个可以为 null 的 System.DateTime，用于指定最后一次登录此帐户的日期和时间。
        /// </summary>
        public DateTime? LastLogon { get; set; }

        /// <summary>
        ///     获取允许此主体登录的工作站的列表。
        /// </summary>
        public PrincipalValueCollection<string> PermittedWorkstations { get; set; }

        /// <summary>
        ///     获取或设置主体可以登录的次数。
        /// </summary>
        public byte[] PermittedLogonTimes { get; set; }

        /// <summary>
        ///     获取或设置一个可以为 null 的 System.DateTime，用于指定帐户过期的日期和时间。
        /// </summary>
        public DateTime? AccountExpirationDate { get; set; }

        /// <summary>
        ///     获取或设置一个布尔值，该值指定登录帐户是否需要智能卡。
        /// </summary>
        public bool SmartcardLogonRequired { get; set; }

        /// <summary>
        ///     获取或设置一个可以为 null 的布尔值，该值指定是否可以委托帐户。
        /// </summary>
        public bool DelegationPermitted { get; set; }

        /// <summary>
        ///     获取对此帐户使用不正确的凭据进行登录的尝试次数。
        /// </summary>
        public int BadLogonCount { get; set; }

        /// <summary>
        ///     获取或设置此帐户的主目录。
        /// </summary>
        public string HomeDirectory { get; set; }

        /// <summary>
        ///     获取或设置此帐户的主驱动器。
        /// </summary>
        public string HomeDrive { get; set; }

        /// <summary>
        ///     获取或设置此帐户的脚本路径。
        /// </summary>
        public string ScriptPath { get; set; }

        /// <summary>
        ///     获取一个可以为 null 的 System.DateTime，用于指定最后一次为此帐户设置密码的日期和时间。
        /// </summary>
        public DateTime? LastPasswordSet { get; set; }

        /// <summary>
        ///     获取一个可以为 null 的 System.DateTime，用于指定最后一次对此帐户进行不正确的密码尝试的日期和时间。
        /// </summary>
        public DateTime? LastBadPasswordAttempt { get; set; }

        /// <summary>
        ///     获取或设置一个布尔值，该值指定此帐户是否需要密码。
        /// </summary>
        public bool PasswordNotRequired { get; set; }

        /// <summary>
        ///     获取或设置一个布尔值，该值指定此帐户的密码是否会过期。
        /// </summary>
        public bool PasswordNeverExpires { get; set; }

        /// <summary>
        ///     获取或设置一个布尔值，该值指定用户是否可以更改此帐户的密码。
        ///     不要将此属性和 System.DirecoryServices.AccountManagement.ComputerPrincipal 一起使用。
        /// </summary>
        public bool UserCannotChangePassword { get; set; }

        /// <summary>
        ///     获取或设置一个布尔值，该值指定是否为此帐户启用可逆密码加密。
        /// </summary>
        public bool AllowReversiblePasswordEncryption { get; set; }

        /// <summary>
        ///     查询用户
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="samAccountName">1100189*</param>
        public static List<DomainPrincipal> SearchUser(string domainName, string samAccountName)
        {
            var users = new List<DomainPrincipal>();
            using (var principalContext = new PrincipalContext(ContextType.Domain, domainName))
            {
                using (var userPrincipal = new UserPrincipal(principalContext))
                {
                    using (var principalSearcher = new PrincipalSearcher(userPrincipal))
                    {
                        principalSearcher.QueryFilter = new UserPrincipal(principalContext)
                            {SamAccountName = samAccountName};

                        foreach (UserPrincipal userPrincipalSearchResult in principalSearcher.FindAll())
                        {
                            var user = new DomainPrincipal(userPrincipalSearchResult);
                            users.Add(user);
                        }
                    }
                }
            }

            PrintDomainUser(users);
            return users;
        }

        private static void PrintDomainUser(List<DomainPrincipal> users)
        {
            foreach (var user in users)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("名称:{0}", user.Name);
                Console.WriteLine("GUID。:{0}", user.Guid);
                Console.WriteLine("可分辨名称 (DN):{0}", user.DistinguishedName);
                Console.WriteLine("关联的用户主要名称 (UPN):{0}", user.UserPrincipalName);
                Console.WriteLine("SAM 帐户名:{0}", user.SamAccountName);
                Console.WriteLine("说明:{0}", user.Description);
                Console.WriteLine("结构对象类目录特性:{0}", user.StructuralObjectClass);
                Console.WriteLine("名字:{0}", user.GivenName);
                Console.WriteLine("中间名:{0}", user.MiddleName);
                Console.WriteLine("姓氏:{0}", user.Surname);
                Console.WriteLine("电子邮件:{0}", user.EmailAddress);
                Console.WriteLine("电话号码:{0}", user.VoiceTelephoneNumber);
                Console.WriteLine("编号:{0}", user.EmployeeId);
                Console.WriteLine("是否支持此帐户进行身份验证:{0}", user.Enabled);
                Console.WriteLine("帐户被锁定的日期和时间:{0}", user.AccountLockoutTime);
                Console.WriteLine("最后一次登录此帐户的日期和时间:{0}", user.LastLogon);
                Console.WriteLine("允许此主体登录的工作站的列表:{0}", string.Join(",", user.PermittedWorkstations.ToList()));
                Console.WriteLine("可以登录的次数:{0}", user.PermittedLogonTimes);
                Console.WriteLine("帐户过期的日期和时间:{0}", user.AccountExpirationDate);
                Console.WriteLine("登录帐户是否需要智能卡:{0}", user.SmartcardLogonRequired);
                Console.WriteLine("是否可以委托帐户:{0}", user.DelegationPermitted);
                Console.WriteLine("帐户使用不正确的凭据进行登录的尝试次数:{0}", user.BadLogonCount);
                Console.WriteLine("帐户的主目录:{0}", user.HomeDirectory);
                Console.WriteLine("帐户的主驱动器:{0}", user.HomeDrive);
                Console.WriteLine("帐户的脚本路径:{0}", user.ScriptPath);
                Console.WriteLine("最后一次为此帐户设置密码的日期和时间:{0}", user.LastPasswordSet);
                Console.WriteLine("最后一次对此帐户进行不正确的密码尝试的日期和时间:{0}", user.LastBadPasswordAttempt);
                Console.WriteLine("帐户是否需要密码:{0}", user.PasswordNotRequired);
                Console.WriteLine("帐户的密码是否会过期:{0}", user.PasswordNeverExpires);
                Console.WriteLine("用户是否可以更改此帐户的密码:{0}", user.UserCannotChangePassword);
                Console.WriteLine("帐户启用可逆密码加密:{0}", user.AllowReversiblePasswordEncryption);
                Console.WriteLine("----------------------------------------");
            }
        }

        [DllImport("advapi32.dll")]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
    }

    #endregion

    #region DirectorySearcher

    public class DsUser
    {
        public DsUser(SearchResult item)
        {
            Sn = GetPropertyValue(item.Properties["sn"]);
            Cn = GetPropertyValue(item.Properties["cn"]);
            SamaccountName = GetPropertyValue(item.Properties["SamaccountName"]);
            Mail = GetPropertyValue(item.Properties["Mail"]);
            Title = GetPropertyValue(item.Properties["Title"]);
            PwdLastSet = GetPropertyValue(item.Properties["PwdLastSet"]);
            WhenCreated = GetPropertyValue(item.Properties["WhenCreated"]);
            WhenChanged = GetPropertyValue(item.Properties["WhenChanged"]);
            LastLogon = GetPropertyValue(item.Properties["LastLogon"]);
            BadPasswordTime = GetPropertyValue(item.Properties["BadPasswordTime"]);
            LastLogonTimeStamp = GetPropertyValue(item.Properties["LastLogonTimeStamp"]);
            Name = GetPropertyValue(item.Properties["Name"]);
            MailNickName = GetPropertyValue(item.Properties["MailNickName"]);
            LogonCount = GetPropertyValue(item.Properties["LogonCount"]);
            BadPwdCount = GetPropertyValue(item.Properties["BadPwdCount"]);
            Department = GetPropertyValue(item.Properties["Department"]);
            Company = GetPropertyValue(item.Properties["Company"]);
        }

        public string Sn { get; set; }
        public string Cn { get; set; }
        public string SamaccountName { get; set; }
        public string Mail { get; set; }
        public string Title { get; set; }
        public string PwdLastSet { get; set; }
        public string WhenCreated { get; set; }
        public string WhenChanged { get; set; }
        public string LastLogon { get; set; }
        public string BadPasswordTime { get; set; }
        public string LastLogonTimeStamp { get; set; }
        public string Name { get; set; }
        public string MailNickName { get; set; }
        public string LogonCount { get; set; }
        public string BadPwdCount { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }

        /// <summary>
        ///     查询用户
        /// </summary>
        /// <param name="adPath">LDAP://sogal.org</param>
        /// <param name="filter">(&(objectClass=user)(objectCategory=Person)(samaccountname=1100189*))</param>
        /// <returns></returns>
        public static List<DsUser> SearchUser(string adPath, string filter)
        {
            var users = new List<DsUser>();
            using (var dr = new DirectorySearcher())
            {
                dr.Filter = filter;
                var ss = dr.FindAll();
                foreach (SearchResult item in ss)
                {
                    var sb = new StringBuilder();
                    foreach (DictionaryEntry property in item.Properties)
                    {
                        //sb.AppendLine(string.Format("{0}:{1}", property.Key, ""));
                        sb.AppendLine(string.Format("{0}:{1}", property.Key, GetPropertyValue(property.Value)));
                    }

                    var user = new DsUser(item);
                    users.Add(user);
                    var entry = item.GetDirectoryEntry();
                }
            }

            return users;
        }

        public static string GetPropertyValue(object obj)
        {
            var rpvc = obj as ResultPropertyValueCollection;
            if (rpvc == null) return "";
            var str = new StringBuilder();
            for (var i = 0; i < rpvc.Count; i++)
            {
                str.Append(rpvc[i]);
            }

            return str.ToString();
        }
    }

    public class LdapAuthentication
    {
        private string _filterAttribute;
        private string _path;

        /// <summary>
        /// </summary>
        /// <param name="path">不能包含 LDAP:// </param>
        public LdapAuthentication(string path)
        {
            _path = "LDAP://" + path;
        }

        /// <summary>
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool IsAuthenticated(string domain, string username, string pwd)
        {
            var domainAndUsername = domain + @"\" + username;
            var entry = new DirectoryEntry(_path, domainAndUsername, pwd);

            try
            {
                //Bind to the native AdsObject to force authentication.			
                var obj = entry.NativeObject;
                var search = new DirectorySearcher(entry) {Filter = "(SAMAccountName=" + username + ")"};

                search.PropertiesToLoad.Add("cn");
                var result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string) result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                return false;
                //throw new Exception("Error authenticating user. " + ex.Message);
            }

            return true;
        }

        public string GetGroups()
        {
            var search = new DirectorySearcher(_path) {Filter = "(cn=" + _filterAttribute + ")"};
            search.PropertiesToLoad.Add("memberOf");
            var groupNames = new StringBuilder();

            try
            {
                var result = search.FindOne();

                var propertyCount = result.Properties["memberOf"].Count;

                string dn;
                int equalsIndex, commaIndex;

                for (var propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (string) result.Properties["memberOf"][propertyCounter];

                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }

                    groupNames.Append(dn.Substring(equalsIndex + 1, commaIndex - equalsIndex - 1));
                    groupNames.Append("|");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }

            return groupNames.ToString();
        }
    }

    #endregion
}