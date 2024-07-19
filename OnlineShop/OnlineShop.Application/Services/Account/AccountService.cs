using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Contracts.JWT;
using OnlineShop.Application.Dtos.JWT;
using OnlineShop.Application.Dtos.UserManagementAppDtos.AccountDtos;
using OnlineShop.Application.Dtos.UserManagementAppDtos.UserAppDtos;
using OnlineShop.Application.Services.UserManagmentServices;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;
using System.Security.Cryptography;

namespace OnlineShop.Application.Services.Account
{
    public class AccountService : IdentityUserLogin<string>
    {
        #region [- Ctor & Fields -]
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IAppJwtBlacklistService _jwtBlacklist;
        private readonly UserService _userService;

        public AccountService(UserManager<AppUser> userManager,
            IConfiguration configuration,
            IAppJwtBlacklistService jwtBlacklist,
            UserService userService
            /*,IdentityUserClaim<string> userClaim*/)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtBlacklist = jwtBlacklist;
            _userService = userService;
            // _userClaim = userClaim;
        }
        #endregion

        #region [- Login -]
        public async Task<IResponse<object>> Login(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null || user.IsActive == false || user.IsDeleted == true) return new Response<object>(MessageResource.Error_UserNotFound);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0) return new Response<object>(MessageResource.Error_RoleNotFound);
            var password = model.Password;
            var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, password);

            if (passwordIsCorrect == true)
            {
                var token = GenerateJwtToken(user.UserName, roles);
                var refreshToken = GenerateRefreshToken();
                return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, token/* new { token, refreshToken }*/, HttpStatusCode.OK);
            }

            return new Response<object>(false, string.Empty, MessageResource.Error_FailProcess, null, HttpStatusCode.OK);
        }
        #endregion

        #region [- Token -]
        private string GenerateJwtToken(string userName, IList<string> roles)
        {
            var jwtBuilder = JwtBuilder.Create();
            jwtBuilder.AddClaim("Name", userName)
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_configuration["JWT:Secret"])
                .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());

            foreach (var userRole in roles)
            {
                jwtBuilder.AddClaim("roles", userRole);
            }
            return jwtBuilder.Encode();
        }
        private string GenerateRefreshToken()
        {
            var rndNumbeer = new byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(rndNumbeer);
            return Convert.ToBase64String(rndNumbeer);
        }
        #endregion

        #region [- Logout -]
        public async Task<IResponse<object>> LogOut(LogoutDto model)
        {
            var postModel = new PostBlacklistTokensAppDto();
            if (model.Token != null)
            {
                postModel.ExpireDate = DateTime.Now;
                postModel.Token = model.Token;
            }
            var result = await _jwtBlacklist.PostAsync(postModel);
            await _jwtBlacklist.SaveChanges();
            if (result == null) return new Response<object>(MessageResource.Info_LogoutSuccessFul);
            return new Response<object>(true, string.Empty, MessageResource.Info_LogoutSuccessFul, result, HttpStatusCode.OK);
        }
        #endregion
       
        #region [-Signin-]
        public async Task<IResponse<object>> Signin(PostUserAppDto model)
        {
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.FirstName.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.LastName.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Password.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.ConfirmPassword.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Cellphone.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            var result = await _userService.PostAsync(model);
            if (!result.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            var loginModel = new LoginDto()
            {
                UserName = model.Cellphone,
                Password = model.Password,
            };
            var loginResult = await Login(loginModel);
            if (!loginResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(loginResult);
        }
        #endregion

        #region [-Signout-]
        public async Task<IResponse<object>> Signout(SignoutDto model/*DeleteUserAppDto*/ )
        {
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Token.IsNullOrEmpty() || model.UserName.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);

            var loogoutModel = new LogoutDto()
            {
                Token = model.Token
            };

            var deleteModel = await _userManager.FindByNameAsync(model.UserName);
            var postModel = new PostBlacklistTokensAppDto();
            if (model.Token != null)
            {
                postModel.ExpireDate = DateTime.Now;
                postModel.Token = model.Token;
            }
            try
            {
                var logoutResult = await _jwtBlacklist.PostAsync(postModel);
                if (!logoutResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);

                var result = await _userService.DeleteAsync(deleteModel.Id);
                await _jwtBlacklist.SaveChanges();
                if (!result.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            }
            catch (Exception)
            {
                return new Response<object>(MessageResource.Error_FailProcess);
            }

            return new Response<object>(true, MessageResource.Info_SignoutSuccessFull, string.Empty, null, HttpStatusCode.OK);
        }
        #endregion

        #region [-Cookie-]
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
        #endregion

    }
}
