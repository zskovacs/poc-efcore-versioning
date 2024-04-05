namespace EntityFrameworkVersioning.Entities.Common;

public abstract class VersionEntity<T> : IVersionEntity where T : class, IVersionBaseEntity, new()
{
    public Guid BaseId { get; set; }
    public T Base { get; private set; } = new();
    public int Revision { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    
    public object Clone()
    {
        var result = (MemberwiseClone() as VersionEntity<T>)!;
        result.Base = null!; //Changetracker miatt ki kell nullozni, egyébként mindig hozzá akarja adni
        return result;
    }
}