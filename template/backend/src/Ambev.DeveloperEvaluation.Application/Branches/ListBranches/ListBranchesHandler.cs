using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

public class ListBranchesHandler : IRequestHandler<ListBranchesCommand, ListBranchesResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListBranchesHandler> _logger;

    public ListBranchesHandler(
        IBranchRepository branchRepository, 
        IMapper mapper,
        ILogger<ListBranchesHandler> logger)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ListBranchesResult> Handle(ListBranchesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing branches with page {PageNumber} and size {PageSize}", request.PageNumber, request.PageSize);

        var (branches, totalCount) = await _branchRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        _logger.LogInformation("Found {Count} branches, total: {TotalCount}", branches.Count(), totalCount);

        var branchItems = _mapper.Map<IEnumerable<BranchListItem>>(branches);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        _logger.LogDebug("Returning {BranchCount} branches with {TotalPages} total pages", branchItems.Count(), totalPages);

        return new ListBranchesResult
        {
            Branches = branchItems,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }
} 