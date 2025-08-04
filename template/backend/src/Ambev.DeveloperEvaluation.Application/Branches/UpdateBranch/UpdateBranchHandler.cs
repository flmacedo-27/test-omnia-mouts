using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

public class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, UpdateBranchResult?>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateBranchHandler> _logger;

    public UpdateBranchHandler(
        IBranchRepository branchRepository, 
        IMapper mapper,
        ILogger<UpdateBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UpdateBranchResult?> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating branch with ID: {BranchId}", request.Id);

        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (branch == null)
        {
            _logger.LogWarning("Branch not found for update with ID: {BranchId}", request.Id);
            return null;
        }

        _logger.LogDebug("Branch found, updating with new values. Old name: {OldName}, New name: {NewName}", 
            branch.Name, request.Name);

        branch.Update(request.Name, request.Code, request.Address);
        
        var updatedBranch = await _branchRepository.UpdateAsync(branch, cancellationToken);
        
        _logger.LogInformation("Branch updated successfully with ID: {BranchId}", updatedBranch.Id);

        return _mapper.Map<UpdateBranchResult>(updatedBranch);
    }
} 