using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MobileApiServer.Core;
using MobileApiServer.Extensions;
using MobileApiServer.Models;
using MobileApiServer.Properties;
using MobileApiServer.Services;
using Newtonsoft.Json;

namespace MobileApiServer.Controllers
{
    /// <summary>
    /// Get Token from game provider
    /// 1.Saba Sport
    /// doc https://developer.onebook.dev/MobileAPPIntg/Index?PageName=integration N88_develop / n887730
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly ILogger<TokenController> _logger;
        private readonly ITokenService _auth;
        private readonly IWebHostEnvironment _env;
        private readonly IStringLocalizer _localizer;

        public TokenController(ILogger<TokenController> logger, ITokenService auth, IWebHostEnvironment env, IStringLocalizerFactory factory)
        {
            _logger = logger;
            _auth = auth;
            _env = env;
            _localizer = factory.Create(typeof(ErrorMessage));
        }

        /// <summary>
        /// Get Token
        /// </summary>
        /// <param name="tp">third party, default = 1</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<dynamic> GetToken(int tp = 1)
        {
            var uid = GetVendorMemberId(User.Identity.CustId(), User.Identity.SiteId());
            _logger.LogInformation("GetToken:{uid}", uid);
            var result = await _auth.GetToken(uid);
            _logger.LogInformation("Result:{result}", JsonConvert.SerializeObject(result));
            return ResultParser(result);
        }

        /// <summary>
        /// Disable token
        /// </summary>
        /// <param name="tp">third party, default = 1</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<dynamic> Invalidate(int tp = 1)
        {
            var uid = GetVendorMemberId(User.Identity.CustId(), User.Identity.SiteId());
            _logger.LogInformation("Invalidate Token:{uid}", uid);
            
            var result = await _auth.InvalidateToken(uid);
            _logger.LogInformation("Result:{result}", JsonConvert.SerializeObject(result));
            return ResultParser(result);
        }

        [HttpPost("[action]")]
        public async Task<dynamic> VerifyToken(VerifyTokenInput input)
        {
            _logger.LogInformation("VerifyToken:{token}", input.Token);
            var result = await _auth.VerifyToken(input.Token);
            _logger.LogInformation("Result:{result}", JsonConvert.SerializeObject(result));
            return ResultParser(result);
        }

        #region private
        private static readonly string[] SiteName = {"Nova88", "Goal123", "Bwing"};

        /// <summary>
        /// Get custom vendor member id 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private string GetVendorMemberId(int uid, int siteId)
        {
            //TODO should add an SP to get vendor_name from DB
            var result = $"{SiteName[siteId - 1]}_{uid}";
            if (_env.IsDevelopment())
            {
                result = "D" + result;
                //stupid special case
                if (uid == 7 && siteId == 1) result = "D1_7";
            }
            return result;
        }

        /// <summary>
        /// parse saba sports result to our result.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private InnerResult ResultParser(InnerResult input)
        {
            var result = input;
            switch (input.Code)
            {
                case 1:     //System Error/ exception
                    result.Code = (int)PublicError.ExternalException;
                    result.Message = _localizer[PublicError.ExternalException.ToString()];
                    break;
                case 2:     //member not found
                    result.Code = (int) PublicError.TargetNotFound;
                    result.Message = _localizer[PublicError.TargetNotFound.ToString()];
                    break;
                case 9:     //Invalid vendor_id
                    result.Code = (int)PublicError.InvalidTarget;
                    result.Message = _localizer[PublicError.InvalidTarget.ToString()];
                    break;
                case 10:     //System is under maintenance
                    result.Code = (int)PublicError.SystemMaintenance;
                    result.Message = _localizer[PublicError.SystemMaintenance.ToString()];
                    break;
            }
            return result;
        }
        #endregion
    }

    public class VerifyTokenInput
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("tp")]
        public int Tp { get; set; }
    }
}
