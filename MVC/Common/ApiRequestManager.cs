using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace MVC.Common
{
    public class ApiRequestManager
    {
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

        /// <summary>
        /// Basic Auth for Web Api Controller: [ApiAuthenticationFilter] . Builds HttpClient object for a given URL and HTTP method and initializes its basic properties 
        /// </summary>
        /// <returns></returns>
        public static HttpClient ConfigureBasicAuthRequest()
        {
            var request = new HttpClient();
            request.BaseAddress = new Uri(GetApiBaseUrl());
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                "YWRtaW46QWRtaW5XZWJBcGlEaQ=="
                //Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{user}:{password}"))
                );
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return request;
        }

        public static HttpClient ConfigureTokenAuthOnlyRequest(string token)
        {
            var request = new HttpClient();
            request.BaseAddress = new Uri(GetApiBaseUrl());
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Add("Token", token);
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return request;
        }

        /// <summary>
        /// Get Token for each Action call if not in session or expired
        /// </summary>
        /// <returns></returns>
        public static TokenHeaders RequestToken()
        {
            var tokenHeaders = new TokenHeaders();
            try
            {
                var request = ConfigureBasicAuthRequest();

                using (
                    var response = request.PostAsync("login", null).ContinueWith((taskWithResponse) =>
                    {

                        if (taskWithResponse != null)
                        {
                            if (taskWithResponse.Status != TaskStatus.RanToCompletion)
                            {
                                throw new Exception(
                                    $"Server error (HTTP {taskWithResponse.Result?.StatusCode}: {taskWithResponse.Exception?.InnerException} : {taskWithResponse.Exception?.Message}).");
                            }
                            else if (taskWithResponse.Result.IsSuccessStatusCode)
                            {
                                var jsonString = taskWithResponse.Result.Content.ReadAsStringAsync();
                                jsonString.Wait();
                                var headers = taskWithResponse.Result.Headers;
                                tokenHeaders.Token = headers.GetValues("Token")?.First();
                                var expiresOn = headers.GetValues("TokenExpiresOn")?.First();
                                if (expiresOn != null)
                                    tokenHeaders.ExpiresOn = DateTime.Parse(expiresOn);
                            }
                            else
                            {
                                throw new Exception($"{taskWithResponse.Result.ReasonPhrase} : {taskWithResponse.Result.StatusCode} ");
                            }

                        }

                    }))
                {
                    response.Wait();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return tokenHeaders;
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