using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDomain.Aggregates.Sale
{
    public class ProductCategory:SimpleEntityBase //, IDbSetEntity
    {
        //Naming Convension
        public Guid? ParentId { get; set; }
        List<Product> Products { get; set;} 

    }
}
