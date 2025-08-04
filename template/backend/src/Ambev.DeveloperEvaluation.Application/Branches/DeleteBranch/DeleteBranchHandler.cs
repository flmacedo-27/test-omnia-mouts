using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, DeleteBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<DeleteBranchHandler> _logger;
    private readonly DeleteBranchCommandValidator _validator;

    public DeleteBranchHandler(
        IBranchRepository branchRepository,
        ILogger<DeleteBranchHandler> logger,
        DeleteBranchCommandValidator validator)
    {
        _branchRepository = branchRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<DeleteBranchResult> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting branch with ID: {BranchId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for branch deletion: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        await _branchRepository.DeleteAsync(request.Id, cancellationToken);
        
        _logger.LogInformation("Branch deleted successfully with ID: {BranchId}", request.Id);

        return new DeleteBranchResult
        {
            Success = true,
            Message = "Branch deleted successfully"
        };
    }
} 