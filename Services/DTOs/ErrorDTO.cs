using System;

namespace Services.DTOs
{
    public partial class ErrorDTO
    {
        public int ID { get; set; }
        
        public string Message { get; set; }
        
        public string Controller { get; set; }
        
        public string UserAgent { get; set; }
        
        public string StackTrace { get; set; }
        
        public string Ip { get; set; }
        
        public string TargetResult { get; set; }
        
        public DateTime Timestamp { get; set; }
    }
}
