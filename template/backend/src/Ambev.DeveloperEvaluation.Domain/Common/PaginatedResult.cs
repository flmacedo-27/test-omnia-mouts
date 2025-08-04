namespace Ambev.DeveloperEvaluation.Domain.Common;

/// <summary>
/// Represents a paginated result with metadata
/// </summary>
/// <typeparam name="T">The type of items in the result</typeparam>
public class PaginatedResult<T>
{
    /// <summary>
    /// Gets or sets the list of items
    /// </summary>
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    
    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int CurrentPage { get; set; }
    
    /// <summary>
    /// Gets or sets the total number of pages
    /// </summary>
    public int TotalPages { get; set; }
    
    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// Gets or sets the total count of items
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// Gets whether there is a previous page
    /// </summary>
    public bool HasPrevious => CurrentPage > 1;
    
    /// <summary>
    /// Gets whether there is a next page
    /// </summary>
    public bool HasNext => CurrentPage < TotalPages;
    
    /// <summary>
    /// Initializes a new instance of PaginatedResult
    /// </summary>
    /// <param name="items">The list of items</param>
    /// <param name="totalCount">The total count of items</param>
    /// <param name="currentPage">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public PaginatedResult(IEnumerable<T> items, int totalCount, int currentPage, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
    }
} 