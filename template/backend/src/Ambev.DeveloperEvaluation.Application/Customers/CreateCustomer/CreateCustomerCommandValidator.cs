using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

/// <summary>
/// Validator for CreateCustomerCommand that defines business-specific validation rules.
/// </summary>
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateCustomerCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - Email: Must not already exist in the system
    /// - DocumentNumber: Must not already exist in the system
    /// </remarks>
    public CreateCustomerCommandValidator(ICustomerRepository customerRepository)
    {
        RuleFor(customer => customer.Email)
            .MustAsync(async (email, cancellation) => 
                !await customerRepository.ExistsByEmailAsync(email, cancellation))
            .WithMessage("Customer email already exists in the system");

        RuleFor(customer => customer.DocumentNumber)
            .MustAsync(async (documentNumber, cancellation) => 
                !await customerRepository.ExistsByDocumentNumberAsync(documentNumber, cancellation))
            .WithMessage("Customer document number already exists in the system");
    }
} 