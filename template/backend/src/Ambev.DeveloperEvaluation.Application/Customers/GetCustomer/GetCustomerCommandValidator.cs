using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

public class GetCustomerCommandValidator : AbstractValidator<GetCustomerCommand>
{
    public GetCustomerCommandValidator(ICustomerRepository customerRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, cancellation) => 
                await customerRepository.GetByIdAsync(id, cancellation) != null)
            .WithMessage("Customer not found in the system");
    }
} 