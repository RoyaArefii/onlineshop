namespace OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.SaleDtos.Order
{
    public class PutOrderControllerDto
    {
        public PutOrderHeaderControllerDto orderHeader { get; set; }
        public List<PutOrderDetailControllerDto> orderDetails { get; set; }
    }
}
