using System.ComponentModel.DataAnnotations;

namespace OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.UserManagementDtos.RoleControllerDtos
{
    public class DeleteRoleControllerDto
    {
        [Required]
        public string Id { get; set; }

    }
}
