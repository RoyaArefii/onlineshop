

namespace OnlineShopDomain.Aggregates.Sale
{
    public class OrderHeader
    {
        public Guid Id{ get; set; }
        public string Code { get; set; }
        public DateTime OrderDate{ get; set; }
        public Guid Seller{ get; set; }
        public Guid Buyer{ get; set; }
        List<OrderDetail> List { get; set;}
    }
}
