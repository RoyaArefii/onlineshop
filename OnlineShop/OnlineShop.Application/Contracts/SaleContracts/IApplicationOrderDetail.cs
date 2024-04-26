using OnlineShop.Application.Dtos.SaleAppDtos.OrderDetailAppDtos;
using OnlineShopDomain.Aggregates.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Contracts.SaleContracts
{
    public interface IApplicationOrderDetail:IApplicationService<OrderDetail , PutOrderDetailAppDto , GetOrderDetailAppDto , PostOrderDetailAppDto , DeleteOrderDetailAppDto , Guid>
    {
    }
}
