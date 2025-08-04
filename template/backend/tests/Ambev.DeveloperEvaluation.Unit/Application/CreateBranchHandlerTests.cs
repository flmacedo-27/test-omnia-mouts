using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateBranchHandler"/> class.
/// </summary>
public class CreateBranchHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateBranchHandler> _logger;
    private readonly CreateBranchCommandValidator _validator;
    private readonly CreateBranchHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBranchHandlerTests"/> class.
    /// </summary>
    public CreateBranchHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateBranchHandler>>();
        _validator = new CreateBranchCommandValidator(_branchRepository);
        _handler = new CreateBranchHandler(_branchRepository, _mapper, _logger, _validator);
    }

    /// <summary>
    /// Tests that a valid branch creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid branch data When creating branch Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateBranchHandlerTestData.GenerateValidCommand();

        var branch = new Branch
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Code = command.Code,
            Address = command.Address
        };

        var result = new CreateBranchResult
        {
            Id = branch.Id,
            Name = branch.Name,
            Code = branch.Code,
            Address = branch.Address
        };

        _mapper.Map<CreateBranchResult>(Arg.Any<Branch>()).Returns(result);
        _branchRepository.CreateAsync(Arg.Any<Branch>(), Arg.Any<CancellationToken>())
            .Returns(branch);

        _branchRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        var createBranchResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createBranchResult.Should().NotBeNull();
        createBranchResult.Id.Should().Be(branch.Id);
        createBranchResult.Name.Should().Be(command.Name);
        createBranchResult.Code.Should().Be(command.Code);
        createBranchResult.Address.Should().Be(command.Address);
        await _branchRepository.Received(1).CreateAsync(Arg.Any<Branch>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid branch creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid branch data When creating branch Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = CreateBranchHandlerTestData.GenerateCommandWithExistingCode();
        
        _branchRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>())
            .Returns(true);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that the branch is created with correct properties.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then creates branch with correct properties")]
    public async Task Handle_ValidRequest_CreatesBranchWithCorrectProperties()
    {
        // Given
        var command = new CreateBranchCommand
        {
            Name = "Filial Norte",
            Code = "NORTE001",
            Address = "Av. Principal, 456 - Norte"
        };

        var branch = new Branch
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Code = command.Code,
            Address = command.Address
        };

        _mapper.Map<CreateBranchResult>(Arg.Any<Branch>()).Returns(new CreateBranchResult());
        _branchRepository.CreateAsync(Arg.Any<Branch>(), Arg.Any<CancellationToken>())
            .Returns(branch);

        _branchRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _branchRepository.Received(1).CreateAsync(
            Arg.Is<Branch>(b => 
                b.Name == command.Name &&
                b.Code == command.Code &&
                b.Address == command.Address),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that logging is performed correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then logs information correctly")]
    public async Task Handle_ValidRequest_LogsInformationCorrectly()
    {
        // Given
        var command = new CreateBranchCommand
        {
            Name = "Filial Sul",
            Code = "SUL001",
            Address = "Rua do Comércio, 789 - Sul"
        };

        var branch = new Branch
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Code = command.Code,
            Address = command.Address
        };

        _mapper.Map<CreateBranchResult>(Arg.Any<Branch>()).Returns(new CreateBranchResult());
        _branchRepository.CreateAsync(Arg.Any<Branch>(), Arg.Any<CancellationToken>())
            .Returns(branch);

        _branchRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString().Contains(command.Name)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }
} 