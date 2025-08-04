using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Sale entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Sale entities.
    /// The generated sales will have valid:
    /// - SaleNumber (alphanumeric codes)
    /// - SaleDate (recent dates)
    /// - CustomerId (valid GUIDs)
    /// - BranchId (valid GUIDs)
    /// - TotalAmount (realistic amounts)
    /// - Status (Active by default)
    /// - Items (empty collection by default)
    /// </summary>
    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10).ToUpper())
        .RuleFor(s => s.SaleDate, f => f.Date.Recent(30))
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.BranchId, f => f.Random.Guid())
        .RuleFor(s => s.TotalAmount, f => f.Random.Decimal(10.00m, 5000.00m))
        .RuleFor(s => s.Status, f => f.PickRandom(SaleStatus.Active, SaleStatus.Cancelled))
        .RuleFor(s => s.Items, f => new List<SaleItem>())
        .RuleFor(s => s.CancelledAt, f => (DateTime?)null)
        .RuleFor(s => s.CancellationReason, f => (string?)null);

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// The generated sale will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static Sale GenerateValidSale()
    {
        return SaleFaker.Generate();
    }

    /// <summary>
    /// Generates a valid sale number using Faker.
    /// </summary>
    /// <returns>A valid sale number.</returns>
    public static string GenerateValidSaleNumber()
    {
        return new Faker().Random.AlphaNumeric(10).ToUpper();
    }

    /// <summary>
    /// Generates a valid sale date using Faker.
    /// </summary>
    /// <returns>A valid sale date.</returns>
    public static DateTime GenerateValidSaleDate()
    {
        return new Faker().Date.Recent(30);
    }

    /// <summary>
    /// Generates a valid customer ID using Faker.
    /// </summary>
    /// <returns>A valid customer ID.</returns>
    public static Guid GenerateValidCustomerId()
    {
        return new Faker().Random.Guid();
    }

    /// <summary>
    /// Generates a valid branch ID using Faker.
    /// </summary>
    /// <returns>A valid branch ID.</returns>
    public static Guid GenerateValidBranchId()
    {
        return new Faker().Random.Guid();
    }

    /// <summary>
    /// Generates a valid total amount using Faker.
    /// </summary>
    /// <returns>A valid total amount.</returns>
    public static decimal GenerateValidTotalAmount()
    {
        return new Faker().Random.Decimal(10.00m, 5000.00m);
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
    /// Generates an invalid sale number (empty or too long).
    /// </summary>
    /// <returns>An invalid sale number.</returns>
    public static string GenerateInvalidSaleNumber()
    {
        var faker = new Faker();
        return faker.Random.Bool() ? "" : new string('A', 21);
    }

    /// <summary>
    /// Generates an invalid total amount (negative).
    /// </summary>
    /// <returns>An invalid total amount.</returns>
    public static decimal GenerateInvalidTotalAmount()
    {
        return new Faker().Random.Decimal(-5000.00m, -1.00m);
    }

    /// <summary>
    /// Generates an invalid cancellation reason (too long).
    /// </summary>
    /// <returns>An invalid cancellation reason.</returns>
    public static string GenerateInvalidCancellationReason()
    {
        return new string('A', 501); // Too long
    }

    /// <summary>
    /// Generates an active sale for testing.
    /// </summary>
    /// <returns>An active Sale entity.</returns>
    public static Sale GenerateActiveSale()
    {
        var sale = SaleFaker.Generate();
        sale.Status = SaleStatus.Active;
        sale.CancelledAt = null;
        sale.CancellationReason = null;
        return sale;
    }

    /// <summary>
    /// Generates a cancelled sale for testing.
    /// </summary>
    /// <returns>A cancelled Sale entity.</returns>
    public static Sale GenerateCancelledSale()
    {
        var sale = SaleFaker.Generate();
        sale.Status = SaleStatus.Cancelled;
        sale.CancelledAt = DateTime.UtcNow;
        sale.CancellationReason = GenerateValidCancellationReason();
        return sale;
    }

    /// <summary>
    /// Generates a sale with high total amount for testing.
    /// </summary>
    /// <returns>A Sale entity with high total amount.</returns>
    public static Sale GenerateSaleWithHighAmount()
    {
        var sale = SaleFaker.Generate();
        sale.TotalAmount = new Faker().Random.Decimal(1000.00m, 10000.00m);
        return sale;
    }

    /// <summary>
    /// Generates a sale with low total amount for testing.
    /// </summary>
    /// <returns>A Sale entity with low total amount.</returns>
    public static Sale GenerateSaleWithLowAmount()
    {
        var sale = SaleFaker.Generate();
        sale.TotalAmount = new Faker().Random.Decimal(1.00m, 100.00m);
        return sale;
    }

    /// <summary>
    /// Generates a sale with zero total amount for testing.
    /// </summary>
    /// <returns>A Sale entity with zero total amount.</returns>
    public static Sale GenerateSaleWithZeroAmount()
    {
        var sale = SaleFaker.Generate();
        sale.TotalAmount = 0;
        return sale;
    }

    /// <summary>
    /// Generates a sale with specific properties for testing.
    /// </summary>
    /// <param name="saleNumber">The sale number</param>
    /// <param name="saleDate">The sale date</param>
    /// <param name="customerId">The customer ID</param>
    /// <param name="branchId">The branch ID</param>
    /// <param name="totalAmount">The total amount</param>
    /// <param name="status">The sale status</param>
    /// <param name="cancelledAt">The cancellation date</param>
    /// <param name="cancellationReason">The cancellation reason</param>
    /// <returns>A Sale entity with the specified properties.</returns>
    public static Sale GenerateSale(string saleNumber, DateTime saleDate, Guid customerId, Guid branchId, decimal totalAmount, SaleStatus status, DateTime? cancelledAt = null, string? cancellationReason = null)
    {
        return new Sale
        {
            SaleNumber = saleNumber,
            SaleDate = saleDate,
            CustomerId = customerId,
            BranchId = branchId,
            TotalAmount = totalAmount,
            Status = status,
            CancelledAt = cancelledAt,
            CancellationReason = cancellationReason,
            Items = new List<SaleItem>()
        };
    }

    /// <summary>
    /// Generates a sale with items for testing.
    /// </summary>
    /// <param name="items">The sale items</param>
    /// <returns>A Sale entity with the specified items.</returns>
    public static Sale GenerateSaleWithItems(List<SaleItem> items)
    {
        var sale = SaleFaker.Generate();
        sale.Items = items;
        sale.CalculateTotal();
        return sale;
    }

    /// <summary>
    /// Generates a sale with a single item for testing.
    /// </summary>
    /// <param name="product">The product for the item</param>
    /// <param name="quantity">The quantity</param>
    /// <returns>A Sale entity with a single item.</returns>
    public static Sale GenerateSaleWithSingleItem(Product product, int quantity)
    {
        var sale = SaleFaker.Generate();
        var item = new SaleItem
        {
            ProductId = product.Id,
            Product = product,
            Quantity = quantity,
            UnitPrice = product.Price,
            Status = SaleItemStatus.Active
        };
        item.CalculateDiscount();
        
        sale.Items = new List<SaleItem> { item };
        sale.CalculateTotal();
        return sale;
    }

    /// <summary>
    /// Generates a sale with multiple items for testing.
    /// </summary>
    /// <param name="products">The products for the items</param>
    /// <returns>A Sale entity with multiple items.</returns>
    public static Sale GenerateSaleWithMultipleItems(List<Product> products)
    {
        var sale = SaleFaker.Generate();
        var items = new List<SaleItem>();
        
        foreach (var product in products)
        {
            var quantity = new Faker().Random.Int(1, 5);
            var item = new SaleItem
            {
                ProductId = product.Id,
                Product = product,
                Quantity = quantity,
                UnitPrice = product.Price,
                Status = SaleItemStatus.Active
            };
            item.CalculateDiscount();
            items.Add(item);
        }
        
        sale.Items = items;
        sale.CalculateTotal();
        return sale;
    }
} 