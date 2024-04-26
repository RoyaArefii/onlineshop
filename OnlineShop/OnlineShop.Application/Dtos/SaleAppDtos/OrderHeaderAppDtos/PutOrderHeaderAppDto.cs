﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderHeaderAppDtos
{
    public class PutOrderHeaderAppDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid Seller { get; set; }
        public Guid Buyer { get; set; }
    }
}
