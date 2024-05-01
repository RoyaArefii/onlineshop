using Microsoft.AspNetCore.Identity;
using OnlineShop.Application.Contracts.UserManagementContracts;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.RepositoryDesignPatern.Services.UserManagement;
using OnlineShopDomain.Aggregates.Sale;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;

namespace OnlineShop.Application.Services.UserManagmentServices
{
    public class UserService : IAppUserService 
    {
        private readonly UserRepository _repository;
        private readonly UserManager<AppUser> _userManagerRep;

        #region [-Ctor-]
        public UserService(UserRepository repository , UserManager<AppUser> userManager )
        {
            _repository = repository;
            _userManagerRep = userManager;
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(string id)-]
        public  async Task<IResponse<object>> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            }
            var userDelete = _userManagerRep.FindByIdAsync(id); 
            if (userDelete == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var result = await _repository.DeleteAsync(id);
            if (!result.IsSuccessful)
            {
                return new Response<object>(MessageResource.Error_FailProcess);
            }
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, result, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(DeleteUserAppDto model)-]
        public async Task<IResponse<object>> DeleteAsync(DeleteUserAppDto model)
        {
            var deleteAppUser = new AppUser
            {
                Id = model.Id,

            };
            if (deleteAppUser == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var resultDelete = await _repository.DeleteAsync(deleteAppUser);
            if (!resultDelete.IsSuccessful)
                return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, deleteAppUser, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<GetUserAppDto>> FindById(string id)-]

        public async Task<IResponse<GetUserAppDto>> FindById(string id)
        {
            #region [-Validation-]
            if (id.Equals(null)) return new Response<GetUserAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var findResult = await _repository.FindById(id);
            var findAppUser = new GetUserAppDto()
            {
                Id = findResult.Result.Id,
                FirstName = findResult.Result.FirstName,
                LastName = findResult.Result.LastName,
                Cellphone = findResult.Result.Cellphone,
            };
            #endregion

            #region [-Result-]
            if (!findResult.IsSuccessful) return new Response<GetUserAppDto>(MessageResource.Error_FailProcess);
            return new Response<GetUserAppDto>(true, MessageResource.Info_SuccessfullProcess, string.Empty, findAppUser, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<List<GetUserAppDto>>> GetAsync()-]
        public async Task<IResponse<List<GetUserAppDto>>> GetAsync()
        {
            var getResult = await _repository.Select();
            if (!getResult.IsSuccessful) return new Response<List<GetUserAppDto>>(MessageResource.Error_FailProcess);
            var getAppUserList = new List<GetUserAppDto>();
            var getAppUsers = getResult.Result.Select(item => new GetUserAppDto()
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Cellphone = item.Cellphone,
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
            if (model.Cellphone.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var postAppUser = new AppUser
            {
                Id = model.Id,
                FirstName = model.FirstName,        
                LastName = model.LastName,  
                Cellphone = model.Cellphone, 
            };
            if (postAppUser == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var postResult = await _repository.InsertAsync(postAppUser);
            #endregion

            #region [-Result-] 
            if (!postResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
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
            #endregion

            #region [-Task-]
            var putAppUser = new AppUser
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Cellphone = model.Cellphone,
            };
            if (putAppUser == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putResult = await _repository.UpdateAsync(putAppUser);
            #endregion

            #region [-Result-] 
            if (!putResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, putResult, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-async Task SaveChanges()-]
        public async Task SaveChanges()
        {
            await _repository.SaveChanges();
        }
        #endregion
    }
}
