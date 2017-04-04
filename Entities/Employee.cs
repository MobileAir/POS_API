using System.Collections.Generic;
namespace Entities
{
    /// <summary>
    /// An Employee can have many Transactions
    /// </summary>
    public partial class Employee
    {
        public Employee()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
    

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
