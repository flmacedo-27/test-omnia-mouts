using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for GetUserHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class GetUserHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid GetUserCommand instances.
    /// The generated commands will have valid user IDs.
    /// </summary>
    private static readonly Faker<GetUserCommand> getUserCommandFaker = new Faker<GetUserCommand>()
        .CustomInstantiator(f => new GetUserCommand(f.Random.Guid()));

    /// <summary>
    /// Configures the Faker to generate valid User entities.
    /// The generated users will have valid:
    /// - Username (using person names)
    /// - Email (valid format)
    /// - Phone (Brazilian format)
    /// - Role (Admin or Customer)
    /// - Status (Active or Inactive)
    /// </summary>
    private static readonly Faker<User> userFaker = new Faker<User>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Username, f => f.Person.FullName)
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Admin, UserRole.Customer, UserRole.Manager))
        .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Inactive, UserStatus.Suspended))
        .RuleFor(u => u.CreatedAt, f => f.Date.Past())
        .RuleFor(u => u.UpdatedAt, f => f.Date.Recent());

    /// <summary>
    /// Generates a valid GetUserCommand with randomized data.
    /// The generated command will have a valid user ID.
    /// </summary>
    /// <returns>A valid GetUserCommand with randomly generated data.</returns>
    public static GetUserCommand GenerateValidCommand()
    {
        return getUserCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a GetUserCommand with a specific user ID.
    /// </summary>
    /// <param name="userId">The user ID to use</param>
    /// <returns>A GetUserCommand with the specified user ID.</returns>
    public static GetUserCommand GenerateCommandWithId(Guid userId)
    {
        return new GetUserCommand(userId);
    }

    /// <summary>
    /// Generates an invalid GetUserCommand with empty ID for testing validation failure.
    /// </summary>
    /// <returns>A GetUserCommand with an empty ID that should fail validation.</returns>
    public static GetUserCommand GenerateInvalidCommand()
    {
        return new GetUserCommand(Guid.Empty);
    }

    /// <summary>
    /// Generates a valid User entity with randomized data.
    /// The generated user will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid User entity with randomly generated data.</returns>
    public static User GenerateValidUser()
    {
        return userFaker.Generate();
    }

    /// <summary>
    /// Generates a User entity with a specific ID.
    /// </summary>
    /// <param name="userId">The user ID to use</param>
    /// <returns>A User entity with the specified ID.</returns>
    public static User GenerateUserWithId(Guid userId)
    {
        var user = userFaker.Generate();
        user.Id = userId;
        return user;
    }

    /// <summary>
    /// Generates an active User entity.
    /// </summary>
    /// <returns>An active User entity.</returns>
    public static User GenerateActiveUser()
    {
        var user = userFaker.Generate();
        user.Status = UserStatus.Active;
        return user;
    }

    /// <summary>
    /// Generates an inactive User entity.
    /// </summary>
    /// <returns>An inactive User entity.</returns>
    public static User GenerateInactiveUser()
    {
        var user = userFaker.Generate();
        user.Status = UserStatus.Inactive;
        return user;
    }

    /// <summary>
    /// Generates a User entity with admin role.
    /// </summary>
    /// <returns>A User entity with admin role.</returns>
    public static User GenerateAdminUser()
    {
        var user = userFaker.Generate();
        user.Role = UserRole.Admin;
        return user;
    }

    /// <summary>
    /// Generates a User entity with customer role.
    /// </summary>
    /// <returns>A User entity with customer role.</returns>
    public static User GenerateCustomerUser()
    {
        var user = userFaker.Generate();
        user.Role = UserRole.Customer;
        return user;
    }

    /// <summary>
    /// Generates a User entity with manager role.
    /// </summary>
    /// <returns>A User entity with manager role.</returns>
    public static User GenerateManagerUser()
    {
        var user = userFaker.Generate();
        user.Role = UserRole.Manager;
        return user;
    }

    /// <summary>
    /// Generates a GetUserResult with randomized data.
    /// </summary>
    /// <returns>A GetUserResult with randomly generated data.</returns>
    public static GetUserResult GenerateValidResult()
    {
        var user = userFaker.Generate();
        return new GetUserResult
        {
            Id = user.Id,
            Name = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            Status = user.Status
        };
    }

    /// <summary>
    /// Generates a GetUserResult with specific properties.
    /// </summary>
    /// <param name="user">The user entity to base the result on</param>
    /// <returns>A GetUserResult based on the provided user.</returns>
    public static GetUserResult GenerateResultFromUser(User user)
    {
        return new GetUserResult
        {
            Id = user.Id,
            Name = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role,
            Status = user.Status
        };
    }
} 