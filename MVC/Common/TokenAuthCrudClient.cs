using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
namespace MVC.Common
{
    public class TokenAuthCrudClient
    {
        /// <summary>
        /// Generic GET API caller
        /// </summary>
        /// <returns></returns>
        public ApiResponse<T> Get<T>(string url, string token, string userAgent) where T : class
        {
            var result = new ApiResponse<T>();
            try
            {
                HttpClient request = null;

                if (!token.IsNullOrWhiteSpace() && !userAgent.IsNullOrWhiteSpace())
                    request = TokenAuthRequestManager.ConfigureRequest(token, userAgent);
                else
                    return new ApiResponse<T>() { ReasonPhrase = "No token or proper user agent were included in the request", StatusCode = HttpStatusCode.ExpectationFailed };

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
                            }
                            else
                            {
                                result.ReasonPhrase = taskWithResponse.Result.ReasonPhrase;
                                result.StatusCode = taskWithResponse.Result.StatusCode;
                            }

                        }
                    }))
                {
                    response.Wait(); // SYNC
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
        /// Generic Web API caller for POST
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="url">Url</param>
        /// <param name="token"></param>
        /// <param name="userAgent"></param>
        /// <param name="o">Object to post</param>
        /// <returns></returns>
        public ApiResponse<T> Post<T>(string url, string token, string userAgent, T o) where T : class
        {
            var result = new ApiResponse<T>();
            try
            {
                HttpClient request = null;

                if (!token.IsNullOrWhiteSpace() && !userAgent.IsNullOrWhiteSpace())
                    request = TokenAuthRequestManager.ConfigureRequest(token, userAgent);
                else
                    return new ApiResponse<T>() { ReasonPhrase = "No token or proper user agent were included in the request", StatusCode = HttpStatusCode.ExpectationFailed };

                //TODO: do check for null bef convert
                var content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
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
                            }
                            else
                            {
                                result.ReasonPhrase = taskWithResponse.Result.ReasonPhrase;
                                result.StatusCode = taskWithResponse.Result.StatusCode;
                            }

                        }

                    }))
                {
                    response.Wait(); // SYNC
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
        /// Async Web API caller for Register Keys
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="url">Url</param>
        /// <param name="token"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public ApiResponse<T> AsyncPost<T>(string url, string token, string userAgent, T o) where T : class
        {
            var result = new ApiResponse<T>();
            try
            {
                HttpClient request = null;

                if (!token.IsNullOrWhiteSpace() && !userAgent.IsNullOrWhiteSpace())
                    request = TokenAuthRequestManager.ConfigureRequest(token, userAgent);
                else
                    return new ApiResponse<T>() { ReasonPhrase = "No token or proper user agent were included in the request", StatusCode = HttpStatusCode.ExpectationFailed };

                //TODO: do check for null bef convert
                var content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
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
                                var data = JsonConvert.DeserializeObject(jsonString.Result);
                                result.Message = data.ToString();
                            }
                            else
                            {
                                result.ReasonPhrase = taskWithResponse.Result.ReasonPhrase;
                                result.StatusCode = taskWithResponse.Result.StatusCode;
                            }

                        }

                    }))
                {
                    // ASYNC
                    //response.Wait(); 
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
        /// <param name="userAgent"></param>
        /// <param name="o">Object to post</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ApiResponse<T> Put<T>(string url, string token, string userAgent, T o) where T : class
        {
            var result = new ApiResponse<T>();
            try
            {
                HttpClient request = null;

                if (!token.IsNullOrWhiteSpace() && !userAgent.IsNullOrWhiteSpace())
                    request = TokenAuthRequestManager.ConfigureRequest(token, userAgent);
                else
                    return new ApiResponse<T>() { ReasonPhrase = "No token or proper user agent were included in the request", StatusCode = HttpStatusCode.ExpectationFailed };

                var content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
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
                                taskWithResponse.Wait();
                                result.Success = true;
                                result.ReasonPhrase = taskWithResponse.Result.ReasonPhrase;
                                result.StatusCode = taskWithResponse.Result.StatusCode;
                            }
                            else
                            {
                                result.ReasonPhrase = taskWithResponse.Result.ReasonPhrase;
                                result.StatusCode = taskWithResponse.Result.StatusCode;
                            }

                        }
                    }))
                {
                    response.Wait(); // SYNC
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
        /// <param name="token"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public ApiResponse<T> Delete<T>(string url, string token, string userAgent) where T : class
        {
            var result = new ApiResponse<T>();
            try
            {
                HttpClient request = null;
                if (!token.IsNullOrWhiteSpace() && !userAgent.IsNullOrWhiteSpace())
                    request = TokenAuthRequestManager.ConfigureRequest(token, userAgent);
                else
                    return new ApiResponse<T>() { ReasonPhrase = "No token or proper user agent were included in the request", StatusCode = HttpStatusCode.ExpectationFailed };

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
                                taskWithResponse.Wait();
                                result.Success = true;
                                result.ReasonPhrase = taskWithResponse.Result.ReasonPhrase;
                                result.StatusCode = taskWithResponse.Result.StatusCode;
                            }
                            else
                            {
                                result.ReasonPhrase = taskWithResponse.Result.ReasonPhrase;
                                result.StatusCode = taskWithResponse.Result.StatusCode;
                            }

                        }


                    }))
                {
                    response.Wait(); // SYNC
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = ex.ToString();

            }
            return result;
        }
    }
}