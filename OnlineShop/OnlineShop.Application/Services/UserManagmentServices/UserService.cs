using OnlineShop.Application.Contracts.UserManagementContracts;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;


namespace OnlineShop.Application.Services.UserManagmentServices
{
    public class UserService : IAppUserService
    {
        private readonly IRepository<AppUser, string> _repository;

        public UserService(IRepository<AppUser, string> repository)
        {
            _repository = repository;
        }

        #region [-Delete(DeleteUserAppDto model)-]
        public async Task<IResponse<object>> Delete(DeleteUserAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_TheParameterIsNull);
            #endregion

            #region [-Task-]
            var deleteUser = await _repository.FindById(model.Id);
            var result = await _repository.DeleteAsync(deleteUser.ToString());
            #endregion

            #region [- Return -]

            return new Response<object>(true,
                                        MessageResource.Info_SuccessfullProcess,
                                        string.Empty,
                                        new DeleteUserResultAppDto()
                                        {
                                            Id = model.Id,
                                            UserName = model.UserName,
                                            IsDeleted = true,
                                        },
                                        HttpStatusCode.OK);
            #endregion

        }
        #endregion
        public Task<IResponse<List<GetUserAppDto>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<object>> Post(PostUserAppDto model)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<object>> Put(PutUserAppDto model)
        {
            throw new NotImplementedException();
        }
    }
}
