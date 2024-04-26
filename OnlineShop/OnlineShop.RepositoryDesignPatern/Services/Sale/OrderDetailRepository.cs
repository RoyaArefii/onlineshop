using OnlineShopDomain.Aggregates.Sale;
using onlineshop.repositorydesignpatern.frameworks.bases;
using OnlineShop.EFCore;

namespace OnlineShop.RepositoryDesignPatern.Services.Sale
{
    public class OrderDetailRepository : BaseRepository<OnlineShopDbContext, OrderDetail, Guid>
    {
        public OrderDetailRepository(OnlineShopDbContext dbContext) : base(dbContext)
        {
        }
    }
}
