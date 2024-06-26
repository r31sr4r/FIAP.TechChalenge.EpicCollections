﻿using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FIAP.TechChalenge.EpicCollections.Api.Extensions;
using FIAP.TechChalenge.EpicCollections.Api.Filters;
using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Collection;

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
}
