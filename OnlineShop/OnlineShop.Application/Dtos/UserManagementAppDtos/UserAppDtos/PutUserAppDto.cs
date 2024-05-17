using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos
{
    public class PutUserAppDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cellphone { get; set; }
        public byte[]? picture { get; set; }
        public string? Location { get; set; }
        public bool IsActive { get; set; }
    }
}
