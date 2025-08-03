using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;

/// <summary>
/// Request for getting a customer.
/// </summary>
public class GetCustomerRequest
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    [Required(ErrorMessage = "ID do cliente é obrigatório")]
    public Guid Id { get; set; }
} 