using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a customer in the system.
/// </summary>
public class Customer : BaseEntity
{
    /// <summary>
    /// Gets the customer name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the customer email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets the customer phone number.
    /// Must be a valid phone number format following the pattern (XX) XXXXX-XXXX.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets the customer type (CPF or CNPJ).
    /// </summary>
    public CustomerType CustomerType { get; set; }

    /// <summary>
    /// Gets the customer document number (CPF/CNPJ), up to 14 characters.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets whether the customer is active.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Initializes a new instance of the Customer class.
    /// </summary>
    public Customer()
    {
        Active = true;
    }

    /// <summary>
    /// Updates the customer information.
    /// </summary>
    /// <param name="name">The new customer name</param>
    /// <param name="email">The new customer email</param>
    /// <param name="phone">The new customer phone</param>
    /// <param name="customerType">The new customer type</param>
    /// <param name="documentNumber">The new customer document number</param>
    public void Update(string name, string email, string phone, CustomerType customerType, string documentNumber, bool active)
    {
        Name = name;
        Email = email;
        Phone = phone;
        CustomerType = customerType;
        DocumentNumber = documentNumber;
        Active = active;
        UpdateTimestamp();
    }
}