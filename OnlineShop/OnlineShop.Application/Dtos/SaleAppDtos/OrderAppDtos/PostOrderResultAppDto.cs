using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using OnlineShopDomain.Aggregates.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos
{
    public class PostOrderResultAppDto
    {
        //public PostOrderHeaderResultDto OrderHeader { get; set; }   
        //public List<PostOrderDetailResultDto> OrderDetails { get; set; }

        public OrderHeader OrderHeader { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
