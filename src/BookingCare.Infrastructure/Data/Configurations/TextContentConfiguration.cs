using BookingCare.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookingCare.Infrastructure.Data.Configurations
{
    public class TextContentConfiguration : IEntityTypeConfiguration<TextContent>
    {
        public void Configure(EntityTypeBuilder<TextContent> builder)
        {
        }
    }
}