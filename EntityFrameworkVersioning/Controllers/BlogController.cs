namespace EntityFrameworkVersioning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public BlogController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("get/{id:guid}")]
    [ProducesResponseType(typeof(BlogEntity), StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _dbContext.Blogs.SingleOrDefaultAsync(x => x.BaseId == id, HttpContext.RequestAborted);

        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpPatch]
    [Route("update")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update([FromBody] UpdateBlogRequest request)
    {
        var response = await _dbContext.Blogs.SingleOrDefaultAsync(x => x.BaseId == request.Id, HttpContext.RequestAborted);

        if (response is null)
            return NotFound();
        
        response.Name = request.Name;
        await _dbContext.SaveChangesAsync(HttpContext.RequestAborted);
        
        return Ok(response);
    }

    [HttpPost]
    [Route("create")]
    [ProducesResponseType(typeof(BlogEntity), StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateBlogRequest request)
    {
        var details = new BlogEntity()
        {
            Name = request.Name,
        };
        await _dbContext.AddAsync(details, HttpContext.RequestAborted);
        await _dbContext.SaveChangesAsync(HttpContext.RequestAborted);

        return Created(new Uri($"{HttpContext.Request.Host}/blog/get/{details.BaseId}"), details);
    }

    [HttpGet]
    [Route("list")]
    [ProducesResponseType(typeof(List<BlogEntity>), StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> List()
    {
        //using var dbContext = new DisableVersioningDbContext(_dbContext);
        
        var response = await _dbContext.Blogs.ToListAsync(HttpContext.RequestAborted);
        return Ok(response);
    }
    
    [HttpDelete]
    [Route("delete/{id:guid}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _dbContext.Blogs.SingleOrDefaultAsync(x => x.BaseId == id, HttpContext.RequestAborted);

        if (result is not null)
        {
            _dbContext.Remove(result);
            await _dbContext.SaveChangesAsync(HttpContext.RequestAborted);
        }

        return NoContent();
    }
    
}