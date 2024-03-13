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
    public class RoleRepository : BaseRepository<OnlineShopDbContext, AppRole, string>
    {
        public RoleRepository(OnlineShopDbContext dbContext) : base(dbContext)
        {
        }
    }
}
