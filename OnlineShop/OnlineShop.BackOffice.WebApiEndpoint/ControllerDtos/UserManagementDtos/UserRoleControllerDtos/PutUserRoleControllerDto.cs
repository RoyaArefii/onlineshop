using System.ComponentModel.DataAnnotations;

namespace OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.UserManagementDtos.UserRoleControllerDtos
{
    public class PutUserRoleControllerDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RoleId { get; set; }
    }
}
