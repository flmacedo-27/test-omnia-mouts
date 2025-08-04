using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Product entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class ProductTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Product entities.
    /// The generated products will have valid:
    /// - Name (product names)
    /// - Code (alphanumeric codes)
    /// - Description (product descriptions)
    /// - Price (realistic prices)
    /// - StockQuantity (positive quantities)
    /// - SKU (stock keeping units)
    /// - Active status (true by default)
    /// </summary>
    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Code, f => f.Random.AlphaNumeric(8).ToUpper())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1.00m, 1000.00m))
        .RuleFor(p => p.StockQuantity, f => f.Random.Int(0, 1000))
        .RuleFor(p => p.SKU, f => f.Random.AlphaNumeric(10).ToUpper())
        .RuleFor(p => p.Active, f => f.Random.Bool())
        .UseSeed(Environment.TickCount);

    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// The generated product will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid Product entity with randomly generated data.</returns>
    public static Product GenerateValidProduct()
    {
        return ProductFaker.Generate();
    }

    /// <summary>
    /// Generates a valid product name using Faker.
    /// </summary>
    /// <returns>A valid product name.</returns>
    public static string GenerateValidProductName()
    {
        return new Faker().Commerce.ProductName();
    }

    /// <summary>
    /// Generates a valid product code using Faker.
    /// </summary>
    /// <returns>A valid product code.</returns>
    public static string GenerateValidProductCode()
    {
        return new Faker().Random.AlphaNumeric(8).ToUpper();
    }

    /// <summary>
    /// Generates a valid product description using Faker.
    /// </summary>
    /// <returns>A valid product description.</returns>
    public static string GenerateValidProductDescription()
    {
        return new Faker().Commerce.ProductDescription();
    }

    /// <summary>
    /// Generates a valid product price using Faker.
    /// </summary>
    /// <returns>A valid product price.</returns>
    public static decimal GenerateValidProductPrice()
    {
        return new Faker().Random.Decimal(1.00m, 1000.00m);
    }

    /// <summary>
    /// Generates a valid stock quantity using Faker.
    /// </summary>
    /// <returns>A valid stock quantity.</returns>
    public static int GenerateValidStockQuantity()
    {
        return new Faker().Random.Int(0, 1000);
    }

    /// <summary>
    /// Generates a valid SKU using Faker.
    /// </summary>
    /// <returns>A valid SKU.</returns>
    public static string GenerateValidSKU()
    {
        return new Faker().Random.AlphaNumeric(10).ToUpper();
    }

    /// <summary>
    /// Generates an invalid product name (empty or too long).
    /// </summary>
    /// <returns>An invalid product name.</returns>
    public static string GenerateInvalidProductName()
    {
        var faker = new Faker();
        return faker.Random.Bool() ? "" : new string('A', 101);
    }

    /// <summary>
    /// Generates an invalid product code (empty or too long).
    /// </summary>
    /// <returns>An invalid product code.</returns>
    public static string GenerateInvalidProductCode()
    {
        var faker = new Faker();
        return faker.Random.Bool() ? "" : new string('A', 21);
    }

    /// <summary>
    /// Generates an invalid product description (empty or too long).
    /// </summary>
    /// <returns>An invalid product description.</returns>
    public static string GenerateInvalidProductDescription()
    {
        var faker = new Faker();
        return faker.Random.Bool() ? "" : new string('A', 501);
    }

    /// <summary>
    /// Generates an invalid product price (negative).
    /// </summary>
    /// <returns>An invalid product price.</returns>
    public static decimal GenerateInvalidProductPrice()
    {
        return new Faker().Random.Decimal(-1000.00m, -1.00m);
    }

    /// <summary>
    /// Generates an invalid stock quantity (negative).
    /// </summary>
    /// <returns>An invalid stock quantity.</returns>
    public static int GenerateInvalidStockQuantity()
    {
        return new Faker().Random.Int(-1000, -1);
    }

    /// <summary>
    /// Generates an invalid SKU (empty or too long).
    /// </summary>
    /// <returns>An invalid SKU.</returns>
    public static string GenerateInvalidSKU()
    {
        var faker = new Faker();
        return faker.Random.Bool() ? "" : new string('A', 21);
    }

    /// <summary>
    /// Generates an active product for testing.
    /// </summary>
    /// <returns>An active Product entity.</returns>
    public static Product GenerateActiveProduct()
    {
        var product = ProductFaker.Generate();
        product.Active = true;
        return product;
    }

    /// <summary>
    /// Generates an inactive product for testing.
    /// </summary>
    /// <returns>An inactive Product entity.</returns>
    public static Product GenerateInactiveProduct()
    {
        var product = ProductFaker.Generate();
        product.Active = false;
        return product;
    }

    /// <summary>
    /// Generates a product with zero stock for testing.
    /// </summary>
    /// <returns>A Product entity with zero stock.</returns>
    public static Product GenerateProductWithZeroStock()
    {
        var product = ProductFaker.Generate();
        product.StockQuantity = 0;
        return product;
    }

    /// <summary>
    /// Generates a product with high stock for testing.
    /// </summary>
    /// <returns>A Product entity with high stock.</returns>
    public static Product GenerateProductWithHighStock()
    {
        var product = ProductFaker.Generate();
        product.StockQuantity = new Faker().Random.Int(100, 1000);
        return product;
    }

    /// <summary>
    /// Generates a product with low price for testing.
    /// </summary>
    /// <returns>A Product entity with low price.</returns>
    public static Product GenerateProductWithLowPrice()
    {
        var product = ProductFaker.Generate();
        product.Price = new Faker().Random.Decimal(1.00m, 10.00m);
        return product;
    }

    /// <summary>
    /// Generates a product with high price for testing.
    /// </summary>
    /// <returns>A Product entity with high price.</returns>
    public static Product GenerateProductWithHighPrice()
    {
        var product = ProductFaker.Generate();
        product.Price = new Faker().Random.Decimal(100.00m, 1000.00m);
        return product;
    }

    /// <summary>
    /// Generates a product with specific properties for testing.
    /// </summary>
    /// <param name="name">The product name</param>
    /// <param name="code">The product code</param>
    /// <param name="description">The product description</param>
    /// <param name="price">The product price</param>
    /// <param name="stockQuantity">The stock quantity</param>
    /// <param name="sku">The product SKU</param>
    /// <param name="active">Whether the product is active</param>
    /// <returns>A Product entity with the specified properties.</returns>
    public static Product GenerateProduct(string name, string code, string description, decimal price, int stockQuantity, string sku, bool active = true)
    {
        return new Product
        {
            Name = name,
            Code = code,
            Description = description,
            Price = price,
            StockQuantity = stockQuantity,
            SKU = sku,
            Active = active
        };
    }
} 