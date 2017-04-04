using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Join table for many to many between Products and Transactions
    /// </summary>
    public partial class ProductTransaction
    {
        [Key, Column(Order = 0)]
        public int ProductID { get; set; }

        [Key, Column(Order = 1)]
        public int TransactionID { get; set; }

        [ForeignKey("ID")]
        public virtual ICollection<Transaction> Transactions { get; set; }

        [ForeignKey("ID")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
