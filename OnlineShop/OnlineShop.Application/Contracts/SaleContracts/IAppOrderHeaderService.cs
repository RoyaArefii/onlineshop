using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using OnlineShopDomain.Aggregates.Sale;
using ResponseFramework;

namespace OnlineShop.Application.Contracts.SaleContracts
{
    public interface IAppOrderHeaderService<D>:IApplicationService<OrderHeader , PutOrderAppDto , GetOrdersAppDto, PostOrderAppDto, DeleteOrderDetailAppDtos , Guid>
    {
        Task<IResponse<object>> DeleteOrderDetailAsync(List<D> model);
    }
}
