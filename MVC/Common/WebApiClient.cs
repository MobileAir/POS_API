using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
                baseUrl = ConfigurationManager.AppSettings["RestApiBaseUrl"] ?? "Not Found";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading app settings: " + ex);
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

        /// <summary>
        /// Token Authorization for web api controller decorate : [AuthorizationRequired] -> this force the use of valid token
        /// </summary>
        /// <returns></returns>
        private HttpClient ConfigureTokenRequest()
        {
            if (!CheckSessionTokens())
            {
                // fail as well Race condition shared ressource. Cannot lock on session
                var session = System.Web.HttpContext.Current.Session;
                //RequestToken();
                var request = ConfigureBasicAuthRequest();
                var response = request.PostAsync("login", null).ContinueWith((taskWebResp) =>
                {
                    try
                    {
                        var headers = taskWebResp.Result.Headers;
                        IEnumerable<string> values;

                        if (headers.TryGetValues("Token", out values))
                        {
                            string token = values.First();
                            session["Token"] = token;
                        }
                        if (headers.TryGetValues("TokenExpiry", out values))
                        {
                            string expiry = values.First();
                            session["TokenExpiry"] = expiry;
                        }
                        if (headers.TryGetValues("TokenIssuedOn", out values))
                        {
                            string issued = values.First();
                            session["TokenIssuedOn"] = issued;
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
                });
                response.Wait();
                if (response.Status == TaskStatus.RanToCompletion)
                {
                    var req = new HttpClient { BaseAddress = new Uri(GetRestApiBaseUrl()) };
                    req.DefaultRequestHeaders.Add("Token", session["Token"].ToString()); // fail as well Race condition shared ressource. Cannot lock on session.//GetSessionToken()); // RACE COND WOULD PROB MAKE THIS call with "" then resp 401 Un-Auth
                    req.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    return request;
                }
            }

            var r = new HttpClient { BaseAddress = new Uri(GetRestApiBaseUrl()) };
            r.DefaultRequestHeaders.Add("Token", GetSessionToken()); // RACE COND WOULD PROB MAKE THIS call with "" then resp 401 Un-Auth
            r.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return r;
        }

        /// <summary>
        /// WEb APi controller with Atribvute [AuthorizationRequired], below called from Singleton BaseController
        /// HAve an issue where if Token expires, how to call login, store token then finish orghinal call...seems to be a  race condition
        /// </summary>
        public void RequestToken()
        {
            var request = ConfigureBasicAuthRequest();
            var response = request.PostAsync("login", null).ContinueWith(SetSessionTokens);
            response.Wait();
        }

        /// <summary>
        /// Generic Web API caller
        /// </summary>
        /// <returns></returns>
        public WebApiResponse<T> Get<T>(string url) where T : class
        {
            var result = new WebApiResponse<T>();
            try
            {
                var request = ConfigureBasicAuthRequest();
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
                                taskWithResponse.Wait(); // TODO no Wait and keep async...?
                                                         //SetSessionToken(taskWithResponse);
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