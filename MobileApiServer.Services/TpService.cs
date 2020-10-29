using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MobileApiServer.Core;
using MobileApiServer.Models;
using Newtonsoft.Json;

namespace MobileApiServer.Services
{
    public class TpService
    {
        private readonly HttpClient _client;
        private readonly Uri _url;

        public TpService(HttpClient httpClient, IConfiguration config)
        {
            _client = httpClient;
            _url = new Uri(config["TpService"]);
        }
        
        public async Task<InnerResult> GetBalance(int uid, int tpId=1)
        {
            var url = new Uri(_url, "/api/GetBalance");
            var body = new TpInfo{CustId = uid,TpId = tpId };
            var result = await SendPost(url, body);

            //add token
            if (result.Success)
            {
                //string resultToken = result.Data;
            }
            return result;
        }

        protected async Task<InnerResult> SendPost(Uri url,  dynamic body)
        {
            var postBody = new StringContent(JsonConvert.SerializeObject(body),Encoding.UTF8, "application/json");
            
            using var response = await _client.PostAsync(url, postBody);
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
            var result = JsonConvert.DeserializeObject<BalanceResponse>(text);

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
    }

    public class TpInfo
    {
        public int TpId { set; get; }
        public int CustId { set; get; }
        public string LangId { set; get; } = "en";
        public int AppType { set; get; } = 0;
        public Dictionary<string, string> Inputs { set; get; } = null;
    }

    public class TpResponse
    {
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("rawErrorCode")]
        public string RawErrorCode { get; set; }

        [JsonProperty("rawMessage")]
        public string RawMessage { get; set; }
    }

    public class BalanceResponse : TpResponse
    {
        [JsonProperty("data")]
        public TpBalance Data { get; set; }
    }

    public class TpBalance
    {
        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("bonus")]
        public decimal Bonus { get; set; }

        [JsonProperty("outstanding")]
        public decimal Outstanding { get; set; }

        [JsonProperty("currency")]
        public byte Currency { get; set; }
    }
}
