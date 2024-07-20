using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.EFCore;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using PublicTools.Tools;
using ResponseFramework;
using System.Net;
using static PublicTools.Constants.DatabaseConstants;

namespace OnlineShop.Application.Services.UserManagmentServices
{
    public class UserService
    {

        #region [-Ctor & Fields-]
        private readonly UserManager<AppUser> _userService;
        private readonly RoleManager<AppRole> _roleService;
        private readonly OnlineShopDbContext _context;
        public UserService(UserManager<AppUser> userRepository, RoleManager<AppRole> roleRepository, OnlineShopDbContext onlineShopDbContext)
        {
            _userService = userRepository;
            _roleService = roleRepository;
            _context = onlineShopDbContext;
        }
        #endregion

        #region [-Task<IResponse<object>> PostAsync(PostUserAppDto model)-]
        public async Task<IResponse<object>> PostAsync(PostUserAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.FirstName.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.LastName.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Password.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.ConfirmPassword.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Cellphone.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Endpoint.IsNullOrEmpty()) model.Endpoint = "Backoffice";
            if (!(UniqUser(model.Cellphone, null).Result)) return new Response<object>(MessageResource.Error_UserDuplicate);

            #endregion

            #region [-Task-]
            var postAppUser = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Cellphone = model.Cellphone,
                UserName = model.Cellphone,
                PasswordHash = model.Password,
                Picture = model.Picture,
                Location = model.Location,
                IsActive = true,
                IsModified = false,
                IsDeleted = false,
                DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                DateCreatedLatin = DateTime.Now
            };
            if (postAppUser == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            IdentityResult postResult;
            var result = new PostUserAppDto();
            var newUser = new AppUser();
            #region [Transaction]
            using (_context.Database.BeginTransaction())
            {
                try
                {
                    postResult = await _userService.CreateAsync(postAppUser, postAppUser.PasswordHash);
                    if (!postResult.Succeeded) return new Response<object>(postResult.Errors);

                    newUser = await _userService.FindByNameAsync(postAppUser.UserName);
                    if (newUser.IsDeleted == true) return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);
                    //var defaultRole = await _roleService.FindByNameAsync(DefaultRoles.NormalName);
                    //var asignUserRoleAppDto = new AsignUserRoleAppDto
                    //{
                    //    UserId = newUser.Id,
                    //    RoleId = DefaultRoles.NormalId
                    //};
                    if (model.Endpoint == "Office")
                    {
                        var userRole = _userService.AddToRoleAsync(newUser, DefaultRoles.BuyerName);
                        if (userRole.Result == null) return new Response<object>(MessageResource.Error_FailedToAssignRoleToUser);
                    }
                    else if (model.Endpoint == "BackOffice")
                    {
                        var userRole = _userService.AddToRoleAsync(newUser, DefaultRoles.NormalName);
                        if (userRole.Result == null) return new Response<object>(MessageResource.Error_FailedToAssignRoleToUser);
                    }

                    //AsignUserToRole(asignUserRoleAppDto);
                    _context.Database.CommitTransaction();
                }
                catch (Exception ex)
                {
                    _context.Database.RollbackTransaction();
                    return new Response<object>(MessageResource.Error_FailProcess);
                }
            }
            #endregion
            #endregion

