namespace EntityFrameworkVersioning.Models;

public class CreateArticleRequest
{
    public string Name { get; set; }
    public Guid BlogId { get; set; }
}