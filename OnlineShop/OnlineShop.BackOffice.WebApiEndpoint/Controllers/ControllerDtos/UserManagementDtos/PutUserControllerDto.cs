﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.ControllerDtos.UserManagementDtos
{
    public class PutUserControllerDto
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
