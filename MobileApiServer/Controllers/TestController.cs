//using System.Linq;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MobileApiServer.Extensions;

//namespace MobileApiServer.Controllers
//{
//    [Authorize]
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TestController : ControllerBase
//    {
//        #region for test , remove when ready
//#if DEBUG
//        [HttpGet]
//        public IActionResult GetClaims()
//        {
//            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
//        }

//        [HttpGet]
//        public IActionResult GetUserName()
//        {
//            return Ok(User.Identity.Name);
//        }

//        [HttpGet]
//        public IActionResult GetLangId()
//        {
//            return Ok(User.Identity.LangId());
//        }

//        [HttpGet]
//        public IActionResult GetUniqueId()
//        {
//            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
//            return Ok(jti.Value);
//        }
//#endif
//#endregion
//    }
//}
