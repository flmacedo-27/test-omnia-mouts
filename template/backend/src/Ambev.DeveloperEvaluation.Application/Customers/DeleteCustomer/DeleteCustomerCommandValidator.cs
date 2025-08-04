using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

/// <summary>
/// Validator for DeleteCustomerCommand that defines business-specific validation rules.
/// </summary>
public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    /// <summary>
    /// Initializes a new instance of the DeleteCustomerCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - Customer must exist in the system before deletion
    /// </remarks>
    public DeleteCustomerCommandValidator(ICustomerRepository customerRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellation) => 
                await customerRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("Customer not found in the system");
    }
} 