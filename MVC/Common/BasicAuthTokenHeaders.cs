using System;

namespace MVC.Common
{
    public class BasicAuthTokenHeaders
    {
        public string Token { get; set; }
        public DateTime? ExpiresOn { get; set; }
    }
}