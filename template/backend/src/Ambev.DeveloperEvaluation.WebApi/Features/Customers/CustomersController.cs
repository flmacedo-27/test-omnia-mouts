using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.ListCustomers;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.DeleteCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;
using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;
using Microsoft.AspNetCore.Authorization;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers;

/// <summary>
/// Controller for managing customers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the CustomersController class.
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CustomersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="request">The create customer request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created customer</returns>
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateCustomerRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateCustomerCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        
        var response = _mapper.Map<CreateCustomerResponse>(result);
        return CreatedAtAction(nameof(GetCustomer), new { id = response.Id }, response);
    }

    /// <summary>
    /// Gets a customer by ID.
    /// </summary>
    /// <param name="id">The customer ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer if found</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCustomerRequest { Id = id };
        var validator = new GetCustomerRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = new GetCustomerCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result == null)
            return NotFound();
        
        var response = _mapper.Map<GetCustomerResponse>(result);
        return Ok(response);
    }

    /// <summary>
    /// Lists customers with pagination.
    /// </summary>
    /// <param name="request">The list customers request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of customers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<CustomerListItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListCustomers([FromQuery] ListCustomersRequest request, CancellationToken cancellationToken)
    {
        var validator = new ListCustomersRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<ListCustomersCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return OkPaginated(response.ToPaginatedList());
    }

    /// <summary>
    /// Updates a customer.
    /// </summary>
    /// <param name="id">The customer ID</param>
    /// <param name="request">The update customer request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated customer</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer([FromRoute] Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateCustomerRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateCustomerCommand>(request);
        command.Id = id;
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result == null)
            return NotFound();
        
        var response = _mapper.Map<UpdateCustomerResponse>(result);
        return Ok(response);
    }

    /// <summary>
    /// Deletes a customer.
    /// </summary>
    /// <param name="id">The customer ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the customer was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteCustomerRequest { Id = id };
        var validator = new DeleteCustomerRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(new { message = "Invalid request" });

        var command = _mapper.Map<DeleteCustomerCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return NotFound(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
} 