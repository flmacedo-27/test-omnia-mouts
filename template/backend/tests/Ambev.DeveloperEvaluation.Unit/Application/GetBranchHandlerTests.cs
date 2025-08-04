using Ambev.DeveloperEvaluation.Application.Branches.GetBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="GetBranchHandler"/> class.
/// </summary>
public class GetBranchHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetBranchHandler> _logger;
    private readonly GetBranchHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetBranchHandlerTests"/> class.
    /// </summary>
    public GetBranchHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetBranchHandler>>();
        _handler = new GetBranchHandler(_branchRepository, _mapper, _logger);
    }

    /// <summary>
    /// Tests that a valid branch is returned when it exists.
    /// </summary>
    [Fact(DisplayName = "Given existing branch When getting branch Then returns branch data")]
    public async Task Handle_ExistingBranch_ReturnsBranchData()
    {
        // Given
        var branchId = Guid.NewGuid();
        var command = new GetBranchCommand { Id = branchId };

        var branch = new Branch
        {
            Id = branchId,
            Name = "Filial Centro",
            Code = "CENTRO001",
            Address = "Rua das Flores, 123 - Centro"
        };

        var result = new GetBranchResult
        {
            Id = branch.Id,
            Name = branch.Name,
            Code = branch.Code,
            Address = branch.Address
        };

        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>())
            .Returns(branch);
        _mapper.Map<GetBranchResult>(branch).Returns(result);

        // When
        var getBranchResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getBranchResult.Should().NotBeNull();
        getBranchResult!.Id.Should().Be(branchId);
        getBranchResult.Name.Should().Be(branch.Name);
        getBranchResult.Code.Should().Be(branch.Code);
        getBranchResult.Address.Should().Be(branch.Address);
        await _branchRepository.Received(1).GetByIdAsync(branchId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that null is returned when branch does not exist.
    /// </summary>
    [Fact(DisplayName = "Given non-existing branch When getting branch Then returns null")]
    public async Task Handle_NonExistingBranch_ReturnsNull()
    {
        // Given
        var branchId = Guid.NewGuid();
        var command = new GetBranchCommand { Id = branchId };

        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>())
            .Returns((Branch?)null);

        // When
        var getBranchResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getBranchResult.Should().BeNull();
        await _branchRepository.Received(1).GetByIdAsync(branchId, Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<GetBranchResult>(Arg.Any<Branch>());
    }

    /// <summary>
    /// Tests that logging is performed correctly when branch is found.
    /// </summary>
    [Fact(DisplayName = "Given existing branch When getting branch Then logs debug information")]
    public async Task Handle_ExistingBranch_LogsDebugInformation()
    {
        // Given
        var branchId = Guid.NewGuid();
        var command = new GetBranchCommand { Id = branchId };

        var branch = new Branch
        {
            Id = branchId,
            Name = "Filial Norte",
            Code = "NORTE001",
            Address = "Av. Principal, 456 - Norte"
        };

        var result = new GetBranchResult();
        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>())
            .Returns(branch);
        _mapper.Map<GetBranchResult>(branch).Returns(result);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _logger.Received(1).Log(
            LogLevel.Debug,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString().Contains(branchId.ToString()) && o.ToString().Contains(branch.Name)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }

    /// <summary>
    /// Tests that warning is logged when branch is not found.
    /// </summary>
    [Fact(DisplayName = "Given non-existing branch When getting branch Then logs warning")]
    public async Task Handle_NonExistingBranch_LogsWarning()
    {
        // Given
        var branchId = Guid.NewGuid();
        var command = new GetBranchCommand { Id = branchId };

        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>())
            .Returns((Branch?)null);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _logger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString().Contains(branchId.ToString())),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }

    /// <summary>
    /// Tests that information is logged when starting the operation.
    /// </summary>
    [Fact(DisplayName = "Given branch ID When getting branch Then logs information")]
    public async Task Handle_ValidCommand_LogsInformation()
    {
        // Given
        var branchId = Guid.NewGuid();
        var command = new GetBranchCommand { Id = branchId };

        var branch = new Branch
        {
            Id = branchId,
            Name = "Filial Sul",
            Code = "SUL001",
            Address = "Rua do Comércio, 789 - Sul"
        };

        var result = new GetBranchResult();
        _branchRepository.GetByIdAsync(branchId, Arg.Any<CancellationToken>())
            .Returns(branch);
        _mapper.Map<GetBranchResult>(branch).Returns(result);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString().Contains(branchId.ToString())),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }
} 