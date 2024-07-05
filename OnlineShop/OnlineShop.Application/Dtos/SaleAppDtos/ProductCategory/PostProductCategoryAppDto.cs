using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.ProductCategory
{
    public class PostProductCategoryAppDto
    {
        public string Title { get; set; }
        public Guid? ParentId { get; set; }
        public string EntityDescription { get; set; }
        public string UserName { get; set; }

    }
}
