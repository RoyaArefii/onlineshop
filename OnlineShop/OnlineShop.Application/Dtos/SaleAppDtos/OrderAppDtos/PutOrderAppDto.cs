using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;


namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos
{
    public class PutOrderAppDto
    {
        public PutOrderHeaderAppDto orderHeader { get; set; }
        public List<PutOrderDetailAppDto> orderDetails { get; set; }
        public string UserName { get; set; }
    }
}
