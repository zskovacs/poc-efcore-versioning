namespace EntityFrameworkVersioning.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo =
        typeof(AppDbContext).GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);

    public DbSet<BlogBaseEntity> BlogBases => Set<BlogBaseEntity>();
    public DbSet<BlogEntity> Blogs => Set<BlogEntity>();
    public DbSet<ArticleEntity> Articles => Set<ArticleEntity>();
    public DbSet<ArticleBaseEntity> ArticleBases => Set<ArticleBaseEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BlogEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleEntityConfiguration());
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            ConfigureGlobalFiltersMethodInfo
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });
    }

    protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
        where TEntity : class
    {
        if (entityType.BaseType == null && typeof(IVersionEntity).IsAssignableFrom(typeof(TEntity)))
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(CreateVersionFilterExpression<TEntity>());
        }
    }

    protected virtual Expression<Func<TEntity, bool>> CreateVersionFilterExpression<TEntity>() where TEntity : class
    {
        return e => ((IVersionEntity)e).ValidTo == DateTime.MaxValue;
        ;
    }
}