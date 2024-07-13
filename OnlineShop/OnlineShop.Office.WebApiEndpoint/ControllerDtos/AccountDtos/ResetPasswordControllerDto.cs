using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Office.WebApiEndpoint.ControllerDtos.AccountDtos
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
