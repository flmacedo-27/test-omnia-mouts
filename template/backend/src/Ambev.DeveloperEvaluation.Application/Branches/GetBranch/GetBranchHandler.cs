using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

public class GetBranchHandler : IRequestHandler<GetBranchCommand, GetBranchResult?>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetBranchHandler> _logger;

    public GetBranchHandler(
        IBranchRepository branchRepository, 
        IMapper mapper,
        ILogger<GetBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetBranchResult?> Handle(GetBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting branch with ID: {BranchId}", request.Id);

        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (branch == null)
        {
            _logger.LogWarning("Branch not found with ID: {BranchId}", request.Id);
            return null;
        }

        _logger.LogDebug("Branch found with ID: {BranchId}, Name: {BranchName}", request.Id, branch.Name);

        return _mapper.Map<GetBranchResult>(branch);
    }
} 