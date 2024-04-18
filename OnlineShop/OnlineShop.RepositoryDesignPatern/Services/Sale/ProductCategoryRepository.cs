using onlineshop.repositorydesignpatern.frameworks.bases;
using OnlineShop.EFCore;
using OnlineShopDomain.Aggregates.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.RepositoryDesignPatern.Services.Sale
{
    public class ProductCategoryRepository : BaseRepository<OnlineShopDbContext, ProductCategory, string>
    {
        public ProductCategoryRepository(OnlineShopDbContext dbContext) : base(dbContext)
        {
        }
    }
}
