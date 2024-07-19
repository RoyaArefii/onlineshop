

namespace OnlineShopDomain.Aggregates.Sale
{
    public class OrderHeader : MainEntityBase
    {
        public string SellerId{ get; set; }
        public string BuyerId{ get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
