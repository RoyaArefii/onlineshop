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

        #region Ok

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

            //var users = _userService.Users.ToList().Any(p => p.Cellphone == model.Cellphone);
            //if (users) return new Response<object>(MessageResource.Error_UserDuplicate);
            if (!UniqUser(model.Cellphone).Result) return new Response<object>(MessageResource.Error_UserDuplicate);
            #endregion

            #region [-Task-]
            var postAppUser = new AppUser
            {
                //Id = Convert.ToString( new Guid()),
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
            #region [Transaction]
            using (_context.Database.BeginTransaction())
            {
                try
                {
                    postResult = await _userService.CreateAsync(postAppUser, postAppUser.PasswordHash);
                    if (!postResult.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);

                    var newUser = await _userService.FindByNameAsync(postAppUser.UserName);
                    //var defaultRole = await _roleService.FindByNameAsync(DefaultRoles.NormalName);
                    //var asignUserRoleAppDto = new AsignUserRoleAppDto
                    //{
                    //    UserId = newUser.Id,
                    //    RoleId = DefaultRoles.NormalId
                    //};
                    var userRole =_userService.AddToRoleAsync(newUser , DefaultRoles.NormalName);   
                        //AsignUserToRole(asignUserRoleAppDto);
                    if (userRole.Result == null) return new Response<object>(MessageResource.Error_FailedToAssignRoleToUser);
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
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, postResult, HttpStatusCode.OK);
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
            if (findUser == null) return new Response<GetUserAppDto>(MessageResource.Error_FailProcess);
            if (userLogin == null) return new Response<GetUserAppDto>(MessageResource.Error_UserNotFound);

            var roles = await _userService.GetRolesAsync(userLogin);
            var accessFlag = false;
            foreach (var role in roles)
            {

                if ( await _userService.IsInRoleAsync(userLogin, "GodAdmin"))

                    accessFlag = true;
            }

            if (!accessFlag && (userLogin.UserName != findUser.UserName)) return new Response<GetUserAppDto>(MessageResource.Error_Accessdenied);
            #endregion

            #region [-Task-]
            //var findUser = await _userService.FindByIdAsync(id);

            var findAppUser = new GetUserAppDto()
            {
                Id = findUser.Id,
                UserName = findUser.Cellphone,
                FirstName = findUser.FirstName,
                LastName = findUser.LastName,
                Cellphone = findUser.Cellphone,
                //PasswordHash = findUser.PasswordHash,
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
                //PasswordHash = item.PasswordHash,
                IsActive = item.IsActive,
                IsModified = item.IsModified,
                IsDeleted = item.IsDeleted,
                DateCreatedLatin = item.DateCreatedLatin,
                DateCreatedPersian = item.DateCreatedPersian,
                DateModifiedLatin = item.DateModifiedLatin,
                DateModifiedPersian = item.DateModifiedPersian,
                DateSoftDeletedLatin = item.DateSoftDeletedLatin,
                DateSoftDeletedPersian = item.DateSoftDeletedPersian,
            }).ToList();

            return new Response<List<GetUserAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getAppUsers, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(string id)-]////// این متد احراز هویت نمی شود چون API ندارد 
        public async Task<IResponse<object>> DeleteAsync(string id)
        {
            
            #region [-Validation-]
            if (id.IsNullOrEmpty())
            {
                return new Response<object>(MessageResource.Error_ModelNull);
            }
            var userDelete = await _userService.FindByIdAsync(id);
            if (userDelete == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
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
            //var result = await _userService.DeleteAsync(userDelete);
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
            if ((user == null)) return new Response<object>(MessageResource.Error_UserNotFound);

            var roles = await _userService.GetRolesAsync(userLogin);
            var accessFlag = false;
            foreach (var role in roles)
            {

                if ( await _userService.IsInRoleAsync(userLogin, "GodAdmin") || model.Id == userLogin.Id)

                    accessFlag = true;
            }
            if (!accessFlag && (userLogin.UserName != user.UserName)) return new Response<object>(MessageResource.Error_Accessdenied);
            //var deleteUserRoles = await _userService.GetRolesAsync(user);
            var isGodAdmin = await _userService.IsInRoleAsync(userLogin, "GodAdmin");
                if (isGodAdmin) return new Response<object>(MessageResource.Error_GodAdminUser);
            #endregion
    
            #region [-Task-]
            //var resultDelete = await _userService.DeleteAsync(appUser);
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
        /// <summary>
        /// چون فقط گاددادمین دسترسی دارد میتواند اطلاعات خودش را تغییر دهد 
        /// بنابراین در این متد چک نشد که اطلاعاتی که تغییر میکند برای چه نقشی است
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IResponse<object>> PutAsync(PutUserAppDto model)
        {          
           #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Id.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.FirstName.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.LastName.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Cellphone.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.IsActive.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (!UniqUser(model.Cellphone).Result) return new Response<object>(MessageResource.Error_UserDuplicate);
            var userLogin = await _userService.FindByNameAsync(model.UserName);
            if (userLogin == null) return new Response<object>(MessageResource.Error_UserNotFound);

            var roles = await _userService.GetRolesAsync(userLogin);
            var accessFlag = false;
            foreach (var role in roles)
            {
                if(await _userService.IsInRoleAsync(userLogin, "GodAdmin")|| model.Id == userLogin.Id)

                    accessFlag = true;
            }
            if (!accessFlag && (userLogin.UserName != model.Cellphone)) return new Response<object>(MessageResource.Error_Accessdenied);

            #endregion
           
            #region [-Task-]
            var user = _userService.FindByIdAsync(model.Id);
            if ((user == null)) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putAppUser = user.Result;

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
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, putAppUser, HttpStatusCode.OK);
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
            if (findUser == null) return new Response<object>(MessageResource.Error_FailProcess);

            var userLogin = await _userService.FindByNameAsync(model.UserNameAuthorized);
            if (userLogin == null) return new Response<object>(MessageResource.Error_UserNotFound);

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
        private async Task<bool> UniqUser(string userName)
        {
            var user = await _userService.FindByNameAsync(userName);    
            var users =  _userService.Users.ToList().Any(p => p.Cellphone == userName && p.Id!= user.Id);
            if (users) return false;
            else return true;

        }
        #endregion

        #endregion
        
        #endregion
        
        //#region [- Task<IResponse<object>> DeleteUserRole(DeleteUserRoleAppDto model) با غیرفعال کردن کاربر انجام می شود و نیازی نیست -]
        //public async Task<IResponse<object>> DeleteUserRole(DeleteUserRoleAppDto model)
        //{
        //    if (model.Equals(null)) return new Response<object>(MessageResource.Error_ModelNull);
        //    if (model.UserId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
        //    if (model.RoleId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
        //    var user = await _userService.FindByIdAsync(model.UserId);
        //    if (user == null) return new Response<object>(MessageResource.Error_UserNotFound);
        //    var role = await _roleService.FindByIdAsync(model.RoleId);
        //    if (role == null) return new Response<object>(MessageResource.Error_RoleNotFound);
        //    var result = await _userService.RemoveFromRoleAsync(user, role.Name);
        //    if (!result.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
        //    return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
        //}
        //#endregion

        //#region [- Task<IResponse<object>> AsignUserToRole(AsignUserRoleAppDto model) implement by put userrole -]
        //public async Task<IResponse<object>> AsignUserToRole(AsignUserRoleAppDto model)
        //{
        //    if (model.Equals(null)) return new Response<object>(MessageResource.Error_ModelNull);
        //    if (model.UserId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
        //    if (model.RoleId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);

        //    var user = await _userService.FindByIdAsync(model.UserId);
        //    if (user == null) return new Response<object>(MessageResource.Error_UserNotFound);

        //    var role = await _roleService.FindByIdAsync(model.RoleId);
        //    if (role == null) return new Response<object>(MessageResource.Error_RoleNotFound);

        //    var userInRoleResult = await _userService.IsInRoleAsync(user, role.Name);
        //    if (userInRoleResult) new Response<object>(MessageResource.Error_UserInRole);
        //    var result = await _userService.AddToRoleAsync(user, role.Name);
        //    if (!result.Succeeded) return new Response<object>(MessageResource.Error_FailedToAssignRoleToUser);
        //    return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
        //}
        //#endregion
    }
}
