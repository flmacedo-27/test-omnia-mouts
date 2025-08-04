using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateCustomerHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateCustomerHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateCustomerCommand instances.
    /// The generated commands will have valid:
    /// - Name (using person names)
    /// - Email (valid format)
    /// - DocumentNumber (CPF format)
    /// - Phone (Brazilian format)
    /// - Address (complete address)
    /// - CustomerType (Individual or Corporate)
    /// </summary>
    private static readonly Faker<CreateCustomerCommand> createCustomerCommandFaker = new Faker<CreateCustomerCommand>()
        .RuleFor(c => c.Name, f => f.Person.FullName)
        .RuleFor(c => c.Email, f => f.Internet.Email())
        .RuleFor(c => c.DocumentNumber, f => f.Random.Replace("###.###.###-##"))
        .RuleFor(c => c.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        .RuleFor(c => c.CustomerType, f => f.PickRandom(CustomerType.CPF, CustomerType.CNPJ));

    /// <summary>
    /// Generates a valid CreateCustomerCommand with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid CreateCustomerCommand with randomly generated data.</returns>
    public static CreateCustomerCommand GenerateValidCommand()
    {
        return createCustomerCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a CreateCustomerCommand with existing email for testing validation failure.
    /// </summary>
    /// <returns>A CreateCustomerCommand with an email that already exists in the system.</returns>
    public static CreateCustomerCommand GenerateCommandWithExistingEmail()
    {
        var command = createCustomerCommandFaker.Generate();
        command.Email = "existing@email.com";
        return command;
    }

    /// <summary>
    /// Generates a CreateCustomerCommand with existing document number for testing validation failure.
    /// </summary>
    /// <returns>A CreateCustomerCommand with a document number that already exists in the system.</returns>
    public static CreateCustomerCommand GenerateCommandWithExistingDocument()
    {
        var command = createCustomerCommandFaker.Generate();
        command.DocumentNumber = "123.456.789-01";
        return command;
    }

    /// <summary>
    /// Generates an invalid CreateCustomerCommand for testing validation failure.
    /// </summary>
    /// <returns>A CreateCustomerCommand with invalid data that should fail validation.</returns>
    public static CreateCustomerCommand GenerateInvalidCommand()
    {
        return new CreateCustomerCommand
        {
            Name = "",
            Email = "invalid-email",
            DocumentNumber = "123",
            Phone = "invalid-phone",
            CustomerType = CustomerType.CPF
        };
    }

    /// <summary>
    /// Generates a CreateCustomerCommand with individual customer type.
    /// </summary>
    /// <returns>A CreateCustomerCommand for an individual customer.</returns>
    public static CreateCustomerCommand GenerateIndividualCustomerCommand()
    {
        var command = createCustomerCommandFaker.Generate();
        command.CustomerType = CustomerType.CPF;
        return command;
    }

    /// <summary>
    /// Generates a CreateCustomerCommand with corporate customer type.
    /// </summary>
    /// <returns>A CreateCustomerCommand for a corporate customer.</returns>
    public static CreateCustomerCommand GenerateCorporateCustomerCommand()
    {
        var command = createCustomerCommandFaker.Generate();
        command.CustomerType = CustomerType.CNPJ;
        return command;
    }
} 