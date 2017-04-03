namespace MVC.Common
{
    public class WebApiResponse<T> where T : class
    {

        public bool Success { get; set; }
        public T Data { get; set; }
        public string Exception { get; set; }

    }
}