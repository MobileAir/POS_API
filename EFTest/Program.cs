
using System;
using System.Data.Entity;
using System.Linq;

namespace EFTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = new DataAccess.ApiDbContext();

            var prods = ctx.Products;

            foreach (var product in prods)
            {
                Console.WriteLine(product.Name);
            }

            var prodTrans = ctx.ProductsTransactions;

            foreach (var pt in prodTrans)
            {
                Console.WriteLine(pt.Transaction.Customer.Name);
            }

            Console.ReadLine();
        }
    }
}
