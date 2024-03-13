using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos
{
    public class DeleteUserResultAppDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsDeleted { get; set; }

    }
}
