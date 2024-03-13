using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using ResponseFramework;


namespace OnlineShop.Application.Contracts.UserManagementContracts
{
    public interface IAppUserService
    {
        Task<IResponse<object>> Post(PostUserAppDto model);
        Task<IResponse<object>> Put(PutUserAppDto model);
        Task<IResponse<List<GetUserAppDto>>> GetAll();
        Task<IResponse<object>> Delete(DeleteUserAppDto model);
    }
}
