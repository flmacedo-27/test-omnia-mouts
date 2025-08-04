using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Product entity class.
/// Tests cover business methods and property updates.
/// </summary>
public class ProductTests
{
    /// <summary>
    /// Tests that a new product is created with default active status.
    /// </summary>
    [Fact(DisplayName = "New product should be created with active status")]
    public void Given_NewProduct_When_Created_Then_ShouldBeActive()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        Assert.True(product.Active);
        Assert.Equal(string.Empty, product.Name);
        Assert.Equal(string.Empty, product.Code);
        Assert.Equal(string.Empty, product.Description);
        Assert.Equal(0, product.Price);
        Assert.Equal(0, product.StockQuantity);
        Assert.Equal(string.Empty, product.SKU);
    }

    /// <summary>
    /// Tests that the Update method correctly updates product properties.
    /// </summary>
    [Fact(DisplayName = "Update method should correctly update product properties")]
    public void Given_Product_When_Updated_Then_PropertiesShouldBeUpdated()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var newName = ProductTestData.GenerateValidProductName();
        var newCode = ProductTestData.GenerateValidProductCode();
        var newDescription = ProductTestData.GenerateValidProductDescription();
        var newPrice = ProductTestData.GenerateValidProductPrice();
        var newStockQuantity = ProductTestData.GenerateValidStockQuantity();
        var newSku = ProductTestData.GenerateValidSKU();

        // Act
        product.Update(newName, newCode, newDescription, newPrice, newStockQuantity, newSku);

        // Assert
        Assert.Equal(newName, product.Name);
        Assert.Equal(newCode, product.Code);
        Assert.Equal(newDescription, product.Description);
        Assert.Equal(newPrice, product.Price);
        Assert.Equal(newStockQuantity, product.StockQuantity);
        Assert.Equal(newSku, product.SKU);
    }

    /// <summary>
    /// Tests that the UpdateStock method correctly updates stock quantity.
    /// </summary>
    [Fact(DisplayName = "UpdateStock method should correctly update stock quantity")]
    public void Given_Product_When_StockUpdated_Then_StockQuantityShouldBeUpdated()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var newQuantity = ProductTestData.GenerateValidStockQuantity();

        // Act
        product.UpdateStock(newQuantity);

        // Assert
        Assert.Equal(newQuantity, product.StockQuantity);
    }

    /// <summary>
    /// Tests that the Activate method sets the product as active.
    /// </summary>
    [Fact(DisplayName = "Activate method should set product as active")]
    public void Given_InactiveProduct_When_Activated_Then_ShouldBeActive()
    {
        // Arrange
        var product = ProductTestData.GenerateInactiveProduct();

        // Act
        product.Activate();

        // Assert
        Assert.True(product.Active);
    }

    /// <summary>
    /// Tests that the Deactivate method sets the product as inactive.
    /// </summary>
    [Fact(DisplayName = "Deactivate method should set product as inactive")]
    public void Given_ActiveProduct_When_Deactivated_Then_ShouldBeInactive()
    {
        // Arrange
        var product = ProductTestData.GenerateActiveProduct();

        // Act
        product.Deactivate();

        // Assert
        Assert.False(product.Active);
    }

    /// <summary>
    /// Tests that the Update method can handle zero values.
    /// </summary>
    [Fact(DisplayName = "Update method should handle zero values")]
    public void Given_Product_When_UpdatedWithZeroValues_Then_ShouldAcceptZeroValues()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act
        product.Update("", "", "", 0, 0, "");

        // Assert
        Assert.Equal("", product.Name);
        Assert.Equal("", product.Code);
        Assert.Equal("", product.Description);
        Assert.Equal(0, product.Price);
        Assert.Equal(0, product.StockQuantity);
        Assert.Equal("", product.SKU);
    }

    /// <summary>
    /// Tests that the UpdateStock method can handle negative stock.
    /// </summary>
    [Fact(DisplayName = "UpdateStock method should handle negative stock")]
    public void Given_Product_When_UpdatedWithNegativeStock_Then_ShouldAcceptNegativeStock()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var negativeStock = -10;

        // Act
        product.UpdateStock(negativeStock);

        // Assert
        Assert.Equal(negativeStock, product.StockQuantity);
    }

    /// <summary>
    /// Tests that the Update method can handle negative price.
    /// </summary>
    [Fact(DisplayName = "Update method should handle negative price")]
    public void Given_Product_When_UpdatedWithNegativePrice_Then_ShouldAcceptNegativePrice()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var negativePrice = -10.50m;

        // Act
        product.Update("Test", "TEST", "Test Description", negativePrice, 10, "SKU123");

        // Assert
        Assert.Equal(negativePrice, product.Price);
    }

    /// <summary>
    /// Tests that the Update method can handle long strings.
    /// </summary>
    [Fact(DisplayName = "Update method should handle long strings")]
    public void Given_Product_When_UpdatedWithLongStrings_Then_ShouldAcceptLongValues()
    {
        // Arrange
        var product = new Product();
        var longName = new string('A', 100);
        var longCode = new string('B', 20);
        var longDescription = new string('C', 500);
        var longSku = new string('D', 20);

        // Act
        product.Update(longName, longCode, longDescription, 10.50m, 100, longSku);

        // Assert
        Assert.Equal(longName, product.Name);
        Assert.Equal(longCode, product.Code);
        Assert.Equal(longDescription, product.Description);
        Assert.Equal(longSku, product.SKU);
    }

    /// <summary>
    /// Tests that product properties can be set directly.
    /// </summary>
    [Fact(DisplayName = "Product properties should be settable directly")]
    public void Given_Product_When_PropertiesSet_Then_ShouldBeSetCorrectly()
    {
        // Arrange
        var product = new Product();
        var name = ProductTestData.GenerateValidProductName();
        var code = ProductTestData.GenerateValidProductCode();
        var description = ProductTestData.GenerateValidProductDescription();
        var price = ProductTestData.GenerateValidProductPrice();
        var stockQuantity = ProductTestData.GenerateValidStockQuantity();
        var sku = ProductTestData.GenerateValidSKU();

        // Act
        product.Name = name;
        product.Code = code;
        product.Description = description;
        product.Price = price;
        product.StockQuantity = stockQuantity;
        product.SKU = sku;
        product.Active = false;

        // Assert
        Assert.Equal(name, product.Name);
        Assert.Equal(code, product.Code);
        Assert.Equal(description, product.Description);
        Assert.Equal(price, product.Price);
        Assert.Equal(stockQuantity, product.StockQuantity);
        Assert.Equal(sku, product.SKU);
        Assert.False(product.Active);
    }

    /// <summary>
    /// Tests that the UpdateStock method updates the timestamp.
    /// </summary>
    [Fact(DisplayName = "UpdateStock method should update timestamp")]
    public void Given_Product_When_StockUpdated_Then_ShouldUpdateTimestamp()
    {
        // Arrange
        var product = new Product();
        
        Thread.Sleep(10);
        var originalTimestamp = product.UpdatedAt;

        // Act
        product.UpdateStock(50);

        // Assert
        Assert.True(product.UpdatedAt > originalTimestamp || (originalTimestamp == null && product.UpdatedAt != null));
    }

    /// <summary>
    /// Tests that multiple products can be created with different data.
    /// </summary>
    [Fact(DisplayName = "Multiple products should be created with different data")]
    public void Given_MultipleProducts_When_Created_Then_ShouldHaveDifferentData()
    {
        // Arrange & Act
        var product1 = ProductTestData.GenerateValidProduct();
        var product2 = ProductTestData.GenerateValidProduct();

        // Assert
        Assert.NotEqual(product1.Name, product2.Name);
        Assert.NotEqual(product1.Code, product2.Code);
        Assert.NotEqual(product1.Description, product2.Description);
        Assert.NotEqual(product1.SKU, product2.SKU);
    }

    /// <summary>
    /// Tests that products with different stock levels can be created.
    /// </summary>
    [Fact(DisplayName = "Products with different stock levels should be created correctly")]
    public void Given_ProductsWithDifferentStock_When_Created_Then_ShouldHaveCorrectStock()
    {
        // Arrange & Act
        var productWithZeroStock = ProductTestData.GenerateProductWithZeroStock();
        var productWithHighStock = ProductTestData.GenerateProductWithHighStock();

        // Assert
        Assert.Equal(0, productWithZeroStock.StockQuantity);
        Assert.True(productWithHighStock.StockQuantity >= 100);
    }

    /// <summary>
    /// Tests that products with different prices can be created.
    /// </summary>
    [Fact(DisplayName = "Products with different prices should be created correctly")]
    public void Given_ProductsWithDifferentPrices_When_Created_Then_ShouldHaveCorrectPrices()
    {
        // Arrange & Act
        var productWithLowPrice = ProductTestData.GenerateProductWithLowPrice();
        var productWithHighPrice = ProductTestData.GenerateProductWithHighPrice();

        // Assert
        Assert.True(productWithLowPrice.Price <= 10.00m);
        Assert.True(productWithHighPrice.Price >= 100.00m);
    }
} 