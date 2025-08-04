using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateSaleHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateSaleItemCommand instances.
    /// </summary>
    private static readonly Faker<CreateSaleItemCommand> createSaleItemCommandFaker = new Faker<CreateSaleItemCommand>()
        .RuleFor(i => i.ProductId, f => f.Random.Guid())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(1.00m, 100.00m));

    /// <summary>
    /// Configures the Faker to generate valid CreateSaleCommand instances.
    /// The generated commands will have valid:
    /// - CustomerId (valid GUID)
    /// - BranchId (valid GUID)
    /// - Items (list of valid sale items)
    /// </summary>
    private static readonly Faker<CreateSaleCommand> createSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.BranchId, f => f.Random.Guid())
        .RuleFor(s => s.Items, f => createSaleItemCommandFaker.Generate(f.Random.Int(1, 3)));

    /// <summary>
    /// Generates a valid CreateSaleCommand with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid CreateSaleCommand with randomly generated data.</returns>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a CreateSaleCommand with a single item for testing.
    /// </summary>
    /// <returns>A CreateSaleCommand with one sale item.</returns>
    public static CreateSaleCommand GenerateCommandWithSingleItem()
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = new List<CreateSaleItemCommand>
        {
            createSaleItemCommandFaker.Generate()
        };
        return command;
    }

    /// <summary>
    /// Generates a CreateSaleCommand with multiple items for testing discount scenarios.
    /// </summary>
    /// <param name="itemCount">The number of items to generate.</param>
    /// <returns>A CreateSaleCommand with multiple sale items.</returns>
    public static CreateSaleCommand GenerateCommandWithMultipleItems(int itemCount)
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = createSaleItemCommandFaker.Generate(itemCount);
        return command;
    }

    /// <summary>
    /// Generates a CreateSaleCommand with items that qualify for 10% discount (4-9 items).
    /// </summary>
    /// <returns>A CreateSaleCommand with items for 10% discount.</returns>
    public static CreateSaleCommand GenerateCommandForTenPercentDiscount()
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = new List<CreateSaleItemCommand>
        {
            new CreateSaleItemCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = 5,
                UnitPrice = 10.00m
            }
        };
        return command;
    }

    /// <summary>
    /// Generates a CreateSaleCommand with items that qualify for 20% discount (10-20 items).
    /// </summary>
    /// <returns>A CreateSaleCommand with items for 20% discount.</returns>
    public static CreateSaleCommand GenerateCommandForTwentyPercentDiscount()
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = new List<CreateSaleItemCommand>
        {
            new CreateSaleItemCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = 15,
                UnitPrice = 10.00m
            }
        };
        return command;
    }

    /// <summary>
    /// Generates a CreateSaleCommand with items that have no discount (1-3 items).
    /// </summary>
    /// <returns>A CreateSaleCommand with items for no discount.</returns>
    public static CreateSaleCommand GenerateCommandForNoDiscount()
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = new List<CreateSaleItemCommand>
        {
            new CreateSaleItemCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 10.00m
            }
        };
        return command;
    }

    /// <summary>
    /// Generates an invalid CreateSaleCommand for testing validation failure.
    /// </summary>
    /// <returns>A CreateSaleCommand with invalid data that should fail validation.</returns>
    public static CreateSaleCommand GenerateInvalidCommand()
    {
        return new CreateSaleCommand
        {
            CustomerId = Guid.Empty,
            BranchId = Guid.Empty,
            Items = new List<CreateSaleItemCommand>()
        };
    }

    /// <summary>
    /// Generates a CreateSaleCommand with items that exceed the maximum quantity (above 20).
    /// </summary>
    /// <returns>A CreateSaleCommand with items that should fail validation.</returns>
    public static CreateSaleCommand GenerateCommandWithExcessiveQuantity()
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = new List<CreateSaleItemCommand>
        {
            new CreateSaleItemCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = 25,
                UnitPrice = 10.00m
            }
        };
        return command;
    }

    /// <summary>
    /// Generates a CreateSaleCommand with specific customer and branch IDs for testing.
    /// </summary>
    /// <param name="customerId">The customer ID to use.</param>
    /// <param name="branchId">The branch ID to use.</param>
    /// <returns>A CreateSaleCommand with the specified IDs.</returns>
    public static CreateSaleCommand GenerateCommandWithSpecificIds(Guid customerId, Guid branchId)
    {
        var command = createSaleCommandFaker.Generate();
        command.CustomerId = customerId;
        command.BranchId = branchId;
        return command;
    }

    /// <summary>
    /// Generates a CreateSaleItemCommand with specific values for testing.
    /// </summary>
    /// <param name="productId">The product ID to use.</param>
    /// <param name="quantity">The quantity to use.</param>
    /// <param name="unitPrice">The unit price to use.</param>
    /// <returns>A CreateSaleItemCommand with the specified values.</returns>
    public static CreateSaleItemCommand GenerateSaleItem(Guid productId, int quantity, decimal unitPrice)
    {
        return new CreateSaleItemCommand
        {
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
} 