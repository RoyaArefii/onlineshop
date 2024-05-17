using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Dtos.UserManagementAppDtos.RoleAppDtos;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using PublicTools.Tools;
using ResponseFramework;
using System.Net;


namespace OnlineShop.Application.Services.UserManagmentServices
{
    public class RoleService
    {
        private readonly RoleManager<AppRole> _repository;

        #region [-Ctor-]
        public RoleService(RoleManager<AppRole> repository)
        {
            _repository = repository;
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(string id)-]
        public async Task<IResponse<object>> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            }
            var roleDelete = await _repository.FindByIdAsync(id);
            if (roleDelete == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var result = await _repository.DeleteAsync(roleDelete);
            if (!result.Succeeded)
            {
                return new Response<object>(MessageResource.Error_FailProcess);
            }
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(DeleteUserAppDto model)-]
        public async Task<IResponse<object>> DeleteAsync(DeleteRoleAppDto model)
        {
            var appRole = await _repository.FindByIdAsync(model.Id);
            if (appRole == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var resultDelete = await _repository.DeleteAsync(appRole);
            if (!resultDelete.Succeeded)
                return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, appRole, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> PostAsync(PostRoleAppDto model)-]
        public async Task<IResponse<object>> PostAsync(PostRoleAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Name.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion 
            #region [-Task-]
            var postAppRole = new AppRole
            {
                Name = model.Name,
                EntityDescription = model.EntityDescription,
                IsActive = true,
                IsDeleted = false,
                DateCreatedPersian =Helpers.ConvertToPersianDate(DateTime.Now),
                DateCreatedLatin = DateTime.Now,
            };
            if (postAppRole == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var postResult = await _repository.CreateAsync(postAppRole);
            #endregion

            #region [-Result-] 
            if (!postResult.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, postResult, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<List<GetUserAppDto>>> GetAsync()-]
        public async Task<IResponse<List<GetRoleAppDto>>> GetAsync()
        {
            var getResult = await _repository.Roles.ToListAsync();  
            //if (!getResult.IsSuccessful) return new Response<List<GetUserAppDto>>(MessageResource.Error_FailProcess);????????????
            var getAppRoleList = new List<GetRoleAppDto>();
            var getAppRoles = getResult.Select(item => new GetRoleAppDto()
            {
                Id = item.Id,   
                Name = item.Name,
                IsActive = item.IsActive,    
                IsDeleted = item.IsDeleted,  
                IsModified =item.IsModified,
                EntityDescription = item.EntityDescription,
                DateCreatedLatin = item.DateCreatedLatin,
                DateCreatedPersian = item.DateCreatedPersian,
                DateModifiedLatin = item.DateCreatedLatin,
                DateModifiedPersian = item.DateModifiedPersian,
                DateSoftDeletedLatin = (DateTime)item.DateSoftDeletedLatin,
                DateSoftDeletedPersian = item.DateSoftDeletedPersian
            }).ToList();
            return new Response<List<GetRoleAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getAppRoles, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> PutAsync(PutUserAppDto model)-]
        public async Task<IResponse<object>> PutAsync(PutRoleAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Name.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);

            #endregion

            #region [-Task-]
            var role = _repository.FindByIdAsync(model.Id);
            if ((role == null)) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putAppRole = role.Result;

            putAppRole.Id = model.Id;
            putAppRole.Name = model.Name;
            putAppRole.IsActive = model.IsActive;
            putAppRole.EntityDescription = model.EntityDescription; 
            putAppRole.IsModified = true;
            putAppRole.DateModifiedLatin = DateTime.Now;
            putAppRole.DateModifiedPersian = Helpers.ConvertToPersianDate(DateTime.Now);

            if (putAppRole == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putResult = await _repository.UpdateAsync(putAppRole);
            #endregion

            #region [-Result-] 
            if (!putResult.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, putResult, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<GetRoleAppDto>> FindById(string id)-]

        public async Task<IResponse<GetRoleAppDto>> FindById(string id)
        {
            #region [-Validation-]
            if (id.Equals(null)) return new Response<GetRoleAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var findResult = await _repository.FindByIdAsync(id);
            if (findResult == null) return new Response<GetRoleAppDto>(MessageResource.Error_FailProcess);
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

        //#region [-async Task SaveChanges()-]?????????????
        //public Task SaveChanges()
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion
    }
}
