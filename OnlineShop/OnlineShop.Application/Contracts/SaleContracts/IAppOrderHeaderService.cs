using OnlineShop.Application.Dtos.SaleAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using OnlineShopDomain.Aggregates.Sale;
using ResponseFramework;

namespace OnlineShop.Application.Contracts.SaleContracts
{
    public interface IAppOrderHeaderService<D>:IApplicationService<OrderHeader , PutOrderHeaderAppDto , GetOrdersAppDto, PostOrder, DeleteOrderDetailAppDtos , Guid>
    {
        Task<IResponse<object>> DeleteOrderDetailAsync(List<D> model);
    }
}
