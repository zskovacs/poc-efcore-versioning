using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace EntityFrameworkVersioning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public ArticleController(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("get/{id:guid}")]
    [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _dbContext.Articles
            .ProjectTo<ArticleDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);

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

        if (request.BlogId.HasValue)
            response.Base.BlogId = request.BlogId.Value;

        await _dbContext.SaveChangesAsync(HttpContext.RequestAborted);

        return Ok(response);
    }

    [HttpPost]
    [Route("create")]
    [ProducesResponseType(typeof(ArticleEntity), StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateArticleRequest request)
    {
        if (!request.BlogId.HasValue)
            return BadRequest("Blog id missing");

        var details = new ArticleEntity()
        {
            Name = request.Name,
            Base = { BlogId = request.BlogId.Value }
        };

        await _dbContext.AddAsync(details, HttpContext.RequestAborted);
        await _dbContext.SaveChangesAsync(HttpContext.RequestAborted);

        return Created(new Uri($"{HttpContext.Request.Host}/article/get/{details.BaseId}"), details);
    }

    [HttpGet]
    [Route("list")]
    [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> List()
    {
        var response = await _dbContext.Articles
            .ProjectTo<ArticleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(HttpContext.RequestAborted);
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