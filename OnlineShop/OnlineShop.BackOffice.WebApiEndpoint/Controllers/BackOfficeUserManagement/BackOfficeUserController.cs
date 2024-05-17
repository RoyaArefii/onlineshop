using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.Application.Services.UserManagmentServices;
using PublicTools.Resources;
using ResponseFramework;
using System.Reflection.Metadata.Ecma335;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeUserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeUserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly UserRoleService _userRoleService;

        public BackOfficeUserController(UserService appUserService , UserRoleService userRoleService)
        {
            _userService = appUserService;
            _userRoleService = userRoleService; 
        }

        private static JsonResult Guard(PutUserAppDto model)
        {
            if (model == null) return new JsonResult (new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.FirstName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.LastName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Cellphone.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.IsActive.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)): new JsonResult(null);
        }

        private static JsonResult Guard(PostUserAppDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.FirstName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.LastName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Password.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.ConfirmPassword.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.Cellphone.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)):new JsonResult(null);
       }

        [HttpPut(Name = "PutUser")]
        public async Task<IActionResult> Put(PutUserAppDto model)
        {
            Guard(model);
            var putResult = await _userService.PutAsync(model);
            return new JsonResult(putResult);
        }

        [HttpPost(Name = "PostUser")]
        public async Task<IActionResult> Post(PostUserAppDto model)
        {
            Guard(model);
            var postResult = await _userService.PostAsync(model);
            return new JsonResult(postResult);
        }

        [HttpDelete(Name = "DeleteUser")]
        public async Task<IActionResult> Delete(DeleteUserAppDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var postResult = await _userService.DeleteAsync(model);
            return new JsonResult(postResult);
        }

        [HttpGet(Name = "GetUser")]
        public async Task<IActionResult> GetAll()
        {
            var getresult = await _userService.GetAsync();
            return new JsonResult(getresult);
        }
    }
}
