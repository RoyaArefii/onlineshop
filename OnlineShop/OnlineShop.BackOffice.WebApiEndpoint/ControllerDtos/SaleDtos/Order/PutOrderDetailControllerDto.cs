namespace OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.SaleDtos.Order
{
    public class PutOrderDetailControllerDto
    {
        public Guid? Id { get; set; }
        public Guid ProductId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public string EntityDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
