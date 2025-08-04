using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeleteBranch;
using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranch;
using Ambev.DeveloperEvaluation.Application.Branches.ListBranches;
using Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;
using Microsoft.AspNetCore.Authorization;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches;

/// <summary>
/// Controller for managing branches.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BranchesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the BranchesController class.
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public BranchesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new branch.
    /// </summary>
    /// <param name="request">The create branch request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created branch</returns>
    [HttpPost]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateBranchRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateBranchCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        
        var response = _mapper.Map<CreateBranchResponse>(result);
        return CreatedAtAction(nameof(GetBranch), new { id = response.Id }, response);
    }

    /// <summary>
    /// Gets a branch by ID.
    /// </summary>
    /// <param name="id">The branch ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch if found</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBranch([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetBranchRequest { Id = id };
        var validator = new GetBranchRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = new GetBranchCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result == null)
            return NotFound();
        
        var response = _mapper.Map<GetBranchResponse>(result);
        return Ok(response);
    }

    /// <summary>
    /// Lists branches with pagination.
    /// </summary>
    /// <param name="request">The list branches request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of branches</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<BranchListItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListBranches([FromQuery] ListBranchesRequest request, CancellationToken cancellationToken)
    {
        var validator = new ListBranchesRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<ListBranchesCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return OkPaginated(response.ToPaginatedList());
    }

    /// <summary>
    /// Updates a branch.
    /// </summary>
    /// <param name="id">The branch ID</param>
    /// <param name="request">The update branch request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated branch</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBranch([FromRoute] Guid id, [FromBody] UpdateBranchRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateBranchRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateBranchCommand>(request);
        command.Id = id;
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result == null)
            return NotFound();
        
        var response = _mapper.Map<UpdateBranchResponse>(result);
        return Ok(response);
    }

    /// <summary>
    /// Deletes a branch.
    /// </summary>
    /// <param name="id">The branch ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the branch was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBranch([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteBranchRequest { Id = id };
        var validator = new DeleteBranchRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(new { message = "Invalid request" });

        var command = _mapper.Map<DeleteBranchCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return NotFound(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
} 