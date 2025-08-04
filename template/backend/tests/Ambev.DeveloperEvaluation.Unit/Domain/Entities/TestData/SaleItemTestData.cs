using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for SaleItem entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleItemTestData
{
    /// <summary>
    /// Configures the Faker to generate valid SaleItem entities.
    /// The generated sale items will have valid:
    /// - SaleId (valid GUIDs)
    /// - ProductId (valid GUIDs)
    /// - Quantity (positive quantities)
    /// - UnitPrice (realistic prices)
    /// - DiscountPercentage (calculated based on quantity)
    /// - DiscountAmount (calculated based on discount percentage)
    /// - TotalAmount (calculated based on quantity, price and discount)
    /// - Status (Active by default)
    /// </summary>
    private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
        .RuleFor(si => si.SaleId, f => f.Random.Guid())
        .RuleFor(si => si.ProductId, f => f.Random.Guid())
        .RuleFor(si => si.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(si => si.UnitPrice, f => f.Random.Decimal(1.00m, 100.00m))
        .RuleFor(si => si.DiscountPercentage, f => 0m)
        .RuleFor(si => si.DiscountAmount, f => 0m)
        .RuleFor(si => si.TotalAmount, f => 0m)
        .RuleFor(si => si.Status, f => f.PickRandom(SaleItemStatus.Active, SaleItemStatus.Cancelled))
        .RuleFor(si => si.CancelledAt, f => (DateTime?)null)
        .RuleFor(si => si.CancellationReason, f => (string?)null);

    /// <summary>
    /// Generates a valid SaleItem entity with randomized data.
    /// The generated sale item will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid SaleItem entity with randomly generated data.</returns>
    public static SaleItem GenerateValidSaleItem()
    {
        var item = SaleItemFaker.Generate();
        item.CalculateDiscount();
        return item;
    }

    /// <summary>
    /// Generates a valid sale ID using Faker.
    /// </summary>
    /// <returns>A valid sale ID.</returns>
    public static Guid GenerateValidSaleId()
    {
        return new Faker().Random.Guid();
    }

    /// <summary>
    /// Generates a valid product ID using Faker.
    /// </summary>
    /// <returns>A valid product ID.</returns>
    public static Guid GenerateValidProductId()
    {
        return new Faker().Random.Guid();
    }

    /// <summary>
    /// Generates a valid quantity using Faker.
    /// </summary>
    /// <returns>A valid quantity.</returns>
    public static int GenerateValidQuantity()
    {
        return new Faker().Random.Int(1, 20);
    }

    /// <summary>
    /// Generates a valid unit price using Faker.
    /// </summary>
    /// <returns>A valid unit price.</returns>
    public static decimal GenerateValidUnitPrice()
    {
        return new Faker().Random.Decimal(1.00m, 100.00m);
    }

    /// <summary>
    /// Generates a valid cancellation reason using Faker.
    /// </summary>
    /// <returns>A valid cancellation reason.</returns>
    public static string GenerateValidCancellationReason()
    {
        return new Faker().Lorem.Sentence();
    }

    /// <summary>
    /// Generates an invalid quantity (zero or negative).
    /// </summary>
    /// <returns>An invalid quantity.</returns>
    public static int GenerateInvalidQuantity()
    {
        var faker = new Faker();
        return faker.Random.Bool() ? 0 : faker.Random.Int(-100, -1);
    }

    /// <summary>
    /// Generates an invalid unit price (negative).
    /// </summary>
    /// <returns>An invalid unit price.</returns>
    public static decimal GenerateInvalidUnitPrice()
    {
        return new Faker().Random.Decimal(-100.00m, -1.00m);
    }

    /// <summary>
    /// Generates an invalid cancellation reason (too long).
    /// </summary>
    /// <returns>An invalid cancellation reason.</returns>
    public static string GenerateInvalidCancellationReason()
    {
        return new string('A', 501);
    }

    /// <summary>
    /// Generates an active sale item for testing.
    /// </summary>
    /// <returns>An active SaleItem entity.</returns>
    public static SaleItem GenerateActiveSaleItem()
    {
        var item = SaleItemFaker.Generate();
        item.Status = SaleItemStatus.Active;
        item.CancelledAt = null;
        item.CancellationReason = null;
        item.CalculateDiscount();
        return item;
    }

    /// <summary>
    /// Generates a cancelled sale item for testing.
    /// </summary>
    /// <returns>A cancelled SaleItem entity.</returns>
    public static SaleItem GenerateCancelledSaleItem()
    {
        var item = SaleItemFaker.Generate();
        item.Status = SaleItemStatus.Cancelled;
        item.CancelledAt = DateTime.UtcNow;
        item.CancellationReason = GenerateValidCancellationReason();
        item.CalculateDiscount();
        return item;
    }

    /// <summary>
    /// Generates a sale item with low quantity (1-3 items) for testing.
    /// </summary>
    /// <returns>A SaleItem entity with low quantity (no discount).</returns>
    public static SaleItem GenerateSaleItemWithLowQuantity()
    {
        var item = SaleItemFaker.Generate();
        item.Quantity = new Faker().Random.Int(1, 3);
        item.CalculateDiscount();
        return item;
    }

    /// <summary>
    /// Generates a sale item with medium quantity (4-9 items) for testing.
    /// </summary>
    /// <returns>A SaleItem entity with medium quantity (10% discount).</returns>
    public static SaleItem GenerateSaleItemWithMediumQuantity()
    {
        var item = SaleItemFaker.Generate();
        item.Quantity = new Faker().Random.Int(4, 9);
        item.CalculateDiscount();
        return item;
    }

    /// <summary>
    /// Generates a sale item with high quantity (10-20 items) for testing.
    /// </summary>
    /// <returns>A SaleItem entity with high quantity (20% discount).</returns>
    public static SaleItem GenerateSaleItemWithHighQuantity()
    {
        var item = SaleItemFaker.Generate();
        item.Quantity = new Faker().Random.Int(10, 20);
        item.CalculateDiscount();
        return item;
    }

    /// <summary>
    /// Generates a sale item with maximum quantity (20 items) for testing.
    /// </summary>
    /// <returns>A SaleItem entity with maximum quantity (20% discount).</returns>
    public static SaleItem GenerateSaleItemWithMaximumQuantity()
    {
        var item = SaleItemFaker.Generate();
        item.Quantity = 20;
        item.CalculateDiscount();
        return item;
    }

    /// <summary>
    /// Generates a sale item with low price for testing.
    /// </summary>
    /// <returns>A SaleItem entity with low price.</returns>
    public static SaleItem GenerateSaleItemWithLowPrice()
    {
        var item = SaleItemFaker.Generate();
        item.UnitPrice = new Faker().Random.Decimal(1.00m, 10.00m);
        item.CalculateDiscount();
        return item;
    }

    /// <summary>
    /// Generates a sale item with high price for testing.
    /// </summary>
    /// <returns>A SaleItem entity with high price.</returns>
    public static SaleItem GenerateSaleItemWithHighPrice()
    {
        var item = SaleItemFaker.Generate();
        item.UnitPrice = new Faker().Random.Decimal(50.00m, 100.00m);
        item.CalculateDiscount();
        return item;
    }

    /// <summary>
    /// Generates a sale item with specific properties for testing.
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="productId">The product ID</param>
    /// <param name="quantity">The quantity</param>
    /// <param name="unitPrice">The unit price</param>
    /// <param name="status">The item status</param>
    /// <param name="cancelledAt">The cancellation date</param>
    /// <param name="cancellationReason">The cancellation reason</param>
    /// <returns>A SaleItem entity with the specified properties.</returns>
    public static SaleItem GenerateSaleItem(Guid saleId, Guid productId, int quantity, decimal unitPrice, SaleItemStatus status, DateTime? cancelledAt = null, string? cancellationReason = null)
    {
        var item = new SaleItem
        {
            SaleId = saleId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Status = status,
            CancelledAt = cancelledAt,
            CancellationReason = cancellationReason
        };
        item.CalculateDiscount();
        return item;
    }

    /// <summary>
    /// Generates a sale item with product for testing.
    /// </summary>
    /// <param name="product">The product</param>
    /// <param name="quantity">The quantity</param>
    /// <returns>A SaleItem entity with the specified product and quantity.</returns>
    public static SaleItem GenerateSaleItemWithProduct(Product product, int quantity)
    {
        var item = new SaleItem
        {
            ProductId = product.Id,
            Product = product,
            Quantity = quantity,
            UnitPrice = product.Price,
            Status = SaleItemStatus.Active
        };
        item.CalculateDiscount();
        return item;
    }

    /// <summary>
    /// Generates multiple sale items for testing.
    /// </summary>
    /// <param name="count">The number of items to generate</param>
    /// <returns>A list of SaleItem entities.</returns>
    public static List<SaleItem> GenerateMultipleSaleItems(int count)
    {
        var items = new List<SaleItem>();
        for (int i = 0; i < count; i++)
        {
            items.Add(GenerateValidSaleItem());
        }
        return items;
    }
} 