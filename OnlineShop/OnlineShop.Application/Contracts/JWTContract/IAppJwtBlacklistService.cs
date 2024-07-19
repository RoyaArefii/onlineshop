using OnlineShop.Application.Dtos.JWT;
using OnlineShop.RepositoryDesignPatern.Frameworks.Contracs.JWT;
using OnlineShopDomain.Aggregates.JWT;


namespace OnlineShop.Application.Contracts.JWT
{
    public interface IAppJwtBlacklistService:IApplicationService<BlackListToken , PutBlacklistTokensAppDto , GetBlacklistTokensAppDto, PostBlacklistTokensAppDto , DeleteBlacklistTokensAppDto, Guid>
    {
        Task<bool> IsInBlacklist(string token);
    }
}
