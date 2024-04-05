namespace EntityFrameworkVersioning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ArticleController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("get/{id:guid}")]
    [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _dbContext.Articles
            .Include(x => x.Base)
            .ThenInclude(x => x.Blog)
            .ThenInclude(x => x.Details)
            .Where(x => x.BaseId == id)
            .Select(x => new ArticleDto()
            {
                Id = x.BaseId,
                Name = x.Name,
                Blog = x.Base.Blog.Details.Single().Name,
                Revision = x.Revision,
                ValidFrom = x.ValidFrom
            })
            .SingleOrDefaultAsync(HttpContext.RequestAborted);

        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpPatch]
    [Route("update")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update([FromBody] UpdateArticleRequest request)
    {
        var response = await _dbContext.Articles.Include(x => x.Base).SingleOrDefaultAsync(x => x.BaseId == request.Id, HttpContext.RequestAborted);

        if (response is null)
            return NotFound();
        
        response.Name = request.Name;
        response.Base.BlogId = request.BlogId;
        
        await _dbContext.SaveChangesAsync(HttpContext.RequestAborted);
        
        return Ok(response);
    }

    [HttpPost]
    [Route("create")]
    [ProducesResponseType(typeof(ArticleEntity), StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateArticleRequest request)
    {
        var details = new ArticleEntity
        {
            Name = request.Name,
            Base = { BlogId = request.BlogId }
        };
        
        await _dbContext.AddAsync(details, HttpContext.RequestAborted);
        await _dbContext.SaveChangesAsync(HttpContext.RequestAborted);

        return Created(new Uri($"{HttpContext.Request.Host}/article/get/{details.BaseId}"), details);
    }

    [HttpGet]
    [Route("list")]
    [ProducesResponseType(typeof(List<ArticleEntity>), StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> List()
    {
        var response = await _dbContext.Articles.ToListAsync(HttpContext.RequestAborted);
        return Ok(response);
    }
    
    [HttpDelete]
    [Route("delete/{id:guid}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _dbContext.Articles.SingleOrDefaultAsync(x => x.BaseId == id, HttpContext.RequestAborted);

        if (result is not null)
        {
            _dbContext.Remove(result);
            await _dbContext.SaveChangesAsync(HttpContext.RequestAborted);
        }

        return NoContent();
    }
    
}