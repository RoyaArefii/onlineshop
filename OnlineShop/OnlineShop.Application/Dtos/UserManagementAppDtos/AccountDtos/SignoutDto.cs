using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.UserManagementAppDtos.AccountDtos
{
    public class SignoutDto
    {
        public string Token { get; set; }
        public string UserName { get; set; }
    }
}
