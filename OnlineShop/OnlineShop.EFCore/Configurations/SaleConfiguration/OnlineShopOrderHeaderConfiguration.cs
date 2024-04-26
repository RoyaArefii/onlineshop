using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopDomain.Aggregates.Sale;


namespace OnlineShop.EFCore.Configurations.SaleConfiguration
{
    public class OnlineShopOrderHeaderConfiguration : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.ToTable(nameof(OrderHeader) , "Sale"); 
            //builder.HasKey(x=>x.Id);
        }
    }
}
