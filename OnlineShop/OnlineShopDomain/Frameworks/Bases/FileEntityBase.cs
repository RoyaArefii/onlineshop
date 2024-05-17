using OnlineShopDomain.Frameworks.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopDomain.Frameworks.Bases
{
    public class FileEntityBase : IFileEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? EntityDescription { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DateSoftDeletedLatin { get; set; }
        public string? DateSoftDeletedPersian { get; set; }
    }
}
