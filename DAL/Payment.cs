namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Payment
    {
        public int ID { get; set; }

        public decimal Amount { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateTime { get; set; }

        public int PaymentMethodID { get; set; }

        public int TransactionID { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
