namespace EntityFrameworkVersioning.Entities;

public class ArticleBaseEntity : VersionBaseEntity<ArticleEntity>
{
    public Guid BlogId { get; set; }
    public BlogBaseEntity Blog { get; set; }
}