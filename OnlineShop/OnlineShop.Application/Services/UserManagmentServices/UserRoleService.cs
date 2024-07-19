using Microsoft.AspNetCore.Identity;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserRoleAppDto;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using PublicTools.Tools;
using ResponseFramework;
using System.Net;


namespace OnlineShop.Application.Services.UserManagmentServices
{
    public class UserRoleService
    {

        #region [-Ctor & Fields-]
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public UserRoleService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #endregion
        
        #region [-Task<IResponse<object>> Put(PutUserRoleAppDto model)-]
        public async Task<IResponse<object>> Put(PutUserRoleAppDto model)
        {
            #region Validation
            if (model == null) return new Response<object>(MessageResource.Error_ModelNull);
            if (model.UserId == null) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.RoleId == null) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (Helpers.IsDeleted(user) || user == null) return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);
            var userLogin = await _userManager.FindByNameAsync(model.UserName);
            if (Helpers.IsDeleted(userLogin) || userLogin == null) return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);
            var newRole = await _roleManager.FindByIdAsync(model.RoleId);
            if (Helpers.IsDeleted(newRole) || newRole == null) return new Response<object>(true, string.Empty, MessageResource.Error_RoleNotFound, null, HttpStatusCode.OK);
            if (await _userManager.IsInRoleAsync(user, newRole.Name)) return new Response<object>(MessageResource.Error_UserInRole);
            if (newRole.Name == "GodAdmin") return new Response<object>(MessageResource.Error_AddGodAdminRole);
            var accessFlag = false;

            if (await _userManager.IsInRoleAsync(userLogin, "GodAdmin"))

                accessFlag = true;
            if (!accessFlag) return new Response<object>(MessageResource.Error_Accessdenied);

            var isGodAdmin = await _userManager.IsInRoleAsync(userLogin, "GodAdmin");
            if (!isGodAdmin) return new Response<object>(MessageResource.Error_Accessdenied);
            #endregion

            #region Task
            var oldRoles = await _userManager.GetRolesAsync(user);
            if (oldRoles == null) return new Response<object>(MessageResource.Error_UserWithoutRole);
            foreach (var role in oldRoles)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, role);
                if (!result.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
            }
            var resultAddRole = await _userManager.AddToRoleAsync(user, newRole.Name);
            #endregion

            #region Result
            if (!resultAddRole.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, new {model.RoleId , model.UserId}, HttpStatusCode.OK);
            #endregion
        }
        #endregion 
        
    }
}
