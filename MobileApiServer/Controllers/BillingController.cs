using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MobileApiServer.Core;
using MobileApiServer.Extensions;
using MobileApiServer.Handlers;
using MobileApiServer.Models;
using MobileApiServer.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// Transfer fund 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<dynamic> Transfer(TransferViewModel input)
        {
            var failResult = new InnerResult {Success = false};
            #region check value
            if (input.Amount < 1)
            {
                failResult.Code = (int)PublicError.WrongAmount;
                failResult.Message = _localizer[PublicError.WrongAmount.ToString()];
                return failResult;
            }
            if (input.TpId !=1 ||(input.Direction > 2 || input.Direction < 0))
            {
                failResult.Code = (int)PublicError.WrongVariables;
                failResult.Message = _localizer[PublicError.WrongVariables.ToString()];
                return failResult;
            }
            #endregion
            var ip = Tools.GetRemoteIpAddressV4(Request.HttpContext);
            var host = Request.GetDisplayUrl();
            //result from tp server
            var result = await _billing.Transfer(User.Identity.CustId(), input.Amount,
                (TpDirection) input.Direction, host, ip.ToString());
            //success
            if (result.Success)
            {
                return result;
            }
            #region deal with fail message
            switch (result.Code)
            {
                case (int)InnerResultError.StatusNotSuccess:
                    failResult.Code = (int)PublicError.StatusNotSuccess;
                    failResult.Message = _localizer[PublicError.StatusNotSuccess.ToString()];
                    break;
                case 1000:
                    failResult.Code = (int)PublicError.AmountNotInLimit;
                    failResult.Message = _localizer[PublicError.AmountNotInLimit.ToString()];
                    break;
                case 1201:
                case 2000:
                    failResult.Code = (int)PublicError.WrongAmount;
                    failResult.Message = _localizer[PublicError.WrongAmount.ToString()];
                    break;
                default:
                    failResult.Code = (int)PublicError.OperationFailed;
                    failResult.Message = _localizer[PublicError.OperationFailed.ToString()];
                    break;
            }
            #endregion
            return failResult;
        }
    }

    public class TransferViewModel
    {
        [JsonProperty("tpId")] 
        public int TpId { get; set; } = 1;
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("direction")]
        public int Direction { get; set; }
    }
}
