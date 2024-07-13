namespace OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.SaleDtos.Order
{
    public class PutOrderHeaderControllerDto
    {
        public Guid Id { get; set; }
        public string? EntityDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
