namespace OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.SaleDtos.Product
{
    public class PutProductControllerDto
    {
        public Guid Id { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsActive { get; set; }
        public string EntityDescription { get; set; }
    }
}
