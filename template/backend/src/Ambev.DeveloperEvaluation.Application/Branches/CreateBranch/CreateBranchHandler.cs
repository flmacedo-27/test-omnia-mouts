using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

public class CreateBranchHandler : IRequestHandler<CreateBranchCommand, CreateBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateBranchHandler> _logger;
    private readonly CreateBranchCommandValidator _validator;

    public CreateBranchHandler(
        IBranchRepository branchRepository, 
        IMapper mapper,
        ILogger<CreateBranchHandler> logger,
        CreateBranchCommandValidator validator)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<CreateBranchResult> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating branch with name: {BranchName}", request.Name);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for branch creation: {ValidationErrors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            throw new ValidationException(validationResult.Errors);
        }

        var branch = new Branch
        {
            Name = request.Name,
            Code = request.Code,
            Address = request.Address
        };

        var createdBranch = await _branchRepository.CreateAsync(branch, cancellationToken);

        _logger.LogInformation("Branch created successfully with ID: {BranchId}", createdBranch.Id);

        return _mapper.Map<CreateBranchResult>(createdBranch);
    }
} 