using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for GetSaleHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class GetSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid GetSaleCommand instances.
    /// The generated commands will have valid sale IDs.
    /// </summary>
    private static readonly Faker<GetSaleCommand> getSaleCommandFaker = new Faker<GetSaleCommand>()
        .RuleFor(c => c.Id, f => f.Random.Guid());

    /// <summary>
    /// Configures the Faker to generate valid Sale entities.
    /// The generated sales will have valid:
    /// - SaleNumber (alphanumeric codes)
    /// - SaleDate (recent dates)
    /// - CustomerId and BranchId (valid GUIDs)
    /// - TotalAmount (realistic amounts)
    /// - Status (Active or Cancelled)
    /// </summary>
    private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
        .RuleFor(s => s.Id, f => f.Random.Guid())
        .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10).ToUpper())
        .RuleFor(s => s.SaleDate, f => f.Date.Recent())
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.BranchId, f => f.Random.Guid())
        .RuleFor(s => s.TotalAmount, f => f.Random.Decimal(10.00m, 1000.00m))
        .RuleFor(s => s.Status, f => f.PickRandom(SaleStatus.Active, SaleStatus.Cancelled))
        .RuleFor(s => s.CancelledAt, f => f.Date.Recent())
        .RuleFor(s => s.CancellationReason, f => f.Lorem.Sentence())
        .RuleFor(s => s.CreatedAt, f => f.Date.Past())
        .RuleFor(s => s.UpdatedAt, f => f.Date.Recent());

    /// <summary>
    /// Generates a valid GetSaleCommand with randomized data.
    /// The generated command will have a valid sale ID.
    /// </summary>
    /// <returns>A valid GetSaleCommand with randomly generated data.</returns>
    public static GetSaleCommand GenerateValidCommand()
    {
        return getSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a GetSaleCommand with a specific sale ID.
    /// </summary>
    /// <param name="saleId">The sale ID to use</param>
    /// <returns>A GetSaleCommand with the specified sale ID.</returns>
    public static GetSaleCommand GenerateCommandWithId(Guid saleId)
    {
        return new GetSaleCommand { Id = saleId };
    }

    /// <summary>
    /// Generates an invalid GetSaleCommand with empty ID for testing validation failure.
    /// </summary>
    /// <returns>A GetSaleCommand with an empty ID that should fail validation.</returns>
    public static GetSaleCommand GenerateInvalidCommand()
    {
        return new GetSaleCommand { Id = Guid.Empty };
    }

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// The generated sale will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static Sale GenerateValidSale()
    {
        return saleFaker.Generate();
    }

    /// <summary>
    /// Generates a Sale entity with a specific ID.
    /// </summary>
    /// <param name="saleId">The sale ID to use</param>
    /// <returns>A Sale entity with the specified ID.</returns>
    public static Sale GenerateSaleWithId(Guid saleId)
    {
        var sale = saleFaker.Generate();
        sale.Id = saleId;
        return sale;
    }

    /// <summary>
    /// Generates an active Sale entity.
    /// </summary>
    /// <returns>An active Sale entity.</returns>
    public static Sale GenerateActiveSale()
    {
        var sale = saleFaker.Generate();
        sale.Status = SaleStatus.Active;
        sale.CancelledAt = null;
        sale.CancellationReason = null;
        return sale;
    }

    /// <summary>
    /// Generates a cancelled Sale entity.
    /// </summary>
    /// <returns>A cancelled Sale entity.</returns>
    public static Sale GenerateCancelledSale()
    {
        var sale = saleFaker.Generate();
        sale.Status = SaleStatus.Cancelled;
        return sale;
    }

    /// <summary>
    /// Generates a Sale entity with high total amount.
    /// </summary>
    /// <returns>A Sale entity with high total amount.</returns>
    public static Sale GenerateHighValueSale()
    {
        var sale = saleFaker.Generate();
        sale.TotalAmount = new Faker().Random.Decimal(500.00m, 1000.00m);
        return sale;
    }

    /// <summary>
    /// Generates a Sale entity with low total amount.
    /// </summary>
    /// <returns>A Sale entity with low total amount.</returns>
    public static Sale GenerateLowValueSale()
    {
        var sale = saleFaker.Generate();
        sale.TotalAmount = new Faker().Random.Decimal(10.00m, 100.00m);
        return sale;
    }

    /// <summary>
    /// Generates a GetSaleResult with randomized data.
    /// </summary>
    /// <returns>A GetSaleResult with randomly generated data.</returns>
    public static GetSaleResult GenerateValidResult()
    {
        var sale = saleFaker.Generate();
        return new GetSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = new Faker().Person.FullName,
            BranchId = sale.BranchId,
            BranchName = new Faker().Company.CompanyName(),
            TotalAmount = sale.TotalAmount,
            Status = sale.Status,
            CancelledAt = sale.CancelledAt,
            CancellationReason = sale.CancellationReason,
            Items = new List<GetSaleItemResult>()
        };
    }

    /// <summary>
    /// Generates a GetSaleResult with specific properties.
    /// </summary>
    /// <param name="sale">The sale entity to base the result on</param>
    /// <returns>A GetSaleResult based on the provided sale.</returns>
    public static GetSaleResult GenerateResultFromSale(Sale sale)
    {
        return new GetSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = new Faker().Person.FullName,
            BranchId = sale.BranchId,
            BranchName = new Faker().Company.CompanyName(),
            TotalAmount = sale.TotalAmount,
            Status = sale.Status,
            CancelledAt = sale.CancelledAt,
            CancellationReason = sale.CancellationReason,
            Items = new List<GetSaleItemResult>()
        };
    }
} 