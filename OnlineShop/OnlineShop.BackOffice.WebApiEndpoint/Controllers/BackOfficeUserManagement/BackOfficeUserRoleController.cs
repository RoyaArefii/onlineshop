using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserRoleAppDto;
using OnlineShop.Application.Services.UserManagmentServices;
using OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.UserManagementDtos.UserRoleControllerDtos;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;
using System.Security.Claims;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeUserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeUserRoleController : ControllerBase
    {
        #region [- Ctor & Fields-]
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly UserRoleService _userRoleService;

        public BackOfficeUserRoleController(UserRoleService userRoleService , UserService userService, RoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
            _userRoleService = userRoleService; 

        }
        #endregion

        #region [Guard]
        private static JsonResult Guard(PutUserRoleControllerDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.UserId.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.RoleId.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        } 
        #endregion

        #region [-Put-]
        [HttpPut]
        [Authorize (Roles="GodAdmin")]
        public async Task<IActionResult> Put(PutUserRoleControllerDto model)
        {
            Guard(model);
            var userRole = new PutUserRoleAppDto();
            userRole.RoleId = model.RoleId;
            userRole.UserId = model.UserId;
            userRole.UserName = GetCurrentUser().Value.ToString();

            var result = await _userRoleService.Put(userRole);
            if (!result.IsSuccessful) return new JsonResult(new Response<object>(result.ErrorMessage));
            return new JsonResult(new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK));
        }
        #endregion
        
        #region [-OtherMethods-]
        private JsonResult GetCurrentUser()
        {
            var claims = User.Claims.ToList<Claim>();
            foreach (var claim in claims)
            {
                if (claim.Type == "Name")
                {
                    var user = claim.Value;
                    return new JsonResult(user);
                }
            }
            return new JsonResult(null);
        } 
        #endregion

    }
}