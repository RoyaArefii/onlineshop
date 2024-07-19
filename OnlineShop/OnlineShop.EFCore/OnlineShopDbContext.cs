using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.EFCore.Frameworks;
using OnlineShopDomain.Aggregates.UserManagement;
using OnlineShopDomain.Frameworks.Abstracts;
using PublicTools.Constants;
using System.Reflection;

 

namespace OnlineShop.EFCore
{
    public class OnlineShopDbContext : IdentityDbContext<AppUser, AppRole, string,
        IdentityUserClaim<string>, AppUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options) : base(options)
        {
        }

        protected OnlineShopDbContext()
        {
        }
        #region [- ConfigureConventions(ModelConfigurationBuilder configurationBuilder) -]
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
         
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.HasDefaultSchema(DatabaseConstants.Schemas.UserManagement);

            #region [- ApplyConfigurationsFromAssembly() -]
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            #endregion

            #region [- RegisterAllEntities() -]
            builder.RegisterAllEntities<IDbSetEntity>(typeof(IDbSetEntity).Assembly);
            #endregion

            builder.HasDefaultSchema(DatabaseConstants.Schemas.UserManagement);
            base.OnModelCreating(builder);
        }
    }
}
