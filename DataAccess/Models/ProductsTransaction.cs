namespace DataAccess.Models
{
    public partial class ProductsTransaction
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public int TransactionID { get; set; }

        public virtual Product Product { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
