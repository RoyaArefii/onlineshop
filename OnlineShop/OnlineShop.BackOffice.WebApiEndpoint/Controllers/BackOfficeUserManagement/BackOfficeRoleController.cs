using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Dtos.UserManagementAppDtos.RoleAppDtos;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.Application.Services.UserManagmentServices;
using PublicTools.Resources;
using ResponseFramework;
using System.Reflection.Metadata.Ecma335;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeUserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeRoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public BackOfficeRoleController(RoleService roleService)
        {
            _roleService = roleService;
        }
        private static JsonResult Guard(PutRoleAppDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.Name.Equals(null))?  new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null) ;
        }

        private static JsonResult Guard(PostRoleAppDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            return (model.Name.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        
        [HttpDelete(Name = "DeleteRole")]
        public async Task<IActionResult> Delete(DeleteRoleAppDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var postResult = await _roleService.DeleteAsync(model.Id);
            return new JsonResult(postResult);
        }
        
        [HttpPost(Name = "PostRole")]
        public async Task<IActionResult> Post(PostRoleAppDto model)
        {
            Guard(model);
            var postResult = await _roleService.PostAsync(model);
            return new JsonResult(postResult);
        }
        
        [HttpGet(Name = "GetRole")]
        public async Task<IActionResult> GetAll()
        {
            var getresult = await _roleService.GetAsync();
            return new JsonResult(getresult);
        }

        [HttpPut (Name= "putRole")]
        public async Task<IActionResult> Put(PutRoleAppDto model)
        {
            Guard(model);
            var putRole = await _roleService.PutAsync(model);
            return new JsonResult(putRole); 
        }
    }

}
