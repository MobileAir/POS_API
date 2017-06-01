namespace DataAccess.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int ID { get; set; }

        public bool IsAdmin { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        public virtual ICollection<Product> Products { get; set; }
    }
}
