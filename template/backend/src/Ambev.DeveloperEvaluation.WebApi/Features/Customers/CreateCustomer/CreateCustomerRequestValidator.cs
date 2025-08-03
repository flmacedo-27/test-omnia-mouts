using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(100)
            .WithMessage("Customer name cannot be longer than 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Customer email is required")
            .EmailAddress()
            .WithMessage("Customer email must be valid")
            .MaximumLength(100)
            .WithMessage("Customer email cannot be longer than 100 characters");
        
        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Customer phone is required")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must follow the pattern (XX) XXXXX-XXXX");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty()
            .WithMessage("Document number is required")
            .MaximumLength(20)
            .WithMessage("Document number cannot be longer than 20 characters");

        RuleFor(x => x.CustomerType)
            .NotNull()
            .WithMessage("Customer type is required")
            .Must(ct => ct == CustomerType.CPF || ct == CustomerType.CNPJ)
            .WithMessage("Customer type must be '1 - CPF' or '2 - CNPJ'");
    }
} 