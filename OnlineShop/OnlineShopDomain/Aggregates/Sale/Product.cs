using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDomain.Aggregates.Sale
{
    public class Product :MainEntityBase
    {
        #region [ForignKey]
        public Guid ProductCategoryId { get; set; }
        //Navigation Property
        public ProductCategory ProductCategory { get; set; }
        #endregion

        public decimal UnitPrice { get; set; }

        //One to Mnay Product& OrderDetail
        List<OrderDetail> OrderDetails { get; set; }
    }
}
