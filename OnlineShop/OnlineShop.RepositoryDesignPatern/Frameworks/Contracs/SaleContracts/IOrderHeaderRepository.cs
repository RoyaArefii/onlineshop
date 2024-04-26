using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using OnlineShopDomain.Aggregates.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.RepositoryDesignPatern.Frameworks.Contracs.SaleContracts
{
    public interface IOrderHeaderRepository :IRepository<OrderHeader , Guid>
    {
    }
}
