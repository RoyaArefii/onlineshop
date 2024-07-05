using OnlineShopDomain.Aggregates.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.UserManagementAppDtos.UserRoleAppDto
{
    public class GetUserRolesAppDto
    {
        //public int Id { get; set; }
        public AppUser User { get; set; }
    }
}
