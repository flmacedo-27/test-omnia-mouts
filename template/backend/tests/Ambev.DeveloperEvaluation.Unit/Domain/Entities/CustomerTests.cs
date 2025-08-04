using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Customer entity class.
/// Tests cover business methods and property updates.
/// </summary>
public class CustomerTests
{
    /// <summary>
    /// Tests that a new customer is created with default active status.
    /// </summary>
    [Fact(DisplayName = "New customer should be created with active status")]
    public void Given_NewCustomer_When_Created_Then_ShouldBeActive()
    {
        // Arrange & Act
        var customer = new Customer();

        // Assert
        Assert.True(customer.Active);
        Assert.Equal(string.Empty, customer.Name);
        Assert.Equal(string.Empty, customer.Email);
        Assert.Equal(string.Empty, customer.Phone);
        Assert.Equal(string.Empty, customer.DocumentNumber);
    }

    /// <summary>
    /// Tests that the Update method can handle empty strings.
    /// </summary>
    [Fact(DisplayName = "Update method should handle empty strings")]
    public void Given_Customer_When_UpdatedWithEmptyStrings_Then_ShouldAcceptEmptyValues()
    {
        // Arrange
        var customer = CustomerTestData.GenerateValidCustomer();

        // Act
        customer.Update("", "", "", CustomerType.CPF, "", true);

        // Assert
        Assert.Equal("", customer.Name);
        Assert.Equal("", customer.Email);
        Assert.Equal("", customer.Phone);
        Assert.Equal("", customer.DocumentNumber);
    }

    /// <summary>
    /// Tests that the Update method can handle long strings.
    /// </summary>
    [Fact(DisplayName = "Update method should handle long strings")]
    public void Given_Customer_When_UpdatedWithLongStrings_Then_ShouldAcceptLongValues()
    {
        // Arrange
        var customer = new Customer();
        var longName = new string('A', 100);
        var longEmail = new string('B', 100) + "@test.com";
        var longPhone = new string('C', 50);
        var longDocument = new string('D', 20);

        // Act
        customer.Update(longName, longEmail, longPhone, CustomerType.CPF, longDocument, true);

        // Assert
        Assert.Equal(longName, customer.Name);
        Assert.Equal(longEmail, customer.Email);
        Assert.Equal(longPhone, customer.Phone);
        Assert.Equal(longDocument, customer.DocumentNumber);
    }

    /// <summary>
    /// Tests that customer properties can be set directly.
    /// </summary>
    [Fact(DisplayName = "Customer properties should be settable directly")]
    public void Given_Customer_When_PropertiesSet_Then_ShouldBeSetCorrectly()
    {
        // Arrange
        var customer = new Customer();
        var name = CustomerTestData.GenerateValidCustomerName();
        var email = CustomerTestData.GenerateValidEmail();
        var phone = CustomerTestData.GenerateValidPhone();
        var cpf = CustomerTestData.GenerateValidCPF();

        // Act
        customer.Name = name;
        customer.Email = email;
        customer.Phone = phone;
        customer.CustomerType = CustomerType.CPF;
        customer.DocumentNumber = cpf;
        customer.Active = false;

        // Assert
        Assert.Equal(name, customer.Name);
        Assert.Equal(email, customer.Email);
        Assert.Equal(phone, customer.Phone);
        Assert.Equal(CustomerType.CPF, customer.CustomerType);
        Assert.Equal(cpf, customer.DocumentNumber);
        Assert.False(customer.Active);
    }

    /// <summary>
    /// Tests that the Update method updates the timestamp.
    /// </summary>
    [Fact(DisplayName = "Update method should update timestamp")]
    public void Given_Customer_When_Updated_Then_ShouldUpdateTimestamp()
    {
        // Arrange
        var customer = new Customer();
        
        Thread.Sleep(10);
        var originalTimestamp = customer.UpdatedAt;

        // Act
        customer.Update("Novo Nome", "novo@email.com", "+5511999999999", CustomerType.CPF, "123.456.789-00", true);

        // Assert
        Assert.True(customer.UpdatedAt > originalTimestamp || (originalTimestamp == null && customer.UpdatedAt != null));
    }

    /// <summary>
    /// Tests that customer with CPF type can be set correctly.
    /// </summary>
    [Fact(DisplayName = "Customer with CPF type should be set correctly")]
    public void Given_Customer_When_CPFTypeSet_Then_ShouldBeSetCorrectly()
    {
        // Arrange
        var customer = new Customer();
        var cpf = CustomerTestData.GenerateValidCPF();

        // Act
        customer.CustomerType = CustomerType.CPF;
        customer.DocumentNumber = cpf;

        // Assert
        Assert.Equal(CustomerType.CPF, customer.CustomerType);
        Assert.Equal(cpf, customer.DocumentNumber);
    }

    /// <summary>
    /// Tests that customer with CNPJ type can be set correctly.
    /// </summary>
    [Fact(DisplayName = "Customer with CNPJ type should be set correctly")]
    public void Given_Customer_When_CNPJTypeSet_Then_ShouldBeSetCorrectly()
    {
        // Arrange
        var customer = new Customer();
        var cnpj = CustomerTestData.GenerateValidCNPJ();

        // Act
        customer.CustomerType = CustomerType.CNPJ;
        customer.DocumentNumber = cnpj;

        // Assert
        Assert.Equal(CustomerType.CNPJ, customer.CustomerType);
        Assert.Equal(cnpj, customer.DocumentNumber);
    }

