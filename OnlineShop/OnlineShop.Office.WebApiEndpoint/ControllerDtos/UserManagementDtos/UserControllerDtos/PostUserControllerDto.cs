using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Office.WebApiEndpoint.ControllerDtos.UserManagementDtos.UserControllerDtos
{
    public class PostUserControllerDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string Cellphone { get; set; }
        public byte[]? Picture { get; set; }
        public string? Location { get; set; }
    }
}
