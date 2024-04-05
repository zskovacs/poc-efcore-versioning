namespace EntityFrameworkVersioning.Entities;

public class BlogEntity : VersionEntity<BlogBaseEntity>
{
    public string Name { get; set; }
}

