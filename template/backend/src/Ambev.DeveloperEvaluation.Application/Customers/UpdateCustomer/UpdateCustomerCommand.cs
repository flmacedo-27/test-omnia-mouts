using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

public class UpdateCustomerCommand : IRequest<UpdateCustomerResult?>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public CustomerType CustomerType { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public bool Active { get; set; }
} 