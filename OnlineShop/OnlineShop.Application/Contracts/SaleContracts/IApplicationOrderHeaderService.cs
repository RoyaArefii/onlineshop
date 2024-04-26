using OnlineShop.Application.Dtos.SaleAppDtos.OrderHeaderAppDtos;
using OnlineShopDomain.Aggregates.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Contracts.SaleContracts
{
    public interface IApplicationOrderHeaderService:IApplicationService<OrderHeader , PutOrderHeaderAppDto , GetOrderHeaderAppDto , PostOrderHeaderAppDto , DeleteOrderHeaderAppDtos , Guid>
    {
    }
}
