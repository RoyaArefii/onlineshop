using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.ProductCategory
{
    public class GetProductCategoryAppDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid? ParentId { get; set; }
        public Boolean IsActive { get; set; }
        public string EntityDescription { get; set; }

    }
}
