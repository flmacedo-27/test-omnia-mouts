using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// Tests cover business methods and property updates.
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Tests that a new sale item is created with default active status.
    /// </summary>
    [Fact(DisplayName = "New sale item should be created with active status")]
    public void Given_NewSaleItem_When_Created_Then_ShouldBeActive()
    {
        // Arrange & Act
        var saleItem = new SaleItem();

        // Assert
        Assert.Equal(SaleItemStatus.Active, saleItem.Status);
        Assert.Equal(Guid.Empty, saleItem.SaleId);
        Assert.Equal(Guid.Empty, saleItem.ProductId);
        Assert.Equal(0, saleItem.Quantity);
        Assert.Equal(0, saleItem.UnitPrice);
        Assert.Equal(0, saleItem.DiscountPercentage);
        Assert.Equal(0, saleItem.DiscountAmount);
        Assert.Equal(0, saleItem.TotalAmount);
        Assert.Null(saleItem.CancelledAt);
        Assert.Null(saleItem.CancellationReason);
    }

    /// <summary>
    /// Tests that the CalculateDiscount method correctly calculates discount for low quantity (1-3 items).
    /// </summary>
    [Fact(DisplayName = "CalculateDiscount method should apply no discount for low quantity")]
    public void Given_SaleItemWithLowQuantity_When_CalculateDiscount_Then_ShouldApplyNoDiscount()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateSaleItemWithLowQuantity();

        // Act
        saleItem.CalculateDiscount();

        // Assert
        Assert.Equal(0, saleItem.DiscountPercentage);
        Assert.Equal(0, saleItem.DiscountAmount);
        Assert.Equal(saleItem.UnitPrice * saleItem.Quantity, saleItem.TotalAmount);
    }

    /// <summary>
    /// Tests that the CalculateDiscount method correctly calculates discount for medium quantity (4-9 items).
    /// </summary>
    [Fact(DisplayName = "CalculateDiscount method should apply 10% discount for medium quantity")]
    public void Given_SaleItemWithMediumQuantity_When_CalculateDiscount_Then_ShouldApplyTenPercentDiscount()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateSaleItemWithMediumQuantity();

        // Act
        saleItem.CalculateDiscount();

        // Assert
        Assert.Equal(10, saleItem.DiscountPercentage);
        var expectedDiscount = (saleItem.UnitPrice * saleItem.Quantity * 10) / 100;
        Assert.Equal(expectedDiscount, saleItem.DiscountAmount);
        var expectedTotal = (saleItem.UnitPrice * saleItem.Quantity) - expectedDiscount;
        Assert.Equal(expectedTotal, saleItem.TotalAmount);
    }

    /// <summary>
    /// Tests that the CalculateDiscount method correctly calculates discount for high quantity (10-20 items).
    /// </summary>
    [Fact(DisplayName = "CalculateDiscount method should apply 20% discount for high quantity")]
    public void Given_SaleItemWithHighQuantity_When_CalculateDiscount_Then_ShouldApplyTwentyPercentDiscount()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateSaleItemWithHighQuantity();

        // Act
        saleItem.CalculateDiscount();

        // Assert
        Assert.Equal(20, saleItem.DiscountPercentage);
        var expectedDiscount = (saleItem.UnitPrice * saleItem.Quantity * 20) / 100;
        Assert.Equal(expectedDiscount, saleItem.DiscountAmount);
        var expectedTotal = (saleItem.UnitPrice * saleItem.Quantity) - expectedDiscount;
        Assert.Equal(expectedTotal, saleItem.TotalAmount);
    }

    /// <summary>
    /// Tests that the CalculateDiscount method throws exception for quantity above 20.
    /// </summary>
    [Fact(DisplayName = "CalculateDiscount method should throw exception for quantity above 20")]
    public void Given_SaleItemWithQuantityAboveTwenty_When_CalculateDiscount_Then_ShouldThrowException()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Quantity = 21,
            UnitPrice = 10.00m
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => saleItem.CalculateDiscount());
        Assert.Equal("Cannot sell more than 20 identical items", exception.Message);
    }

    /// <summary>
    /// Tests that the Cancel method correctly cancels the sale item.
    /// </summary>
    [Fact(DisplayName = "Cancel method should correctly cancel the sale item")]
    public void Given_ActiveSaleItem_When_Cancelled_Then_ShouldBeCancelled()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateActiveSaleItem();
        var reason = SaleItemTestData.GenerateValidCancellationReason();

        // Act
        saleItem.Cancel(reason);

        // Assert
        Assert.Equal(SaleItemStatus.Cancelled, saleItem.Status);
        Assert.NotNull(saleItem.CancelledAt);
        Assert.Equal(reason, saleItem.CancellationReason);
    }

    /// <summary>
    /// Tests that the Cancel method sets the cancellation date.
    /// </summary>
    [Fact(DisplayName = "Cancel method should set cancellation date")]
    public void Given_SaleItem_When_Cancelled_Then_ShouldSetCancellationDate()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateActiveSaleItem();
        var reason = SaleItemTestData.GenerateValidCancellationReason();

        // Act
        saleItem.Cancel(reason);

        // Assert
        Assert.NotNull(saleItem.CancelledAt);
        Assert.True(saleItem.CancelledAt.Value > DateTime.UtcNow.AddMinutes(-1));
    }

    /// <summary>
    /// Tests that sale item properties can be set directly.
    /// </summary>
    [Fact(DisplayName = "Sale item properties should be settable directly")]
    public void Given_SaleItem_When_PropertiesSet_Then_ShouldBeSetCorrectly()
    {
        // Arrange
        var saleItem = new SaleItem();
        var saleId = SaleItemTestData.GenerateValidSaleId();
        var productId = SaleItemTestData.GenerateValidProductId();
        var quantity = SaleItemTestData.GenerateValidQuantity();
        var unitPrice = SaleItemTestData.GenerateValidUnitPrice();

        // Act
        saleItem.SaleId = saleId;
        saleItem.ProductId = productId;
        saleItem.Quantity = quantity;
        saleItem.UnitPrice = unitPrice;
        saleItem.Status = SaleItemStatus.Cancelled;

        // Assert
        Assert.Equal(saleId, saleItem.SaleId);
        Assert.Equal(productId, saleItem.ProductId);
        Assert.Equal(quantity, saleItem.Quantity);
        Assert.Equal(unitPrice, saleItem.UnitPrice);
        Assert.Equal(SaleItemStatus.Cancelled, saleItem.Status);
    }

    /// <summary>
    /// Tests that the Cancel method updates the timestamp.
    /// </summary>
    [Fact(DisplayName = "Cancel method should update timestamp")]
    public void Given_SaleItem_When_Cancelled_Then_ShouldUpdateTimestamp()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateActiveSaleItem();
        
        Thread.Sleep(10);
        var originalTimestamp = saleItem.UpdatedAt;
        var reason = SaleItemTestData.GenerateValidCancellationReason();

        // Act
        saleItem.Cancel(reason);

        // Assert
        Assert.True(saleItem.UpdatedAt > originalTimestamp);
    }

    /// <summary>
    /// Tests that sale item status can be set directly.
    /// </summary>
    [Fact(DisplayName = "Sale item status should be settable directly")]
    public void Given_SaleItem_When_StatusSet_Then_ShouldBeSetCorrectly()
    {
        // Arrange
        var saleItem = new SaleItem();

        // Act
        saleItem.Status = SaleItemStatus.Cancelled;

        // Assert
        Assert.Equal(SaleItemStatus.Cancelled, saleItem.Status);
    }

    /// <summary>
    /// Tests that sale item quantity can be set to zero.
    /// </summary>
    [Fact(DisplayName = "Sale item quantity should accept zero value")]
    public void Given_SaleItem_When_QuantitySetToZero_Then_ShouldAcceptZero()
    {
        // Arrange
        var saleItem = new SaleItem();

        // Act
        saleItem.Quantity = 0;

        // Assert
        Assert.Equal(0, saleItem.Quantity);
    }

    /// <summary>
    /// Tests that sale item quantity can be set to negative value.
    /// </summary>
    [Fact(DisplayName = "Sale item quantity should accept negative value")]
    public void Given_SaleItem_When_QuantitySetToNegative_Then_ShouldAcceptNegative()
    {
        // Arrange
        var saleItem = new SaleItem();
        var negativeQuantity = -5;

        // Act
        saleItem.Quantity = negativeQuantity;

        // Assert
        Assert.Equal(negativeQuantity, saleItem.Quantity);
    }

    /// <summary>
    /// Tests that sale item unit price can be set to zero.
    /// </summary>
    [Fact(DisplayName = "Sale item unit price should accept zero value")]
    public void Given_SaleItem_When_UnitPriceSetToZero_Then_ShouldAcceptZero()
    {
        // Arrange
        var saleItem = new SaleItem();

        // Act
        saleItem.UnitPrice = 0;

        // Assert
        Assert.Equal(0, saleItem.UnitPrice);
    }

    /// <summary>
    /// Tests that sale item unit price can be set to negative value.
    /// </summary>
    [Fact(DisplayName = "Sale item unit price should accept negative value")]
    public void Given_SaleItem_When_UnitPriceSetToNegative_Then_ShouldAcceptNegative()
    {
        // Arrange
        var saleItem = new SaleItem();
        var negativePrice = -10.50m;

        // Act
        saleItem.UnitPrice = negativePrice;

        // Assert
        Assert.Equal(negativePrice, saleItem.UnitPrice);
    }

    /// <summary>
    /// Tests that sale item cancellation reason can be set to empty string.
    /// </summary>
    [Fact(DisplayName = "Sale item cancellation reason should accept empty string")]
    public void Given_SaleItem_When_CancelledWithEmptyReason_Then_ShouldAcceptEmptyReason()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateActiveSaleItem();

        // Act
        saleItem.Cancel("");

        // Assert
        Assert.Equal("", saleItem.CancellationReason);
    }

    /// <summary>
    /// Tests that sale item cancellation reason can be set to long string.
    /// </summary>
    [Fact(DisplayName = "Sale item cancellation reason should accept long string")]
    public void Given_SaleItem_When_CancelledWithLongReason_Then_ShouldAcceptLongReason()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateActiveSaleItem();
        var longReason = new string('A', 500);

        // Act
        saleItem.Cancel(longReason);

        // Assert
        Assert.Equal(longReason, saleItem.CancellationReason);
    }

    /// <summary>
    /// Tests that multiple sale items can be created with different data.
    /// </summary>
    [Fact(DisplayName = "Multiple sale items should be created with different data")]
    public void Given_MultipleSaleItems_When_Created_Then_ShouldHaveDifferentData()
    {
        // Arrange & Act
        var saleItem1 = SaleItemTestData.GenerateValidSaleItem();
        var saleItem2 = SaleItemTestData.GenerateValidSaleItem();

        // Assert
        Assert.NotEqual(saleItem1.SaleId, saleItem2.SaleId);
        Assert.NotEqual(saleItem1.ProductId, saleItem2.ProductId);
        Assert.NotEqual(saleItem1.UnitPrice, saleItem2.UnitPrice);
    }

    /// <summary>
    /// Tests that sale items with different quantities can be created.
    /// </summary>
    [Fact(DisplayName = "Sale items with different quantities should be created correctly")]
    public void Given_SaleItemsWithDifferentQuantities_When_Created_Then_ShouldHaveCorrectQuantities()
    {
        // Arrange & Act
        var saleItemWithLowQuantity = SaleItemTestData.GenerateSaleItemWithLowQuantity();
        var saleItemWithMediumQuantity = SaleItemTestData.GenerateSaleItemWithMediumQuantity();
        var saleItemWithHighQuantity = SaleItemTestData.GenerateSaleItemWithHighQuantity();
        var saleItemWithMaximumQuantity = SaleItemTestData.GenerateSaleItemWithMaximumQuantity();

        // Assert
        Assert.True(saleItemWithLowQuantity.Quantity >= 1 && saleItemWithLowQuantity.Quantity <= 3);
        Assert.True(saleItemWithMediumQuantity.Quantity >= 4 && saleItemWithMediumQuantity.Quantity <= 9);
        Assert.True(saleItemWithHighQuantity.Quantity >= 10 && saleItemWithHighQuantity.Quantity <= 20);
        Assert.Equal(20, saleItemWithMaximumQuantity.Quantity);
    }

    /// <summary>
    /// Tests that sale items with different prices can be created.
    /// </summary>
    [Fact(DisplayName = "Sale items with different prices should be created correctly")]
    public void Given_SaleItemsWithDifferentPrices_When_Created_Then_ShouldHaveCorrectPrices()
    {
        // Arrange & Act
        var saleItemWithLowPrice = SaleItemTestData.GenerateSaleItemWithLowPrice();
        var saleItemWithHighPrice = SaleItemTestData.GenerateSaleItemWithHighPrice();

        // Assert
        Assert.True(saleItemWithLowPrice.UnitPrice <= 10.00m);
        Assert.True(saleItemWithHighPrice.UnitPrice >= 50.00m);
    }

    /// <summary>
    /// Tests that sale item with product can be created correctly.
    /// </summary>
    [Fact(DisplayName = "Sale item with product should be created correctly")]
    public void Given_SaleItemWithProduct_When_Created_Then_ShouldHaveCorrectProduct()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var quantity = 5;

        // Act
        var saleItem = SaleItemTestData.GenerateSaleItemWithProduct(product, quantity);

        // Assert
        Assert.Equal(product.Id, saleItem.ProductId);
        Assert.Equal(product, saleItem.Product);
        Assert.Equal(quantity, saleItem.Quantity);
        Assert.Equal(product.Price, saleItem.UnitPrice);
        Assert.Equal(SaleItemStatus.Active, saleItem.Status);
        Assert.True(saleItem.TotalAmount > 0);
    }

    /// <summary>
    /// Tests that sale item with specific properties can be created.
    /// </summary>
    [Fact(DisplayName = "Sale item with specific properties should be created correctly")]
    public void Given_SaleItemWithSpecificProperties_When_Created_Then_ShouldHaveCorrectProperties()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var quantity = 10;
        var unitPrice = 25.50m;
        var status = SaleItemStatus.Active;

        // Act
        var saleItem = SaleItemTestData.GenerateSaleItem(saleId, productId, quantity, unitPrice, status);

        // Assert
        Assert.Equal(saleId, saleItem.SaleId);
        Assert.Equal(productId, saleItem.ProductId);
        Assert.Equal(quantity, saleItem.Quantity);
        Assert.Equal(unitPrice, saleItem.UnitPrice);
        Assert.Equal(status, saleItem.Status);
        Assert.True(saleItem.TotalAmount > 0);
    }

    /// <summary>
    /// Tests that multiple sale items can be generated.
    /// </summary>
    [Fact(DisplayName = "Multiple sale items should be generated correctly")]
    public void Given_MultipleSaleItems_When_Generated_Then_ShouldHaveCorrectCount()
    {
        // Arrange
        var count = 5;

        // Act
        var saleItems = SaleItemTestData.GenerateMultipleSaleItems(count);

        // Assert
        Assert.Equal(count, saleItems.Count);
        Assert.All(saleItems, item => Assert.True(item.TotalAmount > 0));
    }

    /// <summary>
    /// Tests that sale item can handle zero values for numeric properties.
    /// </summary>
    [Fact(DisplayName = "Sale item should handle zero values for numeric properties")]
    public void Given_SaleItem_When_PropertiesSetToZero_Then_ShouldAcceptZeroValues()
    {
        // Arrange
        var saleItem = new SaleItem();

        // Act
        saleItem.Quantity = 0;
        saleItem.UnitPrice = 0;
        saleItem.DiscountPercentage = 0;
        saleItem.DiscountAmount = 0;
        saleItem.TotalAmount = 0;

        // Assert
        Assert.Equal(0, saleItem.Quantity);
        Assert.Equal(0, saleItem.UnitPrice);
        Assert.Equal(0, saleItem.DiscountPercentage);
        Assert.Equal(0, saleItem.DiscountAmount);
        Assert.Equal(0, saleItem.TotalAmount);
    }

    /// <summary>
    /// Tests that sale item can handle high values for numeric properties.
    /// </summary>
    [Fact(DisplayName = "Sale item should handle high values for numeric properties")]
    public void Given_SaleItem_When_PropertiesSetToHighValues_Then_ShouldAcceptHighValues()
    {
        // Arrange
        var saleItem = new SaleItem();
        var highQuantity = 1000;
        var highPrice = 999999.99m;
        var highDiscount = 100.00m;
        var highTotal = 999999.99m;

        // Act
        saleItem.Quantity = highQuantity;
        saleItem.UnitPrice = highPrice;
        saleItem.DiscountPercentage = highDiscount;
        saleItem.DiscountAmount = highDiscount;
        saleItem.TotalAmount = highTotal;

        // Assert
        Assert.Equal(highQuantity, saleItem.Quantity);
        Assert.Equal(highPrice, saleItem.UnitPrice);
        Assert.Equal(highDiscount, saleItem.DiscountPercentage);
        Assert.Equal(highDiscount, saleItem.DiscountAmount);
        Assert.Equal(highTotal, saleItem.TotalAmount);
    }
} 