using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos
{
    public class PutOrderDetailAppDto
    {
        public Guid ProductId { get; set; }
        public Guid OrderHeaderId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
    }
}
