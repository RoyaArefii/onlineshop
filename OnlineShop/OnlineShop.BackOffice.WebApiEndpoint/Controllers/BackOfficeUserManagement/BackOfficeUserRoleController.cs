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

        //#region [- Post -]
        //[HttpPost(Name = "AssignRoleToUser")]
        //public async Task<IActionResult> Post(AsignUserRoleAppDto model)/// با آپدیت userRole  اصلاح شود.
        //{
        //    #region [Guard]
        //    if (model.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ModelNull));
        //    if (model.UserId.IsNullOrEmpty()) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
        //    if (model.RoleId.IsNullOrEmpty()) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));

        //    var authorizedUser = GetCurrentUser().Value.ToString();
        //    var userName = GetCurrentUser();
        //    var getUser = new GetUserByIdAppDto()
        //    {
        //        Id = model.UserId,
        //        UserName = userName.ToString()
        //    };
        //    var user = await _userService.FindById(getUser);
        //    if (!user.IsSuccessful) return new JsonResult(new Response<object>(MessageResource.Error_UserNotFound));
        //    var role = await _roleService.FindById(model.RoleId);
        //    if (!role.IsSuccessful) return new JsonResult(new Response<object>(MessageResource.Error_RoleNotFound));
        //    if (await _userService.HasUserRole (user.Result.Id, role.Result.Name))
        //        return new JsonResult(new Response<object>(MessageResource.Error_UserInRole));
        //    var userRole = new AsignUserRoleAppDto
        //    {
        //        UserId = user.Result.Id,
        //        RoleId = role.Result.Id,
        //        UserName = authorizedUser
        //    };
        //    var result = await _userService.AsignUserToRole(userRole);
        //    if (!result.IsSuccessful) return new JsonResult(new Response<object>(MessageResource.Error_FailProcess));

        //    return new JsonResult(new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK));
        //} 
        //    #endregion
        //#endregion
    }
}