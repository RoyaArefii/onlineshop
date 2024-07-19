using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using OnlineShopDomain.Aggregates.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.RepositoryDesignPatern.Frameworks.Contracs.JWT
{
    public interface IJwtRepository:IRepository<BlackListToken,Guid>
    {
    }
}
