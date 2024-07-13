

namespace OnlineShopDomain.Aggregates.Sale
{
    public class OrderHeader : MainEntityBase
    {
        //public DateTime OrderDate{ get; set; }
        public string SellerId{ get; set; }
        public string BuyerId{ get; set; }
        public List<OrderDetail>? OrderDetails { get; set; } // name convenshion is ok for this prop ?
    }
}
