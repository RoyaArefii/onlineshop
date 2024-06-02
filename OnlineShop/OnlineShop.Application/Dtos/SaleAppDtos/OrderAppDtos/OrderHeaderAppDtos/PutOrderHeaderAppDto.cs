using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos
{
    public class PutOrderHeaderAppDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderDate { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
    }
}