    /// <summary>
    /// Tests that active customer can be deactivated.
    /// </summary>
    [Fact(DisplayName = "Active customer should be deactivated")]
    public void Given_ActiveCustomer_When_Deactivated_Then_ShouldBeInactive()
    {
        // Arrange
        var customer = CustomerTestData.GenerateActiveCustomer();

        // Act
        customer.Active = false;

        // Assert
        Assert.False(customer.Active);
    }

    /// <summary>
    /// Tests that inactive customer can be activated.
    /// </summary>
    [Fact(DisplayName = "Inactive customer should be activated")]
    public void Given_InactiveCustomer_When_Activated_Then_ShouldBeActive()
    {
        // Arrange
        var customer = CustomerTestData.GenerateInactiveCustomer();

        // Act
        customer.Active = true;

        // Assert
        Assert.True(customer.Active);
    }

    /// <summary>
    /// Tests that multiple customers can be created with different data.
    /// </summary>
    [Fact(DisplayName = "Multiple customers should be created with different data")]
    public void Given_MultipleCustomers_When_Created_Then_ShouldHaveDifferentData()
    {
        // Arrange & Act
        var customer1 = CustomerTestData.GenerateValidCustomer();
        var customer2 = CustomerTestData.GenerateValidCustomer();

        // Assert
        Assert.NotEqual(customer1.Name, customer2.Name);
        Assert.NotEqual(customer1.Email, customer2.Email);
        Assert.NotEqual(customer1.Phone, customer2.Phone);
        Assert.NotEqual(customer1.DocumentNumber, customer2.DocumentNumber);
    }

    /// <summary>
    /// Tests that customers with different types can be created.
    /// </summary>
    [Fact(DisplayName = "Customers with different types should be created correctly")]
    public void Given_CustomersWithDifferentTypes_When_Created_Then_ShouldHaveCorrectTypes()
    {
        // Arrange & Act
        var cpfCustomer = CustomerTestData.GenerateCPFCustomer();
        var cnpjCustomer = CustomerTestData.GenerateCNPJCustomer();

        // Assert
        Assert.Equal(CustomerType.CPF, cpfCustomer.CustomerType);
        Assert.Equal(CustomerType.CNPJ, cnpjCustomer.CustomerType);
        Assert.True(cpfCustomer.DocumentNumber.Contains(".") && cpfCustomer.DocumentNumber.Contains("-"));
        Assert.True(cnpjCustomer.DocumentNumber.Contains(".") && cnpjCustomer.DocumentNumber.Contains("/"));
    }

    /// <summary>
    /// Tests that customers with different status can be created.
    /// </summary>
    [Fact(DisplayName = "Customers with different status should be created correctly")]
    public void Given_CustomersWithDifferentStatus_When_Created_Then_ShouldHaveCorrectStatus()
    {
        // Arrange & Act
        var activeCustomer = CustomerTestData.GenerateActiveCustomer();
        var inactiveCustomer = CustomerTestData.GenerateInactiveCustomer();

        // Assert
        Assert.True(activeCustomer.Active);
        Assert.False(inactiveCustomer.Active);
    }

    /// <summary>
    /// Tests that customer with specific properties can be created.
    /// </summary>
    [Fact(DisplayName = "Customer with specific properties should be created correctly")]
    public void Given_CustomerWithSpecificProperties_When_Created_Then_ShouldHaveCorrectProperties()
    {
        // Arrange
        var name = "João Silva";
        var email = "joao@test.com";
        var phone = "+5511999999999";
        var customerType = CustomerType.CPF;
        var documentNumber = "123.456.789-00";

        // Act
        var customer = CustomerTestData.GenerateCustomer(name, email, phone, customerType, documentNumber);

        // Assert
        Assert.Equal(name, customer.Name);
        Assert.Equal(email, customer.Email);
        Assert.Equal(phone, customer.Phone);
        Assert.Equal(customerType, customer.CustomerType);
        Assert.Equal(documentNumber, customer.DocumentNumber);
        Assert.True(customer.Active);
    }

    /// <summary>
    /// Tests that customer can handle zero values for numeric properties.
    /// </summary>
    [Fact(DisplayName = "Customer should handle zero values for numeric properties")]
    public void Given_Customer_When_PropertiesSetToZero_Then_ShouldAcceptZeroValues()
    {
        // Arrange
        var customer = new Customer();

        // Act
        customer.Update("", "", "", CustomerType.CPF, "", true);

        // Assert
        Assert.Equal("", customer.Name);
        Assert.Equal("", customer.Email);
        Assert.Equal("", customer.Phone);
        Assert.Equal("", customer.DocumentNumber);
        Assert.True(customer.Active);
    }

    /// <summary>
    /// Tests that customer can handle negative values for status.
    /// </summary>
    [Fact(DisplayName = "Customer should handle status changes")]
    public void Given_Customer_When_StatusChanged_Then_ShouldHandleStatusChanges()
    {
        // Arrange
        var customer = CustomerTestData.GenerateActiveCustomer();

        // Act - Deactivate
        customer.Active = false;

        // Assert
        Assert.False(customer.Active);

        // Act - Activate
        customer.Active = true;

        // Assert
        Assert.True(customer.Active);
    }
} 