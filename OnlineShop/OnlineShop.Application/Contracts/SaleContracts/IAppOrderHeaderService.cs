using OnlineShop.Application.Dtos.SaleAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using OnlineShopDomain.Aggregates.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Contracts.SaleContracts
{
    public interface IAppOrderHeaderService:IApplicationService<OrderHeader , PutOrderHeaderAppDto , GetOrderHeaderAppDto , PostOrder, DeleteOrderHeaderAppDtos , Guid>
    {
    }
}
