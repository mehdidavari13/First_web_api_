using FirstWebAPI.Core.Context;
using FirstWebAPI.Core.Contracts;
using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
