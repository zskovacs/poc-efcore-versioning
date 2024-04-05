namespace EntityFrameworkVersioning.Entities;

public class BlogBaseEntity : VersionBaseEntity<BlogEntity>, IVersionBaseEntity
{
    public Guid Id { get; set; }

}