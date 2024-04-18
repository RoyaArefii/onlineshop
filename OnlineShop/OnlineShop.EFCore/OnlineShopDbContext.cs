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
            // این برای ایجاد تنظیمات ثابتی است که برای همه جداول استفاده می شود مثلا برای 
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            /// In code schema default ro UserManagement mizare va barayae in neveshte shode ke nemishe schema Identity
            /// ro bedoone in harekat set kard
            builder.HasDefaultSchema(DatabaseConstants.Schemas.UserManagement);

            #region [- ApplyConfigurationsFromAssembly() -]
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            #endregion
            /// in code ham mige har entity ee ke IDbSetEntity ro implement karde bood dar database barash table besaz
            #region [- RegisterAllEntities() -]
            builder.RegisterAllEntities<IDbSetEntity>(typeof(IDbSetEntity).Assembly);
            #endregion

            builder.HasDefaultSchema(DatabaseConstants.Schemas.UserManagement);
            base.OnModelCreating(builder);
        }
    }
}
