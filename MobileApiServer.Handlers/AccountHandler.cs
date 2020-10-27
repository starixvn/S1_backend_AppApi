using MobileApiServer.Core;
using MobileApiServer.Models;
using MobileApiServer.Providers;

namespace MobileApiServer.Handlers
{
    public class AccountHandler
    {
        private readonly AccountProvider _accountProvider;

        public AccountHandler(AccountProvider accountProvider)
        {
            _accountProvider = accountProvider;
        }

        /// <summary>
        /// Try to login and save login log
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="pwd">password</param>
        /// <param name="siteId">site id send by app</param>
        /// <param name="ip">remote ip</param>
        /// <param name="fromUrl">refer url</param>
        /// <param name="userAgent"></param>
        /// <param name="customer">output</param>
        /// <param name="token">output</param>
        /// <returns></returns>
        public InnerResult UserLogin(string userName, string pwd, int siteId, string ip, string fromUrl, string userAgent, out Customer customer, out string token)
        {
            customer = _accountProvider.GetCustInfoByUserName(userName, siteId);
            token = null;
            var failResult = new InnerResult {Success = false};
            if (customer == null)
            {
                failResult.Code = (int) ErrorStatus.NotExists;
                failResult.Message = ErrorStatus.NotExists.ToString();
                return failResult;
            }

            if (customer.UserPwd != Tools.Md5Encrypt(pwd))
            {
                failResult.Code = (int)ErrorStatus.WrongPassword;
                failResult.Message = ErrorStatus.WrongPassword.ToString();
                return failResult;
            }

            if (customer.Status == (int)CustStatus.Suspend || customer.Status == (int)CustStatus.Closed)
            {
                failResult.Code = (int)ErrorStatus.NotAvailable;
                failResult.Message = ErrorStatus.NotAvailable.ToString();
                return failResult;
            }

            _accountProvider.AddLoginRecord(customer.CustId, customer.SiteId, ip, out token);
            _accountProvider.AddUserLoginLog(customer.CustId, customer.UserName, customer.SiteId, ip, fromUrl, userAgent);

            return new InnerResult();
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool Logout(int uid,string token, string ip)
        {
            return _accountProvider.UpdateOnline(uid, false, token, ip);
        }
    }
}
