namespace EFTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Error
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        [StringLength(50)]
        public string Controller { get; set; }

        [StringLength(500)]
        public string UserAgent { get; set; }

        public string StackTrace { get; set; }

        [StringLength(100)]
        public string Ip { get; set; }

        [StringLength(200)]
        public string TargetResult { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Timestamp { get; set; }
    }
}
