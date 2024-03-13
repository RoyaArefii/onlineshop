using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos
{
    public class DeleteUserAppDto
    {
        public string Id  { get; set; }
        public string UserName { get; set; }    
        public string Cellphone { get; set; }
        public bool IsModified { get; set; }
        public bool IsDeleted { get; set; }
    }
}
