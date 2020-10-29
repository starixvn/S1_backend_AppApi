using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MobileApiServer.Core;
using MobileApiServer.Extensions;
using MobileApiServer.Handlers;
using MobileApiServer.Models;
using MobileApiServer.Properties;

namespace MobileApiServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BillingController : ControllerBase
    {
        private readonly BillingHandler _billing;
        private readonly IStringLocalizer _localizer;
        public BillingController(BillingHandler billingHandler, IStringLocalizerFactory factory)
        {
            _billing = billingHandler;
            _localizer = factory.Create(typeof(ErrorMessage));
        }

        /// <summary>
        /// Get User Wallet
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<dynamic> WalletInfo()
        {
            var result = await _billing.GetWalletInfo(User.Identity.CustId(), User.Identity.SiteId());
            //success
            if (result != null)
            {
                return new InnerResult {Data = result};
            }
            return new InnerResult
            {
                Code = (int)PublicError.InternalException,
                Message = _localizer[PublicError.InternalException.ToString()]
            };
        }
    }
}
