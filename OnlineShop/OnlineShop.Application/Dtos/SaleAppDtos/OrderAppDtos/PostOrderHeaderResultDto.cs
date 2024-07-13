namespace OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos
{
        public class PostOrderHeaderResultDto
        {
            public Guid Id { get; set; }
            public string BuyerId { get; set; }
            public string SellerId { get; set; }
            public string Code { get; set; }
            public string Title { get; set; }
            public DateTime DateCreatedLatin { get; set; }
            public string DateCreatedPersian { get; set; }
            public string EntityDescription { get; set; } 
    }
}
