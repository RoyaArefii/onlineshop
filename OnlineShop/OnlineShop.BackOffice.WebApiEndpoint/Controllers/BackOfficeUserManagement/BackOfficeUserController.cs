using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.Application.Services.UserManagmentServices;
using OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.UserManagementDtos.AccountDtos;
using OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.UserManagementDtos.UserControllerDtos;
using PublicTools.Resources;
using ResponseFramework;
using System.Data;
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
        private static JsonResult Guard(PostUserAppDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.FirstName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.LastName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Password.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.ConfirmPassword.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.Cellphone.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        private static JsonResult Guard(PutUserControllerDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.FirstName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.LastName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Cellphone.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.IsActive.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
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

        #region [- Post -]
        [HttpPost(Name = "PostUser")]
        [Authorize(Roles = "GodAdmin")]
        public async Task<IActionResult> Post(PostUserAppDto model)
        {
            Guard(model);
            var postResult = await _userService.PostAsync(model);
            return new JsonResult(postResult);
        }
        #endregion

        #region [- Put -]
        [HttpPut(Name = "PutUser")]
        [Authorize]
        public async Task<IActionResult> Put(PutUserControllerDto model)
        {
            Guard(model);
            var userName = GetCurrentUser().Value.ToString();
            if (userName == null) return new JsonResult(new Response<object>(MessageResource.Error_UserNotFound));

            var putUsser = new PutUserAppDto()
            {
                Cellphone = model.Cellphone,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Location = model.Location,
                Id = model.Id,
                IsActive = model.IsActive,
                picture = model.picture,
                UserName = userName
            };
            var putResult = await _userService.PutAsync(putUsser);
            return new JsonResult(putResult);
        }
        #endregion

        #region [- Delete -]
        [HttpDelete("SoftDelete" , Name = "SoftDeleteUser")]
        [Authorize]
        public async Task<IActionResult> Delete(DeleteUserControllerDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var userName = GetCurrentUser().Value.ToString();
            if (userName == null) return new JsonResult(new Response<object>(MessageResource.Error_UserNotFound));
            var deleteUser = new DeleteUserAppDto()
            {
                Id = model.Id,
                UserName = userName
            };
            var postResult = await _userService.DeleteAsync(deleteUser);
            return new JsonResult(postResult);
        }
        #endregion

        #region [GetAll]
        [HttpGet(Name = "GetAllUser")]
        [Authorize(Roles = "GodAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var getresult = await _userService.GetAsync();
            return new JsonResult(getresult);
        }
        #endregion

        #region [GetUser]
        [HttpGet("GetUser", Name = "GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUser(GetUserByIdControllerDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var userName = GetCurrentUser().Value.ToString();
            if (userName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_UserNotFound));
            var getUser = new GetUserByIdAppDto()
            {
                Id = model.Id,
                UserName = userName
            };
            var result = await _userService.FindById(getUser);
            if (!result.IsSuccessful) return new JsonResult(new Response<Object>(result.ErrorMessage));
            return new JsonResult(result);
        } 
        #endregion

        #endregion

        #region [-OherMethods-]
        [HttpPost("ResetPassword", Name = "ResetPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword(ResetPasswordControllerDto model)
        {
            Guard(model);
            var userName = GetCurrentUser().Value.ToString();
            if (userName == null) return new JsonResult(new Response<object>(MessageResource.Error_UserNotFound));
            var resetModel = new ResetPassDto()
            {
                ConfirmPassword = model.ConfirmPassword,
                Password = model.Password,
                UserName = model.UserName,
                UserNameAuthorized = userName
            };
            var result = await _userService.ResetPassword(resetModel);
            return new JsonResult(result);
        }
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
