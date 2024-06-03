using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos
{
    public class GetOrderHeaderAppDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderDate { get; set; }
        public string SellerId { get; set; }
        public string BuyerId { get; set; }


    }
}
