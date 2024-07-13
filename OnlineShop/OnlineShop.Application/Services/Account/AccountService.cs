using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Dtos.UserManagementAppDtos.AccountDtos;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using ResponseFramework;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace OnlineShop.Application.Services.Account
{
    public class AccountService : IdentityUserLogin<string>
    {
        #region [- Ctor & Fields -]
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountService(UserManager<AppUser> userManager, IConfiguration configuration /*,IdentityUserClaim<string> userClaim*/)
        {
            _userManager = userManager;
            _configuration = configuration;
            // _userClaim = userClaim;
        }
        #endregion

        #region [Ok]
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
                var userRoles = await _userManager.GetRolesAsync(user);
                var jwtBuilder = JwtBuilder.Create();
                jwtBuilder.AddClaim("Name", user.UserName)
                //jwtBuilder.AddClaim("Roles", userRoles)
                //jwtBuilder.AddClaim("age", "80")
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    //.WithSecret(_configuration.GetSection("JWT:Secret").Value);
                    .WithSecret(_configuration["JWT:Secret"]);



                foreach (var userrole in userRoles)
                {
                    jwtBuilder.AddClaim("roles", userrole);
                }

                var token = jwtBuilder.Encode();

                return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, token, HttpStatusCode.OK);
            }
            #region [-Hediye ghadimi-]
            //if (passwordIsCorrect == true)
            //{
            //    var claims = new List<Claim>()
            //    {
            //        new Claim(ClaimTypes.Name,user.UserName)
            //    };

            //    //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            //    //var c = (roles.Select(role => new Claim(ClaimTypes.Role, role))).ToList();

            //    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            //    //var creds = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);
            //    //var token = new JwtSecurityToken(
            //    //    issuer: _configuration["JWT:Issuer"],
            //    //    audience: _configuration["JWT:Audience"],
            //    //    claims: claims,
            //    //    expires: DateTime.Now.AddMinutes(30),
            //    //    signingCredentials: creds
            //    //    );
            //    //var tokenString =new JwtSecurityTokenHandler().WriteToken(token);

            //    var jwtBuilder = JwtBuilder.Create();
            //    jwtBuilder.AddClaim("Name", user.UserName);
            //    //jwtBuilder.AddClaim("age", "80")
            //     //jwtBuilder.AddClaim("Role", string.Join(",", roles))
            //    jwtBuilder.AddClaim("Role", claims)
            //    .WithAlgorithm(new HMACSHA256Algorithm())
            //    .WithSecret(_configuration.GetSection("JWT:Secret").Value)
            //    .WithSecret(_configuration["JWT:Secret"]);

            //    foreach (var role in roles)
            //    {
            //        jwtBuilder.AddClaim("Role", role);
            //        //jwtBuilder.AddClaim(claim.Type, claim.Value);
            //    }
            //    var str = jwtBuilder.Encode();
            //    return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, str, HttpStatusCode.OK);
            //} 
            #endregion

            #region [Hediye]
            ///iiiiiiiiiinja:
            //var claims = new List<Claim>
            //    {
            //        new Claim(ClaimTypes.Name, user.UserName)

            //    };

            //foreach (var userRole in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, userRole));
            //}
            //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            //var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            //var jwtToken2 = new JwtSecurityToken(
            //            claims: claims,
            //            expires: DateTime.Now.AddMinutes(50),
            //            signingCredentials: signinCredentials
            //    );
            //var token = GenerateToken(claims);


            //var Token = new JwtSecurityTokenHandler().WriteToken(token);
            //var Expiration = token.ValidTo;
            ///ta injaaaaaaaaaa 
            #endregion

            return new Response<object>(false, string.Empty, MessageResource.Error_FailProcess, null, HttpStatusCode.OK);
        }

        #endregion
        private JwtSecurityToken GenerateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
        #region [- Logout -]
        public async Task<IResponse<object>> LogOut(LoginDto model)
        {

           
            return new Response<object>(false, string.Empty, MessageResource.Error_FailProcess, null, HttpStatusCode.OK);

        } 
        #endregion

        //private readonly UserRoleService _roleService;

        //private readonly SignInManager<AppUser> _signInManager;



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


        //public async Task<IResponse<object>> Login(LoginDto model)
        //{

        //    var user = await _userManager.FindByNameAsync(model.UserName);
        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Name, user.UserName),
        //    };

        //    var userRoles = await _userManager.GetRolesAsync(user);

        //    foreach (var userRole in userRoles)
        //    {

        //        claims.Add(new Claim("role", userRole));

        //        var role = await _roleManager.FindByNameAsync(userRole);

        //        if (role == null)
        //        {
        //            return new Response<object>(MessageResource.Error_RoleNotFound);
        //        }

        //        var roleClaims = await _roleManager.GetClaimsAsync(role);

        //        foreach (Claim roleClaim in roleClaims)
        //        {
        //            claims.Add(roleClaim);
        //        }
        //    }

        //   // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Issuer"],
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(30)
        //        //signingCredentials: creds
        //    );
        //    var jwtBuilder = new JwtBuilder();
        //    var str = jwtBuilder.Encode();
        //    return new Response<object>(str);   
        //}
    }
}
