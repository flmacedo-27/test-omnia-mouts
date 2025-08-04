using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for GetCustomerHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class GetCustomerHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid GetCustomerCommand instances.
    /// The generated commands will have valid customer IDs.
    /// </summary>
    private static readonly Faker<GetCustomerCommand> getCustomerCommandFaker = new Faker<GetCustomerCommand>()
        .RuleFor(c => c.Id, f => f.Random.Guid());

    /// <summary>
    /// Configures the Faker to generate valid Customer entities.
    /// The generated customers will have valid:
    /// - Name (using person names)
    /// - Email (valid format)
    /// - DocumentNumber (CPF format)
    /// - Phone (Brazilian format)
    /// - CustomerType (Individual or Corporate)
    /// - Active status
    /// </summary>
    private static readonly Faker<Customer> customerFaker = new Faker<Customer>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.Name, f => f.Person.FullName)
        .RuleFor(c => c.Email, f => f.Internet.Email())
        .RuleFor(c => c.DocumentNumber, f => f.Random.Replace("###.###.###-##"))
        .RuleFor(c => c.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        .RuleFor(c => c.CustomerType, f => f.PickRandom(CustomerType.CPF, CustomerType.CNPJ))
        .RuleFor(c => c.Active, f => f.Random.Bool())
        .RuleFor(c => c.CreatedAt, f => f.Date.Past())
        .RuleFor(c => c.UpdatedAt, f => f.Date.Recent());

    /// <summary>
    /// Generates a valid GetCustomerCommand with randomized data.
    /// The generated command will have a valid customer ID.
    /// </summary>
    /// <returns>A valid GetCustomerCommand with randomly generated data.</returns>
    public static GetCustomerCommand GenerateValidCommand()
    {
        return getCustomerCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a GetCustomerCommand with a specific customer ID.
    /// </summary>
    /// <param name="customerId">The customer ID to use</param>
    /// <returns>A GetCustomerCommand with the specified customer ID.</returns>
    public static GetCustomerCommand GenerateCommandWithId(Guid customerId)
    {
        return new GetCustomerCommand { Id = customerId };
    }

    /// <summary>
    /// Generates an invalid GetCustomerCommand with empty ID for testing validation failure.
    /// </summary>
    /// <returns>A GetCustomerCommand with an empty ID that should fail validation.</returns>
    public static GetCustomerCommand GenerateInvalidCommand()
    {
        return new GetCustomerCommand { Id = Guid.Empty };
    }

    /// <summary>
    /// Generates a valid Customer entity with randomized data.
    /// The generated customer will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid Customer entity with randomly generated data.</returns>
    public static Customer GenerateValidCustomer()
    {
        return customerFaker.Generate();
    }

    /// <summary>
    /// Generates a Customer entity with a specific ID.
    /// </summary>
    /// <param name="customerId">The customer ID to use</param>
    /// <returns>A Customer entity with the specified ID.</returns>
    public static Customer GenerateCustomerWithId(Guid customerId)
    {
        var customer = customerFaker.Generate();
        customer.Id = customerId;
        return customer;
    }

    /// <summary>
    /// Generates an active Customer entity.
    /// </summary>
    /// <returns>An active Customer entity.</returns>
    public static Customer GenerateActiveCustomer()
    {
        var customer = customerFaker.Generate();
        customer.Active = true;
        return customer;
    }

    /// <summary>
    /// Generates an inactive Customer entity.
    /// </summary>
    /// <returns>An inactive Customer entity.</returns>
    public static Customer GenerateInactiveCustomer()
    {
        var customer = customerFaker.Generate();
        customer.Active = false;
        return customer;
    }

    /// <summary>
    /// Generates a Customer entity with individual customer type.
    /// </summary>
    /// <returns>A Customer entity for an individual customer.</returns>
    public static Customer GenerateIndividualCustomer()
    {
        var customer = customerFaker.Generate();
        customer.CustomerType = CustomerType.CPF;
        return customer;
    }

    /// <summary>
    /// Generates a Customer entity with corporate customer type.
    /// </summary>
    /// <returns>A Customer entity for a corporate customer.</returns>
    public static Customer GenerateCorporateCustomer()
    {
        var customer = customerFaker.Generate();
        customer.CustomerType = CustomerType.CNPJ;
        return customer;
    }

    /// <summary>
    /// Generates a GetCustomerResult with randomized data.
    /// </summary>
    /// <returns>A GetCustomerResult with randomly generated data.</returns>
    public static GetCustomerResult GenerateValidResult()
    {
        var customer = customerFaker.Generate();
        return new GetCustomerResult
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            CustomerType = customer.CustomerType,
            DocumentNumber = customer.DocumentNumber,
            Active = customer.Active,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }
} 