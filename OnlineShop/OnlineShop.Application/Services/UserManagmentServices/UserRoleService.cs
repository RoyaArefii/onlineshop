using Microsoft.AspNetCore.Identity;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserRoleAppDto;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;


namespace OnlineShop.Application.Services.UserManagmentServices
{
    public class UserRoleService
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly UserManager<AppUser> _userManager;



        public UserRoleService(RoleService roleService, UserService userService, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _roleService = roleService;
            _userManager = userManager;
        }

        public async Task<IResponse<object>> Post(AsignUserRoleAppDto model)
        {
            if (model == null) return new Response<object>(MessageResource.Error_ModelNull);

            var user = await _userService.FindById(model.UserId);
            if (user.Result == null) return new Response<object>(MessageResource.Error_UserNotFound);

            var role = await _roleService.FindById(model.RoleId);
            if (role.Result == null) return new Response<object>(MessageResource.Error_RoleNotFound);

            var mapUser = await _userManager.FindByIdAsync(user.Result.Id);

            //var mapUser = new AppUser();
            //{
            //    mapUser.Id = user.Result.Id;
            //    mapUser.FirstName = user.Result.FirstName;
            //    mapUser.LastName = user.Result.LastName;
            //    mapUser.Password = user.Result.Password;
            //    mapUser.UserName = user.Result.UserName;
            //    mapUser.Cellphone = user.Result.Cellphone;
            //    mapUser.ConfirmPassword = user.Result.ConfirmPassword;
            //    mapUser.Location = user.Result.Location;
            //    mapUser.DateCreatedLatin = user.Result.DateCreatedLatin;
            //    mapUser.DateCreatedPersian = user.Result.DateCreatedPersian;
            //    mapUser.DateModifiedLatin = user.Result.DateModifiedLatin;
            //    mapUser.DateModifiedPersian = user.Result.DateModifiedPersian;
            //    mapUser.DateSoftDeletedLatin = user.Result.DateSoftDeletedLatin;
            //    mapUser.DateSoftDeletedPersian = user.Result.DateSoftDeletedPersian;
            //    mapUser.IsDeleted = user.Result.IsDeleted;
            //    mapUser.IsActive = user.Result.IsActive;
            //    mapUser.IsModified = (bool)user.Result.IsModified;
            //};
            //var mapRole = role.Result;
            //mapRole.Id = role.Result.Id;
            //mapRole.Name = role.Result.Name;
            //mapRole.DateCreatedLatin = (DateTime)role.Result.DateCreatedLatin;
            //mapRole.DateCreatedPersian = role.Result.DateCreatedPersian;
            //mapRole.DateModifiedLatin = role.Result.DateModifiedLatin;
            //mapRole.DateModifiedPersian = role.Result.DateModifiedPersian;
            //mapRole.IsDeleted = role.Result.IsDeleted;
            //mapRole.IsActive = role.Result.IsActive;
            //mapRole.DateSoftDeletedLatin = role.Result.DateSoftDeletedLatin;
            //mapRole.DateSoftDeletedPersian = role.Result.DateSoftDeletedPersian;
            //mapRole.EntityDescription = role.Result.EntityDescription;
            //mapRole.IsModified = role.Result.IsModified;

            var result = await _userManager.AddToRoleAsync(mapUser, role.Result.Name);

            if (user ==null) return new Response<Object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, user, HttpStatusCode.OK);
        }
    }
}
