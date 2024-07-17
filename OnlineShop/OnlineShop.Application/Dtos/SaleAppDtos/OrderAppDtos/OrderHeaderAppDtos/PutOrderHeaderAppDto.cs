using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos;
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
        public string? EntityDescription { get; set; }
        public string SellerId { get; set; }
        public bool IsActive { get; set; }
    }
}
