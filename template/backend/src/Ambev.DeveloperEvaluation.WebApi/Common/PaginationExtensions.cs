using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

/// <summary>
/// Extension methods for pagination
/// </summary>
public static class PaginationExtensions
{
    /// <summary>
    /// Converts a PaginatedResult to PaginatedList
    /// </summary>
    /// <typeparam name="T">The type of items</typeparam>
    /// <param name="paginatedResult">The paginated result to convert</param>
    /// <returns>A PaginatedList</returns>
    public static PaginatedList<T> ToPaginatedList<T>(this PaginatedResult<T> paginatedResult)
    {
        return new PaginatedList<T>(
            paginatedResult.Items.ToList(),
            paginatedResult.TotalCount,
            paginatedResult.CurrentPage,
            paginatedResult.PageSize
        );
    }
} 