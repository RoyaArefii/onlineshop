namespace OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.SaleDtos.ProductCategory
{
    public class PutProductCategoryControllerDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public Guid? ParentId { get; set; }
        public string EntityDescription { get; set; }
    }
}
