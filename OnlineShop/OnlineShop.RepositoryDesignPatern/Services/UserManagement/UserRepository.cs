using onlineshop.repositorydesignpatern.frameworks.bases;
using OnlineShop.EFCore;
using OnlineShopDomain.Aggregates.UserManagement;

namespace OnlineShop.RepositoryDesignPatern.Services.UserManagement
{
    public class UserRepository : BaseRepository<OnlineShopDbContext, AppUser, string>
    {
        public UserRepository(OnlineShopDbContext dbContext) : base(dbContext)
        {
        }
    }
}
