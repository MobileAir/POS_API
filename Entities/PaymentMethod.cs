using System.Collections.Generic;
namespace Entities
{
    /// <summary>
    /// A Payment Method can have many Payments
    /// </summary>
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            this.Payments = new HashSet<Payment>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
