using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos.UserManagementAppDtos.RoleAppDtos;
using OnlineShop.Application.Services.UserManagmentServices;
using OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.UserManagementDtos.RoleControllerDtos;
using PublicTools.Resources;
using PublicTools.Tools;
using ResponseFramework;
using System.Security.Claims;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeUserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeRoleController : ControllerBase
    {
        #region [- Ctor -]
        private readonly RoleService _roleService;

        public BackOfficeRoleController(RoleService roleService)
        {
            _roleService = roleService;
        } 
        #endregion

        #region [- Guard -]
        private static JsonResult Guard(PostRoleControllerDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            return (model.Name.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        private static JsonResult Guard(PutRoleControllerDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.IsActive.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.Name.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        #endregion

        #region [- CRUD -]
        #region [- Post -]
        [HttpPost(Name = "PostRole")]
        [Authorize(Roles = "GodAdmin")]
        public async Task<IActionResult> Post(PostRoleControllerDto model)
        {
            Guard(model);
            var userName = GetCurrentUser().Value.ToString();
            if (userName == null) return new JsonResult(new Response<object>(MessageResource.Error_UserNotFound));
            var postModel = new PostRoleAppDto()
            {
                Name = model.Name,
                EntityDescription = model.EntityDescription,
                UserName = userName
            };
            var postResult = await _roleService.PostAsync(postModel);
            return new JsonResult(postResult);
        }
        #endregion

        #region [- Put -]
        [HttpPut(Name = "PutRole")]
        [Authorize (Roles ="GodAdmin")]
        public async Task<IActionResult> Put(PutRoleControllerDto model)
        {
            Guard(model);
            var userName = GetCurrentUser().Value.ToString();
            if (userName == null) return new JsonResult(new Response<object>(MessageResource.Error_UserNotFound));
            var putModel = new PutRoleAppDto()
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                EntityDescription=model.EntityDescription,
                UserName = userName
            };
            var putRole = await _roleService.PutAsync(putModel);
            return new JsonResult(putRole);
        }
        #endregion

        #region [-Delete-]
        [HttpDelete(Name = "DeleteRole")]
        [Authorize(Roles = "GodAdmin")]
        public async Task<IActionResult> Delete(DeleteRoleControllerDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var userName = GetCurrentUser().Value.ToString();
            var deleteModel = new DeleteRoleAppDto()
            {
                Id = model.Id,
                UserName = userName
            };
            if (userName == null) return new JsonResult(new Response<object>(MessageResource.Error_UserNotFound));

            var result = await _roleService.DeleteAsync(deleteModel);
            return new JsonResult(result);
        }
        #endregion

        #region [GetAll]
        [HttpGet(Name = "GetAllRoles")]
        [Authorize(Roles = "GodAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var getresult = await _roleService.GetAsync();
            return new JsonResult(getresult);
        }
        #endregion

        #endregion

        #region [- Other -]        
        #region [-JsonResult GetCurrentUser()-]
        private JsonResult GetCurrentUser()
        {
            var claims = User.Claims.ToList<Claim>();
            foreach (var claim in claims)
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
        #endregion
    }

}
