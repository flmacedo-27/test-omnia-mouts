using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;

/// <summary>
/// Response for creating a new customer.
/// </summary>
public class CreateCustomerResponse
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer document number.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer type.
    /// </summary>
    public CustomerType CustomerType { get; set; }

    /// <summary>
    /// Gets or sets whether the customer is active.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }
} 