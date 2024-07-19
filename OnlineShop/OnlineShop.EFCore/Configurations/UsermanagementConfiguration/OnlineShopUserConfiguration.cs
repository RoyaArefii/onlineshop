using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Constants;
using PublicTools.Tools;


namespace OnlineShop.EFCore.Configurations.IdentityConfiguration
{
    public class OnlineShopUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        #region [-Configuration-]
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable(nameof(AppUser)).HasData(
                        new AppUser
                        {
                            Id = DatabaseConstants.GodAdminUsers.ArefiUserId,
                            FirstName = DatabaseConstants.GodAdminUsers.ArefiFirstName,
                            LastName = DatabaseConstants.GodAdminUsers.ArefiLastName,
                            Cellphone = DatabaseConstants.GodAdminUsers.ArefiCellPhone,
                            UserName = DatabaseConstants.GodAdminUsers.ArefiCellPhone,
                            PasswordHash = "AQAAAAIAAYagAAAAEEA9Xu3Vqw19qGPCF+O/kiEMRnmVmS8D5bLILKtlYkiuzjIWlSa9c+qvrr1qnJdQdg==",
                            //Password :!QAZ1qaz
                            //DatabaseConstants.GodAdminUsers.ArefiPassword.GetHashCode().ToString(),
                            IsActive = true,
                            DateCreatedLatin = DateTime.Now,
                            DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                            IsDeleted = false,
                            IsModified = false,
                            NormalizedUserName = DatabaseConstants.GodAdminUsers.ArefiCellPhone.ToString(),
                            LockoutEnabled = true
                        });

            builder.Property(p => p.FirstName).IsRequired();
            builder.Property(p => p.LastName).IsRequired();

            builder.Property(p => p.Cellphone).IsRequired();
            builder.Property(p => p.Cellphone).IsUnicode();

            builder.Property(p => p.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(p => p.IsModified).HasDefaultValue(false);
            builder.Property(p => p.IsDeleted).HasDefaultValue(false);

            builder.Property(p => p.DateCreatedLatin).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(p => p.DateCreatedPersian).IsRequired().HasDefaultValue(Helpers.ConvertToPersianDate(DateTime.Now));
        } 
        #endregion

    }
}
