using onlineshop.repositorydesignpatern.frameworks.bases;
using OnlineShop.EFCore;
using OnlineShopDomain.Aggregates.Sale;


namespace OnlineShop.RepositoryDesignPatern.Services.Sale
{
    public class OrderHeaderRepository : BaseRepository<OnlineShopDbContext, OrderHeader, Guid>
    {
        public OrderHeaderRepository(OnlineShopDbContext dbContext) : base(dbContext)
        {
        }
    }
}
