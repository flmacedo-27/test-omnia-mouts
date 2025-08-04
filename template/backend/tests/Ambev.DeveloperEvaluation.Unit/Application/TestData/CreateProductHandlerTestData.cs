using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateProductHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateProductHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateProductCommand instances.
    /// The generated commands will have valid:
    /// - Name (product names)
    /// - Code (alphanumeric codes)
    /// - Description (product descriptions)
    /// - Price (realistic prices)
    /// - StockQuantity (positive quantities)
    /// - SKU (stock keeping units)
    /// </summary>
    private static readonly Faker<CreateProductCommand> createProductCommandFaker = new Faker<CreateProductCommand>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Code, f => f.Random.AlphaNumeric(8).ToUpper())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1.00m, 1000.00m))
        .RuleFor(p => p.StockQuantity, f => f.Random.Int(0, 1000))
        .RuleFor(p => p.SKU, f => f.Random.AlphaNumeric(10).ToUpper());

    /// <summary>
    /// Configures the Faker to generate valid Product entities.
    /// The generated products will have valid:
    /// - Name (product names)
    /// - Code (alphanumeric codes)
    /// - Description (product descriptions)
    /// - Price (realistic prices)
    /// - StockQuantity (positive quantities)
    /// - SKU (stock keeping units)
    /// - Active status
    /// </summary>
    private static readonly Faker<Product> productFaker = new Faker<Product>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Code, f => f.Random.AlphaNumeric(8).ToUpper())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1.00m, 1000.00m))
        .RuleFor(p => p.StockQuantity, f => f.Random.Int(0, 1000))
        .RuleFor(p => p.SKU, f => f.Random.AlphaNumeric(10).ToUpper())
        .RuleFor(p => p.Active, f => f.Random.Bool())
        .RuleFor(p => p.CreatedAt, f => f.Date.Past())
        .RuleFor(p => p.UpdatedAt, f => f.Date.Recent());

    /// <summary>
    /// Generates a valid CreateProductCommand with randomized data.
    /// The generated command will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid CreateProductCommand with randomly generated data.</returns>
    public static CreateProductCommand GenerateValidCommand()
    {
        return createProductCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a CreateProductCommand with existing SKU for testing validation failure.
    /// </summary>
    /// <returns>A CreateProductCommand with an SKU that already exists in the system.</returns>
    public static CreateProductCommand GenerateCommandWithExistingSKU()
    {
        var command = createProductCommandFaker.Generate();
        command.SKU = "EXISTING-SKU-001";
        return command;
    }

    /// <summary>
    /// Generates a CreateProductCommand with existing code for testing validation failure.
    /// </summary>
    /// <returns>A CreateProductCommand with a code that already exists in the system.</returns>
    public static CreateProductCommand GenerateCommandWithExistingCode()
    {
        var command = createProductCommandFaker.Generate();
        command.Code = "EXISTING001";
        return command;
    }

    /// <summary>
    /// Generates an invalid CreateProductCommand for testing validation failure.
    /// </summary>
    /// <returns>A CreateProductCommand with invalid data that should fail validation.</returns>
    public static CreateProductCommand GenerateInvalidCommand()
    {
        return new CreateProductCommand
        {
            Name = "",
            Code = "",
            Description = "",
            Price = -10.00m,
            StockQuantity = -5,
            SKU = ""
        };
    }

    /// <summary>
    /// Generates a CreateProductCommand with high price for testing.
    /// </summary>
    /// <returns>A CreateProductCommand with high price.</returns>
    public static CreateProductCommand GenerateCommandWithHighPrice()
    {
        var command = createProductCommandFaker.Generate();
        command.Price = new Faker().Random.Decimal(500.00m, 1000.00m);
        return command;
    }

    /// <summary>
    /// Generates a CreateProductCommand with low price for testing.
    /// </summary>
    /// <returns>A CreateProductCommand with low price.</returns>
    public static CreateProductCommand GenerateCommandWithLowPrice()
    {
        var command = createProductCommandFaker.Generate();
        command.Price = new Faker().Random.Decimal(1.00m, 10.00m);
        return command;
    }

    /// <summary>
    /// Generates a CreateProductCommand with high stock quantity for testing.
    /// </summary>
    /// <returns>A CreateProductCommand with high stock quantity.</returns>
    public static CreateProductCommand GenerateCommandWithHighStock()
    {
        var command = createProductCommandFaker.Generate();
        command.StockQuantity = new Faker().Random.Int(500, 1000);
        return command;
    }

    /// <summary>
    /// Generates a CreateProductCommand with zero stock quantity for testing.
    /// </summary>
    /// <returns>A CreateProductCommand with zero stock quantity.</returns>
    public static CreateProductCommand GenerateCommandWithZeroStock()
    {
        var command = createProductCommandFaker.Generate();
        command.StockQuantity = 0;
        return command;
    }

    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// The generated product will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid Product entity with randomly generated data.</returns>
    public static Product GenerateValidProduct()
    {
        return productFaker.Generate();
    }

    /// <summary>
    /// Generates a Product entity with a specific ID.
    /// </summary>
    /// <param name="productId">The product ID to use</param>
    /// <returns>A Product entity with the specified ID.</returns>
    public static Product GenerateProductWithId(Guid productId)
    {
        var product = productFaker.Generate();
        product.Id = productId;
        return product;
    }

    /// <summary>
    /// Generates an active Product entity.
    /// </summary>
    /// <returns>An active Product entity.</returns>
    public static Product GenerateActiveProduct()
    {
        var product = productFaker.Generate();
        product.Active = true;
        return product;
    }

    /// <summary>
    /// Generates an inactive Product entity.
    /// </summary>
    /// <returns>An inactive Product entity.</returns>
    public static Product GenerateInactiveProduct()
    {
        var product = productFaker.Generate();
        product.Active = false;
        return product;
    }

    /// <summary>
    /// Generates a CreateProductResult with randomized data.
    /// </summary>
    /// <returns>A CreateProductResult with randomly generated data.</returns>
    public static CreateProductResult GenerateValidResult()
    {
        var product = productFaker.Generate();
        return new CreateProductResult
        {
            Id = product.Id,
            Name = product.Name,
            Code = product.Code,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            SKU = product.SKU,
            Active = product.Active,
            CreatedAt = product.CreatedAt
        };
    }

    /// <summary>
    /// Generates a CreateProductResult with specific properties.
    /// </summary>
    /// <param name="product">The product entity to base the result on</param>
    /// <returns>A CreateProductResult based on the provided product.</returns>
    public static CreateProductResult GenerateResultFromProduct(Product product)
    {
        return new CreateProductResult
        {
            Id = product.Id,
            Name = product.Name,
            Code = product.Code,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            SKU = product.SKU,
            Active = product.Active,
            CreatedAt = product.CreatedAt
        };
    }
} 