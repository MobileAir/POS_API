namespace Entities
{
    /// <summary>
    /// A Payment as one Transaction
    /// A Payment as one Payment Method
    /// </summary>
    public partial class Payment
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime DateTime { get; set; }
        public int PaymentMethodID { get; set; }
        public int TransactionID { get; set; }
    
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual Transaction Transaction { get; set; } 
    }
}
