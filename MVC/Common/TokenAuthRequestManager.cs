using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace MVC.Common
{
    public class TokenAuthRequestManager
    {
        private const string SecurityToken = "Token";
        private const string ClientUserAgent = "Client-User-Agent";

        /// <summary>
        /// Retrieves REST API base URL from configuration file
        /// </summary>
        /// <returns></returns>
        public static string GetApiBaseUrl()
        {
            string baseUrl = "";
            try
            {
                baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "Not Found";
            }
            catch (Exception ex)
            {
                // Log...
            }
            return baseUrl;
        }

        public static HttpClient ConfigureRequest(string token, string userAgent)
        {
            var request = new HttpClient();
            request.BaseAddress = new Uri(GetApiBaseUrl());
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Add(ClientUserAgent, userAgent);
            request.DefaultRequestHeaders.Add(SecurityToken, token);
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return request;
        }

        public static string GetIP(HttpRequestBase request)
        {
            string ip = request.Headers["X-Forwarded-For"]; // AWS compatibility

            if (string.IsNullOrEmpty(ip))
            {
                ip = request.UserHostAddress;
            }

            return ip;
        }
    }
}