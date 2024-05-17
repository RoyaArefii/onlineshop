using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.UserManagementAppDtos.UserRoleAppDto
{
    public class PutUserRoleAppDto
    {
        //public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
