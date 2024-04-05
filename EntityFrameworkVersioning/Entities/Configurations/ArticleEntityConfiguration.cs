namespace EntityFrameworkVersioning.Entities.Configurations;

public class ArticleEntityConfiguration : IEntityTypeConfiguration<ArticleEntity>
{
    public void Configure(EntityTypeBuilder<ArticleEntity> builder)
    {
        builder.ToTable("Articles");
        
        builder.HasKey(e => new { e.BaseId, e.Revision });

        builder.Property(e => e.Revision) // Optimista konkurenciakezeléshez
            .IsConcurrencyToken();
    }
}