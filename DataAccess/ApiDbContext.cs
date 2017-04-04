using DataAccess.Models;

namespace DataAccess
{
    using System.Data.Entity;

    public partial class ApiDbContext : DbContext
    {
        public ApiDbContext()
            : base("name=ApiDbContext")
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductsTransaction> ProductsTransactions { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PaymentMethod>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.PaymentMethod)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Transaction>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.Transaction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tokens)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
