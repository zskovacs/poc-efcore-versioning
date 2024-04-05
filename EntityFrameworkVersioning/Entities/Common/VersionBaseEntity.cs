namespace EntityFrameworkVersioning.Entities.Common;

public class VersionBaseEntity<T> : IVersionBaseEntity where T : IVersionEntity
{
    public Guid Id { get; set; }
    public ICollection<T> Details = new HashSet<T>();
}