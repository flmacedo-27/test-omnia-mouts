using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateBranchHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateBranchHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateBranchCommand instances.
    /// The generated commands will have valid:
    /// - Name (using company names)
    /// - Code (unique branch code)
    /// - Address (complete address)
    /// - Phone (Brazilian format)
    /// </summary>
    private static readonly Faker<CreateBranchCommand> createBranchCommandFaker = new Faker<CreateBranchCommand>()
        .RuleFor(b => b.Name, f => f.Company.CompanyName())
        .RuleFor(b => b.Code, f => $"BR{f.Random.Number(1000, 9999)}")
        .RuleFor(b => b.Address, f => f.Address.FullAddress());

    /// <summary>
    /// Generates a valid CreateBranchCommand with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid CreateBranchCommand with randomly generated data.</returns>
    public static CreateBranchCommand GenerateValidCommand()
    {
        return createBranchCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a CreateBranchCommand with existing code for testing validation failure.
    /// </summary>
    /// <returns>A CreateBranchCommand with a code that already exists in the system.</returns>
    public static CreateBranchCommand GenerateCommandWithExistingCode()
    {
        var command = createBranchCommandFaker.Generate();
        command.Code = "BR1234";
        return command;
    }

    /// <summary>
    /// Generates an invalid CreateBranchCommand for testing validation failure.
    /// </summary>
    /// <returns>A CreateBranchCommand with invalid data that should fail validation.</returns>
    public static CreateBranchCommand GenerateInvalidCommand()
    {
        return new CreateBranchCommand
        {
            Name = "",
            Code = "",
            Address = "",
        };
    }

    /// <summary>
    /// Generates a CreateBranchCommand with a specific code for testing.
    /// </summary>
    /// <param name="code">The branch code to use.</param>
    /// <returns>A CreateBranchCommand with the specified code.</returns>
    public static CreateBranchCommand GenerateCommandWithCode(string code)
    {
        var command = createBranchCommandFaker.Generate();
        command.Code = code;
        return command;
    }

    /// <summary>
    /// Generates a CreateBranchCommand with a specific name for testing.
    /// </summary>
    /// <param name="name">The branch name to use.</param>
    /// <returns>A CreateBranchCommand with the specified name.</returns>
    public static CreateBranchCommand GenerateCommandWithName(string name)
    {
        var command = createBranchCommandFaker.Generate();
        command.Name = name;
        return command;
    }

    /// <summary>
    /// Generates multiple valid CreateBranchCommand instances for testing bulk operations.
    /// </summary>
    /// <param name="count">The number of commands to generate.</param>
    /// <returns>A list of valid CreateBranchCommand instances.</returns>
    public static List<CreateBranchCommand> GenerateValidCommands(int count)
    {
        return createBranchCommandFaker.Generate(count);
    }
} 