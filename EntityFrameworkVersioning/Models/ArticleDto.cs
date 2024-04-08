using AutoMapper;

namespace EntityFrameworkVersioning.Models;

public class ArticleDto
{
    public string Name { get; set; }
    public string Blog { get; set; }
    public int Revision { get; set; }
    public DateTime ValidFrom { get; set; }
    public Guid Id { get; set; }
}

public class ArticleMapper : Profile
{
    public ArticleMapper()
    {
        CreateMap<ArticleEntity, ArticleDto>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.BaseId))
            .ForMember(dst => dst.Blog, opt => opt.MapFrom(src => src.Base.Blog.Details.Single().Name));
    }
}