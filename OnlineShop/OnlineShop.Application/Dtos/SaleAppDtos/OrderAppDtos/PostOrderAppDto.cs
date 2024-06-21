using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using OnlineShopDomain.Aggregates.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos
{
    public class PostOrderAppDto
    {
        public PostOrderHeaderAppDto OrderHeader { get; set; }
        public List<PostOrderDetailAppDto> OrderDetails { get; set; }
        public string UserName { get; set; }
    }
}
