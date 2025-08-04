using FluentValidation;
using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;

/// <summary>
/// Validator for ListBranchesRequest.
/// </summary>
public class ListBranchesRequestValidator : AbstractValidator<ListBranchesRequest>
{
    /// <summary>
    /// Initializes a new instance of the ListBranchesRequestValidator class.
    /// </summary>
    public ListBranchesRequestValidator()
    {
        Include(new PaginatedRequestValidator());
    }
} 