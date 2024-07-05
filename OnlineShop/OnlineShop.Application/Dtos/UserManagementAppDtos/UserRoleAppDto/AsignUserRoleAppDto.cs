using OnlineShopDomain.Aggregates.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.UserManagementAppDtos.UserRoleAppDto
{
    public class AsignUserRoleAppDto
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public string UserName { get; set; }    
    }
}
