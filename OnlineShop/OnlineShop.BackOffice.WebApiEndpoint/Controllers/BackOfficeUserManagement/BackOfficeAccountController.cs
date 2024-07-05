using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using OnlineShop.Application.Dtos.UserManagementAppDtos.AccountDtos;
using OnlineShop.Application.Services.UserManagmentServices;
using PublicTools.Resources;
using ResponseFramework;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeUserManagement
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

        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (loginDto == null) return new JsonResult(new Response<object>(MessageResource.Error_ModelNull));

            // برای حالت کوکی
            //var result = await _accountService.Login (loginDto); 
            //return (!result.IsSuccessful) ? new JsonResult(new Response<object>(MessageResource.Error_FailProcess)): new JsonResult(null);


            var result = await _accountService.Login(loginDto);
            return (!result.IsSuccessful) ? new JsonResult(new Response<object>(result.ErrorMessage)): 
                                            new JsonResult(result.Result);
        }
    }
}
