using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace MVC.Common
{
    public class WebApiClient
    {
        /// <summary>
        /// Retrieves REST API base URL from configuration file
        /// </summary>
        /// <returns></returns>
        private string GetRestApiBaseUrl()
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
        private HttpClient ConfigureBasicAuthRequest()
        {
            var request = new HttpClient();
            request.BaseAddress = new Uri(GetRestApiBaseUrl());
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                "YWRtaW46QWRtaW5XZWJBcGlEaQ=="
                //Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{user}:{password}"))
                );
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return request;
        }

        private HttpClient ConfigureTokenAuthOnlyRequest(string token)
        {
            var request = new HttpClient();
            request.BaseAddress = new Uri(GetRestApiBaseUrl());
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Add("Token", token);
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return request;
        }

        /// <summary>
        /// Get Token for each Action call if not in session or expired
        /// </summary>
        /// <returns></returns>
        public TokenHeaders RequestToken()
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
                                if(expiresOn != null)
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

        /// <summary>
        /// Generic GET API caller
        /// </summary>
        /// <returns></returns>
        public WebApiResponse<T> Get<T>(string url, string token = null) where T : class
        {
            var result = new WebApiResponse<T>();
            try
            {
                HttpClient request = null;
                //if (!token.IsNullOrWhiteSpace())
                //    request = ConfigureTokenAuthOnlyRequest(token);
                //else
                //    request = ConfigureBasicAuthRequest();

                if (!token.IsNullOrWhiteSpace())
                    request = ConfigureTokenAuthOnlyRequest(token);
                else
                    return new WebApiResponse<T>() {ReasonPhrase = "No token included in the request", StatusCode = HttpStatusCode.ExpectationFailed};

                using (
                    var response = request.GetAsync(url).ContinueWith((taskWithResponse) =>
                    {

                        if (taskWithResponse != null)
                        {
                            if (taskWithResponse.Status != TaskStatus.RanToCompletion)
                            {
                                result.Success = false;
                                result.Exception =
                                    $"Server error (HTTP {taskWithResponse.Result?.StatusCode}: {taskWithResponse.Exception?.InnerException} : {taskWithResponse.Exception?.Message}).";
                            }
                            else if (taskWithResponse.Result.IsSuccessStatusCode)
                            {
                                var jsonString = taskWithResponse.Result.Content.ReadAsStringAsync();
                                jsonString.Wait();
                                var data = JsonConvert.DeserializeObject<T>(jsonString.Result);
                                result.Success = true;
                                result.Data = data;
                                taskWithResponse.Wait(); // TODO no Wait and keep async...?
                            }
                            else
                            {
                                result.ReasonPhrase = taskWithResponse.Result.ReasonPhrase;
                                result.StatusCode = taskWithResponse.Result.StatusCode;
                            }

                        }
                    }))
                {
                    response.Wait();
                }
                //var response = await request.GetAsync(url);
                //var res = await response.Content.ReadAsStringAsync();
                //var data = JsonConvert.DeserializeObject<T>(res);
                //return data;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// Generic Web API caller for POST
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="url">Url</param>
        /// <param name="o">Object to post</param>
        /// <returns></returns>
        public WebApiResponse<T> Post<T>(string url, T o) where T : class
        {
            var result = new WebApiResponse<T>();
            try
            {
                var request = ConfigureBasicAuthRequest();

                //TODO: do check for null bef convert
                StringContent content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
                using (
                    var response = request.PostAsync(url, content).ContinueWith((taskWithResponse) =>
                    {

                        if (taskWithResponse != null)
                        {
                            if (taskWithResponse.Status != TaskStatus.RanToCompletion)
                            {
                                result.Success = false;
                                result.Exception =
                                    $"Server error (HTTP {taskWithResponse.Result?.StatusCode}: {taskWithResponse.Exception?.InnerException} : {taskWithResponse.Exception?.Message}).";
                            }
                            else if (taskWithResponse.Result.IsSuccessStatusCode)
                            {
                                var jsonString = taskWithResponse.Result.Content.ReadAsStringAsync();
                                jsonString.Wait();
                                var data = JsonConvert.DeserializeObject<T>(jsonString.Result);
                                result.Success = true;
                                result.Data = data;
                                taskWithResponse.Wait(); // TODO no Wait and keep async...? //SetSessionToken(taskWithResponse);
                            }

                        }

                    }))
                {
                    response.Wait();
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = ex.ToString();

            }
            return result;
        }

        /// <summary>
        /// Generic Web API caller for PUT
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="url">Url</param>
        /// <param name="o">Object to post</param>
        /// <returns></returns>
        public WebApiResponse<T> Put<T>(string url, T o) where T : class
        {
            var result = new WebApiResponse<T>();
            try
            {
                var request = ConfigureBasicAuthRequest();

                StringContent content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
                using (
                    var response = request.PutAsync(url, content).ContinueWith((taskWithResponse) =>
                    {

                        if (taskWithResponse != null)
                        {
                            if (taskWithResponse.Status != TaskStatus.RanToCompletion)
                            {
                                result.Success = false;
                                result.Exception =
                                    $"Server error (HTTP {taskWithResponse.Result?.StatusCode}: {taskWithResponse.Exception?.InnerException} : {taskWithResponse.Exception?.Message}).";
                            }
                            else if (taskWithResponse.Result.IsSuccessStatusCode)
                            {
                                //var jsonString = taskWithResponse.Result.Content.ReadAsStringAsync();
                                //jsonString.Wait();
                                //var data = JsonConvert.DeserializeObject<T>(jsonString.Result);
                                taskWithResponse.Wait(); // TODO use async/await?
                                result.Success = true;
                                //result.Data = data;
                                
                            }

                        }
                    }))
                {
                    response.Wait();
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = ex.ToString();

            }
            return result;
        }

        /// <summary>
        /// Generic Web API caller for DELETE
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns></returns>
        public WebApiResponse<T> Delete<T>(string url) where T : class
        {
            var result = new WebApiResponse<T>();
            try
            {
                var request = ConfigureBasicAuthRequest();
                using (
                    var response = request.DeleteAsync(url).ContinueWith((taskWithResponse) =>
                    {

                        if (taskWithResponse != null)
                        {
                            if (taskWithResponse.Status != TaskStatus.RanToCompletion)
                            {
                                result.Success = false;
                                result.Exception =
                                    $"Server error (HTTP {taskWithResponse.Result?.StatusCode}: {taskWithResponse.Exception?.InnerException} : {taskWithResponse.Exception?.Message}).";
                            }
                            else if (taskWithResponse.Result.IsSuccessStatusCode)
                            {
                                //var jsonString = taskWithResponse.Result.Content.ReadAsStringAsync();
                                //jsonString.Wait();
                                //var data = JsonConvert.DeserializeObject<T>(jsonString.Result);
                                taskWithResponse.Wait();
                                result.Success = true;
                                //result.Data = data;
                                
                            }

                        }


                    }))
                {
                    response.Wait();
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = ex.ToString();

            }
            return result;
        }

        private bool CheckSessionTokens()
        {
            var valid = false;
            if (System.Web.HttpContext.Current.Session["TokenExpiry"] != null && System.Web.HttpContext.Current.Session["Token"] != null && System.Web.HttpContext.Current.Session["TokenIssuedOn"] != null)
            {
                var now = DateTime.Now;

                var issuedOn = DateTime.ParseExact(System.Web.HttpContext.Current.Session["TokenIssuedOn"].ToString(), "F", null);

                var expiresOn = issuedOn.AddSeconds(Convert.ToDouble(System.Web.HttpContext.Current.Session["TokenExpiry"].ToString()));
                if (expiresOn > now)
                    valid = true;
                else // if expired
                {
                    System.Web.HttpContext.Current.Session["TokenExpiry"] = null;
                    System.Web.HttpContext.Current.Session["Token"] = null;
                    System.Web.HttpContext.Current.Session["TokenIssuedOn"] = null;
                }
            }

            return valid;
        }

        private string GetSessionToken()
        {
            var token = "";
            if (System.Web.HttpContext.Current.Session["Token"] != null)
            {
                token = System.Web.HttpContext.Current.Session["Token"].ToString();
            }
            return token;
        }

        /// <summary>
        /// http context is null if called from inside the continue with task
        /// </summary>
        /// <param name="response"></param>
        private void SetSessionTokens(Task<HttpResponseMessage> response)
        {
            try
            {
                var headers = response.Result.Headers;
                IEnumerable<string> values;
                
                if (headers.TryGetValues("Token", out values))
                {
                    string token = values.First();
                    System.Web.HttpContext.Current.Session["Token"] = token; 
                }
                if (headers.TryGetValues("TokenExpiry", out values))
                {
                    string expiry = values.First();
                    System.Web.HttpContext.Current.Session["TokenExpiry"] = expiry; 
                }
                if (headers.TryGetValues("TokenIssuedOn", out values))
                {
                    string issued = values.First();
                    HttpContext.Current.Session["TokenIssuedOn"] = issued;
                }
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}