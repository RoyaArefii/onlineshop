using onlineshop.repositorydesignpatern.frameworks.bases;
using OnlineShop.EFCore;
using OnlineShop.RepositoryDesignPatern.Frameworks.Contracs.SaleContracts;
using OnlineShopDomain.Aggregates.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.RepositoryDesignPatern.Services.Sale
{
    public class ProductRepository : BaseRepository<OnlineShopDbContext, Product, Guid>, IProductRepository
    {
        public ProductRepository(OnlineShopDbContext dbContext) : base(dbContext)
        {
        }
    }
}
