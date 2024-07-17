namespace OnlineShop.Office.WebApiEndpoint.ControllerDtos.SaleDtos
{
    public class PutOrderControllerDto
    {
        public PutOrderHeaderControllerDto orderHeader { get; set; }
        public List<PutOrderDetailControllerDto> orderDetails { get; set; }
    }
}
