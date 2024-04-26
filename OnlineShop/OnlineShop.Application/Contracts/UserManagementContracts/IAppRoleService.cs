using OnlineShop.Application.Dtos.UserManagementAppDtos.RoleAppDtos;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShopDomain.Aggregates.UserManagement;
using ResponseFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Contracts.UserManagementContracts
{
    public interface IAppRoleService :IApplicationService<AppRole , PutRoleAppDto , GetRoleAppDto , PostRoleAppDto, DeleteRoleAppDto , string>
    {
    }
}
