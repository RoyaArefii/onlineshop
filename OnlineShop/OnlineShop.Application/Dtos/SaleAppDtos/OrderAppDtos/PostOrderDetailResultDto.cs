namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos
{
    public class PostOrderDetailResultDto
    {
        public Guid Id  { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
