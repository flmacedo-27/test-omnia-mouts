using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for GetProductHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class GetProductHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid GetProductCommand instances.
    /// The generated commands will have valid product IDs.
    /// </summary>
    private static readonly Faker<GetProductCommand> getProductCommandFaker = new Faker<GetProductCommand>()
        .RuleFor(c => c.Id, f => f.Random.Guid());

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
    /// Generates a valid GetProductCommand with randomized data.
    /// The generated command will have a valid product ID.
    /// </summary>
    /// <returns>A valid GetProductCommand with randomly generated data.</returns>
    public static GetProductCommand GenerateValidCommand()
    {
        return getProductCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a GetProductCommand with a specific product ID.
    /// </summary>
    /// <param name="productId">The product ID to use</param>
    /// <returns>A GetProductCommand with the specified product ID.</returns>
    public static GetProductCommand GenerateCommandWithId(Guid productId)
    {
        return new GetProductCommand { Id = productId };
    }

    /// <summary>
    /// Generates an invalid GetProductCommand with empty ID for testing validation failure.
    /// </summary>
    /// <returns>A GetProductCommand with an empty ID that should fail validation.</returns>
    public static GetProductCommand GenerateInvalidCommand()
    {
        return new GetProductCommand { Id = Guid.Empty };
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
    /// Generates a Product entity with high price for testing.
    /// </summary>
    /// <returns>A Product entity with high price.</returns>
    public static Product GenerateHighPriceProduct()
    {
        var product = productFaker.Generate();
        product.Price = new Faker().Random.Decimal(500.00m, 1000.00m);
        return product;
    }

    /// <summary>
    /// Generates a Product entity with low price for testing.
    /// </summary>
    /// <returns>A Product entity with low price.</returns>
    public static Product GenerateLowPriceProduct()
    {
        var product = productFaker.Generate();
        product.Price = new Faker().Random.Decimal(1.00m, 10.00m);
        return product;
    }

    /// <summary>
    /// Generates a Product entity with high stock quantity for testing.
    /// </summary>
    /// <returns>A Product entity with high stock quantity.</returns>
    public static Product GenerateHighStockProduct()
    {
        var product = productFaker.Generate();
        product.StockQuantity = new Faker().Random.Int(500, 1000);
        return product;
    }

    /// <summary>
    /// Generates a Product entity with zero stock quantity for testing.
    /// </summary>
    /// <returns>A Product entity with zero stock quantity.</returns>
    public static Product GenerateZeroStockProduct()
    {
        var product = productFaker.Generate();
        product.StockQuantity = 0;
        return product;
    }

    /// <summary>
    /// Generates a GetProductResult with randomized data.
    /// </summary>
    /// <returns>A GetProductResult with randomly generated data.</returns>
    public static GetProductResult GenerateValidResult()
    {
        var product = productFaker.Generate();
        return new GetProductResult
        {
            Id = product.Id,
            Name = product.Name,
            Code = product.Code,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            SKU = product.SKU,
            Active = product.Active,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    /// <summary>
    /// Generates a GetProductResult with specific properties.
    /// </summary>
    /// <param name="product">The product entity to base the result on</param>
    /// <returns>A GetProductResult based on the provided product.</returns>
    public static GetProductResult GenerateResultFromProduct(Product product)
    {
        return new GetProductResult
        {
            Id = product.Id,
            Name = product.Name,
            Code = product.Code,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            SKU = product.SKU,
            Active = product.Active,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
} 