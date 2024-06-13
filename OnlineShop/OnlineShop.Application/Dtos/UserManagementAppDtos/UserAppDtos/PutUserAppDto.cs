using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos
{
    public class PutUserAppDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Cellphone { get; set; }
        public byte[]? picture { get; set; }
        public string? Location { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
