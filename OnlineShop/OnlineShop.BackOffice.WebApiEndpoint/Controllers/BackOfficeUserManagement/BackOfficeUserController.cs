﻿using JWT.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.Application.Services.UserManagmentServices;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using ResponseFramework;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeUserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeUserController : ControllerBase
    {
        private readonly UserService _userService;

        public BackOfficeUserController(UserService appUserService)
        {
            _userService = appUserService;
        }

        private static JsonResult Guard(PutUserAppDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.FirstName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.LastName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Cellphone.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.IsActive.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }

        private static JsonResult Guard(PostUserAppDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.FirstName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.LastName.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Password.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.ConfirmPassword.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.Cellphone.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        private static JsonResult Guard(ResetPassDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.Password.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.ConfirmPassword.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            //if (model.Token.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.UserName.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
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
        [Authorize()]
        public async Task<IActionResult> GetAll()
        {
            var getresult = await _userService.GetAsync();
            //var user2 = GetCurrentUser2();
            var user = GetCurrentUser();
            return new JsonResult(getresult);
        }
        [HttpPost("ResetPassword", Name = "ResetPassword")]
        [Authorize()]
        public async Task<IActionResult> ResetPassword(ResetPassDto model)
        {
            Guard(model);
            var result = await _userService.ResetPassword(model);
            return new JsonResult(result);
        }

        private string GetCurrentUser()
        {
            var identity2 = User.Claims.ToList<Claim>();
            foreach (var claim in identity2)
            {
                if (claim.Type == "name")
                return claim.Value;
            }


            return null;
        }
        //[HttpGet("profile")]
        //private async Task<IActionResult> GetCurrentUser2()
        //{
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        var user = new AppUser
        //        {
        //            Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
        //            UserName = User.FindFirst(ClaimTypes.Name)?.Value
        //        };
        //        var result = await _userService.FindById(user.Id);
        //        return new JsonResult(result);
        //    }
        //    return null;
        //}
    }
}
