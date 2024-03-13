using OnlineShop.Application.Dtos.UserManagementAppDtos.RoleAppDtos;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using ResponseFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Contracts.UserManagementContracts
{
    public interface IAppRoleService
    {
        Task<IResponse<object>> Post(PostRoleAppDto model);
        Task<IResponse<object>> Put(PutRoleAppDto model);
        Task<IResponse<List<GetRoleAppDto>>> GetAll();
        Task<IResponse<object>> Delete(DeleteRoleAppDto model);
    }
}
