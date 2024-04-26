using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShopDomain.Aggregates.UserManagement;
using ResponseFramework;


namespace OnlineShop.Application.Contracts.UserManagementContracts
{
    public interface IAppUserService : IApplicationService<AppUser , PutUserAppDto , GetUserAppDto, PostUserAppDto, DeleteUserAppDto ,  string>
    {
    }
}
