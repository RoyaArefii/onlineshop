using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserRoleAppDto;
using OnlineShop.Application.Services.UserManagmentServices;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeUserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeUserRoleController : ControllerBase
    {

        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly UserManager<AppUser> _userManager;



        public BackOfficeUserRoleController(UserService userService , RoleService roleService )
        {
            _userService = userService;
            _roleService = roleService;

        }

        [HttpPost(Name = "AssignRoleToUser")]
        public async Task<IActionResult> Post(AsignUserRoleAppDto model)
        {
            #region [Guard]
            if (model.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ModelNull));
            if (model.UserId.IsNullOrEmpty()) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.RoleId.IsNullOrEmpty()) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            #endregion

            var user = await _userService.FindById(model.UserId);
            if (!user.IsSuccessful) return new JsonResult(new Response<object>(MessageResource.Error_UserNotFound));
            var role = await _roleService.FindById(model.RoleId);
            if (!role.IsSuccessful) return new JsonResult(new Response<object>(MessageResource.Error_RoleNotFound));
            if (await _userService.HasUserRole(user.Result.Id , role.Result.Name))
                return new JsonResult( new Response<object>(MessageResource.Error_UserInRole));
            var userRole = new AsignUserRoleAppDto
            {
                UserId = user.Result.Id,
                RoleId = role.Result.Id
            };
            var result =  await _userService.AsignUserToRole(userRole);
            if (!result.IsSuccessful) return new JsonResult(new Response<object>( MessageResource.Error_FailProcess));

            return new JsonResult(new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK));
        }
    }
}
