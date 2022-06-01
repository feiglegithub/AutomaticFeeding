using NJIS.AppUtility.NjSystem.AD;
using NJIS.Windows.TemplateBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.Windows.TemplateBase
{
    public class NjAuthorization
    {
        private static NjAuthorization _instance;
        public static NjAuthorization Instance
        {
            get
            {
                return _instance ?? (_instance = new NjAuthorization());
            }
        }

        public NjAuthorization()
        {
            Validated = false;
            CurrentUser = null;
        }

        public UserInfo CurrentUser { get; private set; }
        public bool Validated { get; private set; }

        public bool ValidateUser(string account, string password)
        {
            LDAPHelper ldap = new LDAPHelper();
            bool result = ldap.ValidateUser(account, password);
            if (result)
            {
                CurrentUser = new UserInfo();
                SGDomainUser user = ldap.GetUserByAccount(account, account, password);
                CurrentUser.UserAccount = user.AccountName;
                CurrentUser.UserName = user.DisplayName;
                CurrentUser.Email = user.Mail;
                CurrentUser.PhoneNumber = user.TelephoneNumber;
                Validated = true;
            }
            else
            {
                CurrentUser = null;
                Validated = false;
            }
            return result;
        }


        public bool ValidateUser(string account, string password, string group)
        {
            LDAPHelper ldap = new LDAPHelper();
            bool result = ldap.ValidateUser(account, password, group);
            if (result)
            {
                CurrentUser = new UserInfo();
                SGDomainUser user = ldap.GetUserByAccount(account);
                CurrentUser.UserAccount = user.AccountName;
                CurrentUser.UserName = user.DisplayName;
                CurrentUser.Email = user.Mail;
                CurrentUser.PhoneNumber = user.TelephoneNumber;
                Validated = true;
            }
            else
            {
                CurrentUser = null;
                Validated = false;
            }
            return result;
        }

        public void Logout()
        {
            _instance = new NjAuthorization();
        }
    }
}
