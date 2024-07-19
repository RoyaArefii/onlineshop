using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos.UserManagementAppDtos.AccountDtos;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.Application.Services.Account;
using OnlineShop.Office.WebApiEndpoint.ControllerDtos.UserManagementDtos.UserControllerDtos;
using PublicTools.Resources;
using ResponseFramework;
using System.Security.Claims;

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

        #region [- Guard -]
        private static JsonResult Guard(PostUserControllerDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.FirstName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.LastName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Password.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.ConfirmPassword.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.Cellphone.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        #endregion

        #region [- Login -]
        [HttpPost("Login",Name = "Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (loginDto == null) return new JsonResult(new Response<object>(MessageResource.Error_ModelNull));

            var result = await _accountService.Login(loginDto);
            return !result.IsSuccessful ? new JsonResult(new Response<object>(result.ErrorMessage)) :
                                            new JsonResult(result.Result);
        }
        #endregion
       
        #region [- Logout -]
        [HttpPost("Logout", Name = "Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            var cleanHeader = authorizationHeader.Trim(new char[] { '[', ']', '{', '}', ' ' });
            var token = cleanHeader.Split(new string[] { "Bearer" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            if (token != null)
            {
                var logoutModel = new LogoutDto()
                {
                    Token = token,
                };
                var logout = await _accountService.LogOut(logoutModel);
                return new JsonResult(new Response<object>(MessageResource.Info_LogoutSuccessFul));
            }
            return new JsonResult(new Response<object>(MessageResource.Error_LogoutNotSuccessful));
        }
        #endregion

        #region [- Signin -]
        [HttpPost("Signin", Name = "Signin")]
        public async Task<IActionResult> Signin(PostUserControllerDto model)
        {
            Guard(model);
            var signinModel = new PostUserAppDto()
            {
                Cellphone = model.Cellphone,
                ConfirmPassword = model.ConfirmPassword,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Location = model.Location,
                Password = model.Password,
                Picture = model.Picture,
            };
            var result = await _accountService.Signin(signinModel);
            if (!result.IsSuccessful) return new JsonResult(result.Result);
            return new JsonResult(result.Result);
        }
        #endregion

        #region [- Signout -]
        [HttpPost("Signout", Name = "Signout")]
        [Authorize]
        public async Task<IActionResult> Signout()
        {
            var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            var cleanHeader = authorizationHeader.Trim(new char[] { '[', ']', '{', '}', ' ' });
            var token = cleanHeader.Split(new string[] { "Bearer" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            var signoutModel = new SignoutDto()
            {
                Token = token,
                UserName = GetCurrentUser().Value.ToString()
            };
            var result = await _accountService.Signout(signoutModel);
            if (!result.IsSuccessful) new JsonResult(new Response<object>(result.ErrorMessage));
            return new JsonResult(result.Message);
        }
        #endregion

        #region [- Other -]
        private JsonResult GetCurrentUser()
        {
            var identity = User.Claims.ToList<Claim>();
            foreach (var claim in identity)
            {
                if (claim.Type == "Name")
                {
                    string user = claim.Value;
                    return new JsonResult(user);
                }
            }
            return new JsonResult(null);
        }
        #endregion
    }
}
