using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.Application.Services.UserManagmentServices;
using OnlineShop.BackOffice.WebApiEndpoint.Controllers.ControllerDtos.UserManagementDtos;
using PublicTools.Resources;
using ResponseFramework;
using System.Linq;
using System.Security.Claims;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeUserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeUserController : ControllerBase
    {
        #region [-Fields-]
        private readonly UserService _userService;
        //private string? userName;
        #endregion

        #region [-Ctor-]
        public BackOfficeUserController(UserService appUserService)
        {
            _userService = appUserService;
           //userName = GetCurrentUser().Value.ToString();
        }
        #endregion

        #region [- Guards -]
        private static JsonResult Guard(PutUserControllerDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.FirstName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.LastName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Cellphone.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.IsActive.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        private static JsonResult Guard(PostUserControllerDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.FirstName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.LastName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Password.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.ConfirmPassword.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.Cellphone.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        private static JsonResult Guard(ResetPasswordControllerDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.Password.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.ConfirmPassword.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            //if (model.Token.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.UserName.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        #endregion

        #region [-CRUD-]
        [HttpPost(Name = "PostUser")]
        public async Task<IActionResult> Post(PostUserControllerDto model)
        {
            Guard(model);

            var postUsser = new PostUserAppDto()
            {
                Cellphone = model.Cellphone,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Location = model.Location,
                Picture = model.Picture
            };
            var postResult = await _userService.PostAsync(postUsser);
            return new JsonResult(postResult);
        }
        [HttpPut(Name = "PutUser")]
        [Authorize]
        public async Task<IActionResult> Put(PutUserControllerDto model)
        {
            Guard(model);
            var putUsser = new PutUserAppDto()
            {
                Cellphone = model.Cellphone,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Location = model.Location,
                Id = model.Id,
                IsActive = model.IsActive,
                picture = model.picture,
                UserName = GetCurrentUser().Value.ToString()
        };
            var putResult = await _userService.PutAsync(putUsser);
            return new JsonResult(putResult);
        }

        [HttpDelete(Name = "DeleteUser")]
        [Authorize]
        public async Task<IActionResult> Delete(DeleteUserControllerDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var deleteUsser = new DeleteUserAppDto()
            {
                Id = model.Id,
                UserName = GetCurrentUser().Value.ToString()
        };
            var postResult = await _userService.DeleteAsync(deleteUsser);
            return new JsonResult(postResult);
        }

        [HttpGet(Name = "GetUser")]
        [Authorize()]
        public async Task<IActionResult> GetAll()
        {
            var getresult = await _userService.GetAsync();
            return new JsonResult(getresult);
        }
        #endregion

        #region [-OherMethods-]
        [HttpPost("ResetPassword", Name = "ResetPassword")]
        [Authorize()]
        public async Task<IActionResult> ResetPassword(ResetPasswordControllerDto model)
        {
            Guard(model);
            var resetModel = new ResetPassDto()
            {
                ConfirmPassword = model.ConfirmPassword,
                Password = model.Password,
                UserName = model.UserName,
                UserNameAuthorized = GetCurrentUser().Value.ToString()
        };
            var result = await _userService.ResetPassword(resetModel);
            return new JsonResult(result);
        }
        private JsonResult GetCurrentUser()
        {
            var identity2 = User.Claims.ToList<Claim>();
            foreach (var claim in identity2)
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
