using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

public class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, UpdateBranchResult?>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateBranchHandler> _logger;
    private readonly UpdateBranchCommandValidator _validator;

    public UpdateBranchHandler(
        IBranchRepository branchRepository, 
        IMapper mapper,
        ILogger<UpdateBranchHandler> logger,
        UpdateBranchCommandValidator validator)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<UpdateBranchResult?> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating branch with ID: {BranchId}", request.Id);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for branch update: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);
        
        branch.Update(request.Name, request.Code, request.Address);
        
        var updatedBranch = await _branchRepository.UpdateAsync(branch, cancellationToken);
        
        _logger.LogInformation("Branch updated successfully with ID: {BranchId}", updatedBranch.Id);

        return _mapper.Map<UpdateBranchResult>(updatedBranch);
    }
} 