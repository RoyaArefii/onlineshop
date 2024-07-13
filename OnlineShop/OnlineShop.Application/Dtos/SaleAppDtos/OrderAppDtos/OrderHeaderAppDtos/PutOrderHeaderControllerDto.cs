namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos
{
    public class PutOrderHeaderControllerDto
    {
        public Guid Id { get; set; }
        public string EntityDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
