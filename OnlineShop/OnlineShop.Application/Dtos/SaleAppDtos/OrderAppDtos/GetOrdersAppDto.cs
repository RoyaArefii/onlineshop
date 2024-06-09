using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos
{
    public class GetOrdersAppDto
    {
        public GetOrderHeaderAppDto OrderHeader { get; set; }
        public List<GetOrderDetailAppDto>? OrderDetails { get; set; }
    }
}
