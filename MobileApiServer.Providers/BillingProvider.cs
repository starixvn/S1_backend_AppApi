using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MobileApiServer.Models;
using MySqlConnector;

namespace MobileApiServer.Providers
{
    public class BillingProvider
    {
        private readonly string _connectionString;
        public BillingProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        //TODO error catch

        /// <summary>
        /// Get Local Balance info
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public WalletInfo GetWalletInfo(int custId, int siteId)
        {
            using var conn = GetConnection();
            var walletInfo = conn.QueryFirstOrDefault<WalletInfo>("CustInfoLog1_Get", new { CustId = custId, SiteId = siteId }, commandType: CommandType.StoredProcedure);
            var balance = conn.ExecuteScalar<decimal>("Balance_Get", new { CustId = custId }, commandType: CommandType.StoredProcedure);
            walletInfo.Balance = balance;

            return walletInfo;
        }

        public async Task<IEnumerable<TpTransferInfoHistory>>  GetTransferRecent(int custId, int siteId, int topNum)
        {
            await using var conn = GetConnection();
            return await conn.QueryAsync<TpTransferInfoHistory>("Member_TpTransferInfoRecent_Get", new { CustId = custId, SiteId = siteId, TopNum = topNum }, commandType: CommandType.StoredProcedure);
        }
    }

}
