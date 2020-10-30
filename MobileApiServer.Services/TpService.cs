using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        
        /// <summary>
        /// Get saba balance from tp server
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tpId"></param>
        /// <returns></returns>
        public async Task<InnerResult> GetBalance(int uid, int tpId=1)
        {
            var url = new Uri(_url, "/api/GetBalance");
            var body = new TpInfo{CustId = uid,TpId = tpId };
            var result = await SendPost(url, body);
            return result;
        }

        /// <summary>
        /// Transfer fund between server
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="amount"></param>
        /// <param name="direction"></param>
        /// <param name="input"></param>
        /// <param name="tpId"></param>
        /// <returns></returns>
        public async Task<InnerResult> FundTransfer(int uid, decimal amount, TpDirection direction, Dictionary<string, string> input, int tpId = 1)
        {
            //TODO log it
            var url = new Uri(_url, "/api/Transfer");
            var body = new TpTransfer
            {
                CustId = uid, 
                TpId = tpId,
                Amount = amount,
                Direction = direction,
                Inputs = input
            };
            var result = await SendPost(url, body);
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
            var result = JsonConvert.DeserializeObject<TpResponse>(text);

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
    #region related model
    public class TpBase
    {
        public int TpId { set; get; }
        public int CustId { set; get; }
        public Dictionary<string, string> Inputs { set; get; } = null;
    }

    public class TpInfo : TpBase
    {
        public string LangId { set; get; } = "en";
        public int AppType { set; get; } = 0;
    }

    public class TpResponse
    {
        [JsonProperty("data")]
        public dynamic Data { get; set; }
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("rawErrorCode")]
        public string RawErrorCode { get; set; }
        [JsonProperty("rawMessage")]
        public string RawMessage { get; set; }
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

    public class TpTransfer : TpBase
    {
        public decimal Amount { set; get; }
        public TpDirection Direction { set; get; }
    }

    public class TransferResponse
    {
        public int Status { get; set; }
        public string ThirdId { get; set; }
        public decimal Difference { get; set; }
    }
    #endregion
}
