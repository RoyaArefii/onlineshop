using OnlineShop.Application.Services.UserManagmentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.UserManagementAppDtos.RoleAppDtos
{
    public class PostRoleAppDto
    {
        public string Name { get; set; }
        public string? EntityDescription { get; set; }        
        public string UserName { get; set; }        
    }
}
