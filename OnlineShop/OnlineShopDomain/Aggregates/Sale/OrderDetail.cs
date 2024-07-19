using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineShopDomain.Aggregates.Sale
{
    public class OrderDetail:MainEntityBase
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid OrderHeaderId { get; set; }
       [JsonIgnore]
        public OrderHeader OrderHeader { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
    }
}
