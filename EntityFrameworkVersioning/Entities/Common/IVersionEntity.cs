namespace EntityFrameworkVersioning.Entities.Common
{
    public interface IVersionEntity : ICloneable
    {
        Guid BaseId { get; set; }
        int Revision { get; set; }
        DateTime ValidFrom { get; set; }
        DateTime ValidTo { get; set; }
    }
}