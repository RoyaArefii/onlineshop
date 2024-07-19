using Microsoft.EntityFrameworkCore;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using OnlineShopDomain.Aggregates.Sale;
using PublicTools.Resources;
using ResponseFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.RepositoryDesignPatern.Frameworks.Contracs.SaleContracts
{
    public interface IOrderDetailRepository :IRepository<OrderDetail , Guid>
    {

    }
}
