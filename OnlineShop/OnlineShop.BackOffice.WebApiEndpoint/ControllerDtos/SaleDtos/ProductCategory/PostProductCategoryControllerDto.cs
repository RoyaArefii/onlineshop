namespace OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.SaleDtos.ProductCategory
{
    public class PostProductCategoryControllerDto
    {
        public string Title { get; set; }
        public Guid? ParentId { get; set; }
        public string EntityDescription { get; set; }

    }
}
