using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos.UserManagementAppDtos.AccountDtos;
using OnlineShop.Application.Services.Account;
using PublicTools.Resources;
using ResponseFramework;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeAccount
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeAccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public BackOfficeAccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Login" , Name = "Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (loginDto == null) return new JsonResult(new Response<object>(MessageResource.Error_ModelNull));

            // برای حالت کوکی
            //var result = await _accountService.Login (loginDto); 
            //return (!result.IsSuccessful) ? new JsonResult(new Response<object>(MessageResource.Error_FailProcess)): new JsonResult(null);


            var result = await _accountService.Login(loginDto);
            return !result.IsSuccessful ? new JsonResult(new Response<object>(result.ErrorMessage)) :
                                            new JsonResult(result.Result);

        }
        [HttpPost("Logout_Alaki" , Name = "Logout_Alaki")]
        public async Task<IActionResult> RemoveToken(LoginDto loginDto)
        {
            return new JsonResult("Logout Was Successful");
                //Response.Headers.Remove("Authorization"));
        }
    }
}
