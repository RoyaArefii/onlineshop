using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Dtos.UserManagementAppDtos.RoleAppDtos;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using PublicTools.Tools;
using ResponseFramework;
using System.Data;
using System.Net;


namespace OnlineShop.Application.Services.UserManagmentServices
{
    public class RoleService
    {
        #region [-Ctor & Fields-]
        private readonly RoleManager<AppRole> _roleRepository;
        private readonly UserManager<AppUser> _userRepository;
        public RoleService(RoleManager<AppRole> roleRepository, UserManager<AppUser> userManager)
        {
            _roleRepository = roleRepository;
            _userRepository = userManager;
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(string id)-]
        public async Task<IResponse<object>> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            }
            var role = await _roleRepository.FindByIdAsync(id);
            if (Helpers.IsDeleted(role) || role == null) return new Response<object>(true, string.Empty, MessageResource.Error_RoleNotFound, null, HttpStatusCode.OK);

            var roleDelete = new AppRole()
            {
                Id = role.Id,
                DateSoftDeletedLatin = DateTime.Now,
                DateSoftDeletedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                IsDeleted = true
            };
            var result = await _roleRepository.UpdateAsync(roleDelete);
            if (!result.Succeeded)
            {
                return new Response<object>(MessageResource.Error_FailProcess);
            }
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(DeleteRoleAppDto model)-]
        public async Task<IResponse<object>> DeleteAsync(DeleteRoleAppDto model)
        {
            #region [-Validation-]
            if (model.Id == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var appRole = await _roleRepository.FindByIdAsync(model.Id);
            if (Helpers.IsDeleted(appRole) || appRole == null) return new Response<object>(true, string.Empty, MessageResource.Error_RoleNotFound, null, HttpStatusCode.OK);
            if (appRole.Name == "GodAdmin") return new Response<object>(MessageResource.Error_GodAdminRole);
            var userLogin = await _userRepository.FindByNameAsync(model.UserName);
            if (Helpers.IsDeleted(userLogin) || userLogin == null) return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);
            if (!await _userRepository.IsInRoleAsync(userLogin, "GodAdmin")) return new Response<object>(MessageResource.Error_Accessdenied);
            #endregion

            #region [-Task-]
            appRole.IsDeleted = true;
            appRole.DateSoftDeletedLatin = DateTime.UtcNow;
            appRole.DateSoftDeletedPersian = Helpers.ConvertToPersianDate(DateTime.Now);
            var resultDelete = await _roleRepository.UpdateAsync(appRole);          
            #endregion
            
            #region [-Result-]
            if (!resultDelete.Succeeded)
                return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, appRole, HttpStatusCode.OK);
            #endregion
        }
        #endregion
       
        #region [-Task<IResponse<object>> PostAsync(PostRoleAppDto model)-]
        public async Task<IResponse<object>> PostAsync(PostRoleAppDto model)
        {
            
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Name.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            var userLogin = await _userRepository.FindByNameAsync(model.UserName);
            if (Helpers.IsDeleted(userLogin) || userLogin == null) return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);
            if (!await _userRepository.IsInRoleAsync(userLogin, "GodAdmin")) return new Response<object>(MessageResource.Error_Accessdenied);
            #endregion 
            
            #region [-Task-]
            var postAppRole = new AppRole
            {
                Name = model.Name,
                EntityDescription = model.EntityDescription,
                IsActive = true,
                IsDeleted = false,
                DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                DateCreatedLatin = DateTime.Now,
            };
            if (postAppRole == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var postResult = await _roleRepository.CreateAsync(postAppRole);
            #endregion
           
            #region [-Result-] 
            if (!postResult.Succeeded) return new Response<object>(postResult.Errors);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, postAppRole, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<List<GetRoleAppDto>>> GetAsync()-]
        public async Task<IResponse<List<GetRoleAppDto>>> GetAsync()
        {
            var getResult = await _roleRepository.Roles.Where(p=>p.IsDeleted==false).ToListAsync();
            var getAppRoleList = new List<GetRoleAppDto>();
            foreach (var role in getResult)
            {
                var getAppRoles = new GetRoleAppDto()
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsActive = role.IsActive,
                    IsDeleted = role.IsDeleted,
                    IsModified = role.IsModified,
                    EntityDescription = role.EntityDescription,
                    DateCreatedLatin = role.DateCreatedLatin,
                    DateCreatedPersian = role.DateCreatedPersian,
                    DateModifiedLatin = role.DateCreatedLatin,
                    DateModifiedPersian = role.DateModifiedPersian,
                    DateSoftDeletedLatin = role.DateSoftDeletedLatin,
                    DateSoftDeletedPersian = role.DateSoftDeletedPersian
                };
                getAppRoleList.Add(getAppRoles);
            }
            return new Response<List<GetRoleAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getAppRoleList, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> PutAsync(PutRoleAppDto model)-]
        public async Task<IResponse<object>> PutAsync(PutRoleAppDto model)
        {
            
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Name.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.IsActive.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);

            var role = await _roleRepository.FindByIdAsync(model.Id);
            if (Helpers.IsDeleted(role) || role == null) return new Response<object>(true, string.Empty, MessageResource.Error_RoleNotFound, null, HttpStatusCode.OK);
            if (role.Name == "GodAdmin") return new Response<object>(MessageResource.Error_GodAdminRole);

            var userLogin = await _userRepository.FindByNameAsync(model.UserName);
            if (Helpers.IsDeleted(userLogin) || userLogin == null) return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);
            var accessFlag = false;
            if (!await _userRepository.IsInRoleAsync(userLogin, "GodAdmin")) return new Response<object>(MessageResource.Error_Accessdenied);
            #endregion
            
            #region [-Task-]
            var putAppRole = role;

            putAppRole.Id = model.Id;
            putAppRole.Name = model.Name;
            putAppRole.IsActive = model.IsActive;
            putAppRole.EntityDescription = model.EntityDescription;
            putAppRole.IsModified = true;
            putAppRole.DateModifiedLatin = DateTime.Now;
            putAppRole.DateModifiedPersian = Helpers.ConvertToPersianDate(DateTime.Now);
            

            if (putAppRole == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putResult = await _roleRepository.UpdateAsync(putAppRole);
            #endregion
            
            #region [-Result-] 
            if (!putResult.Succeeded) return new Response<object>(putResult.Errors);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, putAppRole, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<GetRoleAppDto>> FindById(GetRoleAppDto model)-]
        public async Task<IResponse<GetRoleAppDto>> FindById(GetRoleAppDto model)
        {
            #region [-Validation-]
            if (model.Id.Equals(null)) return new Response<GetRoleAppDto>(MessageResource.Error_ThisFieldIsMandatory);

            var userLogin = await _userRepository.FindByNameAsync(model.UserName);
            if (Helpers.IsDeleted(userLogin) || userLogin == null) return new Response<GetRoleAppDto>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);

            var isGodAdmin = await _userRepository.IsInRoleAsync(userLogin, "GodAdmin");
            if (!isGodAdmin) return new Response<GetRoleAppDto>(MessageResource.Error_Accessdenied);

            var findResult = await _roleRepository.FindByIdAsync(model.Id);
            if (Helpers.IsDeleted(findResult) || findResult == null) return new Response<GetRoleAppDto>(true, string.Empty, MessageResource.Error_RoleNotFound, null, HttpStatusCode.OK);
            #endregion

            #region [-Task-]
            var findAppRole = new GetRoleAppDto()
            {
                Id = findResult.Id,
                Name = findResult.Name,
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
            return new Response<GetRoleAppDto>(true, MessageResource.Info_SuccessfullProcess, string.Empty, findAppRole, HttpStatusCode.OK);
            #endregion
        }
        #endregion

    }
}
