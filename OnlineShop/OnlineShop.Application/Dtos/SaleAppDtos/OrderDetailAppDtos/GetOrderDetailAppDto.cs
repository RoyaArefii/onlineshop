using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderDetailAppDtos
{
    public class GetOrderDetailAppDto
    {
        public Guid ProductId { get; set; }
        public Guid OrderHeaderid { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
    }
}
