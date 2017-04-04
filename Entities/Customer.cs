
using System.Collections.Generic;
namespace Entities
{
    /// <summary>
    /// A Customer can have many Transactions
    /// </summary>
    public partial class Customer
    {
        public Customer()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
    
        
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