            #region [-Result-]
            result.FirstName = newUser.FirstName;
            result.LastName = newUser.LastName;
            result.Cellphone = newUser.Cellphone;
            result.Location = newUser.Location;
            result.Picture = newUser.Picture;

            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<GetUserAppDto>> FindById(GetUserByIdAppDto model)-]
        public async Task<IResponse<GetUserAppDto>> FindById(GetUserByIdAppDto model)
        {
            #region [-Validation-]
            if (model.Id.IsNullOrEmpty()) return new Response<GetUserAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            var userLogin = await _userService.FindByNameAsync(model.UserName);
            var findUser = await _userService.FindByIdAsync(model.Id);
            if (Helpers.IsDeleted(findUser) || findUser == null) return new Response<GetUserAppDto>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);
            var roles = await _userService.GetRolesAsync(userLogin);
            var accessFlag = false;
            foreach (var role in roles)
            {

                if (await _userService.IsInRoleAsync(userLogin, "GodAdmin"))

                    accessFlag = true;
            }

            if (!accessFlag && (userLogin.UserName != findUser.UserName)) return new Response<GetUserAppDto>(MessageResource.Error_Accessdenied);
            #endregion

            #region [-Task-]

            var findAppUser = new GetUserAppDto()
            {
                Id = findUser.Id,
                UserName = findUser.Cellphone,
                FirstName = findUser.FirstName,
                LastName = findUser.LastName,
                Cellphone = findUser.Cellphone,
                Picture = findUser.Picture,
                Location = findUser.Location,
                IsActive = findUser.IsActive,
                DateCreatedLatin = findUser.DateCreatedLatin,
                DateCreatedPersian = findUser.DateCreatedPersian,
                IsModified = findUser.IsModified,
                DateModifiedLatin = findUser.DateModifiedLatin,
                DateModifiedPersian = findUser.DateModifiedPersian,
                IsDeleted = findUser.IsDeleted,
                DateSoftDeletedLatin = findUser.DateSoftDeletedLatin,
                DateSoftDeletedPersian = findUser.DateSoftDeletedPersian
            };
            #endregion

            #region [-Result-]
            return new Response<GetUserAppDto>(true, MessageResource.Info_SuccessfullProcess, string.Empty, findAppUser, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<List<GetUserAppDto>>> GetAsync()-]
        public async Task<IResponse<List<GetUserAppDto>>> GetAsync()
        {
            var getResult = await _userService.Users.ToListAsync();
            var getAppUserList = new List<GetUserAppDto>();
            var getAppUsers = getResult.Select(item => new GetUserAppDto()
            {
                Id = item.Id,
                UserName = item.Cellphone,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Cellphone = item.Cellphone,
                IsActive = item.IsActive,
                IsModified = item.IsModified,
                IsDeleted = item.IsDeleted,
                DateCreatedLatin = item.DateCreatedLatin,
                DateCreatedPersian = item.DateCreatedPersian,
                DateModifiedLatin = item.DateModifiedLatin,
                DateModifiedPersian = item.DateModifiedPersian,
                DateSoftDeletedLatin = item.DateSoftDeletedLatin,
                DateSoftDeletedPersian = item.DateSoftDeletedPersian,
            }).Where(p => p.IsDeleted == false).ToList();
            return new Response<List<GetUserAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getAppUsers, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(string id)-]
        public async Task<IResponse<object>> DeleteAsync(string id)
        {

            #region [-Validation-]
            if (id.IsNullOrEmpty())
            {
                return new Response<object>(MessageResource.Error_ModelNull);
            }
            var userDelete = await _userService.FindByIdAsync(id);
            if (Helpers.IsDeleted(userDelete) || userDelete == null)
                return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);

            var isGodAdmin = await _userService.IsInRoleAsync(userDelete, "GodAdmin");
            if (isGodAdmin) return new Response<object>(MessageResource.Error_GodAdminUser);
            #endregion

            #region [-Task-]
            userDelete.IsDeleted = true;
            userDelete.DateSoftDeletedLatin = DateTime.Now;
            userDelete.DateSoftDeletedPersian = Helpers.ConvertToPersianDate(DateTime.Now);

            var result = await _userService.UpdateAsync(userDelete);
            #endregion

            #region [-Result-]
           
            if (!result.Succeeded)
            {
                return new Response<object>(MessageResource.Error_FailProcess);
            }
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(DeleteUserAppDto model)-]
        public async Task<IResponse<object>> DeleteAsync(DeleteUserAppDto model)
        {
            #region [-Validation-]
            if (model == null) return new Response<object>(MessageResource.Error_ModelNull);
            if (model.Id.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);

            var userLogin = await _userService.FindByNameAsync(model.UserName);
            if (userLogin == null) return new Response<object>(MessageResource.Error_UserNotFound);

            var user = await _userService.FindByIdAsync(model.Id);
            if (Helpers.IsDeleted(user) || user == null) return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);


            var roles = await _userService.GetRolesAsync(userLogin);
            var accessFlag = false;
            foreach (var role in roles)
            {

                if (await _userService.IsInRoleAsync(userLogin, "GodAdmin") || model.Id == userLogin.Id)

                    accessFlag = true;
            }
            if (!accessFlag && (userLogin.UserName != user.UserName)) return new Response<object>(MessageResource.Error_Accessdenied);
            var isGodAdmin = await _userService.IsInRoleAsync(user, "GodAdmin");
            if (isGodAdmin) return new Response<object>(MessageResource.Error_GodAdminUser);
            #endregion

            #region [-Task-]
           
            user.IsDeleted = true;
            user.DateSoftDeletedLatin = DateTime.Now;
            user.DateSoftDeletedPersian = Helpers.ConvertToPersianDate(DateTime.Now);

            var result = await _userService.UpdateAsync(user);
            #endregion

            #region [-Result-]

            if (!result.Succeeded)
                return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<object>> PutAsync(PutUserAppDto model)-]
        public async Task<IResponse<object>> PutAsync(PutUserAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Id.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.FirstName.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.LastName.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Cellphone.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.IsActive.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            var user = await _userService.FindByIdAsync(model.Id);
            if (Helpers.IsDeleted(user) || user == null) return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);

            if (!(UniqUser(model.Cellphone, model.Id).Result)) return new Response<object>(MessageResource.Error_UserDuplicate);


            var userLogin = await _userService.FindByNameAsync(model.UserName);
            if (userLogin == null) return new Response<object>(MessageResource.Error_UserNotFound);
            var roles = await _userService.GetRolesAsync(userLogin);
            var accessFlag = false;
            foreach (var role in roles)
            {
                if (await _userService.IsInRoleAsync(userLogin, "GodAdmin") || model.Id == userLogin.Id)

                    accessFlag = true;
            }
            if (!accessFlag && (userLogin.UserName != model.Cellphone)) return new Response<object>(MessageResource.Error_Accessdenied);

            #endregion

            #region [-Task-]
            var putAppUser = user;

            putAppUser.FirstName = model.FirstName;
            putAppUser.LastName = model.LastName;
            putAppUser.Cellphone = model.Cellphone;
            putAppUser.UserName = model.Cellphone;
            putAppUser.Picture = model.picture;
            putAppUser.Location = model.Location;
            putAppUser.IsActive = model.IsActive;
            putAppUser.IsModified = true;
            putAppUser.DateModifiedLatin = DateTime.Now;
            putAppUser.DateModifiedPersian = Helpers.ConvertToPersianDate(DateTime.Now);

            if (putAppUser == null) return new Response<object>(MessageResource.Error_FailProcess);
            var putResult = await _userService.UpdateAsync(putAppUser);
            #endregion

            #region [-Result-] 
            if (!putResult.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
            var result = new PutUserAppDto()
            {
                Id = putAppUser.Id,
                FirstName = putAppUser.FirstName,
                LastName = putAppUser.LastName,
                Cellphone = putAppUser.Cellphone,
                UserName = putAppUser.UserName,
                IsActive = putAppUser.IsActive,
                Location = putAppUser.Location,
                picture = putAppUser.Picture,
            };
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Other Methodes-]

        #region [-Task<IResponse<object>> ResetPassword(ResetPassDto resetPassDto)-]
        public async Task<IResponse<object>> ResetPassword(ResetPassDto model)
        {

            #region[- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_ModelNull);
            if (model.UserName.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Password.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.ConfirmPassword.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            var findUser = await _userService.FindByNameAsync(model.UserName);
            if (Helpers.IsDeleted(findUser) || findUser == null) return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);


            var userLogin = await _userService.FindByNameAsync(model.UserNameAuthorized);
            if (Helpers.IsDeleted(userLogin) || userLogin == null) return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);

            var roles = await _userService.GetRolesAsync(userLogin);
            var accessFlag = false;
            foreach (var role in roles)
            {

                if (await _userService.IsInRoleAsync(userLogin, "GodAdmin"))

                    accessFlag = true;
            }

            if (!accessFlag && (userLogin.UserName != findUser.UserName)) return new Response<object>(MessageResource.Error_Accessdenied);
            if (model.Password == null || model.ConfirmPassword == null || model.UserName == null) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var token = await _userService.GeneratePasswordResetTokenAsync(findUser);
            var result = await _userService.ResetPasswordAsync(findUser, token, model.Password);
            #endregion

            #region [-Result-]
            if (!result.Succeeded) return new Response<object>(MessageResource.Error_ModelNull);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-UniqUser(string userName)-]
        private async Task<bool> UniqUser(string userName, string? id)
        {

            var user = await _userService.FindByNameAsync(userName);
            bool users = new();
            if (id.IsNullOrEmpty())
            {
                users = _userService.Users.ToList().Any(p => p.Cellphone == userName && p.IsDeleted != true);
            }
            else
            {
                users = _userService.Users.ToList().Any(p => p.Cellphone == userName && id != user.Id && p.IsDeleted != true);
            }
            if (users) return false;
            else return true;
        }
        #endregion

        #endregion

    }
}
