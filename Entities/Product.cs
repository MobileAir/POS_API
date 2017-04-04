using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Product can have many Transactions
    /// </summary>
    public partial class Product
    {
        public Product()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
    
        //[ForeignKey("ProductID")]
        public virtual ICollection<Transaction> Transactions { get; set; }

        //public virtual ProductTransaction ProductTransaction { get; set; }
    }
}
