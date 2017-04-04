using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public partial class PaymentMethod
    {
        
        public PaymentMethod()
        {
            Payments = new HashSet<Payment>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
