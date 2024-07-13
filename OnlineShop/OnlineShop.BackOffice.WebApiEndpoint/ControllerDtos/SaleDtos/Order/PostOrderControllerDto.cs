using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.SaleDtos.Order
{
    public class PostOrderControllerDto
    {
        public PostOrderHeaderAppDto OrderHeader { get; set; }
        public List<PostOrderDetailAppDto> OrderDetails { get; set; }
    }
}
