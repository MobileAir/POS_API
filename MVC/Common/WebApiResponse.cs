using System.Net;

namespace MVC.Common
{
    public class WebApiResponse<T> where T : class
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Exception { get; set; }
        public string Token { get; set; }
        public string ReasonPhrase { get; set; }
        public HttpStatusCode StatusCode { get; set; }

    }
}