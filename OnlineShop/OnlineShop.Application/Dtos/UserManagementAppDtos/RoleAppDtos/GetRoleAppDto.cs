using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.UserManagementAppDtos.RoleAppDtos
{
    public class GetRoleAppDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateSoftDeletedLatin { get; set; }
        public string? DateSoftDeletedPersian { get; set; }
        public string? EntityDescription { get; set; }
    }
}
