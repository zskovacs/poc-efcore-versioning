namespace EntityFrameworkVersioning.Entities;

public class ArticleEntity : VersionEntity<ArticleBaseEntity>
{
    public string Name { get; set; }
}

