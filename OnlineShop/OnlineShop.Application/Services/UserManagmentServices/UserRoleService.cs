using Microsoft.AspNetCore.Identity;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserRoleAppDto;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using ResponseFramework;
using System.Data;
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

        #region [-Ok-]
        #region [-Task<IResponse<object>> Put(PutUserRoleAppDto model)-]
        /// <summary>
        /// طبق ویس استاد قرار شد در بک آفیس گاد ادمین به بقیه یوزر ها نقس اساین کند 
        /// چک کردن اینکه یوزر گادادمین است برداشته نشد چون ممکن است هر نوع اندپوینتی داشته باشیم مثل ویندوز که اتورایز ندارد بنابراین کنترل می کنیم 
        /// </summary>
        /// <param name="put user role"></param>
        /// <returns></returns>
        public async Task<IResponse<object>> Put(PutUserRoleAppDto model)
        {
            #region Validation
            if (model == null) return new Response<object>(MessageResource.Error_ModelNull);
            if (model.UserId == null) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.RoleId == null) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return new Response<object>(MessageResource.Error_UserNotFound);
            var userLogin = await _userManager.FindByNameAsync(model.UserName);
            if (userLogin == null) return new Response<object>(MessageResource.Error_UserNotFound);
            var newRole = await _roleManager.FindByIdAsync(model.RoleId);
            if (newRole == null) return new Response<object>(MessageResource.Error_RoleNotFound);
            if (await _userManager.IsInRoleAsync(user, newRole.Name)) return new Response<object>(MessageResource.Error_UserInRole);
            if (newRole.Name == "GodAdmin") return new Response<object>(MessageResource.Error_AddGodAdminRole);
            var accessFlag = false;

            if (await _userManager.IsInRoleAsync(userLogin, "GodAdmin"))

                accessFlag = true;
            if (!accessFlag) return new Response<object>(MessageResource.Error_Accessdenied);

            //var deleteUserRoles = await _userService.GetRolesAsync(user);
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
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, resultAddRole, HttpStatusCode.OK);
            #endregion
        }
        #endregion 
        #endregion
   
        //public async Task<IResponse<object>> Post(AsignUserRoleAppDto model)
        //{
        //    if (model == null) return new Response<object>(MessageResource.Error_ModelNull);

        //    var getUser = new GetUserByIdAppDto()
        //    {
        //        UserName = model.UserName,
        //        Id = model.UserId
        //    };

        //    var user = await _userService.FindById(getUser);
        //    if (user.Result == null) return new Response<object>(MessageResource.Error_UserNotFound);

        //    var role = await _roleService.FindById(model.RoleId);
        //    if (role.Result == null) return new Response<object>(MessageResource.Error_RoleNotFound);

        //    var mapUser = await _userManager.FindByIdAsync(user.Result.Id);

        //    //var mapUser = new AppUser();
        //    //{
        //    //    mapUser.Id = user.Result.Id;
        //    //    mapUser.FirstName = user.Result.FirstName;
        //    //    mapUser.LastName = user.Result.LastName;
        //    //    mapUser.Password = user.Result.Password;
        //    //    mapUser.UserName = user.Result.UserName;
        //    //    mapUser.Cellphone = user.Result.Cellphone;
        //    //    mapUser.ConfirmPassword = user.Result.ConfirmPassword;
        //    //    mapUser.Location = user.Result.Location;
        //    //    mapUser.DateCreatedLatin = user.Result.DateCreatedLatin;
        //    //    mapUser.DateCreatedPersian = user.Result.DateCreatedPersian;
        //    //    mapUser.DateModifiedLatin = user.Result.DateModifiedLatin;
        //    //    mapUser.DateModifiedPersian = user.Result.DateModifiedPersian;
        //    //    mapUser.DateSoftDeletedLatin = user.Result.DateSoftDeletedLatin;
        //    //    mapUser.DateSoftDeletedPersian = user.Result.DateSoftDeletedPersian;
        //    //    mapUser.IsDeleted = user.Result.IsDeleted;
        //    //    mapUser.IsActive = user.Result.IsActive;
        //    //    mapUser.IsModified = (bool)user.Result.IsModified;
        //    //};
        //    //var mapRole = role.Result;
        //    //mapRole.Id = role.Result.Id;
        //    //mapRole.Name = role.Result.Name;
        //    //mapRole.DateCreatedLatin = (DateTime)role.Result.DateCreatedLatin;
        //    //mapRole.DateCreatedPersian = role.Result.DateCreatedPersian;
        //    //mapRole.DateModifiedLatin = role.Result.DateModifiedLatin;
        //    //mapRole.DateModifiedPersian = role.Result.DateModifiedPersian;
        //    //mapRole.IsDeleted = role.Result.IsDeleted;
        //    //mapRole.IsActive = role.Result.IsActive;
        //    //mapRole.DateSoftDeletedLatin = role.Result.DateSoftDeletedLatin;
        //    //mapRole.DateSoftDeletedPersian = role.Result.DateSoftDeletedPersian;
        //    //mapRole.EntityDescription = role.Result.EntityDescription;
        //    //mapRole.IsModified = role.Result.IsModified;

        //    var result = await _userManager.AddToRoleAsync(mapUser, role.Result.Name);

        //    if (user ==null) return new Response<Object>(MessageResource.Error_FailProcess);
        //    return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, user, HttpStatusCode.OK);
        //}
    }
}
