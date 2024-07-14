using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FIAP.TechChalenge.EpicCollections.Api.Extensions;
using FIAP.TechChalenge.EpicCollections.Api.Filters;
using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Collection;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollectionItem;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollectionItems;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollectionItem;

namespace FIAP.TechChalenge.EpicCollections.Api.Controllers;

[ApiController]
[Route("collections/{collectionId}/items")]
[Authorize]
public class CollectionItemsController : ControllerBase
{
    private readonly ILogger<CollectionItemsController> _logger;
    private readonly IMediator _mediator;

    public CollectionItemsController(
        ILogger<CollectionItemsController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CollectionItemModelOutput>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ServiceFilter(typeof(ValidateUserIdFilter))]
    public async Task<IActionResult> AddItem(
        [FromRoute] Guid collectionId,
        [FromBody] AddCollectionItemApiInput apiInput,
        CancellationToken cancellationToken
    )
    {
        var userId = (Guid)HttpContext.Items["UserId"]!;

        var input = new AddCollectionItemInput(
            collectionId,
            apiInput.Name,
            apiInput.Description,
            apiInput.AcquisitionDate,
            apiInput.Value,
            apiInput.PhotoUrl
        );

        var result = await _mediator.Send(input, cancellationToken);
        return CreatedAtAction(
            nameof(AddItem),
            new { collectionId, result.Id },
            new ApiResponse<CollectionItemModelOutput>(result)
        );
    }

    [HttpDelete("{itemId}")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ServiceFilter(typeof(ValidateUserIdFilter))]
    public async Task<IActionResult> DeleteItem(
           [FromRoute] Guid collectionId,
           [FromRoute] Guid itemId,
           CancellationToken cancellationToken
    )
    {
        var userId = (Guid)HttpContext.Items["UserId"]!;

        var input = new DeleteCollectionItemInput(collectionId, itemId);

        await _mediator.Send(input, cancellationToken);
        return Ok(new ApiResponse<string>("Item deleted successfully"));
    }

    [AllowAnonymous]
    [HttpGet("{itemId}")]
    [ProducesResponseType(typeof(ApiResponse<CollectionItemModelOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ServiceFilter(typeof(ValidateUserIdFilter))]
    public async Task<IActionResult> GetItem(
        [FromRoute] Guid collectionId,
        [FromRoute] Guid itemId,
        CancellationToken cancellationToken)
    {
        var input = new GetCollectionItemInput(collectionId, itemId);

        var result = await _mediator.Send(input, cancellationToken);

        return Ok(new ApiResponse<CollectionItemModelOutput>(result));
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<CollectionModelOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListItems(
        [FromRoute] Guid collectionId,
        CancellationToken cancellationToken
    )
    {
        var input = new ListCollectionItemsInput(collectionId);
        var output = await _mediator.Send(input, cancellationToken);
        return Ok(new ApiResponse<CollectionModelOutput>(output));
    }

    [HttpPut("{itemId}")]
    [ProducesResponseType(typeof(ApiResponse<CollectionItemModelOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ServiceFilter(typeof(ValidateUserIdFilter))]
    public async Task<IActionResult> UpdateItem(
       [FromRoute] Guid collectionId,
       [FromRoute] Guid itemId,
       [FromBody] UpdateCollectionItemApiInput apiInput,
       CancellationToken cancellationToken
   )
    {
        var userId = (Guid)HttpContext.Items["UserId"]!;

        var input = new UpdateCollectionItemInput(
            collectionId,
            itemId,
            apiInput.Name,
            apiInput.Description,
            apiInput.AcquisitionDate,
            apiInput.Value,
            apiInput.PhotoUrl
        );

        var result = await _mediator.Send(input, cancellationToken);
        return Ok(new ApiResponse<CollectionItemModelOutput>(result));
    }
}
