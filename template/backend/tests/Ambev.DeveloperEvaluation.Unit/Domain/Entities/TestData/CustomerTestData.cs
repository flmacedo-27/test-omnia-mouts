using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Customer entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CustomerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Customer entities.
    /// The generated customers will have valid:
    /// - Name (person names)
    /// - Email (valid format)
    /// - Phone (Brazilian format)
    /// - CustomerType (CPF or CNPJ)
    /// - DocumentNumber (valid format)
    /// - Active status (true by default)
    /// </summary>
    private static readonly Faker<Customer> CustomerFaker = new Faker<Customer>()
        .RuleFor(c => c.Name, f => f.Person.FullName)
        .RuleFor(c => c.Email, f => f.Internet.Email())
        .RuleFor(c => c.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        .RuleFor(c => c.CustomerType, f => f.PickRandom(CustomerType.CPF, CustomerType.CNPJ))
        .RuleFor(c => c.DocumentNumber, f => f.PickRandom(CustomerType.CPF, CustomerType.CNPJ) == CustomerType.CPF 
            ? f.Random.Replace("###.###.###-##") 
            : f.Random.Replace("##.###.###/####-##"))
        .RuleFor(c => c.Active, f => f.Random.Bool());

    /// <summary>
    /// Generates a valid Customer entity with randomized data.
    /// The generated customer will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid Customer entity with randomly generated data.</returns>
    public static Customer GenerateValidCustomer()
    {
        return CustomerFaker.Generate();
    }

    /// <summary>
    /// Generates a valid customer name using Faker.
    /// </summary>
    /// <returns>A valid customer name.</returns>
    public static string GenerateValidCustomerName()
    {
        return new Faker().Person.FullName;
    }

    /// <summary>
    /// Generates a valid email address using Faker.
    /// </summary>
    /// <returns>A valid email address.</returns>
    public static string GenerateValidEmail()
    {
        return new Faker().Internet.Email();
    }

    /// <summary>
    /// Generates a valid Brazilian phone number using Faker.
    /// </summary>
    /// <returns>A valid Brazilian phone number.</returns>
    public static string GenerateValidPhone()
    {
        var faker = new Faker();
        return $"+55{faker.Random.Number(11, 99)}{faker.Random.Number(100000000, 999999999)}";
    }

    /// <summary>
    /// Generates a valid CPF document number using Faker.
    /// </summary>
    /// <returns>A valid CPF document number.</returns>
    public static string GenerateValidCPF()
    {
        return new Faker().Random.Replace("###.###.###-##");
    }

    /// <summary>
    /// Generates a valid CNPJ document number using Faker.
    /// </summary>
    /// <returns>A valid CNPJ document number.</returns>
    public static string GenerateValidCNPJ()
    {
        return new Faker().Random.Replace("##.###.###/####-##");
    }

    /// <summary>
    /// Generates an invalid customer name (empty or too long).
    /// </summary>
    /// <returns>An invalid customer name.</returns>
    public static string GenerateInvalidCustomerName()
    {
        var faker = new Faker();
        return faker.Random.Bool() ? "" : new string('A', 101);
    }

    /// <summary>
    /// Generates an invalid email address.
    /// </summary>
    /// <returns>An invalid email address.</returns>
    public static string GenerateInvalidEmail()
    {
        return new Faker().Random.Word();
    }

    /// <summary>
    /// Generates an invalid phone number.
    /// </summary>
    /// <returns>An invalid phone number.</returns>
    public static string GenerateInvalidPhone()
    {
        return new Faker().Random.Word();
    }

    /// <summary>
    /// Generates an invalid document number (empty or too long).
    /// </summary>
    /// <returns>An invalid document number.</returns>
    public static string GenerateInvalidDocumentNumber()
    {
        var faker = new Faker();
        return faker.Random.Bool() ? "" : new string('1', 15);
    }

    /// <summary>
    /// Generates a customer with CPF type.
    /// </summary>
    /// <returns>A Customer entity with CPF type.</returns>
    public static Customer GenerateCPFCustomer()
    {
        var customer = CustomerFaker.Generate();
        customer.CustomerType = CustomerType.CPF;
        customer.DocumentNumber = GenerateValidCPF();
        return customer;
    }

    /// <summary>
    /// Generates a customer with CNPJ type.
    /// </summary>
    /// <returns>A Customer entity with CNPJ type.</returns>
    public static Customer GenerateCNPJCustomer()
    {
        var customer = CustomerFaker.Generate();
        customer.CustomerType = CustomerType.CNPJ;
        customer.DocumentNumber = GenerateValidCNPJ();
        return customer;
    }

    /// <summary>
    /// Generates an active customer for testing.
    /// </summary>
    /// <returns>An active Customer entity.</returns>
    public static Customer GenerateActiveCustomer()
    {
        var customer = CustomerFaker.Generate();
        customer.Active = true;
        return customer;
    }

    /// <summary>
    /// Generates an inactive customer for testing.
    /// </summary>
    /// <returns>An inactive Customer entity.</returns>
    public static Customer GenerateInactiveCustomer()
    {
        var customer = CustomerFaker.Generate();
        customer.Active = false;
        return customer;
    }

    /// <summary>
    /// Generates a customer with specific properties for testing.
    /// </summary>
    /// <param name="name">The customer name</param>
    /// <param name="email">The customer email</param>
    /// <param name="phone">The customer phone</param>
    /// <param name="customerType">The customer type</param>
    /// <param name="documentNumber">The document number</param>
    /// <param name="active">Whether the customer is active</param>
    /// <returns>A Customer entity with the specified properties.</returns>
    public static Customer GenerateCustomer(string name, string email, string phone, CustomerType customerType, string documentNumber, bool active = true)
    {
        return new Customer
        {
            Name = name,
            Email = email,
            Phone = phone,
            CustomerType = customerType,
            DocumentNumber = documentNumber,
            Active = active
        };
    }
} 