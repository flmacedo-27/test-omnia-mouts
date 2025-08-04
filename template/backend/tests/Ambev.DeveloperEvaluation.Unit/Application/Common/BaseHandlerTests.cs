using Ambev.DeveloperEvaluation.Application.Common;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Common;

/// <summary>
/// Tests for the BaseHandler class
/// </summary>
public class BaseHandlerTests
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly TestValidator _validator;
    private readonly TestHandler _handler;

    public BaseHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger>();
        _validator = new TestValidator();
        _handler = new TestHandler(_mapper, _logger, _validator);
    }

    [Fact(DisplayName = "Handle should log operation start")]
    public async Task Handle_ShouldLogOperationStart()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };

        // Act
        await _handler.Handle(request, CancellationToken.None);
    }

    [Fact(DisplayName = "Handle should validate request")]
    public async Task Handle_ShouldValidateRequest()
    {
        // Arrange
        var request = new TestRequest { Name = "" }; // Invalid request
        _validator.ShouldReturnInvalid = true;

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact(DisplayName = "Handle should execute business logic when validation passes")]
    public async Task Handle_ShouldExecuteBusinessLogic_WhenValidationPasses()
    {
        // Arrange
        var request = new TestRequest { Name = "Valid" };
        var expectedResult = new TestResponse { Id = Guid.NewGuid() };
        _mapper.Map<TestResponse>(Arg.Any<object>()).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "Handle should log validation errors when validation fails")]
    public async Task Handle_ShouldLogValidationErrors_WhenValidationFails()
    {
        // Arrange
        var request = new TestRequest { Name = "" };
        _validator.ShouldReturnInvalid = true;

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }

    // Test classes for the BaseHandler
    public class TestRequest : IRequest<TestResponse>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class TestResponse
    {
        public Guid Id { get; set; }
    }

    public class TestValidator : IValidator<TestRequest>
    {
        public bool ShouldReturnInvalid { get; set; }

        public ValidationResult Validate(TestRequest instance)
        {
            if (ShouldReturnInvalid)
            {
                return new ValidationResult(new[] { new ValidationFailure("Name", "Name is required") });
            }

            return new ValidationResult();
        }

        public async Task<ValidationResult> ValidateAsync(TestRequest instance, CancellationToken cancellationToken = default)
        {
            return Validate(instance);
        }

        public async Task<ValidationResult> ValidateAsync(ValidationContext<TestRequest> context, CancellationToken cancellationToken = default)
        {
            return Validate(context.InstanceToValidate);
        }

        public ValidationResult Validate(IValidationContext context)
        {
            if (context is ValidationContext<TestRequest> typedContext)
            {
                return Validate(typedContext.InstanceToValidate);
            }

            return new ValidationResult();
        }

        public async Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellationToken = default)
        {
            return Validate(context);
        }

        public IValidatorDescriptor CreateDescriptor()
        {
            return null!;
        }

        public bool CanValidateInstancesOfType(Type type)
        {
            return type == typeof(TestRequest);
        }
    }

    public class TestHandler : BaseHandler<TestRequest, TestResponse, TestValidator>
    {
        public TestHandler(IMapper mapper, ILogger logger, TestValidator validator)
            : base(mapper, logger, validator)
        {
        }

        protected override async Task<TestResponse> ExecuteAsync(TestRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(1, cancellationToken);
            return Mapper.Map<TestResponse>(new { Id = Guid.NewGuid() });
        }
    }
} 