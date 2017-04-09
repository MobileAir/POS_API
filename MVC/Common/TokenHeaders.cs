using System;

namespace MVC.Common
{
    public class TokenHeaders
    {
        public string Token { get; set; }
        public DateTime? ExpiresOn { get; set; }
    }
}