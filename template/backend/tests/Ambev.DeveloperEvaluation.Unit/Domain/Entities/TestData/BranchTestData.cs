using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Branch entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class BranchTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Branch entities.
    /// The generated branches will have valid:
    /// - Name (company names)
    /// - Code (alphanumeric codes)
    /// - Address (realistic addresses)
    /// - Active status (true by default)
    /// </summary>
    private static readonly Faker<Branch> BranchFaker = new Faker<Branch>()
        .RuleFor(b => b.Name, f => f.Company.CompanyName())
        .RuleFor(b => b.Code, f => f.Random.AlphaNumeric(8).ToUpper())
        .RuleFor(b => b.Address, f => f.Address.FullAddress())
        .RuleFor(b => b.Active, f => f.Random.Bool());

    /// <summary>
    /// Generates a valid Branch entity with randomized data.
    /// The generated branch will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid Branch entity with randomly generated data.</returns>
    public static Branch GenerateValidBranch()
    {
        return BranchFaker.Generate();
    }

    /// <summary>
    /// Generates a valid branch name using Faker.
    /// </summary>
    /// <returns>A valid branch name.</returns>
    public static string GenerateValidBranchName()
    {
        return new Faker().Company.CompanyName();
    }

    /// <summary>
    /// Generates a valid branch code using Faker.
    /// </summary>
    /// <returns>A valid branch code.</returns>
    public static string GenerateValidBranchCode()
    {
        return new Faker().Random.AlphaNumeric(8).ToUpper();
    }

    /// <summary>
    /// Generates a valid branch address using Faker.
    /// </summary>
    /// <returns>A valid branch address.</returns>
    public static string GenerateValidBranchAddress()
    {
        return new Faker().Address.FullAddress();
    }

    /// <summary>
    /// Generates an invalid branch name (empty or too long).
    /// </summary>
    /// <returns>An invalid branch name.</returns>
    public static string GenerateInvalidBranchName()
    {
        var faker = new Faker();
        return faker.Random.Bool() ? "" : new string('A', 101); // Empty or too long
    }

    /// <summary>
    /// Generates an invalid branch code (empty or too long).
    /// </summary>
    /// <returns>An invalid branch code.</returns>
    public static string GenerateInvalidBranchCode()
    {
        var faker = new Faker();
        return faker.Random.Bool() ? "" : new string('A', 21); // Empty or too long
    }

    /// <summary>
    /// Generates an invalid branch address (empty or too long).
    /// </summary>
    /// <returns>An invalid branch address.</returns>
    public static string GenerateInvalidBranchAddress()
    {
        var faker = new Faker();
        return faker.Random.Bool() ? "" : new string('A', 201); // Empty or too long
    }

    /// <summary>
    /// Generates a branch with specific properties for testing.
    /// </summary>
    /// <param name="name">The branch name</param>
    /// <param name="code">The branch code</param>
    /// <param name="address">The branch address</param>
    /// <param name="active">Whether the branch is active</param>
    /// <returns>A Branch entity with the specified properties.</returns>
    public static Branch GenerateBranch(string name, string code, string address, bool active = true)
    {
        return new Branch
        {
            Name = name,
            Code = code,
            Address = address,
            Active = active
        };
    }

    /// <summary>
    /// Generates an active branch for testing.
    /// </summary>
    /// <returns>An active Branch entity.</returns>
    public static Branch GenerateActiveBranch()
    {
        var branch = BranchFaker.Generate();
        branch.Active = true;
        return branch;
    }

    /// <summary>
    /// Generates an inactive branch for testing.
    /// </summary>
    /// <returns>An inactive Branch entity.</returns>
    public static Branch GenerateInactiveBranch()
    {
        var branch = BranchFaker.Generate();
        branch.Active = false;
        return branch;
    }
} 