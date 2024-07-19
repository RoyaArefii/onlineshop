using onlineshop.repositorydesignpatern.frameworks.bases;
using OnlineShop.EFCore;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using OnlineShop.RepositoryDesignPatern.Frameworks.Contracs.JWT;
using OnlineShopDomain.Aggregates.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.RepositoryDesignPatern.Services.JWT
{
    public class JwtTokenManagement : BaseRepository<OnlineShopDbContext, BlackListToken, Guid> , IJwtRepository
    {
        public JwtTokenManagement(OnlineShopDbContext dbContext) : base(dbContext)
        {
        }

    }
}
