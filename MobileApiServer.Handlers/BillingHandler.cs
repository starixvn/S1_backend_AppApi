using System.Collections.Generic;
using System.Threading.Tasks;
using MobileApiServer.Core;
using MobileApiServer.Models;
using MobileApiServer.Providers;
using MobileApiServer.Services;
using Newtonsoft.Json.Linq;

namespace MobileApiServer.Handlers
{
    public class BillingHandler
    {
        private readonly BillingProvider _billing;
        private readonly TpService _tpService;

        public BillingHandler(BillingProvider billingProvider, TpService tpService)
        {
            _billing = billingProvider;
            _tpService = tpService;
        }

        /// <summary>
        /// Get balance from saba
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<WalletInfo> GetWalletInfo(int uid, int siteId)
        {
            var wallet = _billing.GetWalletInfo(uid, siteId);
            var tpResult = await _tpService.GetBalance(uid);
            //TODO set the description,tpId and tpName if not only saba sport
            if (tpResult.Success)
            {
                var tpBalance = ((JObject)tpResult.Data).ToObject<TpBalance>();
                var w = new TpWallet
                {
                    TpId = 1,
                    TpName = "Sportsbook Wallet",
                    Description = "Include OneRNG, PP and Platinum Suite(AB)",
                    DisplayKey = "lbl_Wallet_Saba",

                    Balance = tpBalance.Balance,
                    Bonus = tpBalance.Bonus,
                    WalletStatus = WalletStatus.Normal,
                    Transfer = true
                };
                wallet.TpWallets= new List<TpWallet>{w};
            }
            return wallet;
        }

        public async Task<InnerResult> Transfer(int uid, decimal amount, TpDirection direction, string host, string ip)
        {
            //TODO log it
            var input = new Dictionary<string, string> {{"Host", host}, {"ClientIP", ip}};
            var result = await _tpService.FundTransfer(uid, amount, direction, input);
            if (result.Success)
            {
                var tpResult = ((JObject)result.Data).ToObject<TransferResponse>();
                if (tpResult.Status == 0 && tpResult.Difference == 0m)
                {
                    var tr = new TransferResult
                    {
                        Status = tpResult.Status,
                        ThirdId = tpResult.ThirdId,
                        Difference = tpResult.Difference
                    };
                    result.Data = tr;
                    return result;
                }
                return new InnerResult { Success = false, Code = (int)InnerResultError.StatusNotSuccess };
            }
            //fail
            return new InnerResult {Success = false, Code = result.Code};
        }
    }

    public class TransferResult
    {
        public int Status { get; set; } = (int)TransferStatus.Pending;
        public string ThirdId { get; set; }
        public decimal Difference { get; set; }
        public decimal Bonus { get; set; }
        public decimal Rollover { get; set; }
        public decimal Turnover { get; set; }
        public int BonusStatus { get; set; }
    }
    public enum TransferStatus
    {
        Success = 0, Fail = 1, Pending = 2, PendingD = 3
    }
}
