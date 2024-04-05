namespace EntityFrameworkVersioning.Models;

public class ArticleDto
{
    public string Name { get; set; }
    public string Blog { get; set; }
    public int Revision { get; set; }
    public DateTime ValidFrom { get; set; }
    public Guid Id { get; set; }
}