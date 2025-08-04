using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

/// <summary>
/// Validator for UpdateUserRequest that defines input validation rules.
/// </summary>
public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the UpdateUserRequestValidator with input validation rules.
    /// </summary>
    /// <remarks>
    /// Input validation rules include:
    /// - Id: Must not be empty
    /// - Name: Must not be empty and have valid length
    /// - Email: Must be a valid email format
    /// - Phone: Must not be empty and have valid length
    /// - Password: Must not be empty and have valid length
    /// - Role: Must be a valid enum value
    /// - Status: Must be a valid enum value
    /// </remarks>
    public UpdateUserRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters");

        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be in a valid format")
            .MaximumLength(100)
            .WithMessage("Email cannot exceed 100 characters");

        RuleFor(request => request.Phone)
            .NotEmpty()
            .WithMessage("Phone is required")
            .MaximumLength(20)
            .WithMessage("Phone cannot exceed 20 characters");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters")
            .MaximumLength(100)
            .WithMessage("Password cannot exceed 100 characters");

        RuleFor(request => request.Role)
            .IsInEnum()
            .WithMessage("Role must be a valid value");

        RuleFor(request => request.Status)
            .IsInEnum()
            .WithMessage("Status must be a valid value");
    }
} 