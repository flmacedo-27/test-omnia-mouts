using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Validator for UpdateCustomerCommand that defines business-specific validation rules.
/// </summary>
public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateCustomerCommandValidator with business validation rules.
    /// </summary>
    /// <remarks>
    /// Business validation rules include:
    /// - Customer must exist in the system
    /// - New email must not conflict with existing customers (excluding current customer)
    /// - New document number must not conflict with existing customers (excluding current customer)
    /// </remarks>
    public UpdateCustomerCommandValidator(ICustomerRepository customerRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellation) => 
                await customerRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("Customer not found in the system");

        RuleFor(command => command.Email)
            .MustAsync(async (command, email, cancellation) => 
            {
                var existingCustomer = await customerRepository.GetByIdAsync(command.Id, cancellation);
                if (existingCustomer == null) return true; // Will be caught by previous rule
                
                // Check if email exists in other customers (excluding current customer)
                var customerWithSameEmail = await customerRepository.GetByEmailAsync(email, cancellation);
                return customerWithSameEmail == null || customerWithSameEmail.Id == command.Id;
            })
            .WithMessage("Customer email already exists in another customer");

        RuleFor(command => command.DocumentNumber)
            .MustAsync(async (command, documentNumber, cancellation) => 
            {
                var existingCustomer = await customerRepository.GetByIdAsync(command.Id, cancellation);
                if (existingCustomer == null) return true; // Will be caught by previous rule
                
                // Check if document number exists in other customers (excluding current customer)
                var customerWithSameDocument = await customerRepository.GetByDocumentNumberAsync(documentNumber, cancellation);
                return customerWithSameDocument == null || customerWithSameDocument.Id == command.Id;
            })
            .WithMessage("Customer document number already exists in another customer");
    }
} 