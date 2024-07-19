using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using OnlineShopDomain.Aggregates.Sale;
using ResponseFramework;

namespace OnlineShop.Application.Contracts.SaleContracts
{
    public interface IAppOrderService<U ,T>:IApplicationService<OrderHeader , PutOrderAppDto , GetOrdersAppDto, PostOrderAppDto, DeleteOrderAppDto , Guid> 
    {
        Task<IResponse<List<T>>> GetUsersOrder(U model);
    }
}
