using System.Collections.Generic;
namespace Entities
{
    /// <summary>
    /// Each Transaction can have Many Payment.
    /// Each Transaction can have Many Products.
    /// A Transaction have One Employee.
    /// A Transaction have One Customer.
    /// </summary>
    public partial class Transaction
    {
        
        public Transaction()
        {
            this.Payments = new HashSet<Payment>();
            this.Products = new HashSet<Product>();
        }
    
        public int ID { get; set; }
        public decimal Price { get; set; }
        public System.DateTime DateTime { get; set; }
        public int CustomerID { get; set; }
        public int EmployeeID { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
