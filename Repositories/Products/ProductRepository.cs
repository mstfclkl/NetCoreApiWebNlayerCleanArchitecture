using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Products
{
    internal class ProductRepository(AppDbContext context) : GenericRepository<Product>(context), IProductRepository
    {
        public Task<List<Product>> GetTopPriceProductsAsync(int count)
        {
            return Context.Products.OrderByDescending(x => x.Price).Take(count).ToListAsync();
        }

    }

    
}
