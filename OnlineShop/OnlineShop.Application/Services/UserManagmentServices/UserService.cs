using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserRoleAppDto;
using OnlineShop.EFCore;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using PublicTools.Tools;
using ResponseFramework;
using System.Net;
using System.Security.Claims;
using static PublicTools.Constants.DatabaseConstants;

namespace OnlineShop.Application.Services.UserManagmentServices
{
    public class UserService 
    {
        private readonly UserManager<AppUser> _userService;
        private readonly RoleManager<AppRole> _roleService;
        private readonly OnlineShopDbContext _context;


        #region [-Ctor-]
        public UserService(UserManager<AppUser> userRepository , RoleManager<AppRole> roleRepository , OnlineShopDbContext onlineShopDbContext)
        {
            _userService = userRepository;
            _roleService = roleRepository;
            _context = onlineShopDbContext;
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(string id)-]
        public async Task<IResponse<object>> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new Response<object>(MessageResource.Error_ModelNull);
            }
            var userDelete = await _userService.FindByIdAsync(id);
            if (userDelete == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var result = await _userService.DeleteAsync(userDelete);
            if (!result.Succeeded)
            {
                return new Response<object>(MessageResource.Error_FailProcess);
            }
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(DeleteUserAppDto model)-]
        public async Task<IResponse<object>> DeleteAsync(DeleteUserAppDto model)
        {
            if (model == null) return new Response<object>(MessageResource.Error_ModelNull);
            var appUser = await _userService.FindByIdAsync(model.Id);
            if (appUser == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var resultDelete = await _userService.DeleteAsync(appUser);
            if (!resultDelete.Succeeded)
                return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, appUser, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<GetUserAppDto>> FindById(string id)-]
        public async Task<IResponse<GetUserAppDto>> FindById(string id)
        {
            #region [-Validation-]
            if (id.Equals(null)) return new Response<GetUserAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var findResult = await _userService.FindByIdAsync(id);
            if (findResult == null) return new Response<GetUserAppDto>(MessageResource.Error_FailProcess);
            var findAppUser = new GetUserAppDto()
            {
                Id = findResult.Id,
                UserName = findResult.Cellphone,
                FirstName = findResult.FirstName,
                LastName = findResult.LastName,
                Cellphone = findResult.Cellphone,
                PasswordHash = findResult.PasswordHash,  
                Picture = findResult.Picture,
                Location = findResult.Location,
                IsActive = findResult.IsActive,
                DateCreatedLatin = findResult.DateCreatedLatin,
                DateCreatedPersian = findResult.DateCreatedPersian,
                IsModified = findResult.IsModified,
                DateModifiedLatin = findResult.DateModifiedLatin,
                DateModifiedPersian = findResult.DateModifiedPersian,
                IsDeleted = findResult.IsDeleted,
                DateSoftDeletedLatin = findResult.DateSoftDeletedLatin,
                DateSoftDeletedPersian = findResult.DateSoftDeletedPersian
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
                PasswordHash = item.PasswordHash,
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

        #region [-Task<IResponse<object>> PostAsync(PostUserAppDto model)-]
        public async Task<IResponse<object>> PostAsync(PostUserAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.FirstName.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.LastName.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Password.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.ConfirmPassword.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Cellphone.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
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
                    var defaultRole = await _roleService.FindByNameAsync(DefaultRoles.NormalName);
                    var asignUserRoleAppDto = new AsignUserRoleAppDto
                    {
                        UserId = newUser.Id,
                        RoleId = DefaultRoles.NormalId
                    };
                    var userRole = AsignUserToRole(asignUserRoleAppDto);
                    //await _userService.AddToRoleAsync(newUser, DefaultRoles.NormalName);
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

        #region [-Task<IResponse<object>> PutAsync(PutUserAppDto model)-]
        public async Task<IResponse<object>> PutAsync(PutUserAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.FirstName.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.LastName.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Cellphone.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Cellphone.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.IsActive.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
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

            if (putAppUser == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putResult = await _userService.UpdateAsync(putAppUser);
            #endregion

            #region [-Result-] 
            if (!putResult.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, putResult, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [- Task<IResponse<object>> AsignUserToRole(AsignUserRoleAppDto model) -]
        public async Task<IResponse<object>> AsignUserToRole(AsignUserRoleAppDto model)
        {
            if (model.Equals(null)) return new Response<object>(MessageResource.Error_ModelNull);
            if (model.UserId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.RoleId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);

            var user = await _userService.FindByIdAsync(model.UserId);  
            if (user == null) return new Response<object>(MessageResource.Error_UserNotFound);

            var role = await _roleService.FindByIdAsync(model.RoleId);
            if (role == null) return new Response<object>(MessageResource.Error_RoleNotFound);

            var userInRoleResult =  HasUserRole(user.Id , role.Name);   
            if (userInRoleResult.Result) new Response<object>(MessageResource.Error_UserInRole);
            var result = await _userService.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded) return new Response<object>(MessageResource.Error_FailedToAssignRoleToUser);
            return new Response<object>(true,MessageResource.Info_SuccessfullProcess ,string.Empty , result , HttpStatusCode.OK );
        }
        #endregion

        #region [- Task<IResponse<object>> DeleteUserRole(DeleteUserRoleAppDto model) -]
        public async Task<IResponse<object>> DeleteUserRole(DeleteUserRoleAppDto model)
        {
            if (model.Equals(null)) return new Response<object>(MessageResource.Error_ModelNull);
            if (model.UserId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.RoleId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            var user = await _userService.FindByIdAsync(model.UserId);
            if (user == null) return new Response<object>(MessageResource.Error_UserNotFound);
            var role = await _roleService.FindByIdAsync(model.RoleId);
            if (role == null) return new Response<object>(MessageResource.Error_RoleNotFound);
            var result = await _userService.RemoveFromRoleAsync(user, role.Name);
            if (!result.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
        }
        #endregion

        #region [-Other Methodes-]

        #region [-Task<bool> HasUserRole(string userId, string roleName)-]
        public async Task<bool> HasUserRole(string userId, string roleName)
        {
            var appUser = await _userService.FindByIdAsync(userId);
            return roleName != null && appUser != null && !await _userService.IsInRoleAsync(appUser, roleName) ? false : true;
        }
        #endregion
       
        #region [-Task<IResponse<object>> ResetPassword(ResetPassDto resetPassDto)-]
        public async Task<IResponse<object>> ResetPassword(ResetPassDto model)
        {
           
            #region[- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_ModelNull);
            if (model.Password == null || model.ConfirmPassword == null || model.UserName == null) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion
            
            #region [-Task-]
            var user = await _userService.FindByNameAsync(model.UserName);
            var token = await _userService.GeneratePasswordResetTokenAsync(user);
            if (user == null) return new Response<object>(MessageResource.Error_UserNotFound);
            var result = await _userService.ResetPasswordAsync(user, token, model.Password);
            #endregion
           
            #region [-Result-]
            if (!result.Succeeded) return new Response<object>(MessageResource.Error_ModelNull);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK); 
            #endregion
        } 
        #endregion

        #endregion
    }
}
