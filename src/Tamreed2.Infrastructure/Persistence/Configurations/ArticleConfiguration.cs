using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tamreed2.Domain.Articles;

namespace Tamreed2.Infrastructure.Persistence.Configurations;

public sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles");
        builder.HasKey(article => article.Id);

        builder.Property(article => article.Title).HasMaxLength(200).IsRequired();
        builder.Property(article => article.TitleAr).HasMaxLength(200).IsRequired();
        builder.Property(article => article.Slug).HasMaxLength(180).IsRequired();
        builder.HasIndex(article => article.Slug).IsUnique();

        builder.Property(article => article.Excerpt).HasMaxLength(1000).IsRequired();
        builder.Property(article => article.ExcerptAr).HasMaxLength(1000).IsRequired();

        builder.Property(article => article.Content).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(article => article.ContentAr).HasColumnType("nvarchar(max)").IsRequired();

        builder.Property(article => article.CoverImageUrl).HasMaxLength(1000).IsRequired();
        builder.Property(article => article.SeoTitle).HasMaxLength(200).IsRequired();
        builder.Property(article => article.SeoTitleAr).HasMaxLength(200).IsRequired();
        builder.Property(article => article.SeoDescription).HasMaxLength(500).IsRequired();
        builder.Property(article => article.SeoDescriptionAr).HasMaxLength(500).IsRequired();

        builder.Property(article => article.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(article => article.CreatedAtUtc).IsRequired();
        builder.Property(article => article.UpdatedAtUtc).IsRequired();
        builder.Property(article => article.PublishedAtUtc);
    }
}
