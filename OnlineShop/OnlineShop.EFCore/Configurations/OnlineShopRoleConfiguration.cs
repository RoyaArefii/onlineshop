using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.EFCore.Configurations
{
    public class OnlineShopRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {

            builder.ToTable(nameof(AppRole)).HasData(
            new AppRole()
            {
                Id = DatabaseConstants.DefaultRoles.GodAadminId,
                Name = DatabaseConstants.DefaultRoles.GodAadminName
            },
            new AppRole()
            {
                Id = DatabaseConstants.DefaultRoles.AadminId,
                Name = DatabaseConstants.DefaultRoles.AadminName
            },
            new AppRole()
            {
                Id = DatabaseConstants.DefaultRoles.SupportId,
                Name = DatabaseConstants.DefaultRoles.SupportName
            });

            builder.Property(p => p.Name).IsRequired().IsUnicode();
        }
    }
}
