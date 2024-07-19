using OnlineShop.EFCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShopDomain.Aggregates.UserManagement;
using OnlineShop.Application.Services.SaleServices;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.RepositoryDesignPatern.Services.Sale;
using OnlineShop.RepositoryDesignPatern.Frameworks.Contracs.SaleContracts;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using onlineshop.repositorydesignpatern.frameworks.bases;
using OnlineShopDomain.Aggregates.Sale;
using OnlineShop.Application.Services.UserManagmentServices;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos;
using OnlineShop.Application.Services.Account;
using OnlineShop.BackOffice.WebApiEndpoint.Middlewares;
using OnlineShop.Application.Contracts.JWT;
using OnlineShopDomain.Aggregates.JWT;
using OnlineShop.RepositoryDesignPatern.Frameworks.Contracs.JWT;
using OnlineShop.RepositoryDesignPatern.Services.JWT;



var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:Default");

builder.Services.AddDbContext<OnlineShopDbContext>(c => c.UseSqlServer(connectionString));

builder.Services.AddIdentity<AppUser, AppRole>(/* کل بلاک پسورد را میتوان اینجا کانفیگ کرد*/)
    .AddEntityFrameworkStores<OnlineShopDbContext>().AddDefaultTokenProviders();



builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

})
    //.AddJwtBearer(options =>
    //{
    //    options.SaveToken = true;
    //    options.RequireHttpsMetadata = false;
    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuer = false,
    //        ValidateAudience = false,
    //        ValidateLifetime = false,
    //        ValidateIssuerSigningKey = false,
    //        //RequireAudience = false,
    //        //RequireExpirationTime = false,
    //        ValidAudience = builder.Configuration["JWT:Audience"],
    //        ValidIssuer = builder.Configuration["JWT:Issuer"],
    //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
    //        //RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/rol"
    //    };
    //});
    .AddJwtBearer(options =>
     {
         options.SaveToken = true;
         options.RequireHttpsMetadata = false;
         options.TokenValidationParameters = new TokenValidationParameters()
         {
             RequireExpirationTime = false,
             RequireAudience = false,
             ValidateIssuer = false,
             ValidateAudience = false,
             //ValidAudience = builder.Configuration["JWT:ValidAudience"],
             //ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
         };
     });

//builder.Services.AddSingleton<JwtBlacklistTokenService>();
//builder.Services.AddSingleton<IJwtBlacklistTokenService,JwtBlacklistTokenService>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRepository<Product, Guid>, BaseRepository<OnlineShopDbContext, Product, Guid>>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IAppProductService, ProductService>();
 
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IRepository<ProductCategory, Guid>, BaseRepository<OnlineShopDbContext, ProductCategory, Guid>>();
builder.Services.AddScoped<IAppProductCategoryService, ProductCategoryService>();

builder.Services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IAppOrderService< GetAllOrderAppDto, GetOrdersAppDto>, OrderService>();
builder.Services.AddScoped<IRepository<OrderHeader, Guid>, BaseRepository<OnlineShopDbContext, OrderHeader, Guid>>();
builder.Services.AddScoped<IRepository<OrderDetail, Guid>, BaseRepository<OnlineShopDbContext, OrderDetail, Guid>>();
builder.Services.AddScoped<OrderService>();

//builder.Services.AddScoped<IAppOrderDetailService, OrderDdetailService>();

builder.Services.AddScoped<IJwtRepository,JwtTokenManagement >();
builder.Services.AddScoped<IRepository<BlackListToken, Guid>, BaseRepository<OnlineShopDbContext, BlackListToken, Guid>>();
builder.Services.AddScoped<IAppJwtBlacklistService, JwtBlacklistService>();
//builder.Services.AddScoped<JwtBlacklistTokenService>();
//builder.Services.AddScoped<IRepository<ProductCategory, Guid>>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<UserRoleService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<BlacklistMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
