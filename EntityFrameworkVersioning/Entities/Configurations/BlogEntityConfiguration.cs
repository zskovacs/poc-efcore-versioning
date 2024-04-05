namespace EntityFrameworkVersioning.Entities.Configurations;

public class BlogEntityConfiguration : IEntityTypeConfiguration<BlogEntity>
{
    public void Configure(EntityTypeBuilder<BlogEntity> builder)
    {
        builder.ToTable("Blogs");

        builder.HasKey(e => new { e.BaseId, e.Revision });

        builder.HasOne(x => x.Base)
            .WithMany(x => x.Details)
            .HasForeignKey(x => x.BaseId);

        builder.Property(e => e.Revision) // Optimista konkurenciakezeléshez
            .IsConcurrencyToken();
    }
}