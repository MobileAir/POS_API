using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
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
