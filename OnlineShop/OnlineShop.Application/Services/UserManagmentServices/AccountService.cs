using Microsoft.AspNetCore.Identity;
using OnlineShop.Application.Dtos.UserManagementAppDtos.AccountDtos;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using ResponseFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Services.UserManagmentServices
{
    public class AccountService :IdentityUserLogin<string>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountService(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
        }
        public async Task<IResponse<object>> Login(LoginDto model)
        {
            if (model == null) return new Response<object>(MessageResource.Error_ModelNull);
            var userResult = await _userManager.FindByNameAsync(model.UserName); 
            if (userResult == null)return new Response<object>(MessageResource.Error_UserNotFound);
            var passwordResult = await _userManager.CheckPasswordAsync(userResult , model.Password);
            if (!passwordResult)return new Response<object>(MessageResource.Error_WrongPassword);


            var resultLogin= await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);
            if (!resultLogin.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true , MessageResource.Info_SuccessfullProcess , string.Empty , resultLogin , HttpStatusCode.OK);              
        }
    }
}
