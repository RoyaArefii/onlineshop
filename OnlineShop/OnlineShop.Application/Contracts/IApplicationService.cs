using OnlineShop.Application.Dtos.UserManagementAppDtos.RoleAppDtos;
using OnlineShopDomain.Frameworks.Abstracts;
using ResponseFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Contracts
{
    public interface IApplicationService<T, TPut, TGet, TPost , TDelete>
    {
        Task<IResponse<object>> PutAsync(TPut model);
        Task<IResponse<object>> PostAsync(TPost model);
        Task<IResponse<object>> DeleteAsync(string id);
        Task<IResponse<object>> DeleteAsync(TDelete model);
        Task<IResponse<List<TGet>>> GetAsync();
        Task<IResponse<TGet>> FindById(string id);
        Task SaveChanges();
    }
}
