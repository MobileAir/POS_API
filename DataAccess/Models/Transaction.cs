using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public partial class Transaction
    {
        
        public Transaction()
        {
            Payments = new HashSet<Payment>();
            ProductsTransactions = new HashSet<ProductsTransaction>();
        }

        public int ID { get; set; }

        public decimal Price { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateTime { get; set; }

        public int CustomerID { get; set; }

        public int EmployeeID { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Employee Employee { get; set; }
        
        public virtual ICollection<Payment> Payments { get; set; }
        
        public virtual ICollection<ProductsTransaction> ProductsTransactions { get; set; }
    }
}
