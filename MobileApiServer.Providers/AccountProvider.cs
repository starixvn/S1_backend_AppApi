using System;
using System.Data;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.Extensions.Configuration;
using MobileApiServer.Models;
using MySqlConnector;

namespace MobileApiServer.Providers
{
    public class AccountProvider
    {
        private readonly string _connectionString;
        public AccountProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        /// <summary>
        /// Query user by name and siteId
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public Customer GetCustInfoByUserName(string userName, int siteId)
        {
            using var conn = GetConnection();
            return conn.QueryFirstOrDefault<Customer>("LoginCustInfo_Get", new { UserName = userName, SiteId = siteId }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Another save login log, should not split into two SP
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userName"></param>
        /// <param name="siteId"></param>
        /// <param name="loginIp"></param>
        /// <param name="fromUrl"></param>
        /// <param name="userAgent"></param>
        public void AddUserLoginLog(int custId, string userName, int siteId, string loginIp, string fromUrl, string userAgent)
        {
            using var conn = GetConnection();
            conn.Execute("LoginLog_Ins", new { CustId = custId, UserName = userName, SiteId = siteId, LoginIp = loginIp, FromUrl = fromUrl, UserAgent = userAgent }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Save login to OnUser, dunno why need transaction
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="siteId"></param>
        /// <param name="ip"></param>
        /// <param name="token"></param>
        public void AddLoginRecord(int custId, int siteId, string ip, out string token)
        {
            token = GetToken();
            using var conn = GetConnection();
            conn.Open();
            using var trans = conn.BeginTransaction();
            conn.Execute("LoginRecord_Set", new { CustId = custId, SiteId = siteId, IP = ip, Token = token }, trans, commandType: CommandType.StoredProcedure);
            trans.Commit();
        }

        public bool UpdateOnline(int custId, bool isOnline, string token, string ip)
        {
            using var conn = GetConnection();
            return conn.Execute("LoginCheckIn_Set", new { IsOnline = isOnline, CustId = custId, IP = ip, Token = token }, commandType: CommandType.StoredProcedure) > 0;
        }

        #region private
        /// <summary>
        /// Get token for table [OnUser]
        /// </summary>
        /// <returns></returns>
        private static string GetToken()
        {
            var base64 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return Regex.Replace(base64, "[=+]", "");
        }
        #endregion
    }
}
