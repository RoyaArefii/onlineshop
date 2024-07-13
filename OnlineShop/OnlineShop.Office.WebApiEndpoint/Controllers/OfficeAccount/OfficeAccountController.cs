using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos.UserManagementAppDtos.AccountDtos;
using OnlineShop.Application.Services.Account;
using PublicTools.Resources;
using ResponseFramework;

namespace OnlineShop.Office.WebApiEndpoint.Controllers.OfficeAccount
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeAccountController : ControllerBase
    {
        #region [- Ctor & Fields -]
        private readonly AccountService _accountService;

        public OfficeAccountController(AccountService accountService)
        {
            _accountService = accountService;
        }
        #endregion
        
        #region [- Login -]
        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (loginDto == null) return new JsonResult(new Response<object>(MessageResource.Error_ModelNull));

            var result = await _accountService.Login(loginDto);
            return !result.IsSuccessful ? new JsonResult(new Response<object>(result.ErrorMessage)) :
                                            new JsonResult(result.Result);
        } 
        #endregion
    }
}
