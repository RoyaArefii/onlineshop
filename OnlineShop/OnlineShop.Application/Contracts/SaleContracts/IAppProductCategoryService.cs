using OnlineShopDomain.Aggregates.Sale;
using OnlineShop.Application.Dtos.SaleAppDtos.ProductCategory;


namespace OnlineShop.Application.Contracts.SaleContracts
{
    public interface IAppProductCategoryService :IApplicationService< ProductCategory, PutProductCategoryAppDto,  GetProductCategoryAppDto ,PostProductCategoryAppDto ,DeleteProductCategoryAppDto, Guid>
    {
    }
}
