using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace MVC.Attributes
{
    // Not in use... moved to login action
    public class ApiTokenAuthorizeAttribute : AuthorizeAttribute
    {
        private const string SecurityToken = "Token";
        private const string ClientUserAgent = "Client-User-Agent";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }

            HandleUnauthorizedRequest(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }

        private bool Authorize(AuthorizationContext actionContext)
        {
            var auth = "false";
            try
            {
                var token = System.Web.HttpContext.Current.Session[SecurityToken];

                if (token == null || token.ToString().IsNullOrWhiteSpace())
                    return false;

                var client = new HttpClient { BaseAddress = new Uri("http://localhost:62806/") };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add(ClientUserAgent, actionContext.RequestContext.HttpContext.Request.UserAgent);
                client.DefaultRequestHeaders.Add(SecurityToken, token.ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var route = "token/validate";
                StringContent content = new StringContent(JsonConvert.SerializeObject(token.ToString()), Encoding.UTF8, "application/json");
                using (
                    var response = client.PostAsync(route, content).ContinueWith((taskWithResponse) =>
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
                                var data = JsonConvert.DeserializeObject(jsonString.Result);

                                auth = data.ToString();
                            }
                        }

                    }))
                {
                    response.Wait();
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return bool.Parse(auth);
        }
    }
}