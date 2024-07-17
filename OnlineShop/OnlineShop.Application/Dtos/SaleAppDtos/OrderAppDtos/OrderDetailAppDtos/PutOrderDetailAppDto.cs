using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos
{
    public class PutOrderDetailAppDto
    {
        public Guid? Id { get; set; }
        public Guid ProductId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public string EntityDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
