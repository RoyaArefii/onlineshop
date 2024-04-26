﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Guid OrderHeaderid { get; set; }
        //Navigation Property 
        public OrderHeader OrderHeader { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
    }
}
