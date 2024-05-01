using OnlineShopDomain.Aggregates.Sale;
using onlineshop.repositorydesignpatern.frameworks.bases;
using OnlineShop.EFCore;
using OnlineShop.RepositoryDesignPatern.Frameworks.Contracs.SaleContracts;

namespace OnlineShop.RepositoryDesignPatern.Services.Sale
{
    public class OrderDetailRepository : BaseRepository<OnlineShopDbContext, OrderDetail, Guid>, IOrderDetailRepository
    {
        public OrderDetailRepository(OnlineShopDbContext dbContext) : base(dbContext)
        {
        }
    }
}
