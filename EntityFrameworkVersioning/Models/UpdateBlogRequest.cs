namespace EntityFrameworkVersioning.Models;

public class UpdateBlogRequest : CreateBlogRequest
{
    public Guid Id { get; set; }
}