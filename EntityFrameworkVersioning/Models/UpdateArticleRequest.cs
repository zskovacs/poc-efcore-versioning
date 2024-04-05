namespace EntityFrameworkVersioning.Models;

public class UpdateArticleRequest : CreateArticleRequest
{
    public Guid Id { get; set; }
}