using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Net.Http.Headers;
using MobileApiServer.Core;
using MobileApiServer.Extensions;
using MobileApiServer.Handlers;
using MobileApiServer.Helpers;
using MobileApiServer.Models;
using MobileApiServer.Properties;
using Newtonsoft.Json;

namespace MobileApiServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtHelpers _jwt;
        private readonly AccountHandler _accountHandler;
        private readonly IStringLocalizer _localizer;

        public AuthController(JwtHelpers jwt, AccountHandler accountHandler, IStringLocalizerFactory factory)
        {
            _jwt = jwt;
            _accountHandler = accountHandler;
            _localizer = factory.Create(typeof(ErrorMessage));
        }

        /// <summary>
        /// Login with siteId
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("")]
        public dynamic SignIn(LoginViewModel login)
        {
            var header = Request.GetTypedHeaders();

            var ip = Tools.GetRemoteIpAddressV4(Request.HttpContext);
            var result = _accountHandler.UserLogin(login.Username, login.Password, login.SiteId, ip.ToString(), 
                header.Referer?.ToString(), header.Headers[HeaderNames.UserAgent],
                out Customer customer, out string token);
            if (result.Success)
            {
                return new InnerResult
                {
                    Data = _jwt.GenerateToken(customer, token)
                };
            }
            #region deal with fail result
            var ir = new InnerResult {Success = false};
            switch (result.Code)
            {
                case (int)ErrorStatus.NotExists:
                case (int)ErrorStatus.WrongPassword:
                    ir.Code = (int)PublicError.UserOrPasswordNotMatch;
                    ir.Message = _localizer[PublicError.UserOrPasswordNotMatch.ToString()];
                    break;
                case (int)ErrorStatus.NotAvailable:
                    ir.Code = (int)PublicError.NotAvailable;
                    ir.Message = _localizer[PublicError.NotAvailable.ToString()];
                    break;
            }
            #endregion
            return ir;
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public dynamic SignOut()
        {
            var ip = Tools.GetRemoteIpAddressV4(Request.HttpContext);
            //old code have "courierAdapter.MyPostAsync" not sure doing what, didn't implement
            var result = _accountHandler.Logout(User.Identity.CustId(), User.Identity.Token(), ip.ToString());
            //success
            if (result)
            {
                return new InnerResult();
            }

            return new InnerResult
            {
                Success = false,
                Code = (int) InnerResultError.OperationFailed,
                Message = _localizer[InnerResultError.OperationFailed.ToString()]
            };
        }

        public class LoginViewModel
        {
            [JsonProperty("username")]
            public string Username { get; set; }
            [JsonProperty("password")]
            public string Password { get; set; }
            [JsonProperty("siteId")]
            public int SiteId { get; set; }
        }
    }
}
