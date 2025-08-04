using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Branch entity class.
/// Tests cover business methods and property updates.
/// </summary>
public class BranchTests
{
    /// <summary>
    /// Tests that a new branch is created with default active status.
    /// </summary>
    [Fact(DisplayName = "New branch should be created with active status")]
    public void Given_NewBranch_When_Created_Then_ShouldBeActive()
    {
        // Arrange & Act
        var branch = new Branch();

        // Assert
        Assert.True(branch.Active);
        Assert.Equal(string.Empty, branch.Name);
        Assert.Equal(string.Empty, branch.Code);
        Assert.Equal(string.Empty, branch.Address);
    }

    /// <summary>
    /// Tests that the Update method correctly updates branch properties.
    /// </summary>
    [Fact(DisplayName = "Update method should correctly update branch properties")]
    public void Given_Branch_When_Updated_Then_PropertiesShouldBeUpdated()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        var newName = BranchTestData.GenerateValidBranchName();
        var newCode = BranchTestData.GenerateValidBranchCode();
        var newAddress = BranchTestData.GenerateValidBranchAddress();

        // Act
        branch.Update(newName, newCode, newAddress);

        // Assert
        Assert.Equal(newName, branch.Name);
        Assert.Equal(newCode, branch.Code);
        Assert.Equal(newAddress, branch.Address);
    }

    /// <summary>
    /// Tests that the Update method preserves the Active status.
    /// </summary>
    [Fact(DisplayName = "Update method should preserve active status")]
    public void Given_ActiveBranch_When_Updated_Then_ShouldRemainActive()
    {
        // Arrange
        var branch = BranchTestData.GenerateActiveBranch();

        // Act
        branch.Update("Novo Nome", "NOVO001", "Nova Endereço");

        // Assert
        Assert.True(branch.Active);
    }

    /// <summary>
    /// Tests that the Update method preserves the Inactive status.
    /// </summary>
    [Fact(DisplayName = "Update method should preserve inactive status")]
    public void Given_InactiveBranch_When_Updated_Then_ShouldRemainInactive()
    {
        // Arrange
        var branch = BranchTestData.GenerateInactiveBranch();

        // Act
        branch.Update("Novo Nome", "NOVO001", "Nova Endereço");

        // Assert
        Assert.False(branch.Active);
    }

    /// <summary>
    /// Tests that the Update method can handle empty strings.
    /// </summary>
    [Fact(DisplayName = "Update method should handle empty strings")]
    public void Given_Branch_When_UpdatedWithEmptyStrings_Then_ShouldAcceptEmptyValues()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();

        // Act
        branch.Update("", "", "");

        // Assert
        Assert.Equal("", branch.Name);
        Assert.Equal("", branch.Code);
        Assert.Equal("", branch.Address);
    }

    /// <summary>
    /// Tests that the Update method can handle long strings.
    /// </summary>
    [Fact(DisplayName = "Update method should handle long strings")]
    public void Given_Branch_When_UpdatedWithLongStrings_Then_ShouldAcceptLongValues()
    {
        // Arrange
        var branch = new Branch();
        var longName = new string('A', 100);
        var longCode = new string('B', 20);
        var longAddress = new string('C', 200);

        // Act
        branch.Update(longName, longCode, longAddress);

        // Assert
        Assert.Equal(longName, branch.Name);
        Assert.Equal(longCode, branch.Code);
        Assert.Equal(longAddress, branch.Address);
    }

    /// <summary>
    /// Tests that the Update method updates the timestamp.
    /// </summary>
    [Fact(DisplayName = "Update method should update timestamp")]
    public void Given_Branch_When_Updated_Then_ShouldUpdateTimestamp()
    {
        // Arrange
        var branch = new Branch();
        
        Thread.Sleep(10);
        var originalTimestamp = branch.UpdatedAt;

        // Act
        branch.Update("Novo Nome", "NOVO001", "Nova Endereço");

        // Assert
        Assert.NotNull(branch.UpdatedAt);
        Assert.True(branch.UpdatedAt > originalTimestamp || (originalTimestamp == null && branch.UpdatedAt != null));
    }

    /// <summary>
    /// Tests that branch properties can be set directly.
    /// </summary>
    [Fact(DisplayName = "Branch properties should be settable directly")]
    public void Given_Branch_When_PropertiesSet_Then_ShouldBeSetCorrectly()
    {
        // Arrange
        var branch = new Branch();
        var name = BranchTestData.GenerateValidBranchName();
        var code = BranchTestData.GenerateValidBranchCode();
        var address = BranchTestData.GenerateValidBranchAddress();

        // Act
        branch.Name = name;
        branch.Code = code;
        branch.Address = address;
        branch.Active = false;

        // Assert
        Assert.Equal(name, branch.Name);
        Assert.Equal(code, branch.Code);
        Assert.Equal(address, branch.Address);
        Assert.False(branch.Active);
    }

    /// <summary>
    /// Tests that multiple branches can be created with different data.
    /// </summary>
    [Fact(DisplayName = "Multiple branches should be created with different data")]
    public void Given_MultipleBranches_When_Created_Then_ShouldHaveDifferentData()
    {
        // Arrange & Act
        var branch1 = BranchTestData.GenerateValidBranch();
        var branch2 = BranchTestData.GenerateValidBranch();

        // Assert
        Assert.NotEqual(branch1.Name, branch2.Name);
        Assert.NotEqual(branch1.Code, branch2.Code);
        Assert.NotEqual(branch1.Address, branch2.Address);
    }
} 