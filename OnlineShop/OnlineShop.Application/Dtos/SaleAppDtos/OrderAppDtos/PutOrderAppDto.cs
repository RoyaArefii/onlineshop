using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos
{
    public class PutOrderAppDto
    {
        public PutOrderHeaderAppDto orderHeader { get; set; }
        public List<PutOrderDetailAppDto> orderDetails { get; set; }
    }
}
