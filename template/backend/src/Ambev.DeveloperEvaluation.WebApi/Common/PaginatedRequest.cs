namespace Ambev.DeveloperEvaluation.WebApi.Common;

/// <summary>
/// Generic paginated request for WebApi layer.
/// </summary>
public class PaginatedRequest
{
    /// <summary>
    /// Gets or sets the page number (default: 1).
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size (default: 10).
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the search term.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Gets or sets the sort field.
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Gets or sets the sort direction (asc/desc).
    /// </summary>
    public string? SortDirection { get; set; } = "asc";

    /// <summary>
    /// Validates the pagination parameters.
    /// </summary>
    /// <returns>True if valid, false otherwise</returns>
    public bool IsValid()
    {
        return PageNumber > 0 && PageSize > 0 && PageSize <= 100;
    }
}