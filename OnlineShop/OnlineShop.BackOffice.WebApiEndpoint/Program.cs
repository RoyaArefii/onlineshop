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
using PublicTools.Tools;



var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:Default");

builder.Services.AddDbContext<OnlineShopDbContext>(c => c.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppUser, AppRole>(/* کل بلاک پسورد را میتوان اینجا کانفیگ کرد*/).AddEntityFrameworkStores<OnlineShopDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IAppProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IAppProductService, ProductService>();
builder.Services.AddScoped<IAppOrderHeaderService, OrderHeaderService>();
builder.Services.AddScoped<IAppOrderDetailService, OrderDdetailService>();
builder.Services.AddScoped<IRepository<Product, Guid>, BaseRepository<OnlineShopDbContext, Product, Guid>>();
builder.Services.AddScoped<IRepository<ProductCategory, Guid>, BaseRepository<OnlineShopDbContext, ProductCategory, Guid>>();
builder.Services.AddScoped<IRepository<OrderHeader, Guid>, BaseRepository<OnlineShopDbContext, OrderHeader, Guid>>();
builder.Services.AddScoped<IRepository<OrderDetail, Guid>, BaseRepository<OnlineShopDbContext, OrderDetail, Guid>>();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
