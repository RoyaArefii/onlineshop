using OnlineShopDomain.Aggregates.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos
{
    public class PostOrderHeaderAppDto
    {
        public string Code { get; set; }
        public string Title { get; set; }
        //public DateTime OrderDate { get; set; }
        public string SellerId { get; set; }
        //public string BuyerId { get; set; }
        //public List<OrderDetail> ? OrderDetails { get; set; }

        /// <summary>
        /// prop for detail
        /// </summary>
        public string? EntityDescription { get; set; }
        //public decimal Quantity { get; set; }
       // public Guid ProductId { get; set; }

    }
}
