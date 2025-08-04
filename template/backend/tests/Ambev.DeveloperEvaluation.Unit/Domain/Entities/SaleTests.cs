using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover business methods and property updates.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that a new sale is created with default active status.
    /// </summary>
    [Fact(DisplayName = "New sale should be created with active status")]
    public void Given_NewSale_When_Created_Then_ShouldBeActive()
    {
        // Arrange & Act
        var sale = new Sale();

        // Assert
        Assert.Equal(SaleStatus.Active, sale.Status);
        Assert.Equal(string.Empty, sale.SaleNumber);
        Assert.Equal(DateTime.MinValue, sale.SaleDate);
        Assert.Equal(Guid.Empty, sale.CustomerId);
        Assert.Equal(Guid.Empty, sale.BranchId);
        Assert.Equal(0, sale.TotalAmount);
        Assert.Empty(sale.Items);
        Assert.Null(sale.CancelledAt);
        Assert.Null(sale.CancellationReason);
    }

    /// <summary>
    /// Tests that the CalculateTotal method correctly calculates the total amount.
    /// </summary>
    [Fact(DisplayName = "CalculateTotal method should correctly calculate total amount")]
    public void Given_SaleWithItems_When_CalculateTotal_Then_ShouldCalculateCorrectTotal()
    {
        // Arrange
        var sale = SaleTestData.GenerateActiveSale();
        var product1 = ProductTestData.GenerateValidProduct();
        var product2 = ProductTestData.GenerateValidProduct();
        
        var item1 = SaleItemTestData.GenerateSaleItemWithProduct(product1, 2);
        var item2 = SaleItemTestData.GenerateSaleItemWithProduct(product2, 3);
        
        sale.Items = new List<SaleItem> { item1, item2 };

        // Act
        sale.CalculateTotal();

        // Assert
        var expectedTotal = item1.TotalAmount + item2.TotalAmount;
        Assert.Equal(expectedTotal, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that the CalculateTotal method sets zero total when no items.
    /// </summary>
    [Fact(DisplayName = "CalculateTotal method should set zero total when no items")]
    public void Given_SaleWithNoItems_When_CalculateTotal_Then_ShouldSetZeroTotal()
    {
        // Arrange
        var sale = SaleTestData.GenerateActiveSale();
        sale.Items = new List<SaleItem>();

        // Act
        sale.CalculateTotal();

        // Assert
        Assert.Equal(0, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that the Cancel method correctly cancels the sale.
    /// </summary>
    [Fact(DisplayName = "Cancel method should correctly cancel the sale")]
    public void Given_ActiveSale_When_Cancelled_Then_ShouldBeCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateActiveSale();
        var reason = SaleTestData.GenerateValidCancellationReason();

        // Act
        sale.Cancel(reason);

        // Assert
        Assert.Equal(SaleStatus.Cancelled, sale.Status);
        Assert.NotNull(sale.CancelledAt);
        Assert.Equal(reason, sale.CancellationReason);
    }

    /// <summary>
    /// Tests that the Cancel method sets the cancellation date.
    /// </summary>
    [Fact(DisplayName = "Cancel method should set cancellation date")]
    public void Given_Sale_When_Cancelled_Then_ShouldSetCancellationDate()
    {
        // Arrange
        var sale = SaleTestData.GenerateActiveSale();
        var reason = SaleTestData.GenerateValidCancellationReason();

        // Act
        sale.Cancel(reason);

        // Assert
        Assert.NotNull(sale.CancelledAt);
        Assert.True(sale.CancelledAt.Value > DateTime.UtcNow.AddMinutes(-1));
    }

    /// <summary>
    /// Tests that sale properties can be set directly.
    /// </summary>
    [Fact(DisplayName = "Sale properties should be settable directly")]
    public void Given_Sale_When_PropertiesSet_Then_ShouldBeSetCorrectly()
    {
        // Arrange
        var sale = new Sale();
        var saleNumber = SaleTestData.GenerateValidSaleNumber();
        var saleDate = SaleTestData.GenerateValidSaleDate();
        var customerId = SaleTestData.GenerateValidCustomerId();
        var branchId = SaleTestData.GenerateValidBranchId();
        var totalAmount = SaleTestData.GenerateValidTotalAmount();

        // Act
        sale.SaleNumber = saleNumber;
        sale.SaleDate = saleDate;
        sale.CustomerId = customerId;
        sale.BranchId = branchId;
        sale.TotalAmount = totalAmount;
        sale.Status = SaleStatus.Cancelled;

        // Assert
        Assert.Equal(saleNumber, sale.SaleNumber);
        Assert.Equal(saleDate, sale.SaleDate);
        Assert.Equal(customerId, sale.CustomerId);
        Assert.Equal(branchId, sale.BranchId);
        Assert.Equal(totalAmount, sale.TotalAmount);
        Assert.Equal(SaleStatus.Cancelled, sale.Status);
    }

    /// <summary>
    /// Tests that sale status can be set directly.
    /// </summary>
    [Fact(DisplayName = "Sale status should be settable directly")]
    public void Given_Sale_When_StatusSet_Then_ShouldBeSetCorrectly()
    {
        // Arrange
        var sale = new Sale();

        // Act
        sale.Status = SaleStatus.Cancelled;

        // Assert
        Assert.Equal(SaleStatus.Cancelled, sale.Status);
    }

    /// <summary>
    /// Tests that sale total amount can be set to zero.
    /// </summary>
    [Fact(DisplayName = "Sale total amount should accept zero value")]
    public void Given_Sale_When_TotalAmountSetToZero_Then_ShouldAcceptZero()
    {
        // Arrange
        var sale = new Sale();

        // Act
        sale.TotalAmount = 0;

        // Assert
        Assert.Equal(0, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that sale total amount can be set to negative value.
    /// </summary>
    [Fact(DisplayName = "Sale total amount should accept negative value")]
    public void Given_Sale_When_TotalAmountSetToNegative_Then_ShouldAcceptNegative()
    {
        // Arrange
        var sale = new Sale();
        var negativeAmount = -100.50m;

        // Act
        sale.TotalAmount = negativeAmount;

        // Assert
        Assert.Equal(negativeAmount, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that sale total amount can be set to high value.
    /// </summary>
    [Fact(DisplayName = "Sale total amount should accept high value")]
    public void Given_Sale_When_TotalAmountSetToHighValue_Then_ShouldAcceptHighValue()
    {
        // Arrange
        var sale = new Sale();
        var highAmount = 999999.99m;

        // Act
        sale.TotalAmount = highAmount;

        // Assert
        Assert.Equal(highAmount, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that sale cancellation reason can be set to empty string.
    /// </summary>
    [Fact(DisplayName = "Sale cancellation reason should accept empty string")]
    public void Given_Sale_When_CancelledWithEmptyReason_Then_ShouldAcceptEmptyReason()
    {
        // Arrange
        var sale = SaleTestData.GenerateActiveSale();

        // Act
        sale.Cancel("");

        // Assert
        Assert.Equal("", sale.CancellationReason);
    }

    /// <summary>
    /// Tests that sale cancellation reason can be set to long string.
    /// </summary>
    [Fact(DisplayName = "Sale cancellation reason should accept long string")]
    public void Given_Sale_When_CancelledWithLongReason_Then_ShouldAcceptLongReason()
    {
        // Arrange
        var sale = SaleTestData.GenerateActiveSale();
        var longReason = new string('A', 500);

        // Act
        sale.Cancel(longReason);

        // Assert
        Assert.Equal(longReason, sale.CancellationReason);
    }

    /// <summary>
    /// Tests that multiple sales can be created with different data.
    /// </summary>
    [Fact(DisplayName = "Multiple sales should be created with different data")]
    public void Given_MultipleSales_When_Created_Then_ShouldHaveDifferentData()
    {
        // Arrange & Act
        var sale1 = SaleTestData.GenerateValidSale();
        var sale2 = SaleTestData.GenerateValidSale();

        // Assert
        Assert.NotEqual(sale1.SaleNumber, sale2.SaleNumber);
        Assert.NotEqual(sale1.CustomerId, sale2.CustomerId);
        Assert.NotEqual(sale1.BranchId, sale2.BranchId);
    }

    /// <summary>
    /// Tests that sales with different amounts can be created.
    /// </summary>
    [Fact(DisplayName = "Sales with different amounts should be created correctly")]
    public void Given_SalesWithDifferentAmounts_When_Created_Then_ShouldHaveCorrectAmounts()
    {
        // Arrange & Act
        var saleWithLowAmount = SaleTestData.GenerateSaleWithLowAmount();
        var saleWithHighAmount = SaleTestData.GenerateSaleWithHighAmount();
        var saleWithZeroAmount = SaleTestData.GenerateSaleWithZeroAmount();

        // Assert
        Assert.True(saleWithLowAmount.TotalAmount <= 100.00m);
        Assert.True(saleWithHighAmount.TotalAmount >= 1000.00m);
        Assert.Equal(0, saleWithZeroAmount.TotalAmount);
    }

    /// <summary>
    /// Tests that sales with items can be created correctly.
    /// </summary>
    [Fact(DisplayName = "Sales with items should be created correctly")]
    public void Given_SalesWithItems_When_Created_Then_ShouldHaveCorrectItems()
    {
        // Arrange
        var product1 = ProductTestData.GenerateValidProduct();
        var product2 = ProductTestData.GenerateValidProduct();
        var products = new List<Product> { product1, product2 };

        // Act
        var saleWithSingleItem = SaleTestData.GenerateSaleWithSingleItem(product1, 5);
        var saleWithMultipleItems = SaleTestData.GenerateSaleWithMultipleItems(products);

        // Assert
        Assert.Single(saleWithSingleItem.Items);
        Assert.Equal(2, saleWithMultipleItems.Items.Count);
        Assert.True(saleWithSingleItem.TotalAmount > 0);
        Assert.True(saleWithMultipleItems.TotalAmount > 0);
    }
} 