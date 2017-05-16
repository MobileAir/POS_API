namespace EFTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductsTransaction
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public int TransactionID { get; set; }

        public virtual Product Product { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
