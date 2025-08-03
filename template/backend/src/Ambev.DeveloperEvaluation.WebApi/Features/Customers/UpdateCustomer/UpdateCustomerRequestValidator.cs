using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;

public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(100)
            .WithMessage("Customer name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Customer email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Customer phone is required")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must follow the pattern (XX) XXXXX-XXXX");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty()
            .WithMessage("Customer document number is required")
            .MaximumLength(18)
            .WithMessage("Document number cannot exceed 18 characters");
    }
} 