using onlineshop.repositorydesignpatern.frameworks.bases;
using OnlineShop.EFCore;
using OnlineShopDomain.Aggregates.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.RepositoryDesignPatern.Services.UserManagement
{
    public class UserRepository : BaseRepository<OnlineShopDbContext, AppUser, string>
    {
        public UserRepository(OnlineShopDbContext dbContext) : base(dbContext)
        {
        }
    }
}
