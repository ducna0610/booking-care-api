using BookingCare.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookingCare.Infrastructure.Data.Configurations
{
    public class TranslationConfiguration : IEntityTypeConfiguration<Translation>
    {
        public void Configure(EntityTypeBuilder<Translation> builder)
        {
            builder.HasKey(x => new { x.TextContextId, x.LanguageId });

            builder.HasOne(x => x.TextContent)
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.TextContextId);

            builder.HasOne(x => x.Language)
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.LanguageId);
        }
    }
}