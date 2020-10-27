using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace MobileApiServer.Core
{
    public class Tools
    {
        /// <summary>
        /// Get MD5 hash from string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Encrypt(string input)
        {
            using var md5 = MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Get client ip
        /// </summary>
        /// <param name="context"></param>
        /// <param name="allowForwarded"></param>
        /// <returns></returns>
        public static IPAddress GetRemoteIpAddress(HttpContext context, bool allowForwarded = true)
        {
            if (allowForwarded)
            {
                // if you are allowing these forward headers, please ensure you are restricting context.Connection.RemoteIpAddress
                // to cloud flare ips: https://www.cloudflare.com/ips/
                var header = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(header) && IPAddress.TryParse(header, out IPAddress ip))
                {
                    return ip;
                }
            }
            return context.Connection.RemoteIpAddress;
        }

        /// <summary>
        /// Get client ip and turn to IPv4
        /// </summary>
        /// <param name="context"></param>
        /// <param name="allowForwarded"></param>
        /// <returns></returns>
        public static IPAddress GetRemoteIpAddressV4(HttpContext context, bool allowForwarded = true)
        {
            if (allowForwarded)
            {
                // if you are allowing these forward headers, please ensure you are restricting context.Connection.RemoteIpAddress
                // to cloud flare ips: https://www.cloudflare.com/ips/
                var header = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(header) && IPAddress.TryParse(header, out IPAddress ip))
                {
                    return ip.MapToIPv4();
                }
            }
            return context.Connection.RemoteIpAddress.MapToIPv4();
        }
    }
}
