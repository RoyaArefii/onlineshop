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
        //forign Key
        public Guid ProductId { get; set; }
        //Navigation Property 
        public Product Product { get; set; }
        //forign Key
        public Guid OrderHeaderId { get; set; }
        //Navigation Property
       [JsonIgnore]
        public OrderHeader OrderHeader { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
    }
}
