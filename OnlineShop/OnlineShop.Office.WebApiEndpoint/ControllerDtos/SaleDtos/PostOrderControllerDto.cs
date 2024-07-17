using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;

namespace OnlineShop.Office.WebApiEndpoint.ControllerDtos.SaleDtos
{
    public class PostOrderControllerDto
    {
        public PostOrderHeaderAppDto OrderHeader { get; set; }
        public List<PostOrderDetailAppDto> OrderDetails { get; set; }
    }
}
