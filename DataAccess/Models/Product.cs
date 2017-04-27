using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public partial class Product
    {
        
        public Product()
        {
            ProductsTransactions = new HashSet<ProductsTransaction>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        
        public virtual ICollection<ProductsTransaction> ProductsTransactions { get; set; }
    }
}
