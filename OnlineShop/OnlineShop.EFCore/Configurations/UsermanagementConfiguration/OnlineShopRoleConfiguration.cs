using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Constants;
using PublicTools.Tools;

namespace OnlineShop.EFCore.Configurations.IdentityConfiguration
{
    public class OnlineShopRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {

            builder.ToTable(nameof(AppRole))
            .HasData(
            new AppRole()
            {
                Id = DatabaseConstants.DefaultRoles.GodAadminId,
                Name = DatabaseConstants.DefaultRoles.GodAadminName,
                IsActive = true,
                DateCreatedLatin = DateTime.Now,
                DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                IsDeleted = false,
                IsModified = false
            },
            new AppRole()
            {
                Id = DatabaseConstants.DefaultRoles.AadminId,
                Name = DatabaseConstants.DefaultRoles.AadminName,
                IsActive = true,
                DateCreatedLatin = DateTime.Now,
                DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                IsDeleted = false,
                IsModified = false
            },
            new AppRole()
            {
                Id = DatabaseConstants.DefaultRoles.SupportId,
                Name = DatabaseConstants.DefaultRoles.SupportName,
                IsActive = true,
                DateCreatedLatin = DateTime.Now,
                DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                IsDeleted = false,
                IsModified = false
            },
            new AppRole()
            {
                Id = DatabaseConstants.DefaultRoles.NormalId,
                Name = DatabaseConstants.DefaultRoles.NormalName,
                IsActive = true,
                DateCreatedLatin = DateTime.Now,
                DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                IsDeleted = false,
                IsModified = false
            });

            builder.Property(p => p.Name).IsRequired().IsUnicode();
        }
    }
}
