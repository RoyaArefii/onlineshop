using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos
{
    public class GetOrdersAppDto
    {
        public Guid OrderHeaderId { get; set; }
        public Guid OrderDetailId { get; set; }
    }
}
