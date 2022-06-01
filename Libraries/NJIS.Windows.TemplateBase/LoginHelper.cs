using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.Windows.TemplateBase
{
    public class LoginHelper
    { 
        public static string CurrLoginUser = "";
        public static bool IsLoginSuccess = false;

        [DllImport("advapi32.dll")]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        public static void ValidateUserAccount(string astrDomainName, string astrDomainAccount, string astrDomainPassword)
        {
            const int logon32LogonInteractive = 2;          //通过网络验证账户合法性
            const int logon32ProviderDefault = 0;           //使用默认的Windows 2000/NT NTLM验证方
            var tokenHandle = IntPtr.Zero;

            if (LogonUser(astrDomainAccount, astrDomainName, astrDomainPassword, logon32LogonInteractive, logon32ProviderDefault, ref tokenHandle))
            {
                CurrLoginUser = astrDomainAccount;
                IsLoginSuccess = true;
            }
            else
            {
                CurrLoginUser = "";
                IsLoginSuccess = false;
            }
        }

        public static void LoginOut()
        {
            LoginHelper.CurrLoginUser = "";
            LoginHelper.IsLoginSuccess = false;
        }

    }
}
