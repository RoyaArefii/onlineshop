using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Constants;
using PublicTools.Tools;


namespace OnlineShop.EFCore.Configurations.IdentityConfiguration
{
    public class OnlineShopUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        ///*************************************    Questionnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn
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
                            PasswordHash = DatabaseConstants.GodAdminUsers.ArefiPassword.GetHashCode().ToString(),
                            IsActive = true,
                            DateCreatedLatin = DateTime.Now,
                            DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                            IsDeleted = false,
                            IsModified = false
                        });

            //    builder.ToTable(table => table.HasCheckConstraint(
            //DatabaseConstants.CheckConstraints.CellphoneOnlyNumericalTitle,
            //DatabaseConstants.CheckConstraints.CellphoneOnlyNumerical));
            //    builder.ToTable(table => table.HasCheckConstraint(
            //        DatabaseConstants.CheckConstraints.NationalIdOnlyNumericalTitle,
            //        DatabaseConstants.CheckConstraints.NationalIdOnlyNumerical));
            //    builder.ToTable(table => table.HasCheckConstraint(
            //        DatabaseConstants.CheckConstraints.CellphoneOnlyNumericalTitle,
            //        DatabaseConstants.CheckConstraints.CellphoneOnlyNumerical));

            //    builder.ToTable(table => table.HasCheckConstraint(
            //        DatabaseConstants.CheckConstraints.NationalIdCharacterNumberTitle,
            //        DatabaseConstants.CheckConstraints.NationalIdCharacterNumber));

            builder.Property(p => p.FirstName).IsRequired();
            builder.Property(p => p.LastName).IsRequired();

            builder.Property(p => p.Cellphone).IsRequired();
            builder.Property(p => p.Cellphone).IsUnicode();

            builder.Property(p => p.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(p => p.DateCreatedLatin).IsRequired().HasDefaultValue(DateTime.Now);

            builder.Property(p => p.IsModified).HasDefaultValue(false);
            builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        }
    }
}
