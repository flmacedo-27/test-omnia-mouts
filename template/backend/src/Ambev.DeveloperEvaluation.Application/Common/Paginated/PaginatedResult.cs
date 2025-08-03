namespace Ambev.DeveloperEvaluation.Application.Common.Paginated;

/// <summary>
/// Generic paginated result for application layer.
/// </summary>
/// <typeparam name="T">The type of items in the paginated result</typeparam>
public class PaginatedResult<T>
{
    /// <summary>
    /// Gets or sets the collection of items.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Gets or sets the total count of items.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets whether there is a previous page.
    /// </summary>
    public bool HasPrevious => PageNumber > 1;

    /// <summary>
    /// Gets whether there is a next page.
    /// </summary>
    public bool HasNext => PageNumber < TotalPages;

    /// <summary>
    /// Initializes a new instance of the PaginatedResult class.
    /// </summary>
    public PaginatedResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the PaginatedResult class with specified parameters.
    /// </summary>
    /// <param name="items">The collection of items</param>
    /// <param name="totalCount">The total count of items</param>
    /// <param name="pageNumber">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public PaginatedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
    }
}