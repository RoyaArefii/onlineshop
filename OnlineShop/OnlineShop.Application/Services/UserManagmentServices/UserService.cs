using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;

namespace OnlineShop.Application.Services.UserManagmentServices
{
    public class UserService //: IAppUserService 
    {
        //private readonly UserRepository _repository;
        private readonly UserManager<AppUser> _repository;

        #region [-Ctor-]
        public UserService(UserManager<AppUser> repository)
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
            var userDelete = await _repository.FindByIdAsync(id);
            if (userDelete == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var result = await _repository.DeleteAsync(userDelete);
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
            var appUser = await _repository.FindByIdAsync(model.Id);
            if (appUser == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var resultDelete = await _repository.DeleteAsync(appUser);
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
            var findResult = await _repository.FindByIdAsync(id);
            if (findResult == null) return new Response<GetUserAppDto>(MessageResource.Error_FailProcess);
            var findAppUser = new GetUserAppDto()
            {
                Id = findResult.Id,
                UserName = findResult.Cellphone,
                FirstName = findResult.FirstName,
                LastName = findResult.LastName,
                Cellphone = findResult.Cellphone,
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
            var getResult = await _repository.Users.ToListAsync();
            //if (!getResult.IsSuccessful) return new Response<List<GetUserAppDto>>(MessageResource.Error_FailProcess);????????????
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
            }).ToList();

            return new Response<List<GetUserAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getAppUsers, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> PostAsync(PostUserAppDto model)-]
        public async Task<IResponse<object>> PostAsync(PostUserAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.FirstName.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.LastName.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Password.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.ConfirmPassword.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Cellphone.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.IsActive.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.IsModified.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.IsDeleted.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.DateCreatedLatin.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.DateCreatedPersian.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion 
            #region [-Task-]
            var postAppUser = new AppUser
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Cellphone,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Cellphone = model.Cellphone,
                Picture = model.Picture,
                Location = model.Location,
                IsActive = model.IsActive,
                IsModified = model.IsModified,
                IsDeleted = model.IsDeleted,
                DateCreatedPersian = model.DateCreatedPersian,
                DateCreatedLatin = model.DateCreatedLatin,
                DateModifiedLatin = model.DateModifiedLatin,
                DateModifiedPersian = model.DateModifiedPersian,
                DateSoftDeletedLatin = model.DateSoftDeletedLatin,
                DateSoftDeletedPersian = model.DateSoftDeletedPersian,

            };
            if (postAppUser == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var postResult = await _repository.CreateAsync(postAppUser ,postAppUser.Password);
            #endregion

            #region [-Result-] 
            if (!postResult.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
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
            if (model.IsModified.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.DateModifiedLatin.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.DateModifiedPersian.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var user = _repository.FindByIdAsync(model.Id);
            if ((user == null)) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putAppUser = user.Result;

            putAppUser.Id = model.Id;
            putAppUser.FirstName = model.FirstName;
            putAppUser.LastName = model.LastName;
            putAppUser.Cellphone = model.Cellphone;
            putAppUser.UserName = model.Cellphone;
            putAppUser.Picture = model.picture;
            putAppUser.Location = model.Location;       
            putAppUser.IsActive = model.IsActive;
            putAppUser.IsModified = model.IsModified;
            putAppUser.DateModifiedLatin = model.DateModifiedLatin; 
            putAppUser.DateModifiedPersian = model.DateModifiedPersian; 

            if (putAppUser == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putResult = await _repository.UpdateAsync(putAppUser);
            #endregion

            #region [-Result-] 
            if (!putResult.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, putResult, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-async Task SaveChanges()-]?????????????
        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
