using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.SaleAppDtos.ProductCategory
{
    public class PutProductCategoryAppDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public Guid? ParentId { get; set; }
        public Boolean IsActive { get; set; }
        public string EntityDescription { get; set; }
        public string UserName { get; set; }

    }
}
