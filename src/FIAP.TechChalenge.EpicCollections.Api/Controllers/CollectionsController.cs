using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;
using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Collection;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollection;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollection;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollections;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FIAP.TechChalenge.EpicCollections.Api.Extensions;
using FIAP.TechChalenge.EpicCollections.Api.Filters;

namespace FIAP.TechChalenge.EpicCollections.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]

public class CollectionsController : ControllerBase
{
    private readonly ILogger<CollectionsController> _logger;
    private readonly IMediator _mediator;

    public CollectionsController(
        ILogger<CollectionsController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CollectionModelOutput>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ServiceFilter(typeof(ValidateUserIdFilter))]
    public async Task<IActionResult> Create(
        [FromBody] CreateCollectionApiInput apiInput,
        CancellationToken cancellationToken
    )
    {
        var userId = (Guid)HttpContext.Items["UserId"]!;

        var input = new CreateCollectionInput(
            userId,
            apiInput.Name,
            apiInput.Description,
            apiInput.Category
        );

        var result = await _mediator.Send(input, cancellationToken);
        return CreatedAtAction(
            nameof(Create),
            new { result.Id },
            new ApiResponse<CollectionModelOutput>(result)
        );
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CollectionModelOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(
            new GetCollectionInput(id),
            cancellationToken
        );

        return Ok(new ApiResponse<CollectionModelOutput>(result));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ServiceFilter(typeof(ValidateUserIdFilter))]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var userId = (Guid)HttpContext.Items["UserId"]!;

        await _mediator.Send(
            new DeleteCollectionInput(id, userId),
            cancellationToken
        );

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CollectionModelOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ServiceFilter(typeof(ValidateUserIdFilter))]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateCollectionApiInput apiInput,
        CancellationToken cancellationToken
    )
    {
        var userId = (Guid)HttpContext.Items["UserId"]!;

        var input = new UpdateCollectionInput(
            id,
            apiInput.Name,
            apiInput.Description,
            apiInput.Category,
            userId
        );
        var result = await _mediator.Send(input, cancellationToken);
        return Ok(new ApiResponse<CollectionModelOutput>(result));
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(ListCollectionsOutput), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        CancellationToken cancellation,
        [FromQuery] int? page = null,
        [FromQuery(Name = "per_page")] int? perPage = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sort = null,
        [FromQuery] SearchOrder? dir = null
    )
    {
        var input = new ListCollectionsInput();
        if (page.HasValue)
            input.Page = page.Value;
        if (perPage.HasValue)
            input.PerPage = perPage.Value;
        if (!string.IsNullOrWhiteSpace(search))
            input.Search = search;
        if (!string.IsNullOrWhiteSpace(sort))
            input.Sort = sort;
        if (dir.HasValue)
            input.Dir = dir.Value;

        var output = await _mediator.Send(input, cancellation);

        return Ok(new ApiResponseList<CollectionModelOutput>(output));
    }
}
