using OnlineShop.Application.Dtos.SaleAppDtos.ProductAppDtos;
using OnlineShopDomain.Aggregates.Sale;
using ResponseFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Contracts.SaleContracts
{
    public interface IAppProductService :IApplicationService<Product , PutProductAppDto , GetProductAppDto , PostProductAppDto , DeleteProductAppDto, Guid>
    {


    }
}
