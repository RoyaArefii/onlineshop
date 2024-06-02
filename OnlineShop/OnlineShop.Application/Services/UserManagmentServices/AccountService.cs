using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OnlineShop.Application.Dtos.UserManagementAppDtos.AccountDtos;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using ResponseFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Services.UserManagmentServices
{
    public class AccountService : IdentityUserLogin<string>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        //private readonly SignInManager<AppUser> _signInManager;

        public AccountService(UserManager<AppUser> userManager /*, SignInManager<AppUser> signInManager*/, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            //_signInManager = signInManager; 
        }

        // برای حالت کوکی
        //public async Task<IResponse<object>> Login(LoginDto model)
        //{
        //    if (model == null) return new Response<object>(MessageResource.Error_ModelNull);
        //    var userResult = await _userManager.FindByNameAsync(model.UserName); 
        //    if (userResult == null)return new Response<object>(MessageResource.Error_UserNotFound);
        //    var passwordResult = await _userManager.CheckPasswordAsync(userResult , model.Password);
        //    if (!passwordResult)return new Response<object>(MessageResource.Error_WrongPassword);


        //    var resultLogin= await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);
        //    if (!resultLogin.Succeeded) return new Response<object>(MessageResource.Error_FailProcess);
        //    return new Response<object>(true , MessageResource.Info_SuccessfullProcess , string.Empty , resultLogin , HttpStatusCode.OK);              
        //}

        public async Task<IResponse<string>> Login(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var password = model.Password;
            var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, password);

            if (passwordIsCorrect == true)
            {
                var jwtBuilder = JwtBuilder.Create();
                jwtBuilder.AddClaim("name", user.UserName);
                jwtBuilder.AddClaim("age", "80")
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    //.WithSecret(_configuration.GetSection("JWT:Secret").Value);
                    .WithSecret(_configuration["JWT:Secret"]);

                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var userRole in userRoles)
                {
                    jwtBuilder.AddClaim(ClaimTypes.Role, userRole);
                }

                var str = jwtBuilder.Encode();

               return new Response<string>(true , MessageResource.Info_SuccessfullProcess , string.Empty , str , HttpStatusCode.OK);              
            }

            return new Response<string>(false, string.Empty, MessageResource.Error_FailProcess, null, HttpStatusCode.OK);

        }
    }
}
