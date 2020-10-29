using System.Collections.Generic;
using System.Threading.Tasks;
using MobileApiServer.Core;
using MobileApiServer.Models;
using MobileApiServer.Providers;
using MobileApiServer.Services;

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
                var tpBalance = (TpBalance)tpResult.Data;
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
    }
}
