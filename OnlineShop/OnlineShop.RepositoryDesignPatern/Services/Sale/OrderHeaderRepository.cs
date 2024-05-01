using onlineshop.repositorydesignpatern.frameworks.bases;
using OnlineShop.EFCore;
using OnlineShop.RepositoryDesignPatern.Frameworks.Contracs.SaleContracts;
using OnlineShopDomain.Aggregates.Sale;


namespace OnlineShop.RepositoryDesignPatern.Services.Sale
{
    public class OrderHeaderRepository : BaseRepository<OnlineShopDbContext, OrderHeader, Guid>, IOrderHeaderRepository
    {
        public OrderHeaderRepository(OnlineShopDbContext dbContext) : base(dbContext)
        {
        }
    }
}
