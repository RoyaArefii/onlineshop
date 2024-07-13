using System.ComponentModel.DataAnnotations;

namespace OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.UserManagementDtos.AccountDtos
{
    public class ResetPasswordControllerDto
    {
        [Required]
        public string UserName { get; set; }
        //[Required]
        //public string Token { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
