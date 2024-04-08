namespace EntityFrameworkVersioning.Persistence;

public class DisableVersioningDbContext : IDisposable
{
    private readonly AppDbContext _dbContext;
    
    public DbSet<BlogBaseEntity> BlogBases => _dbContext.BlogBases;
    public DbSet<BlogEntity> Blogs => _dbContext.Blogs;
    public DbSet<ArticleEntity> Articles => _dbContext.Articles;
    public DbSet<ArticleBaseEntity> ArticleBases => _dbContext.ArticleBases;

    public DisableVersioningDbContext(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.DisableVersioning();
    }
    
    public void Dispose()
    {
        _dbContext.EnableVersioning();
    }
}