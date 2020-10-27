using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MobileApiServer.Core;
using MobileApiServer.Models;
using Newtonsoft.Json;

namespace MobileApiServer.Services
{
    public interface ITokenService
    {
        Task<InnerResult> GetToken(string uid);
        Task<InnerResult> InvalidateToken(string uid);
        Task<InnerResult> VerifyToken(string uid);
    }
    public class SabaService : ITokenService
    {
        private readonly HttpClient _client;
        private readonly IMemoryCache _cache;
        private readonly Uri _url;
        private readonly string _vendorId;

        public SabaService(HttpClient httpClient, IConfiguration config, IMemoryCache memoryCache)
        {
            _client = httpClient;
            _cache = memoryCache;
            _url = new Uri(config["SabaSport:Url"]);
            _vendorId = config["SabaSport:VendorId"];
        }

        /// <summary>
        /// Get token from Sports book
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<InnerResult> GetToken(string uid)
        {
            //get from cache
            //if (_cache.TryGetValue(uid, out string token))
            //{
            //    return new InnerResult { Data = token };
            //}
            var url = new Uri(_url, "/api/GetToken");
            var formData = PostContent(ContentValue(uid));
            var result = await SendPost(url, formData);

            //add token
            if (result.Success)
            {
                var option = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(29));
                string resultToken = result.Data;
                _cache.Set(uid, resultToken, option);
            }
            return result;
        }

        /// <summary>
        /// InvalidateToken Method
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<InnerResult> InvalidateToken(string uid)
        {
            var url = new Uri(_url, "/api/InvalidateToken");
            var formData = PostContent(ContentValue(uid));
            return await SendPost(url, formData);
        }

        /// <summary>
        /// VerifyToken Method
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<InnerResult> VerifyToken(string token)
        {
            var url = new Uri(_url, "/api/VerifyToken");
            var formData = PostContent(new Dictionary<string, string>
            {
                {"vendor_id", _vendorId},
                {"token", token}
            });
            return await SendPost(url, formData);
        }

        /// <summary>
        /// Post to target
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formData"></param>
        /// <returns></returns>
        protected async Task<InnerResult> SendPost(Uri url, FormUrlEncodedContent formData)
        {
            using var response = await _client.PostAsync(url, formData);
            //Post fail
            if (!response.IsSuccessStatusCode)
                return new InnerResult
                {
                    Success = false,
                    Code = (int)response.StatusCode,
                    Message = response.RequestMessage.ToString()
                };
            #region success call
            var content = response.Content;
            var text = await content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SabaResult>(text);
            
            
            //response empty
            if (result == null)
                return new InnerResult
                {
                    Success = false,
                    Code = (int)InnerResultError.NullOrEmpty,
                    Message = InnerResultError.NullOrEmpty.ToString()
                };

            var ir = new InnerResult();

            //if have data 
            if (result.Data != null)
            {
                ir.Data = result.Data;
            }

            //success
            if (result.ErrorCode == 0)
            {
                ir.Message = result.Message;
                return ir;
            }

            //query fail, no token
            return new InnerResult
            {
                Success = false,
                Code = result.ErrorCode,
                Message = result.Message
            };
            #endregion
        }

        /// <summary>
        /// default content
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        protected Dictionary<string, string> ContentValue(string uid)
        {
            return new Dictionary<string, string>
            {
                {"vendor_id", _vendorId},
                {"vendor_member_id", uid}
            };
        }

        /// <summary>
        /// set post header
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        FormUrlEncodedContent PostContent(Dictionary<string, string> dict)
        {
            var formData = new FormUrlEncodedContent(dict);
            formData.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };
            return formData;
        }
    }

    public class SabaResult
    {
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("Data")]
        public dynamic Data { get; set; }
    }

    public class SabaVerifyResult
    {
        [JsonProperty("vendor_member_id")]
        public string VendorMemberId { get; set; }
    }
}
